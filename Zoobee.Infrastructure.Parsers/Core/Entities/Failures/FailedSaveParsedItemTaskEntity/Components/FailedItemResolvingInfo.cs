using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Zoobee.Infrastructure.Parsers.Core.Entities.Failures.FailedSaveParsedItemTaskEntity
{
	public class FailedItemResolvingInfo
	{
		public Guid? ResolvedById { get; set; }
		public DateTime? ResolvedAt { get; set; }
		public string? ResolutionNotes { get; set; }
	}
}
