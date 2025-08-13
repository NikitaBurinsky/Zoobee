using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ZooStorages.Domain.DataEntities.Products;
using ZooStorages.Domain.DataEntities.Products.Components;
using ZooStorages.Domain.DataEntities.Products.Components.Dimensions;
using ZooStorages.Domain.DataEntities.Products.Components.DynamicAttributes;
using ZooStorages.Domain.DataEntities.Products.Components.ExtendedAttributes;
using ZooStorages.Domain.DataEntities.Products.Components.Reviews;
using ZooStorages.Domain.DataEntities.Products.Components.Tags;
using ZooStores.Application.DtoTypes.Clinics;
using ZooStores.Application.DtoTypes.Companies;
using ZooStores.Application.DtoTypes.Products.Categories;
using ZooStores.Application.DtoTypes.Products.Delivery;
using ZooStores.Application.DtoTypes.Stores;

namespace ZooStorages.Application.Interfaces.DbContext
{
    public interface IApplicationDbContext 
    {
        /// 
        /// Companies, Stores, Clinics
        DbSet<VetStoreEntity> VetStores { get; }
        DbSet<VetClinicEntity> VetClinics { get; }
        DbSet<CompanyEntity> Companies { get; }

        /// 
        ///Products

        DbSet<ProductEntity> Products { get; }
        DbSet<ProductTypeEntity> ProductTypes { get; }
        DbSet<ProductCategoryEntity> ProductCategories { get; }
        DbSet<PetKindEntity> PetKinds { get; }

        ///
        ///Selling Product Slots
        DbSet<ProductSlotEntity> SellingSlots { get; }
        DbSet<DeliveryOptionEntity> DeliveryOptions { get; }
        DbSet<SelfPickupOptionEntity> SelfPickupOptions { get; }

        ///
        ///Dynamic Attributes
        DbSet<ProductAttributeTypeEntity> ExtAttributesTypes { get; }
        DbSet<ProductAttributeEntity> ExtAttributes { get; }

        /// 
        /// Reviews
        DbSet<ReviewEntity> Reviews { get; }

        /// 
        /// Tags
        DbSet<TagEntity> Tags { get; }


        public Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);
    }
}
