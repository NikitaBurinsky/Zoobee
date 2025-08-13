using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ZooStorages.Application.Interfaces.Repositories;
using ZooStorages.Application.Interfaces.Repositories.Products;

namespace ZooStorages.Infrastructure.Repositoties
{
	public class UnitOfWork : IUnitOfWork
	{
		public UnitOfWork(ICompaniesRepository companies,
			IStoresRepository stores, 
			IProductsRepository products,
			IProductSellingSlotsRepository productSellingSlots,
			IProductTypisationRepository productTypes,
			IPetKindsRepository petKinds,
			IDynamicAttributesRepository dynamicAttributes,
			ITagsRepository tags)
		{
			Companies = companies;
			Stores = stores;
			Products = products;
			ProductSellingSlots = productSellingSlots;
			ProductTypes = productTypes;
			PetKinds = petKinds;
			DynamicAttributes = dynamicAttributes;
			Tags = tags;
		}

		public ICompaniesRepository Companies { get; }
		public IStoresRepository Stores { get; }
		/// 
		public IProductsRepository Products { get; }
		public IProductSellingSlotsRepository ProductSellingSlots { get; }
		public IProductTypisationRepository ProductTypes { get; }
		public IPetKindsRepository PetKinds { get; }
		public ITagsRepository Tags { get; }
		public IDynamicAttributesRepository DynamicAttributes { get; }
		public int SaveChanges()
			=> Products.SaveChanges();

		public Task<int> SaveChangesAsync()
			=> Products.SaveChangesAsync();
	}
}
