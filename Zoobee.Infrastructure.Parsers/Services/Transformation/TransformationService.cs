using Microsoft.Extensions.Logging;
using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Zoobee.Application.DTOs.Products.Base;
using Zoobee.Application.DTOs.Products.Types;
using Zoobee.Application.Interfaces.Repositories.UnitsOfWork;
using Zoobee.Application.Interfaces.Services.Products.ProductsStorage;
using Zoobee.Domain.DataEntities.Products;
using Zoobee.Domain.DataEntities.Products.FoodProductEntity;
using Zoobee.Domain.DataEntities.Products.ToiletProductEntity;
using Zoobee.Infrastructure.Parsers.Core.Enums;
using Zoobee.Infrastructure.Parsers.Core.Transformation;
using Zoobee.Infrastructure.Parsers.Interfaces.Repositories;
using Zoobee.Infrastructure.Parsers.Interfaces.Services.Transformation;
using Zoobee.Infrastructure.Parsers.Interfaces.Storage;
using Zoobee.Infrastructure.Parsers.Interfaces.Transformation;

namespace Zoobee.Infrastructure.Parsers.Services.Transformation
{
	public class TransformationService : ITransformationService
	{
		private readonly IScrapingRepository _scrapingRepository;
		private readonly IProductsUnitOfWork _productsUnitOfWork;
		private readonly ITransformerResolver _transformerResolver;
		private readonly ILogger<TransformationService> _logger;
		private readonly IProductsStorageService productsStorageService;

		public TransformationService(
			IScrapingRepository scrapingRepository,
			IProductsUnitOfWork productsUnitOfWork,
			ITransformerResolver transformerResolver,
			IProductsStorageService productsStorage,
			ILogger<TransformationService> logger)
		{
			_scrapingRepository = scrapingRepository;
			_productsUnitOfWork = productsUnitOfWork;
			_transformerResolver = transformerResolver;
			_logger = logger;
			productsStorageService = productsStorage;
		}

		public async Task ProcessPendingDataAsync(CancellationToken ct)
		{
			// 1. Получаем пачку данных, которые скачаны (RawPageStatus.Success), но еще не трансформированы
			// Предполагаем метод GetUntransformedTasksAsync в репозитории
			var pendingTasks = await _scrapingRepository.GetPendingTransformationTasksAsync(batchSize: 10, ct);

			if (!pendingTasks.Any()) return;

			foreach (var (task, content) in pendingTasks)
			{
				if (ct.IsCancellationRequested) break;

				try
				{
					// 2. Находим нужный трансформер (Фасад для сайта)
					var transformer = _transformerResolver.GetTransformer(task.SourceName, task.Type);

					if (transformer == null)
					{
						_logger.LogWarning("Transformer not found for source {Source}. Skipping.", task.SourceName);
						continue;
					}

					// 3. Запускаем трансформацию, передавая тип задачи
					var result = await transformer.TransformAsync(content, task.Url, task.Type);

					if (result.IsSuccess)
					{
						// 4. Обработка найденных ссылок (Seeding новых задач)
						if (result.NewTasks.Any())
						{
							await _scrapingRepository.BulkAddTasksAsync(result.NewTasks, task.SourceName, ct);
							_logger.LogInformation("Added {Count} new tasks from {Url}", result.NewTasks.Count, task.Url);
						}

						// 5. Обработка извлеченных данных (Полиморфное сохранение)
						if (result.ExtractedData.ProductInfo != null && result.ExtractedData.ProductSlot != null)
						{
							await SaveExtractedDataAsync(result.ExtractedData, ct);
						}

						// 6. Помечаем, что данные успешно обработаны (чтобы не брать их снова)
						// Например, ставим флаг в таблице ScrapingData или меняем статус Task
						await _scrapingRepository.MarkAsTransformedAsync(task.Id, ct);
					}
					else
					{
						_logger.LogWarning("Transformation failed for {Url}: {Error}", task.Url, result.ErrorMessage);
						// Логика обработки ошибок (Dead Letter Queue или Retry)
					}
				}
				catch (Exception ex)
				{
					_logger.LogError(ex, "Critical error processing task {Url}", task.Url);
				}
			}
		}

		//TODO Разбить по типам товаров
		private async Task SaveExtractedDataAsync(object data, CancellationToken ct)
		{
			// Используем Pattern Matching для маршрутизации по репозиториям
			switch (data)
			{
				case FoodProductDto food:
					await productsStorageService.CreateProductAndSave<FoodProductDto, FoodProductEntity>(food);
					_logger.LogInformation("Upserted Food: {Name}", food.Name);
					break;

				case ToiletProductDto toilet:
					await productsStorageService.CreateProductAndSave<ToiletProductDto, ToiletProductEntity>(toilet);
					_logger.LogInformation("Upserted Toilet: {Name}", toilet.Name);
					break;

				case BaseProductDto baseProd:
					// Если специфичный тип не определен, сохраняем в общую таблицу (если бизнес-логика позволяет)
					await productsStorageService.CreateProductAndSave<BaseProductDto, BaseProductEntity>(baseProd);
					_logger.LogWarning("Upserted Generic Product (Type Unknown): {Name}", baseProd.Name);
					break;

				default:
					_logger.LogWarning("Unknown data type extracted: {Type}. No repository mapped.", data.GetType().Name);
					break;
			}
		}
	}
}