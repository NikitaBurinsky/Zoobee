using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Zoobee.Application.Shared.DTOs.Business_Items.Base;
using Zoobee.Application.Shared.DTOs.System.Parsing.FailedSaveParsedItemsTask.Components;

namespace Zoobee.Application.Shared.DTOs.System.Parsing
{
	public enum FailedItemType
	{
		Product,
		SellingSlot,
		Both
	}
	public class FailedToSaveParsedItemTaskDto : BaseEntityItemDto
	{
		public Guid TaskId { get; set; }
		public string? ProductJSON { get; set; }
		public string? ProductType { get; set; }
		public string? SellingSlotJSON { get; set; }
		public FailedItemType FailedItemType { get; set; }
		public FailedItemErrorDataDto ErrorData { get; set; }
		public bool IsResolved { get; set; }
		/// <summary>
		/// Resolving information, only if the failed item has been resolved and IsResolved flag is true.
		/// If the item is not resolved, this field will be null.
		/// </summary>
		public FailedItemResolvingInfoDto? ResolvingInfo { get; set; }
	}
}
