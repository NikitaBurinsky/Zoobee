using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Zoobee.Application.DtoTypes.Base;
using Zoobee.Domain.DataEntities.Products;

namespace Zoobee.Infrastructure.Parsers.Core.Entities
{
	public class ResolvedMetaData
	{
		public DateTime? ResolvedAt { get; set; }
		public Guid? ResolvedById { get; set; }
	}
	public class FailureDataSet
	{
		public FailureSignalType FailSignalType { get; set; }
		public string? ExceptionType { get; set; } // "DbUpdateException", "ValidationException"
		public string Message { get; set; } // Ограничьте длину (500-1000 символов)
		public string? StackTrace { get; set; }
	}

	public enum FailureSignalType
	{
		OperationResult,
		Exception
	}

	public class FailedParsedSaveTaskEntity : BaseEntity
	{
		public Guid Id { get; set; }
		public bool IsResolved { get; set; }
		public ResolvedMetaData? ResolvedMetadata { get; set; }
		public FailureDataSet FailureData { get; set; }
		public BaseProductEntity? FailedProduct { get; set; }
		public SellingSlotEntity? FailedSlot { get; set; }

	}

	public class FailedTransformationEntityConfigurator : IEntityTypeConfiguration<FailedParsedSaveTaskEntity>
	{
		public void Configure(EntityTypeBuilder<FailedParsedSaveTaskEntity> builder)
		{
			builder.HasKey(x => x.Id);
			builder.ComplexProperty(e => e.ResolvedMetadata)
				.IsRequired(false);
			builder.ComplexProperty(e => e.FailureData)
				.IsRequired(true);


		}
	}


}
