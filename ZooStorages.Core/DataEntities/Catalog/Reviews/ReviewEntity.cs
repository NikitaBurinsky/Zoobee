using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Zoobee.Domain.DataEntities.Identity.Users;
using Zoobee.Application.DtoTypes.Base;
using Zoobee.Domain.DataEntities.Products;

namespace Zoobee.Domain.DataEntities.Catalog.Reviews
{
    public class ReviewEntity : BaseEntity
    {
        public Guid Id { get; set; }
		public CustomerUser ReviewerUser { get; set; }
        //TODO
        public float Rating { get; set; }
        public string Text { get; set; }
		public BaseProductEntity ReviewedProduct { get; set; }
    }

    public class ReviewEntityConfigurator : IEntityTypeConfiguration<ReviewEntity>
    {
        public void Configure(EntityTypeBuilder<ReviewEntity> builder)
        {
            builder.HasKey(e => e.Id);
            builder.Property(e => e.Rating)
                .IsRequired(true);
            builder.Property(e => e.Text)
				.HasMaxLength(750)
				.IsRequired(true);
			builder.HasOne(e => e.ReviewerUser);
			builder.HasOne(e => e.ReviewedProduct);
        }
    }




}
