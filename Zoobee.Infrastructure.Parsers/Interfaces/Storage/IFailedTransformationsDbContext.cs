using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Zoobee.Domain.DataEntities.Products;
using Zoobee.Infrastructure.Parsers.Core.Entities;

namespace Zoobee.Infrastructure.Parsers.Interfaces.Storage
{
	public interface IFailedTransformationsDbContext
	{
		public DbSet<FailedParsedSaveTaskEntity> FailedInfos { get; } 
		public DbSet<BaseProductEntity> FailedProducts { get; }
		public DbSet<SellingSlotEntity> FailedSellings { get; }
	}
}
