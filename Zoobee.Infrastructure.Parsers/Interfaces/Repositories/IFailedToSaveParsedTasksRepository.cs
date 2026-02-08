using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Zoobee.Domain;
using Zoobee.Infrastructure.Parsers.Core.Entities.Failures.FailedSaveParsedItemTaskEntity;

namespace Zoobee.Infrastructure.Parsers.Interfaces.Repositories
{
	public interface IFailedToSaveParsedTasksRepository
	{
		public IQueryable<FailedToSaveParsedItemTaskEntity> GetAllResolved();
		public IQueryable<FailedToSaveParsedItemTaskEntity> GetAllPendingResolve();
		public IQueryable<FailedToSaveParsedItemTaskEntity> GetPendingFailedProducts();
		public IQueryable<FailedToSaveParsedItemTaskEntity> GetPendingFailedSellingSlots();
		public OperationResult CreateFailedTransformationTask(FailedToSaveParsedItemTaskEntity fte);
		public FailedToSaveParsedItemTaskEntity Get(Guid Id);
		public OperationResult Delete(Guid Id);
		public FailedToSaveParsedItemTaskEntity SetAsResolved(FailedToSaveParsedItemTaskEntity ResolvedId, string resolutionNotes, Guid ResolvedById);
		public IList<FailedToSaveParsedItemTaskEntity> GetLatest(int count);
		public IList<FailedToSaveParsedItemTaskEntity> GetOldest(int count);
		public int SaveChanges();
	}
}
