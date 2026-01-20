using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Zoobee.Application.Shared.DTOs.System.Parsing.FailedSaveParsedItemsTask.Components
{
	public class FailedItemErrorDataDto
	{
		public FailureDetectionSource DetectionSource { get; set; }
		public string? ParsedFailedUrl { get; set; }
		public DateTime OccurredAt { get; set; }
		public string? Message { get; set; }
		public string? StackTrace { get; set; }
	}
}
