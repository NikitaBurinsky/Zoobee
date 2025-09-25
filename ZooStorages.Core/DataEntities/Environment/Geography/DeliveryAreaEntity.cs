using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Zoobee.Domain.DataEntities.Environment.Manufactures;
using Zoobee.Application.DtoTypes.Base;

namespace Zoobee.Domain.DataEntities.Environment.Geography
{
    public class DeliveryAreaEntity : BaseEntity
    {
		public Guid Id { get; set; }
        /// <summary>e
        /// Обязательно для шаблонов доставки
        /// </summary>
        public string AreaName { get; set; }
        public List<GeoPoint> GeoArea { get; set; }
		/// <summary>
		/// Является ли область шаблонной
		/// </summary>
        public bool IsTemplate { get; set; }
		/// <summary>
		/// Если область является шаблонной, то null.
		/// Иначе, если она соответствует некоторой компании, то ссылка на нее
		/// </summary>
        public SellerCompanyEntity? SellerCompany { get; set; }
    }

    public class DeliveryAreaEntityConfigurator : IEntityTypeConfiguration<DeliveryAreaEntity>
    {
        public void Configure(EntityTypeBuilder<DeliveryAreaEntity> builder)
        {
            builder.OwnsMany(e => e.GeoArea);
			builder.HasOne(e => e.SellerCompany);
			builder.HasIndex(e => e.AreaName)
				.IsUnique(true);
        }
    }
}
