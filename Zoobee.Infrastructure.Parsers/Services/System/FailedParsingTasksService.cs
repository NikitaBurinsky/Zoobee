using Microsoft.Extensions.Logging;
using System.Net;
using System.Text.Json;
using Zoobee.Application.Interfaces.Repositories.IQueriableExtensions;
using Zoobee.Application.Interfaces.Services.MappingService;
using Zoobee.Application.Interfaces.Services.Products.Catalog;
using Zoobee.Application.Interfaces.Services.Products.Catalog.ProductsInfoService;
using Zoobee.Application.Interfaces.Services.System.Parsing;
using Zoobee.Application.Shared.DTOs.Business_Items.Sellings;
using Zoobee.Application.Shared.DTOs.Products.Base;
using Zoobee.Application.Shared.DTOs.System.Parsing;
using Zoobee.Application.Shared.DTOs.System.Parsing.FailedSaveParsedItemsTask.Components;
using Zoobee.Domain;
using Zoobee.Infrastructure.Parsers.Core.Entities.Failures.FailedSaveParsedItemTaskEntity;
using Zoobee.Infrastructure.Parsers.Interfaces.Repositories;

namespace Zoobee.Infrastructure.Parsers.Services
{
	public class FailedParsingTasksService : IParsingFailuresService
	{
		private readonly IFailedToSaveParsedTasksRepository _repository;
		private readonly ILogger<FailedParsingTasksService> _logger;
		private readonly IProductsInfoService _productsInfoService;
		private readonly ISellingSlotsInfoService _sellingSlotsInfoService;
		private readonly IMappingService mapper;

		public FailedParsingTasksService(
			IFailedToSaveParsedTasksRepository repository,
			IMappingService mapper,
			ILogger<FailedParsingTasksService> logger,
			IProductsInfoService productsInfoService,
			ISellingSlotsInfoService sellingSlotsInfoService)
		{
			_repository = repository;
			this.mapper = mapper;
			_logger = logger;
			_productsInfoService = productsInfoService;
			_sellingSlotsInfoService = sellingSlotsInfoService;
		}

		/// <summary>
		/// Gets all unresolved failed parsing tasks with pagination.
		/// </summary>
		public async Task<IEnumerable<FailedToSaveParsedItemTaskDto>> GetAllUnresolvedAsync(int pageNum, int pageSize)
		{
			try
			{
				if (pageNum < 1)
					pageNum = 1;
				if (pageSize < 1)
					pageSize = 10;

				var tasks = _repository.GetAllPendingResolve()
					.OrderByDescending(f => f.Metadata.CreatedAt)
					.Paginate(pageNum, pageSize)
					.ToList();

				var dtos = tasks.Select(MapEntityToDto).Where(t => t != null).ToList();

				_logger.LogInformation("Retrieved {Count} unresolved failed tasks for page {PageNum}", dtos.Count, pageNum);

				return dtos;
			}
			catch (Exception ex)
			{
				_logger.LogError(ex, "Error retrieving unresolved failed tasks");
				throw;
			}
		}

		/// <summary>
		/// Gets a specific failed parsing task by ID.
		/// </summary>
		public async Task<OperationResult<FailedToSaveParsedItemTaskDto>> GetByIdAsync(Guid id)
		{
			try
			{
				if (id == Guid.Empty)
					return null;

				var entity = _repository.Get(id);
				if (entity == null)
				{
					_logger.LogWarning("Failed task with ID {TaskId} not found", id);
					return null;
				}

				var mapres = mapper.RevMap<FailedToSaveParsedItemTaskDto, FailedToSaveParsedItemTaskEntity>(entity);
				if (mapres.Failed)
					return OperationResult<FailedToSaveParsedItemTaskDto>.Error(mapres);
				return await Task.FromResult(mapres);
			}
			catch (Exception ex)
			{
				_logger.LogError(ex, "Error retrieving failed task with ID {TaskId}", id);
				throw;
			}
		}

