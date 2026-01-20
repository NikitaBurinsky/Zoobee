using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Zoobee.Application.Shared.DTOs.System.Parsing.FailedSaveParsedItemsTask.Components
{
	public enum FailureDetectionSource
	{
		OperationResult,
		Exception,
		Unknown
	}
	public class FailedItemResolvingInfoDto
	{
		public string ResolvedByName { get; set; }
		public DateTime ResolvedAt { get; set; }
		public string ResolutionNotes { get; set; }
	}
}
