using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Zoobee.Domain.DataEntities.Catalog.Reviews;
using Zoobee.Domain.DataEntities.Catalog.Tags;
using Zoobee.Domain.DataEntities.Environment.Creators;
using Zoobee.Domain.DataEntities.Environment.Geography;
using Zoobee.Domain.DataEntities.Environment.Manufactures;
using Zoobee.Domain.DataEntities.Media;
using Zoobee.Domain.DataEntities.Products;
using Zoobee.Domain.DataEntities.SellingsInformation;

namespace Zoobee.Application.Interfaces.DbContext
{
    public interface IApplicationDbContext
    {
        public DbSet<BaseProductEntity> Products { get; set; }
        public DbSet<SellingSlotEntity> SellingSlots { get; set; }
        public DbSet<DeliveryOptionEntity> DeliveryOptions { get; set; }
        public DbSet<SelfPickupOptionEntity> SelfPickupOptions { get; set; }
        public DbSet<ReviewEntity> Reviews { get; set; }
        public DbSet<TagEntity> Tags { get; set; }
        public DbSet<LocationEntity> Locations { get; set; }
        public DbSet<PetKindEntity> PetKinds { get; set; }
        public DbSet<BrandEntity> Brands { get; set; }
        public DbSet<CreatorCountryEntity> CreatorCountries { get; set; }
        public DbSet<CreatorCompanyEntity> CreatorCompanies { get; set; }
        public DbSet<ProductLineupEntity> ProductsLineups { get; set; }
        public DbSet<SellerCompanyEntity> SellerCompanies { get; set; }
        public DbSet<ZooStoreEntity> ZooStores { get; set; }
        public DbSet<DeliveryAreaEntity> DeliveryAreas { get; set; }
        public DbSet<MediaFileEntity> MediaFiles { get; set; }
        public Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);
    }
}
