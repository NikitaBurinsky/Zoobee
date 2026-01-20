using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Zoobee.Domain;
using Zoobee.Infrastructure.Parsers.Core.Entities.Failures.FailedSaveParsedItemTaskEntity;
using Zoobee.Infrastructure.Parsers.Interfaces.Repositories;
using Zoobee.Infrastructure.Parsers.Interfaces.Storage;

namespace Zoobee.Infrastructure.Parsers.Data_Storage.Repositories
{
	public class FailedToSaveParsedTasksRepository : IFailedToSaveParsedTasksRepository
	{
		private readonly IFailedTransformationsDbContext _context;

		public FailedToSaveParsedTasksRepository(IFailedTransformationsDbContext context)
		{
			_context = context;
		}

		/// <summary>
		/// Returns all resolved failed tasks (IsResolved == true).
		/// </summary>
		public IQueryable<FailedParsedSaveTaskEntity> GetAllResolved()
		{
			return _context.FailedInfos
				.AsNoTracking()
				.Where(f => f.IsResolved);
		}

		/// <summary>
		/// Returns all pending failed tasks (IsResolved == false).
		/// </summary>
		public IQueryable<FailedParsedSaveTaskEntity> GetAllPendingResolve()
		{
			return _context.FailedInfos
				.AsNoTracking()
				.Where(f => !f.IsResolved);
		}

		/// <summary>
		/// Returns pending failed tasks where the failed object is a product.
		/// </summary>
		public IQueryable<FailedParsedSaveTaskEntity> GetPendingFailedProducts()
		{
			return _context.FailedInfos
				.AsNoTracking()
				.Where(f => !f.IsResolved && f.FailedObject == FailedItemType.Product);
		}

		/// <summary>
		/// Returns pending failed tasks where the failed object is a selling slot.
		/// </summary>
		public IQueryable<FailedParsedSaveTaskEntity> GetPendingFailedSellingSlots()
		{
			return _context.FailedInfos
				.AsNoTracking()
				.Where(f => !f.IsResolved && f.FailedObject == FailedItemType.SellingSlot);
		}

		/// <summary>
		/// Creates a new failed transformation task.
		/// </summary>
		public OperationResult CreateFailedTransformationTask(FailedParsedSaveTaskEntity fte)
		{
			try
			{
				if (fte == null)
					return OperationResult.Error("Failed transformation entity cannot be null", System.Net.HttpStatusCode.BadRequest);

				if (fte.Id == Guid.Empty)
					fte.Id = Guid.NewGuid();

				_context.FailedInfos.Add(fte);
				_context.SaveChangesAsync().GetAwaiter().GetResult();

				return OperationResult.Success();
			}
			catch (Exception ex)
			{
				return OperationResult.Error($"Error creating failed transformation task: {ex.Message}", System.Net.HttpStatusCode.InternalServerError);
			}
		}

		/// <summary>
		/// Retrieves a failed task by its ID.
		/// </summary>
		public FailedParsedSaveTaskEntity Get(Guid id)
		{
			if (id == Guid.Empty)
				return null;

			return _context.FailedInfos
				.AsNoTracking()
				.FirstOrDefault(f => f.Id == id);
		}

		/// <summary>
		/// Marks a failed task as resolved by a specific user.
		/// </summary>
		public FailedParsedSaveTaskEntity SetAsResolved(Guid resolvedId, Guid resolvedById)
		{
			if (resolvedId == Guid.Empty)
				return null;

			var failedTask = _context.FailedInfos.FirstOrDefault(f => f.Id == resolvedId);
			if (failedTask == null)
				return null;

			failedTask.IsResolved = true;
			failedTask.ResolvedInfo = new FailedItemResolvingInfo
			{
				ResolvedByName = resolvedById.ToString(), // You may want to get the actual user name from the user ID
				ResolvedAt = DateTime.UtcNow,
				ResolutionNotes = null // Can be set later if needed
			};
			failedTask.Metadata.LastModified = DateTime.UtcNow;

			_context.FailedInfos.Update(failedTask);
			_context.SaveChangesAsync().GetAwaiter().GetResult();

			return failedTask;
		}

		/// <summary>
		/// Gets the N most recent failed tasks ordered by creation date descending.
		/// </summary>
		public IList<FailedParsedSaveTaskEntity> GetLatest(int count)
		{
			if (count <= 0)
				count = 10; // Default to 10 if invalid count provided

			return _context.FailedInfos
				.AsNoTracking()
				.OrderByDescending(f => f.Metadata.CreatedAt)
				.Take(count)
				.ToList();
		}

		/// <summary>
		/// Gets the N oldest failed tasks ordered by creation date ascending.
		/// </summary>
		public IList<FailedParsedSaveTaskEntity> GetOldest(int count)
		{
			if (count <= 0)
				count = 10; // Default to 10 if invalid count provided

			return _context.FailedInfos
				.AsNoTracking()
				.OrderBy(f => f.Metadata.CreatedAt)
				.Take(count)
				.ToList();
		}
	}
}
