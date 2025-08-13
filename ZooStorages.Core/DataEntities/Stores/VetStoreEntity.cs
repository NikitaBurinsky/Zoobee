using ZooStorages.Domain.DataEntities.Products;
using ZooStores.Application.DtoTypes.Base;
using ZooStores.Application.DtoTypes.Companies;
using ZooStores.Application.DtoTypes.EntityComponents;

namespace ZooStores.Application.DtoTypes.Stores
{
	public class VetStoreEntity : BaseEntity
	{
		public Guid Id { get; set; }
		public string Name { get; set; }
		public string Info { get; set; }
		public string PhoneNumber { get; set; }

		//Avaibable Products
		public ICollection<ProductSlotEntity> avaibabProducts { get; set; }

		//Location
		//
		public Guid LocationId { get; set; }
		public LocationEntity Location { get; set; }

		//Owner Company
		public Guid ownerCompanyId { get; set; }
		public CompanyEntity ownerCompany { get; set; }

	}
}