		/// <summary>
		/// Updates a failed task with corrected JSON data and attempts to retry processing.
		/// If successful, removes the task from the failed list.
		/// </summary>
		public async Task<OperationResult> UpdateRetryTaskAsync(Guid taskId, string? correctedProductJson, string? correctedSlotJson, string? resolutionNotes,Guid resolvedBy)
		{
			try
			{
				if (taskId == Guid.Empty)
					return OperationResult.Error("Error.FailedParsedTasks.InvalidTaskId", HttpStatusCode.BadRequest);

				var failedTask = _repository.Get(taskId);
				if (failedTask == null)
					return OperationResult.Error("Error.FailedParsedTasks.TaskNotFound", HttpStatusCode.NotFound);

				// Update JSON data if provided
				if (!string.IsNullOrWhiteSpace(correctedProductJson))
					failedTask.FailedProductJSON = correctedProductJson;

				if (!string.IsNullOrWhiteSpace(correctedSlotJson))
					failedTask.FailedSlotJSON = correctedSlotJson;

				// Attempt to deserialize and process the corrected data
				var processResult = await TryProcessFailedTaskAsync(failedTask);

				if (processResult.Succeeded)
				{
					_repository.SetAsResolved(failedTask, resolutionNotes,resolvedBy);
					_logger.LogInformation("Successfully reprocessed failed task {TaskId}", taskId);
					return OperationResult.Success();
				}
				else
				{
					_logger.LogWarning("Reprocessing failed task {TaskId} failed: {Error}", taskId, processResult.Message);
					return OperationResult.Error("Error.FailedParsedTasks.RetryProcessingFailed", System.Net.HttpStatusCode.InternalServerError);
				}
			}
			catch (Exception ex)
			{
				_logger.LogError(ex, "Error updating and retrying failed task {TaskId}", taskId);
				return OperationResult.Error("Error.FailedParsedTasks.UpdateRetryError", System.Net.HttpStatusCode.InternalServerError);
			}
		}

		/// <summary>
		/// Deletes a failed parsing task.
		/// </summary>
		public async Task<OperationResult> DeleteTaskAsync(Guid id)
		{
			try
			{
				if (id == Guid.Empty)
					return OperationResult.Error("Error.FailedParsedTasks.InvalidTaskId", HttpStatusCode.BadRequest);

				var failedTask = _repository.Get(id);
				if (failedTask == null)
					return OperationResult.Error("Error.FailedParsedTasks.TaskNotFound", HttpStatusCode.NotFound);

				_repository.Delete(failedTask.Id);

				_logger.LogInformation("Successfully deleted failed task {TaskId}", id);
				return await Task.FromResult(OperationResult.Success());
			}
			catch (Exception ex)
			{
				_logger.LogError(ex, "Error deleting failed task {TaskId}", id);
				return OperationResult.Error("Error.FailedParsedTasks.DeleteError", HttpStatusCode.InternalServerError);
			}
		}

		/// <summary>
		/// Gets the latest N failed tasks.
		/// </summary>
		public IEnumerable<FailedToSaveParsedItemTaskDto> GetLatestFailedTasks(int count)
		{
			try
			{
				var tasks = _repository.GetLatest(count);
				return tasks.Select(MapEntityToDto).Where(t => t != null).ToList();
			}
			catch (Exception ex)
			{
				_logger.LogError(ex, "Error retrieving latest failed tasks");
				throw;
			}
		}

		/// <summary>
		/// Gets the oldest N failed tasks (oldest first).
		/// </summary>
		public IEnumerable<FailedToSaveParsedItemTaskDto> GetOldestFailedTasks(int count)
		{
			try
			{
				var tasks = _repository.GetOldest(count);
				return tasks.Select(MapEntityToDto).Where(t => t != null).ToList();
			}
			catch (Exception ex)
			{
				_logger.LogError(ex, "Error retrieving oldest failed tasks");
				throw;
			}
		}

		private FailedToSaveParsedItemTaskDto? MapEntityToDto(FailedToSaveParsedItemTaskEntity t)
		{
			var res = mapper.RevMap<FailedToSaveParsedItemTaskDto, FailedToSaveParsedItemTaskEntity>(t);
			if (res.Succeeded)
				return res.Returns;
			else
			{
				_logger.LogWarning("Some FailedSaveParsedTasks cannot be mapped. OperationResult: {@Res}", res);
				return null;
			}
		}

		/// <summary>
		/// Gets count of unresolved failed tasks.
		/// </summary>
		public int GetUnresolvedFailedTasksCount()
		{
			try
			{
				return _repository.GetAllPendingResolve().Count();
			}
			catch (Exception ex)
			{
				_logger.LogError(ex, "Error counting unresolved failed tasks");
				throw;
			}
		}

