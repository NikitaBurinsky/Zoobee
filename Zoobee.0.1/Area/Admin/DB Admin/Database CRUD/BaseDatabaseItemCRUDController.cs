using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Threading.Tasks;
using Zoobee.Application.DtoTypes.Base;
using Zoobee.Application.Shared.Constants;
using Zoobee.Domain;

namespace Zoobee.Web.Area.Admin.DB_Admin
{
	/// <summary>
	/// Generic base controller that provides common helpers and an abstract contract
	/// for CRUD operations. Concrete controllers should inherit from this class and
	/// implement the data-access methods. This class does NOT provide route attributes
	/// for concrete endpoints — derived controllers should declare routes matching the
	/// project's URL scheme (for example: <c>/db/read/pet-kinds</c>, <c>/db/create/brands</c> etc).
	/// 
	/// Usage pattern:
	/// - Inherit: <c>public class PetKindsController : BaseDatabaseItemCRUDController&lt;PetKindDto, PetKindEntity&gt;</c>
	/// - Add route attributes in derived controller and call the protected abstract methods.
	/// </summary>
	[Authorize(Policy = PolicyNames.DatabaseAdministration)]
	[ApiController]
	public abstract class BaseDatabaseItemCRUDController<TDto, TEntity> : ControllerBase
		where TEntity : BaseEntity
	{
		protected readonly ILogger _logger;

		protected BaseDatabaseItemCRUDController(ILogger logger)
		{
			_logger = logger;
		}

		#region Abstract data operations - implement in derived controllers

		/// <summary>
		/// Return all items for the resource.
		/// </summary>
		protected abstract Task<OperationResult<System.Collections.Generic.IEnumerable<TDto>>> GetAllAsync();

		/// <summary>
		/// Return single item by id.
		/// </summary>
		protected abstract Task<OperationResult<TDto>> GetByIdAsync(Guid id);

		/// <summary>
		/// Create new item. Returns created Id when succeeded.
		/// </summary>
		protected abstract Task<OperationResult<Guid>> CreateAsync(TDto dto);

		/// <summary>
		/// Update existing item by id.
		/// </summary>
		protected abstract Task<OperationResult> UpdateAsync(Guid id, TDto dto);

		/// <summary>
		/// Delete item by id.
		/// </summary>
		protected abstract Task<OperationResult> DeleteAsync(Guid id);

		#endregion

		#region Helpers to produce IActionResult from OperationResult

		/// <summary>
		/// Maps a non-generic OperationResult to IActionResult.
		/// </summary>
		protected IActionResult FromOperationResult(OperationResult res)
		{
			if (res == null)
				return StatusCode(500);

			if (res.Succeeded)
				return Ok();

			// If provider included explicit HttpStatusCode in result - prefer it.
			if (res.ErrCode.HasValue)
				return StatusCode((int)res.ErrCode.Value, new { error = res.Message });

			// Fallback to 400 for known client errors, 500 otherwise.
			return StatusCode(500, new { error = res.Message });
		}

		/// <summary>
		/// Maps an OperationResult with return value to IActionResult.
		/// </summary>
		protected IActionResult FromOperationResult<T>(OperationResult<T> res)
		{
			if (res == null)
				return StatusCode(500);

			if (res.Succeeded)
				return Ok(res.Returns);

			if (res.ErrCode.HasValue)
				return StatusCode((int)res.ErrCode.Value, new { error = res.Message });

			return StatusCode(500, new { error = res.Message });
		}

		#endregion
	}
}