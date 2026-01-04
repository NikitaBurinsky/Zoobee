using Microsoft.Extensions.Logging;
using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Zoobee.Application.DTOs.Business_Items.Sellings;
using Zoobee.Application.DTOs.Products.Base;
using Zoobee.Application.DTOs.Products.Types;
using Zoobee.Application.Interfaces.Repositories.UnitsOfWork;
using Zoobee.Application.Interfaces.Services.Products.Catalog;
using Zoobee.Application.Interfaces.Services.Products.Catalog.ProductsInfoService;
using Zoobee.Application.Interfaces.Services.Products.ProductsStorage;
using Zoobee.Application.Interfaces.Services.ProductTypeRegistry;
using Zoobee.Domain;
using Zoobee.Domain.DataEntities.Products;
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

		private readonly IProductsInfoService productsInfoService;
		private readonly ISellingSlotsInfoService sellingSlotsInfoService;
		private readonly IProductTypeRegistryService _productRegistry;

		public TransformationService(IScrapingRepository scrapingRepository,
			IProductsUnitOfWork productsUnitOfWork, ITransformerResolver transformerResolver,
			ILogger<TransformationService> logger, IProductsStorageService productsStorageService,
			IProductsInfoService productsInfoService, ISellingSlotsInfoService sellingSlotsInfoService,
			IProductTypeRegistryService productRegistry)
		{
			_scrapingRepository = scrapingRepository;
			_productsUnitOfWork = productsUnitOfWork;
			_transformerResolver = transformerResolver;
			_logger = logger;
			this.productsStorageService = productsStorageService;
			this.productsInfoService = productsInfoService;
			this.sellingSlotsInfoService = sellingSlotsInfoService;
			_productRegistry = productRegistry;
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
						if (result.ExtractedData.ProductInfo != null || result.ExtractedData.ProductSlot != null)
						{
							await SaveExtractedDataAsync(result.ExtractedData.ProductInfo, result.ExtractedData.ProductSlot, ct);
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

		private async Task SaveExtractedDataAsync(BaseProductDto productData, SellingSlotDto slotData, CancellationToken ct)
		{

			//TODO 
			//В будущем, добавим отдельные хранилища для обьектов, которые распарсились, но не удалось сохранить
			if (productData != null)
			{
				var productType = productData.GetType();
				try
				{
					var mapping = _productRegistry.GetMappingOrDefault(productType);

					var method = typeof(IProductsInfoService).GetMethod(nameof(IProductsInfoService.UpdateOrAddProductInfo));
					var genericMethod = method.MakeGenericMethod(mapping.EntityType, mapping.DtoType);

					var res = (OperationResult)genericMethod.Invoke(productsInfoService, new object[] { productData, slotData.SellingUrl });
					
					if (res.Succeeded)
						_logger.LogInformation("Saved product information: {@ProductData}", productData);
					else
						_logger.LogError("Failed to save selling slot: {@ProductData}.\nError: {@Res}", productData, res);

					if (productType == typeof(BaseProductDto))
					{
						_logger.LogWarning("Upserted Generic Product (Type Unknown): {Name}", productData.Name);
					}
					else
					{
						_logger.LogInformation("Upserted {ProductType}: {Name}",
							productType.Name.Replace("ProductDto", ""),
							productData.Name);
					}
				}
				catch (Exception ex)
				{
					_logger.LogError(ex, "Failed to save product {ProductName} of type {ProductType}",
						productData.Name, productType.Name);
					throw;
				}
				finally
				{
					if(slotData != null)
					{
						var res = sellingSlotsInfoService.MatchAndSaveSellingSlot(slotData);
						if (res.Succeeded)
							_logger.LogInformation("Saved selling slot: {@SlotData}", slotData);
						else
							_logger.LogError("Failed to save selling slot: {@SlotData}.\nError: {@Res}", slotData, res);
					}
				}
			}
		}
	}
}