		/// <summary>
		/// Gets failed tasks filtered by type (Product, SellingSlot).
		/// </summary>
		public async Task<IEnumerable<FailedToSaveParsedItemTaskDto>> GetFailedTasksByType(FailedItemType taskType)
		{
			try
			{
				IQueryable<FailedToSaveParsedItemTaskEntity> query = taskType switch
				{
					FailedItemType.Product => _repository.GetPendingFailedProducts(),
					FailedItemType.SellingSlot => _repository.GetPendingFailedSellingSlots(),
					_ => _repository.GetAllPendingResolve()
				};

				return query.Select(t => MapEntityToDto(t)).ToList();
			}
			catch (Exception ex)
			{
				_logger.LogError(ex, "Error filtering failed tasks by type {Type}", taskType);
				throw;
			}
		}

		/// <summary>
		/// Attempts to process a failed task by deserializing and saving the JSON data.
		/// </summary>
		private async Task<OperationResult> TryProcessFailedTaskAsync(FailedToSaveParsedItemTaskEntity failedTask)
		{
			try
			{
				// Try to deserialize product JSON
				if (!string.IsNullOrWhiteSpace(failedTask.FailedProductJSON))
				{
					var method = typeof(JsonSerializer)
						.GetMethod(nameof(JsonSerializer.Deserialize), new[] { typeof(string), typeof(JsonSerializerOptions) });

					var genericMethod = method.MakeGenericMethod(failedTask.ProductType);
					var productData = genericMethod.Invoke(null, new object[] { failedTask.FailedProductJSON, null });

					if (productData != null)
					{
						var result = _productsInfoService.UpdateOrAddProductInfoOfType((BaseProductDto)productData, failedTask.ProductType, failedTask.FailureData.ParsedFailedUrl);
						if (!result.Succeeded)
							return result;
					}
				}

				// Try to deserialize selling slot JSON
				if (!string.IsNullOrWhiteSpace(failedTask.FailedSlotJSON))
				{
					var slotData = JsonSerializer.Deserialize<SellingSlotDto>(failedTask.FailedSlotJSON);
					if (slotData != null)
					{
						var result = _sellingSlotsInfoService.MatchAndSaveSellingSlot(slotData);
						if (!result.Succeeded)
							return result;
					}
				}
				return OperationResult.Success();
			}
			catch (JsonException ex)
			{
				_logger.LogError(ex, "JSON deserialization error for failed task");
				return OperationResult.Error("Error.FailedParsedTasks.JsonDeserializationError", System.Net.HttpStatusCode.BadRequest);
			}
			catch (Exception ex)
			{
				_logger.LogError(ex, "Error processing failed task");
				return OperationResult.Error("Error.FailedParsedTasks.ProcessingError", System.Net.HttpStatusCode.InternalServerError);
			}
		}


		//TODO: Зарефакторить, много повторяжющегося кода
		public async Task<OperationResult> CreateResolvationTask_FailedProductAsync(BaseProductDto info, string failedUrl, OperationResult productSaveRes, Exception exception = null)
		{
			if (info == null)
				return OperationResult.Error("Error.FailedSaveParsedTasks.CreateTask.FailedProduct.ProductInfoMustBeNotNull", HttpStatusCode.BadRequest);
			if (string.IsNullOrEmpty(failedUrl))
				return OperationResult.Error("Error.FailedSaveParsedTasks.CreateTask.FailedProduct.FailedMustBeNotNullOrEmpty", HttpStatusCode.BadRequest);

			_logger.LogInformation("Создается задание по обработке ошибки парсинга ПРОДУКТА. Url: {FailedUrl},\n DetectionSource: {Type},\n Message: {Message}, StackTrace: {StackTrace}", failedUrl,
				productSaveRes != null ? productSaveRes.GetType().Name : exception.GetType().Name,
				productSaveRes != null ? productSaveRes.Message : exception.Message,
				exception != null ? exception.StackTrace : "");

			FailedToSaveParsedItemTaskEntity failure = new FailedToSaveParsedItemTaskEntity
			{
				ResolvedInfo = null,
				IsResolved = false,
				FailureData = CreateFailedItemData(failedUrl, productSaveRes, exception),
				FailedSlotJSON = null,
				FailedObject = FailedItemType.Product,
				ProductType = info.GetType(),
			};
			failure.FailedProductJSON = JsonSerializer.Serialize(info);

			return OperationResult.Success();
		}

		private FailedItemErrorData CreateFailedItemData(string url, OperationResult res, Exception ex)
		{
			var FailureData = new FailedItemErrorData()
			{
				ParsedFailedUrl = url,
				DetectionSource = res != null ? FailureDetectionSource.OperationResult : FailureDetectionSource.Exception,
				Message = res != null ? res.Message : ex.Message,
				OccurredAt = DateTime.UtcNow,
				StackTrace = ex?.StackTrace ?? null
			};
			return FailureData;
		}

