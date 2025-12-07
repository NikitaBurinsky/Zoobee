using Microsoft.Extensions.Localization;
using Zoobee.Application.DTOs.Products.Types;
using Zoobee.Application.Interfaces.Repositories.Catalog;
using Zoobee.Application.Interfaces.Repositories.UnitsOfWork.Environtment;
using Zoobee.Core.Errors;
using Zoobee.Domain;
using Zoobee.Domain.DataEntities.Products.FoodProductEntity;

namespace Zoobee.Application.DTOs.Mapping_Profiles.Products
{
	public class FoodProductDtoMappingProfile : BaseProductDtoMappingProfile<FoodProductDto, FoodProductEntity>
	{
		public FoodProductDtoMappingProfile(IStringLocalizer<Errors> localizer,
			IEnvirontmentDataUnitOfWork envUOW,
			ITagsRepository tagsRepository) :
			base(localizer, envUOW, tagsRepository)
		{
		}

		public override OperationResult<FoodProductEntity> Map(FoodProductDto from)
		{
			var entity = new FoodProductEntity
			{
				FoodType = from.FoodType,
				PetAgeRange = from.PetAgeRange,
				ProductWeightGramms = from.ProductWeightGrams
			};
			var res = SetBaseProductFields(entity, from);
			return res.Succeeded ?
				OperationResult<FoodProductEntity>.Success(entity) :
				OperationResult<FoodProductEntity>.Error(res.Message, res.ErrCode);
		}
		public OperationResult<FoodProductDto> RevMap(FoodProductEntity from)
		{
			var dto = new FoodProductDto
			{
				FoodType = from.FoodType,
				PetAgeRange = from.PetAgeRange,
				ProductWeightGrams = from.ProductWeightGramms,
			};
			var res = SetBaseProductFields(dto, from);
			return res.Succeeded ?
				OperationResult<FoodProductDto>.Success(dto) :
				OperationResult<FoodProductDto>.Error(res.Message, res.ErrCode);
		}
	}
}
