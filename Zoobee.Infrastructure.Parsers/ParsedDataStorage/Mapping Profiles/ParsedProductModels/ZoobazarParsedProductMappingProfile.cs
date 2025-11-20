using Zoobee.Application.Interfaces.Services.Products.ProductsMapperService.Mapping_Profiles;
using Zoobee.Domain;
using Zoobee.Domain.DataEntities.Products;
using Zoobee.Infrastructure.Parsers.Storage.ParsedDataEntities.ParsedProductsModels.Zoobazar;

namespace Zoobee.Infrastructure.Parsers.ParsedDataStorage.Mapping_Profiles.ParsedProductModels
{

	/// <summary>
	/// Трансформация в слот
	/// Матчинг имени и др
	/// </summary>
	///
	public class ZoobazarParsedProductMappingProfile : IBaseMappingProfile<ZoobazarParsedProduct, BaseProductEntity>
	{
		public OperationResult<BaseProductEntity> Map(ZoobazarParsedProduct from)
		{





			//TODO Углубленный парсинг
			throw new NotImplementedException();
		}
	};
}
