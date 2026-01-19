using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Zoobee.Application.DtoTypes.Base;
using Zoobee.Domain.DataEntities.Products;

namespace Zoobee.Application.DTOs.System.Parsing.FailedSaveTasks
{
	public class FailedToSaveParsedProductInfoDto
	{
		public Guid Id { get; set; }
		public bool IsResolved { get; set; }
		public DateTime? ResolvedAt { get; set; }
		public Guid? ResolvedById { get; set; }
		public string FailSignalType { get; set; } // Exception, OperationResult
		public string? ExceptionType { get; set; } // "DbUpdateException", "ValidationException"
		public string Message { get; set; } // Ограничьте длину (500-1000 символов)
		public string? StackTrace { get; set; }
		public BaseProductEntity? FailedProduct { get; set; }
	}
}
