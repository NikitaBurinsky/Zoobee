using Zoobee.Application.DTOs.Products.Base;
using Zoobee.Application.Interfaces.Repositories.Catalog;
using Zoobee.Application.Interfaces.Repositories.Environtment.Manufactures;
using Zoobee.Domain;
using Zoobee.Domain.DataEntities.Products;

namespace Zoobee.Infrastructure.UpdateProductsSpecificInfoProfiles
{
	public class BaseProductUpdateSpecInfoProfile : IUpdateProductSpecificProfile<BaseProductDto, BaseProductEntity>
	{
		IProductLineupRepository productLineups;
		IBrandsRepository brands;
		ITagsRepository tags;
		ICreatorCountriesRepository creatorCountries;
		ICreatorCompaniesRepository creatorCompanies;


		public BaseProductUpdateSpecInfoProfile(IProductLineupRepository productLineups,
			IBrandsRepository brands, ITagsRepository tags, 
			ICreatorCountriesRepository creatorCountries, 
			ICreatorCompaniesRepository creatorCompanies)
		{
			this.productLineups = productLineups;
			this.brands = brands;
			this.tags = tags;
			this.creatorCountries = creatorCountries;
			this.creatorCompanies = creatorCompanies;
		}

		public OperationResult UpdateSpecificInfo(BaseProductDto newInfo, BaseProductEntity entityToUpdate)
		{
			UpdateEANUPC(newInfo, entityToUpdate);
			UpdateMedia(newInfo, entityToUpdate);
			UpdateManufactureInfo(newInfo, entityToUpdate);
			UpdateSiteArticle(newInfo, entityToUpdate);
			UpdateTags(newInfo, entityToUpdate);
			return OperationResult.Success();
		}

		private void UpdateTags(BaseProductDto newInfo, BaseProductEntity entityToUpdate)
		{
			if (newInfo.Tags != null && newInfo.Tags.Any())
			{
				var newTagsList = newInfo.Tags.Select(tags.Get);
				entityToUpdate.Tags = entityToUpdate.Tags
					.Union(newTagsList)
					.Where(x => x != null)
					.ToList();
			}
		}

		private static void UpdateSiteArticle(BaseProductDto newInfo, BaseProductEntity entityToUpdate)
		{

			//TODO Подразумевается что на каждом источнике по одному артикулу
			//ХЗ, может пересмотрим позже
			if (newInfo.SiteArticles != null && newInfo.SiteArticles.Any())
			{
				string normalizedKeyInfo = newInfo.SiteArticles.FirstOrDefault()
						.Key.ToLower().Trim();
				if (!entityToUpdate.SiteArticles.Keys
					.Select(e => e.ToLower().Trim())
					.Contains(normalizedKeyInfo))
				{
					entityToUpdate.SiteArticles.TryAdd(normalizedKeyInfo, newInfo.SiteArticles.First().Value);
				}
			}
		}

		private void UpdateManufactureInfo(BaseProductDto newInfo, BaseProductEntity entityToUpdate)
		{
			if (entityToUpdate.CreatorCompany == null)
				entityToUpdate.CreatorCompany = creatorCompanies.Get(newInfo.CreatorCompanyName);

			if (entityToUpdate.CreatorCountry == null)
				entityToUpdate.CreatorCountry = creatorCountries.Get(newInfo.CreatorCountryName);

			if (entityToUpdate.Brand == null)
				entityToUpdate.Brand = brands.Get(newInfo.BrandName);

			if (entityToUpdate.ProductLineup == null)
				entityToUpdate.ProductLineup = productLineups.Get(entityToUpdate.Brand.BrandName, newInfo.ProductLineupName);
		}

		private static void UpdateMedia(BaseProductDto newInfo, BaseProductEntity entityToUpdate)
		{
			entityToUpdate.MediaURI = entityToUpdate.MediaURI.Union(newInfo.MediaURIs).ToList();
		}

		private static void UpdateEANUPC(BaseProductDto newInfo, BaseProductEntity entityToUpdate)
		{
			if (entityToUpdate.EAN == null && newInfo.EAN != null)
				entityToUpdate.EAN = newInfo.EAN;

			if (entityToUpdate.UPC == null && newInfo.UPC != null)
				entityToUpdate.UPC = newInfo.UPC;
		}
	}
}
