using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Zoobee.Infrastructure.Parsers.Core.Entities.Failures
{
	public class FailedItemErrorData
	{
		public FailureDetectionSource DetectionSource { get; set; }
		public string? ParsedFailedUrl { get; set; }
		public DateTime? OccurredAt { get; set; }
		public string? Message { get; set; }
		public string? StackTrace { get; set; }
	}
}
