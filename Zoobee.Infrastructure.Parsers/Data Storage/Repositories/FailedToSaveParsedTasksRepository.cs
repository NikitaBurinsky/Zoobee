using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using Zoobee.Application.Shared.DTOs.System.Parsing;
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
		public IQueryable<FailedToSaveParsedItemTaskEntity> GetAllResolved()
		{
			return _context.FailedInfos
				.AsNoTracking()
				.Where(f => f.IsResolved);
		}

		/// <summary>
		/// Returns all pending failed tasks (IsResolved == false).
		/// </summary>
		public IQueryable<FailedToSaveParsedItemTaskEntity> GetAllPendingResolve()
		{
			return _context.FailedInfos
				.AsNoTracking()
				.Where(f => !f.IsResolved);
		}

		/// <summary>
		/// Returns pending failed tasks where the failed object is a product.
		/// </summary>
		public IQueryable<FailedToSaveParsedItemTaskEntity> GetPendingFailedProducts()
		{
			return _context.FailedInfos
				.AsNoTracking()
				.Where(f => !f.IsResolved && f.FailedObject == FailedItemType.Product);
		}

		/// <summary>
		/// Returns pending failed tasks where the failed object is a selling slot.
		/// </summary>
		public IQueryable<FailedToSaveParsedItemTaskEntity> GetPendingFailedSellingSlots()
		{
			return _context.FailedInfos
				.AsNoTracking()
				.Where(f => !f.IsResolved && f.FailedObject == FailedItemType.SellingSlot);
		}

		/// <summary>
		/// Creates a new failed transformation task.
		/// </summary>
		public OperationResult CreateFailedTransformationTask(FailedToSaveParsedItemTaskEntity fte)
		{
			try
			{
				if (fte == null)
					return OperationResult.Error("Error.FailedParsedTasks.NullEntity", System.Net.HttpStatusCode.BadRequest);

				if (fte.Id == Guid.Empty)
					fte.Id = Guid.NewGuid();

				_context.FailedInfos.Add(fte);
				_context.SaveChanges();

				return OperationResult.Success();
			}
			catch (Exception ex)
			{
				return OperationResult.Error("Error.FailedParsedTasks.WriteDbError", System.Net.HttpStatusCode.InternalServerError);
			}
		}

		/// <summary>
		/// Retrieves a failed task by its ID.
		/// </summary>
		public FailedToSaveParsedItemTaskEntity Get(Guid id)
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
		public FailedToSaveParsedItemTaskEntity SetAsResolved(Guid resolvedId, Guid resolvedById, string resolutionNotes)
		{
			if (resolvedId == Guid.Empty)
				return null;

			var failedTask = _context.FailedInfos.FirstOrDefault(f => f.Id == resolvedId);
			if (failedTask == null)
				return null;

			failedTask.IsResolved = true;
			failedTask.ResolvedInfo = new FailedItemResolvingInfo
			{
				ResolvedById = resolvedById, // You may want to get the actual user name from the user ID
				ResolvedAt = DateTime.UtcNow,
				ResolutionNotes = null // Can be set later if needed
			};
			failedTask.Metadata.LastModified = DateTime.UtcNow;

			_context.FailedInfos.Update(failedTask);
			_context.SaveChanges();

			return failedTask;
		}

		/// <summary>
		/// Gets the N most recent failed tasks ordered by creation date descending.
		/// </summary>
		public IList<FailedToSaveParsedItemTaskEntity> GetLatest(int count)
		{
			if (count <= 0)
				count = 10; 

			return _context.FailedInfos
				.AsNoTracking()
				.OrderByDescending(f => f.Metadata.CreatedAt)
				.Take(count)
				.ToList();
		}

		/// <summary>
		/// Gets the N oldest failed tasks ordered by creation date ascending.
		/// </summary>
		public IList<FailedToSaveParsedItemTaskEntity> GetOldest(int count)
		{
			if (count <= 0)
				count = 10; 

			return _context.FailedInfos
				.AsNoTracking()
				.OrderBy(f => f.Metadata.CreatedAt)
				.Take(count)
				.ToList();
		}

		public OperationResult Delete(Guid Id)
		{
			if (Id == Guid.Empty)
				return OperationResult.Error("Error.FailedToSaveParsedItems.RemoveDbError.FailedToSaveParsedItemNotFound", HttpStatusCode.BadRequest);
			var entity = Get(Id);

			_context.FailedInfos.Remove(Get(Id));	
			_context.SaveChanges();
			return OperationResult.Success();
		}

		public FailedToSaveParsedItemTaskEntity SetAsResolved(FailedToSaveParsedItemTaskEntity resolvedEntity, string resolutionNotes, Guid ResolvedById)
		{
			var entry = _context.FailedInfos.Update(resolvedEntity);
			if(entry == null)
				return null;
			entry.Entity.IsResolved = true;
			entry.Entity.ResolvedInfo = new FailedItemResolvingInfo
			{
				ResolvedById = ResolvedById,
				ResolvedAt = DateTime.UtcNow,
				ResolutionNotes = resolutionNotes,
			};
			_context.SaveChanges();
			return entry.Entity;
		}

		public int SaveChanges() => _context.SaveChanges();
	}
}
