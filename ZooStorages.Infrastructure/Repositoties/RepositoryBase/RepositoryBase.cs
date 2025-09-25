using Microsoft.Extensions.Localization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Zoobee.Application.Interfaces.Repositories;
using Zoobee.Core;
using Zoobee.Core.Errors;
using Zoobee.Application.DtoTypes.Base;
using Zoobee.Infrastructure.Repositoties;

namespace Zoobee.Infrastructure.Repositoties
{
	public abstract class RepositoryBase
	{
		protected ZooStoresDbContext dbContext;
		protected IStringLocalizer<Errors> localizer;
		public int SaveChanges() => dbContext.SaveChanges();
		public async Task<int> SaveChangesAsync() => await dbContext.SaveChangesAsync();
		protected string NormalizeString(string s) => s == null ? null : s.Normalize().ToLowerInvariant().Trim();
		public RepositoryBase(ZooStoresDbContext dbContext, IStringLocalizer<Errors> localizer){
			this.dbContext = dbContext;
			this.localizer = localizer;
		}
	}
}
