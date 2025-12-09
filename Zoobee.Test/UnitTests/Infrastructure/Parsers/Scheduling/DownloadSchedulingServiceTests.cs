using Microsoft.Extensions.Logging;
using Moq;
using NUnit.Framework; // Главный неймспейс NUnit
using Zoobee.Infrastructure.Parsers.Core.Entities;
using Zoobee.Infrastructure.Parsers.Core.Entities.Zoobee.Infrastructure.Parsers.Core.Entities;
using Zoobee.Infrastructure.Parsers.Core.Enums;
using Zoobee.Infrastructure.Parsers.Interfaces.Services.Downloader;
using Zoobee.Infrastructure.Parsers.Interfaces.Storage;
using Zoobee.Infrastructure.Parsers.Services.Scheduling;

namespace Zoobee.Test.Tests.Infrastructure.Parsers.Scheduling
{
	[TestFixture] // Обозначает класс с тестами
	public class DownloadSchedulingServiceTests
	{
		private Mock<IScrapingRepository> _repoMock;
		private Mock<ILogger<DownloadSchedulingService>> _loggerMock;
		private DownloadSchedulingService _service;

		[SetUp] // Этот метод запускается ПЕРЕД каждым тестом
		public void Setup()
		{
			_repoMock = new Mock<IScrapingRepository>();
			_loggerMock = new Mock<ILogger<DownloadSchedulingService>>();
			_service = new DownloadSchedulingService(_repoMock.Object, _loggerMock.Object);
		}

		[Test]
		public async Task HandleDownloadResultAsync_OnSuccess_ShouldUpdateStatusToDownloaded()
		{
			// Arrange
			var entity = new RawPageEntity { Url = "http://test.com", Status = RawPageStatus.Pending };
			var result = new DownloadResult(true, "<html>ok</html>", 200, null);

			// Act
			await _service.HandleDownloadResultAsync(entity, result, CancellationToken.None);

			// Assert (Используем Constraint Model - это современный стиль NUnit)
			Assert.That(entity.Status, Is.EqualTo(RawPageStatus.Downloaded));
			Assert.That(entity.Content, Is.EqualTo("<html>ok</html>"));
			Assert.That(entity.HttpStatusCode, Is.EqualTo(200));

			_repoMock.Verify(r => r.AddOrUpdateAsync(entity, It.IsAny<CancellationToken>()), Times.Once);
		}

		[Test]
		public async Task HandleDownloadResultAsync_On404_ShouldSetStatusToNotFound()
		{
			// Arrange
			var entity = new RawPageEntity { Url = "http://test.com/missing", Status = RawPageStatus.Pending };
			var result = new DownloadResult(false, null, 404, "Not Found");

			// Act
			await _service.HandleDownloadResultAsync(entity, result, CancellationToken.None);

			// Assert
			Assert.That(entity.Status, Is.EqualTo(RawPageStatus.NotFound));
			Assert.That(entity.NextTryAt, Is.EqualTo(DateTime.MaxValue));

			_repoMock.Verify(r => r.AddOrUpdateAsync(entity, It.IsAny<CancellationToken>()), Times.Once);
		}

		[Test]
		public async Task HandleDownloadResultAsync_OnNetworkError_ShouldScheduleRetry()
		{
			// Arrange
			var entity = new RawPageEntity
			{
				Url = "http://test.com",
				Status = RawPageStatus.Pending,
				AttemptCount = 0
			};
			var result = new DownloadResult(false, null, 0, "Connection Refused");

			// Act
			await _service.HandleDownloadResultAsync(entity, result, CancellationToken.None);

			// Assert
			Assert.That(entity.Status, Is.EqualTo(RawPageStatus.Pending));
			Assert.That(entity.AttemptCount, Is.EqualTo(1));
			Assert.That(entity.NextTryAt, Is.GreaterThan(DateTime.UtcNow));

			_repoMock.Verify(r => r.AddOrUpdateAsync(entity, It.IsAny<CancellationToken>()), Times.Once);
		}

		[Test]
		public async Task HandleDownloadResultAsync_OnMaxRetriesExceeded_ShouldFail()
		{
			// Arrange
			var entity = new RawPageEntity
			{
				Url = "http://test.com",
				Status = RawPageStatus.Pending,
				AttemptCount = 5
			};
			var result = new DownloadResult(false, null, 500, "Server Error");

			// Act
			await _service.HandleDownloadResultAsync(entity, result, CancellationToken.None);

			// Assert
			Assert.That(entity.Status, Is.EqualTo(RawPageStatus.Failed));

			_repoMock.Verify(r => r.AddOrUpdateAsync(entity, It.IsAny<CancellationToken>()), Times.Once);
		}
	}
}