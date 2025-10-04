using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Zoobee.Domain.DataEntities.Environment.Geography;
using Zoobee.Application.DtoTypes.Base;

namespace Zoobee.Domain.DataEntities.Environment.Manufactures
{
    public enum ZooStoreType
	{
		Physical = 0,
		OnlyOnline
	}

	public class ZooStoreEntity : BaseEntity
	{
		public Guid Id { get; set; }
		public ZooStoreType StoreType { get; set; }
		public LocationEntity? Location {  get; set; }
		public SellerCompanyEntity SellerCompany { get; set; }
		public string Name { get; set; }
		public string NormalizedName { get; set; }
		public TimeOnly? OpeningTime { get; set; }
		public TimeOnly? ClosingTime { get; set; }

	}

	public class ZooStoreEntityConfigurator : IEntityTypeConfiguration<ZooStoreEntity>
	{
		public void Configure(EntityTypeBuilder<ZooStoreEntity> builder)
		{
			builder.HasOne(e => e.Location);
			builder.HasOne(e => e.SellerCompany);
			builder.Property(e => e.Name)
				.HasMaxLength(60)
				.IsRequired(true);
			builder.Property(e => e.NormalizedName)
				.HasMaxLength(60)
				.IsRequired(true);
		}
	}
}
