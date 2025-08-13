using ZooStores.Application.DtoTypes.Base;

namespace ZooStores.Application.DtoTypes.EntityComponents
{
	public  class LocationEntity : BaseEntity
	{
		public Guid Id { get; set; }
		public string? Address { get; set; }
		public string? City { get; set; }
		public string? Region { get; set; }
		public string? Country { get; set; } = "Belarus";
		public double? Latitude { get; set; }
		public double? Longitude { get; set; }
	}
}
