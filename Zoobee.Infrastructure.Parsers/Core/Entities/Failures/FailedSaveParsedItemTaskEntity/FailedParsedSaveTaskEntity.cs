using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Zoobee.Application.DtoTypes.Base;
using Zoobee.Application.Shared.DTOs.System.Parsing;

namespace Zoobee.Infrastructure.Parsers.Core.Entities.Failures.FailedSaveParsedItemTaskEntity
{
	public class FailedParsedSaveTaskEntity : BaseEntity
	{
		public Guid Id { get; set; }
		public bool IsResolved { get; set; }
		/// <summary>
		/// If the failed item has been resolved, this field contains the resolving metadata.
		/// If the item is not resolved, this field will be null.
		/// </summary>
		public FailedItemResolvingInfo? ResolvedInfo { get; set; }
		public FailedItemErrorData FailureData { get; set; }
		public FailedItemType FailedObject { get; set; }
		public string? FailedProductJSON { get; set; }
		public string? FailedSlotJSON { get; set; }
	}

	public class FailedTransformationEntityConfigurator : IEntityTypeConfiguration<FailedParsedSaveTaskEntity>
	{
		public void Configure(EntityTypeBuilder<FailedParsedSaveTaskEntity> builder)
		{
			builder.HasKey(x => x.Id);
			builder.ComplexProperty(e => e.ResolvedInfo)
				.IsRequired(false);
			builder.ComplexProperty(e => e.FailureData)
				.IsRequired(true);


		}
	}
}
