using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Zoobee.Domain.DataEntities.Products;
using Zoobee.Infrastructure.Parsers.Core.Entities.Failures.FailedSaveParsedItemTaskEntity;

namespace Zoobee.Infrastructure.Parsers.Interfaces.Storage
{
	public interface IFailedTransformationsDbContext
	{
		public DbSet<FailedToSaveParsedItemTaskEntity> FailedInfos { get; } 
		public Task<int> SaveChangesAsync(CancellationToken cancellationToken);
		public int SaveChanges();
	}
}
