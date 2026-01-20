using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Zoobee.Domain;
using Zoobee.Infrastructure.Parsers.Core.Entities.FailedSaveParsedItemTaskEntity;

namespace Zoobee.Infrastructure.Parsers.Interfaces.Repositories
{
	public interface IFailedTransformationsRepository
	{
		public IQueryable<FailedParsedSaveTaskEntity> GetAllResolved();
		public IQueryable<FailedParsedSaveTaskEntity> GetAllPendingResolve();
		public IQueryable<FailedParsedSaveTaskEntity> GetPendingFailedProducts();
		public IQueryable<FailedParsedSaveTaskEntity> GetPendingFailedSellingSlots();
		public OperationResult CreateFailedTransformationTask(FailedParsedSaveTaskEntity fte);
		public FailedParsedSaveTaskEntity Get(Guid Id);
		public FailedParsedSaveTaskEntity SetAsResolved(Guid ResolvedId, Guid ResolvedById);
		public IList<FailedParsedSaveTaskEntity> GetLatest(int count);
		public IList<FailedParsedSaveTaskEntity> GetOldest(int count);
	}
}
