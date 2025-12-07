using Microsoft.Extensions.Localization;
using Zoobee.Application.DTOs.Products.Types;
using Zoobee.Application.Interfaces.Repositories.Catalog;
using Zoobee.Application.Interfaces.Repositories.UnitsOfWork.Environtment;
using Zoobee.Core.Errors;
using Zoobee.Domain;
using Zoobee.Domain.DataEntities.Products.ToiletProductEntity;

namespace Zoobee.Application.DTOs.Mapping_Profiles.Products
{
	public class ToiletProductDtoMappingProfile : BaseProductDtoMappingProfile<ToiletProductDto, ToiletProductEntity>
	{
		public ToiletProductDtoMappingProfile(IStringLocalizer<Errors> localizer,
			IEnvirontmentDataUnitOfWork envUOW,
			ITagsRepository tagsRepository) :
			base(localizer, envUOW, tagsRepository)
		{
		}

		public override OperationResult<ToiletProductEntity> Map(ToiletProductDto from)
		{
			var entity = new ToiletProductEntity();
			var res = SetBaseProductFields(entity, from);
			if (res.Failed)
				return OperationResult<ToiletProductEntity>.Error(res.Message, res.ErrCode);
			entity.ToiletType = from.ToiletType;
			entity.Dimensions = from.SizeDimensions;
			entity.VolumeLiters = from.VolumeLiters;
			entity.PetAgeRange = from.petAgeRange;
			return OperationResult<ToiletProductEntity>.Success(entity);
		}

		public OperationResult<ToiletProductDto> RevMap(ToiletProductEntity from)
		{
			var dto = new ToiletProductDto
			{
				ToiletType = from.ToiletType,
				SizeDimensions = from.Dimensions,
				VolumeLiters = from.VolumeLiters,
				petAgeRange = from.PetAgeRange,
			};
			var res = SetBaseProductFields(dto, from);
			return res.Succeeded ?
				OperationResult<ToiletProductDto>.Success(dto) :
				OperationResult<ToiletProductDto>.Error(res.Message, res.ErrCode);
		}
	}
}
