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
		/// <summary>
		/// Пока не имеем UserDto в приложении, используем guid для идентификации пользователя, который решил проблему.
		/// Система юзеров будет добавлена позже, там введем более удобную ссылку на пользователя.
		/// TODO: заменить на ссылку на пользователя, когда система юзеров будет добавлена.
		/// </summary>
		public Guid? ResolvedById { get; set; }
		public DateTime? ResolvedAt { get; set; }
		public string? ResolutionNotes { get; set; }
	}
}
