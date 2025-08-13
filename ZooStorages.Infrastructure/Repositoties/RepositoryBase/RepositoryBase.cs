using Microsoft.Extensions.Localization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ZooStorages.Application.Interfaces.Repositories;
using ZooStorages.Core.Errors;
using ZooStores.Infrastructure.Repositoties;

namespace ZooStorages.Infrastructure.Repositoties
{
	public class RepositoryBase : IRepositoryBase
	{
		protected ZooStoresDbContext dbContext;
		protected IStringLocalizer<Errors> localizer;
		public int SaveChanges() => dbContext.SaveChanges();
		public async Task<int> SaveChangesAsync() => await dbContext.SaveChangesAsync();

		public RepositoryBase(ZooStoresDbContext dbContext, IStringLocalizer<Errors> localizer)
		{
			this.dbContext = dbContext;
			this.localizer = localizer;
		}
	}
}
