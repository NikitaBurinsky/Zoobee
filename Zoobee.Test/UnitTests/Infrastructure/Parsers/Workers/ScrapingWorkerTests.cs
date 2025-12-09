using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Moq;
using NUnit.Framework;
using Zoobee.Infrastructure.Parsers.Core.Entities;
using Zoobee.Infrastructure.Parsers.Core.Entities.Zoobee.Infrastructure.Parsers.Core.Entities;
using Zoobee.Infrastructure.Parsers.Core.Enums;
using Zoobee.Infrastructure.Parsers.Interfaces.Services.Downloader;
using Zoobee.Infrastructure.Parsers.Interfaces.Services.Scheduling;
using Zoobee.Infrastructure.Parsers.Workers;

namespace Zoobee.Test.Tests.Infrastructure.Parsers.Workers
{
	[TestFixture]
	public class ScrapingWorkerTests
	{
		private Mock<IServiceProvider> _serviceProviderMock;
		private Mock<IServiceScopeFactory> _scopeFactoryMock;
		private Mock<IServiceScope> _scopeMock;
		private Mock<IServiceProvider> _scopedProviderMock;

		private Mock<IDownloadSchedulingService> _schedulerMock;
		private Mock<IHtmlDownloader> _downloaderMock;
		private Mock<ILogger<ScrapingWorker>> _loggerMock;

		private ScrapingWorker _worker;

		[SetUp]
		public void Setup()
		{
			// 1. Инициализация всех моков
			_serviceProviderMock = new Mock<IServiceProvider>();
			_scopeFactoryMock = new Mock<IServiceScopeFactory>();
			_scopeMock = new Mock<IServiceScope>();
			_scopedProviderMock = new Mock<IServiceProvider>(); // Провайдер ВНУТРИ using(scope)

			_schedulerMock = new Mock<IDownloadSchedulingService>();
			_downloaderMock = new Mock<IHtmlDownloader>();
			_loggerMock = new Mock<ILogger<ScrapingWorker>>();

			// 2. Настройка цепочки создания Scope (самое сложное в DI)
			// Когда воркер просит ScopeFactory...
			_serviceProviderMock
				.Setup(x => x.GetService(typeof(IServiceScopeFactory)))
				.Returns(_scopeFactoryMock.Object);

			// Когда фабрика создает Scope...
			_scopeFactoryMock
				.Setup(x => x.CreateScope())
				.Returns(_scopeMock.Object);

			// Когда Scope отдает свой Provider...
			_scopeMock
				.Setup(x => x.ServiceProvider)
				.Returns(_scopedProviderMock.Object);

			// 3. Настройка получения сервисов из Scoped Provider
			_scopedProviderMock
				.Setup(x => x.GetService(typeof(IDownloadSchedulingService)))
				.Returns(_schedulerMock.Object);

			_scopedProviderMock
				.Setup(x => x.GetService(typeof(IHtmlDownloader)))
				.Returns(_downloaderMock.Object);

			// 4. Создаем сам воркер
			_worker = new ScrapingWorker(_serviceProviderMock.Object, _loggerMock.Object);
		}

