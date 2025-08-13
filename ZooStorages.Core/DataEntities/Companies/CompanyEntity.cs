using ZooStores.Application.DtoTypes.Base;
using ZooStores.Application.DtoTypes.Clinics;
using ZooStores.Application.DtoTypes.Stores;

namespace ZooStores.Application.DtoTypes.Companies
{
	public  class CompanyEntity : BaseEntity
	{
		public Guid Id { get; set; }
		public string Name { get; set; }
		public string Description { get; set; }
		public string ContactInfo { get; set; }
		public ICollection<VetClinicEntity> ownedClinics { get; set; }
		public ICollection<VetStoreEntity> ownedStores { get; set; }

	}
}
