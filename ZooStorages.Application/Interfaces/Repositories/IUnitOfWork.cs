using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ZooStorages.Application.Interfaces.Repositories.Products;

namespace ZooStorages.Application.Interfaces.Repositories
{
    public interface IUnitOfWork 
	{
		public ICompaniesRepository Companies { get; }
		public IStoresRepository Stores { get; }
		public IDynamicAttributesRepository DynamicAttributes { get; }
		public ITagsRepository Tags { get; }
		public IProductsRepository Products { get; }
		public IProductSellingSlotsRepository ProductSellingSlots { get; }
		public IProductTypisationRepository ProductTypes { get; }
		public IPetKindsRepository PetKinds { get; }
		public Task<int> SaveChangesAsync();
		public int SaveChanges();
	}
}
