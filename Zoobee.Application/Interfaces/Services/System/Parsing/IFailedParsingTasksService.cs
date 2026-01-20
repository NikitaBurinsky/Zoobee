using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Zoobee.Application.Shared.DTOs.System.Parsing;
using Zoobee.Domain;

namespace Zoobee.Application.Interfaces.Services.System.Parsing
{
	public interface IFailedParsingTasksService
	{
		Task<IEnumerable<FailedSaveParsedItemsTaskDto>> GetAllUnresolvedAsync(int pageNum, int pageSize);
		Task<FailedSaveParsedItemsTaskDto> GetByIdAsync(Guid id);
		/// <summary>
		/// Обновить JSON (админ исправил руками) и попробовать процессить снова.
		/// Если успешно - удаляет таску из списка неудачных.
		/// </summary>
		Task<OperationResult> UpdateRetryTaskAsync(Guid taskId, string correctedProductJson, string correctedSlotJson);
		Task DeleteTaskAsync(Guid id);



	}
}