		public async Task<OperationResult> CreateResolvationTask_FailedSlotAsync(SellingSlotDto info, string failedUrl, OperationResult slotSaveRes, BaseProductDto productInfo, Exception exception = null)
		{
			if (info == null)
				return OperationResult.Error("Error.FailedSaveParsedTasks.CreateTask.FailedSellingSlot.SellingSlotMustBeNotNull", HttpStatusCode.BadRequest);
			if (string.IsNullOrEmpty(failedUrl))
				return OperationResult.Error("Error.FailedSaveParsedTasks.CreateTask.FailedSellingSlot.FailedMustBeNotNullOrEmpty", HttpStatusCode.BadRequest);

			_logger.LogInformation("Создается задание по обработке ошибки парсинга СЛОТА. Url: {FailedUrl},\n DetectionSource: {Type},\n Message: {Message}, StackTrace: {StackTrace}", failedUrl,
				slotSaveRes != null ? slotSaveRes.GetType().Name : exception.GetType().Name,
				slotSaveRes != null ? slotSaveRes.Message : exception.Message,
				exception != null ? exception.StackTrace : "");

			FailedToSaveParsedItemTaskEntity failure = new FailedToSaveParsedItemTaskEntity
			{
				ResolvedInfo = null,
				IsResolved = false,
				FailureData = CreateFailedItemData(failedUrl, slotSaveRes, exception),
				FailedProductJSON = null,
				FailedObject = FailedItemType.SellingSlot,
				ProductType = info.GetType(),
			};
			if(productInfo != null) 
				failure.FailedProductJSON = JsonSerializer.Serialize(productInfo);
			failure.FailedSlotJSON = JsonSerializer.Serialize(info);

			return OperationResult.Success();
		}

		public async Task<OperationResult> CreateResolvationTask_FailedBothAsync(SellingSlotDto slotInfo, BaseProductDto productInfo, string failedUrl, OperationResult slotRes, OperationResult productRes, Exception slotEx = null, Exception productEx = null)
		{

			if (productInfo == null)
				return OperationResult.Error("Error.FailedSaveParsedTasks.CreateTask.FailedBoth.ProductInfoMustBeNotNull", HttpStatusCode.BadRequest);
			if (slotInfo == null)
				return OperationResult.Error("Error.FailedSaveParsedTasks.CreateTask.FailedBoth.SellingSlotInfoMustBeNotNull", HttpStatusCode.BadRequest);
			if (string.IsNullOrEmpty(failedUrl))
				return OperationResult.Error("Error.FailedSaveParsedTasks.CreateTask.FailedBoth.FailedMustBeNotNullOrEmpty", HttpStatusCode.BadRequest);

			_logger.LogInformation("Создается задание по обработке ошибки парсинга СЛОТА и ПРОДУКТА. ||| SlotUrl |||:\n {FailedUrl},\n DetectionSource: {Type},\n Message: {Message}, StackTrace: {StackTrace}", failedUrl,
				slotRes != null ? slotRes.GetType().Name : slotEx.GetType().Name,
				slotRes != null ? slotRes.Message : slotEx.Message,
				slotEx != null ? slotEx.StackTrace : "");

			FailedToSaveParsedItemTaskEntity failure = new FailedToSaveParsedItemTaskEntity
			{
				ResolvedInfo = null,
				IsResolved = false,
				FailureData = new FailedItemErrorData()
				{
					ParsedFailedUrl = failedUrl,
					DetectionSource = slotRes != null || productRes != null ? FailureDetectionSource.OperationResult : FailureDetectionSource.Exception,
					Message = slotRes != null  || productRes != null ? slotRes.Message + " \n\n" + productRes.Message : slotEx.Message + "\n\n" + productEx.Message,
					OccurredAt = DateTime.UtcNow,
					StackTrace = slotEx != null || productEx != null ?
					slotEx.StackTrace + "\n\n" + productEx.StackTrace :
					null
				},
				FailedProductJSON = null,
				FailedObject = FailedItemType.SellingSlot,
				ProductType = productInfo.GetType(),
			};
			if (productInfo != null)
				failure.FailedProductJSON = JsonSerializer.Serialize(productInfo);
			failure.FailedSlotJSON = JsonSerializer.Serialize(slotInfo);

			return OperationResult.Success();
		}
	}
}