		[Test]
		public async Task ExecuteAsync_WhenTaskExists_ShouldDownloadAndSave()
		{
			// Arrange
			var cts = new CancellationTokenSource();
			var taskUrl = "https://zoobazar.by";
			var rawPage = new RawPageEntity { Url = taskUrl, Status = RawPageStatus.Pending };

			// Настраиваем: Очередь возвращает 1 задачу
			_schedulerMock
				.Setup(s => s.GetNextBatchAsync(It.IsAny<int>(), It.IsAny<CancellationToken>()))
				.ReturnsAsync(new List<RawPageEntity> { rawPage });

			// Настраиваем: Загрузчик возвращает успех
			var expectedResult = new DownloadResult(true, "html", 200, null);
			_downloaderMock
				.Setup(d => d.DownloadAsync(taskUrl, It.IsAny<CancellationToken>()))
				.ReturnsAsync(expectedResult);

			// ТРЮК: Как только воркер попытается сохранить результат, мы отменяем токен,
			// чтобы выйти из бесконечного цикла while.
			_schedulerMock
				.Setup(s => s.HandleDownloadResultAsync(rawPage, expectedResult, It.IsAny<CancellationToken>()))
				.Callback(() => cts.Cancel())
				.Returns(Task.CompletedTask);

			// Act
			// Запускаем воркер (он сделает один проход и остановится из-за cts.Cancel())
			try
			{
				await _worker.StartAsync(cts.Token);
				// Ждем завершения, если StartAsync вернул Task, но BackgroundService обычно ждет StopAsync.
				// В данном случае ExecuteAsync внутри StartAsync будет прерван.
				await _worker.ExecuteTask;
			}
			catch (OperationCanceledException)
			{
				// Это ожидаемое поведение при отмене токена
			}

			// Assert
			// Проверяем, что методы были вызваны
			_downloaderMock.Verify(d => d.DownloadAsync(taskUrl, It.IsAny<CancellationToken>()), Times.Once);
			_schedulerMock.Verify(s => s.HandleDownloadResultAsync(rawPage, expectedResult, It.IsAny<CancellationToken>()), Times.Once);
		}

		[Test]
		public async Task ExecuteAsync_WhenDownloaderFails_ShouldSaveErrorResult()
		{
			// Arrange
			var cts = new CancellationTokenSource();
			var rawPage = new RawPageEntity { Url = "http://fail.com" };
			var exception = new Exception("Network crash");

			_schedulerMock
				.Setup(s => s.GetNextBatchAsync(It.IsAny<int>(), It.IsAny<CancellationToken>()))
				.ReturnsAsync(new List<RawPageEntity> { rawPage });

			// Настраиваем: Загрузчик выбрасывает исключение
			_downloaderMock
				.Setup(d => d.DownloadAsync(It.IsAny<string>(), It.IsAny<CancellationToken>()))
				.ThrowsAsync(exception);

			// ТРЮК: Останавливаем цикл при сохранении результата
			_schedulerMock
				.Setup(s => s.HandleDownloadResultAsync(It.IsAny<RawPageEntity>(), It.IsAny<DownloadResult>(), It.IsAny<CancellationToken>()))
				.Callback(() => cts.Cancel())
				.Returns(Task.CompletedTask);

			// Act
			try { await _worker.StartAsync(cts.Token); await _worker.ExecuteTask; }
			catch (OperationCanceledException) { }

			// Assert
			// Проверяем, что воркер НЕ упал, а поймал ошибку и передал ее в Scheduler
			_schedulerMock.Verify(s => s.HandleDownloadResultAsync(
				rawPage,
				It.Is<DownloadResult>(r => r.IsSuccess == false && r.ErrorMessage == exception.Message),
				It.IsAny<CancellationToken>()
			), Times.Once);
		}

		[Test]
		public async Task ExecuteAsync_WhenNoTasks_ShouldWaitAndNotCallDownloader()
		{
			// Arrange
			var cts = new CancellationTokenSource();

			// Очередь пуста
			_schedulerMock
				.Setup(s => s.GetNextBatchAsync(It.IsAny<int>(), It.IsAny<CancellationToken>()))
				.ReturnsAsync(new List<RawPageEntity>());

			// В этом сценарии сложно использовать Callback на методе Handle (т.к. он не вызывается).
			// Поэтому просто даем воркеру поработать немного и отменяем.
			cts.CancelAfter(100);

			// Act
			try { await _worker.StartAsync(cts.Token); await _worker.ExecuteTask; }
			catch (OperationCanceledException) { }

			// Assert
			// Убеждаемся, что загрузчик НЕ вызывался
			_downloaderMock.Verify(d => d.DownloadAsync(It.IsAny<string>(), It.IsAny<CancellationToken>()), Times.Never);
		}

		[TearDown]
		public void TearDown()
		{
			_worker.Dispose();
		}
	}
}