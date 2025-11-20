using Microsoft.Extensions.Localization;
using Zoobee.Core.Errors;

namespace Zoobee.Infrastructure.Repositoties
{
	public abstract class RepositoryBase
	{
		protected ZoobeeAppDbContext dbContext;
		protected IStringLocalizer<Errors> localizer;
		public int SaveChanges() => dbContext.SaveChanges();
		public async Task<int> SaveChangesAsync() => await dbContext.SaveChangesAsync();
		protected string NormalizeString(string s) => s == null ? null : s.Normalize().ToLowerInvariant().Trim();
		public RepositoryBase(ZoobeeAppDbContext dbContext, IStringLocalizer<Errors> localizer)
		{
			this.dbContext = dbContext;
			this.localizer = localizer;
		}
	}
}
