using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ZooStores.Application.DtoTypes.Base;

namespace ZooStorages.Domain.DataEntities.Products.Components.Reviews
{
	public class ReviewEntity : BaseEntity
	{
		public Guid Id { get; set; }
		//TODO USER
		public string ReviewerName { get; set; }
		//TODO
		public decimal Rating { get; set; }
		public string? Title { get; set; }
		public string Text { get; set; }
	}

	public class ReviewEntityConfigurator : IEntityTypeConfiguration<ReviewEntity>
	{
		public void Configure(EntityTypeBuilder<ReviewEntity> builder)
		{
			builder.HasKey(e => e.Id);
			builder.Property(e => e.Rating)
				.HasMaxLength(10)
				.IsRequired(true);
			builder.Property(e => e.Title).HasMaxLength(30);
			builder.Property(e => e.Text).IsRequired(true);
		}
	}




}
