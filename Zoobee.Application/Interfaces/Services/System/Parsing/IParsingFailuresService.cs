using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Zoobee.Application.Shared.DTOs.Business_Items.Sellings;
using Zoobee.Application.Shared.DTOs.Products.Base;
using Zoobee.Application.Shared.DTOs.System.Parsing;
using Zoobee.Domain;

namespace Zoobee.Application.Interfaces.Services.System.Parsing
{
	public interface IParsingFailuresService
	{
		/// <summary>
		/// Creates new FailedToSaveParsedItem with failed product.
		/// </summary>
		public Task<OperationResult> CreateResolvationTask_FailedProductAsync(BaseProductDto info, string failedUrl, OperationResult productSaveRes, Exception exception = null);

		/// <summary>
		/// Creates new FailedToSaveParsedItem with failed slot
		/// </summary>
		public Task<OperationResult> CreateResolvationTask_FailedSlotAsync(SellingSlotDto info, string failedUrl, OperationResult slotSaveRes,BaseProductDto productInfo,Exception exception = null);

		/// <summary>
		/// Creates new FailedToSaveParsedItem with failed slot and product
		/// </summary>
		public Task<OperationResult> CreateResolvationTask_FailedBothAsync(SellingSlotDto slotInfo, BaseProductDto productInfo, string failedUrl,
			OperationResult slotRes, OperationResult productRes,
			Exception slotEx = null, Exception productEx = null);

		public Task<IEnumerable<FailedToSaveParsedItemTaskDto>> GetAllUnresolvedAsync(int pageNum, int pageSize);
		public Task<OperationResult<FailedToSaveParsedItemTaskDto>> GetByIdAsync(Guid id);
		/// <summary>
		/// Обновить JSON (админ исправил руками) и попробовать процессить снова.
		/// Если успешно - удаляет таску из списка неудачных.
		/// </summary>
		public Task<OperationResult> UpdateRetryTaskAsync(Guid taskId, string? correctedProductJson, string? correctedSlotJson, string? resolutionNotes, Guid resolvedBy);
		Task<OperationResult> DeleteTaskAsync(Guid id);
	}
}
