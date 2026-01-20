using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Zoobee.Infrastructure.Parsers.Core.Entities.Failures
{
	public enum FailureDetectionSource
	{
		OperationResult,
		Exception,
		Unknown
	}
	public class FailedItemResolvingInfo
	{
		public string? ResolvedByName { get; set; }
		public DateTime? ResolvedAt { get; set; }
		public string? ResolutionNotes { get; set; }
	}
}
