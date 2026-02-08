using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using Zoobee.Application.Interfaces.Services.Products.ProductsMapperService.Mapping_Profiles;
using Zoobee.Application.Shared.DTOs.System.Parsing;
using Zoobee.Application.Shared.DTOs.System.Parsing.FailedSaveParsedItemsTask.Components;
using Zoobee.Domain;
using Zoobee.Domain.DataEntities.Identity.Users;
using Zoobee.Infrastructure.Parsers.Core.Entities.Failures.FailedSaveParsedItemTaskEntity;

namespace Zoobee.Application.Mapping_Profiles.System.Parsing
{
	public class FailedParsedToSaveItemDtoMappingProfile : IBaseMappingProfile<FailedToSaveParsedItemTaskDto, FailedToSaveParsedItemTaskEntity>
	{
		/// TODO: заменить на специализированный тип юзера, когда система юзеров будет изменена.
		UserManager<BaseApplicationUser> userManager;
		public FailedParsedToSaveItemDtoMappingProfile(UserManager<BaseApplicationUser> userManager)
		{
			this.userManager = userManager;
		}

		public OperationResult<FailedToSaveParsedItemTaskEntity> Map(FailedToSaveParsedItemTaskDto from)
		{
			Type productType = Type.GetType(from.ProductType); 
			var entity = new FailedToSaveParsedItemTaskEntity
			{
				Id = from.TaskId,
				FailedObject = from.FailedItemType,
				FailedProductJSON = from.ProductJSON,
				FailedSlotJSON = from.SellingSlotJSON,
				FailureData = new FailedItemErrorData
				{
					DetectionSource = from.ErrorData.DetectionSource,
					ParsedFailedUrl = from.ErrorData.ParsedFailedUrl,
					Message = from.ErrorData.Message,
					OccurredAt = from.ErrorData.OccurredAt,
					StackTrace = from.ErrorData.StackTrace
				},
				IsResolved = from.IsResolved,
				ProductType = productType,
				ResolvedInfo = from.ResolvingInfo != null ? new FailedItemResolvingInfo
				{
					ResolvedAt = from.ResolvingInfo.ResolvedAt,
					ResolvedById = from.ResolvingInfo.ResolvedById,
					ResolutionNotes = from.ResolvingInfo.ResolutionNotes
				} : null,
			};
			return OperationResult<FailedToSaveParsedItemTaskEntity>.Success(entity);
		}

		public virtual OperationResult<FailedToSaveParsedItemTaskDto> RevMap(FailedToSaveParsedItemTaskEntity from)
		{
			if (from == null)
			{
				return OperationResult<FailedToSaveParsedItemTaskDto>.Error(
					$"Error.FailedToSaveParsedItem.Mapping.EntityToMapMustBeNotNull", HttpStatusCode.BadRequest);
			}

			string productTypeString = from.ProductType?.FullName ?? string.Empty;

			var entity =  new FailedToSaveParsedItemTaskDto
			{
				TaskId = from.Id,
				FailedItemType = from.FailedObject,
				ProductJSON = from.FailedProductJSON ?? string.Empty,
				SellingSlotJSON = from.FailedSlotJSON ?? string.Empty,
				ErrorData = new FailedItemErrorDataDto
				{
					DetectionSource = from.FailureData.DetectionSource,
					ParsedFailedUrl = from.FailureData.ParsedFailedUrl,
					Message = from.FailureData.Message,
					OccurredAt = from.FailureData?.OccurredAt ?? DateTime.UtcNow,
					StackTrace = from.FailureData?.StackTrace
				},
				IsResolved = from.IsResolved,
				ProductType = productTypeString,
				ResolvingInfo = from.ResolvedInfo != null  ? new FailedItemResolvingInfoDto
				{
					ResolvedAt = from.ResolvedInfo.ResolvedAt.Value,
					ResolvedById = from.ResolvedInfo.ResolvedById,
					ResolutionNotes = from.ResolvedInfo.ResolutionNotes
				} : null
			};
			return OperationResult<FailedToSaveParsedItemTaskDto>.Success(entity);
		}
	}
}
