using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Localization;
using System;
using System.Net;
using Zoobee.Application.Interfaces.Repositories.Catalog;
using Zoobee.Application.Interfaces.Repositories.Environtment.Geography;
using Zoobee.Application.Interfaces.Repositories.Sellings;
using Zoobee.Core.Errors;
using Zoobee.Domain;
using Zoobee.Domain.DataEntities.Catalog.Reviews;
using Zoobee.Domain.DataEntities.Environment.Geography;
using Zoobee.Domain.DataEntities.Products;
using Zoobee.Domain.DataEntities.SellingsInformation;
using Zoobee.Infrastructure.Repositoties;
using Zoobee.Infrastructure.Repositoties;

namespace Zoobee.Infrastructure.Repositoties.Environtment.Geography
{
	public class DeliveryAreaRepository : RepositoryBase, IDeliveryAreaRepository
	{
		public DeliveryAreaRepository(ZooStoresDbContext dbContext, IStringLocalizer<Errors> localizer) : base(dbContext, localizer)
		{ }
		public IQueryable<DeliveryAreaEntity> GetAll() => dbContext.DeliveryAreas;
		public IQueryable<DeliveryAreaEntity> GetAllTemplateAreas() => dbContext.DeliveryAreas.Where(e => e.IsTemplate == true);
		public IQueryable<DeliveryAreaEntity> GetAllNonTemplateAreas() => dbContext.DeliveryAreas.Where(e => e.IsTemplate == false);
		public async Task<OperationResult<Guid>> CreateAsync(DeliveryAreaEntity newDeliveryAreaEntity)
		{
			newDeliveryAreaEntity.NormalizedAreaName = NormalizeString(newDeliveryAreaEntity.AreaName); 
			var res = await dbContext.DeliveryAreas.AddAsync(newDeliveryAreaEntity);
			if (res == null)
				return OperationResult<Guid>.Error(localizer["Error.DeliveryAreas.WriteDbError"], HttpStatusCode.InternalServerError);
			return OperationResult<Guid>.Success(res.Entity.Id);
		}
		public OperationResult Delete(DeliveryAreaEntity deletable)
		{
			if (!IsEntityExists(deletable.Id))
				return OperationResult.Error(localizer["Error.DeliveryAreas.DeliveryAreaNotFound"], HttpStatusCode.InternalServerError);
			var res = dbContext.Remove(deletable);
			return OperationResult.Success();
		}
		public DeliveryAreaEntity Get(Guid Id) => dbContext.DeliveryAreas.FirstOrDefault(e => e.Id == Id);
		public OperationResult Update(DeliveryAreaEntity productTypeToUpdate, Action<DeliveryAreaEntity> action)
		{
			var entry = dbContext.Update(productTypeToUpdate);
			if (entry == null)
				return OperationResult.Error(localizer["Error.DeliveryAreas.DeliveryAreaNotFound"], HttpStatusCode.InternalServerError);
			action(entry.Entity);
			return OperationResult.Success();
		}

		public bool IsEntityExists(Guid Id) => dbContext.DeliveryAreas.Any(e => e.Id == Id);

		public bool IsTemplateAreaWithNameExists(string templateAreaName) 
			=> GetAllTemplateAreas()
			.Any(e => e.NormalizedAreaName == NormalizeString(templateAreaName));

		public bool IsCompanyAreaWithNameExists(string areaName, string companyName)
			=> GetAllNonTemplateAreas()
			.Include(e => e.SellerCompany)
				.Any(e => e.SellerCompany.NormalizedCompanyName == NormalizeString(companyName)
				&& e.NormalizedAreaName == NormalizeString(areaName));
	}
}