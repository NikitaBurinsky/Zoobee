using Microsoft.Extensions.Localization;
using System.Net;
using Zoobee.Application.DTOs.Environtment.Geography;
using Zoobee.Application.Interfaces.Repositories.Environtment.Geography;
using Zoobee.Application.Interfaces.Repositories.Environtment.Manufactures;
using Zoobee.Application.Interfaces.Repositories.UnitsOfWork.Environtment;
using Zoobee.Application.Interfaces.Services.GeoServices.GeoLocationService;
using Zoobee.Application.Interfaces.Services.GeoServices.GeoLocationService.GeoCoderApiClient;
using Zoobee.Core.Errors;
using Zoobee.Domain;
using Zoobee.Domain.DataEntities.Environment.Geography;

namespace Zoobee.Infrastructure.Services.GeoServices.GeoLocationService
{
	public class GeoLocationService : IGeoLocationService
	{
		private IStringLocalizer<Errors> localizer;
		private ISellerCompanyRepository sellersRepository { get; set; }
		private IDeliveryAreaRepository deliveryAreaRepository { get; set; }
		private ILocationsRepository locationsRepository { get; set; }
		private IGeoCoderApiClient geoCoder { get; set; }
		public GeoLocationService(IEnvirontmentDataUnitOfWork envUOW,
			IStringLocalizer<Errors> local,
			IGeoCoderApiClient geoCoder)
		{
			sellersRepository = envUOW.ManufacturesUOWork.SellerCompanyRepository;
			deliveryAreaRepository = envUOW.GeoUOWork.DeliveryAreaRepository;
			locationsRepository = envUOW.GeoUOWork.LocationsRepository;
			this.geoCoder = geoCoder;
		}

		public async Task<OperationResult<Guid>> CreateDeliveryArea(DeliveryAreaDto dto)
		{
			var seller = sellersRepository.Get(dto.SellerCompanyName);
			if (seller == null)
				return OperationResult<Guid>.Error(localizer["Error.DeliveryAreas.SellerCompanyNotFound"],
					HttpStatusCode.InternalServerError);
			DeliveryAreaEntity dae = new DeliveryAreaEntity
			{
				AreaName = dto.AreaName,
				GeoArea = dto.GeoArea,
				IsTemplate = dto.IsTemplate,
				SellerCompany = seller,
			};
			return await deliveryAreaRepository.CreateAsync(dae);
		}

		/// <summary>
		/// При создании, отдает предпочтение гео-точке. 
		/// Если она не указана, то новая локация будет создана по адресу
		/// </summary>
		public async Task<OperationResult<Guid>> CreateLocation(LocationDto dto)
		{
			if (dto.GeoPoint != null)
			{
				return await CreateLocation(dto.GeoPoint);
			}
			else
			{
				return await CreateLocation(dto.City, dto.Address);
			}
		}

		public async Task<OperationResult<Guid>> CreateLocation(GeoPoint geoPoint)
		{
			var locRes = await geoCoder.GetLocationByGeoPoint(geoPoint);
			return await SaveLocation(locRes);
		}

		public async Task<OperationResult<Guid>> CreateLocation(string city, string address)
		{
			var locRes = await geoCoder.GetLocationByAddressAsync($"{city}, {address}");
			return await SaveLocation(locRes);
		}
		public bool IsLocationInArea(LocationDto location, Guid deliveryArea)
		{
			DeliveryAreaEntity deliveryAreaEntity = deliveryAreaRepository.Get(deliveryArea);
			if (deliveryArea == null)
				throw new ArgumentNullException("TODO ПЛОХО 431");
			return IsPointInPolygon(deliveryAreaEntity.GeoArea, location.GeoPoint);
		}

		public bool IsLocationInArea(GeoPoint location, Guid deliveryArea)
		{
			DeliveryAreaEntity deliveryAreaEntity = deliveryAreaRepository.Get(deliveryArea);
			if (deliveryArea == null)
				throw new ArgumentNullException("TODO ПЛОХО 432");
			return IsPointInPolygon(deliveryAreaEntity.GeoArea, location);
		}

		public async Task<bool> IsLocationInArea(string city, string address, Guid deliveryArea)
		{
			DeliveryAreaEntity deliveryAreaEntity = deliveryAreaRepository.Get(deliveryArea);
			if (deliveryArea == null)
				throw new ArgumentNullException("TODO ПЛОХО 432");

			var location = locationsRepository.GetByAddress(city, address);
			if (location == null)
			{
				var locRes = await geoCoder.GetLocationByAddressAsync($"{city}, {address}");
				if (locRes.Succeeded)
				{
					return IsPointInPolygon(deliveryAreaEntity.GeoArea, locRes.Returns.GeoPoint);
				}
				else
				{
					throw new Exception("TODO Ничего не найдено. Вообще нихуя 124");
				}
			}
			else
			{
				return IsPointInPolygon(deliveryAreaEntity.GeoArea, location.GeoPoint);
			}
		}

		private async Task<OperationResult<Guid>> SaveLocation(OperationResult<LocationDto> locRes)
		{
			if (locRes.Succeeded)
			{
				var addRes = await locationsRepository.CreateAsync(new LocationEntity
				{
					Address = locRes.Returns.Address,
					City = locRes.Returns.City,
					GeoPoint = locRes.Returns.GeoPoint,
				});
				return addRes;
			}
			else
				return OperationResult<Guid>.Error(locRes.Message, locRes.ErrCode);
		}
		private static bool IsPointInPolygon(ICollection<GeoPoint> polygon, GeoPoint testPoint)
		{
			if (polygon == null || polygon.Count < 3)
				throw new ArgumentException("TODO Полигон должен содержать как минимум 3 точки");

			double normalizedLon = NormalizeLongitude(testPoint.Longitude);
			double normalizedLat = testPoint.Latitude;

			var polygonArray = polygon.Select(p => new GeoPoint(
				NormalizeLongitude(p.Longitude),
				p.Latitude
			)).ToArray();

			bool inside = false;
			int n = polygonArray.Length;

			for (int i = 0, j = n - 1; i < n; j = i++)
			{
				var pi = polygonArray[i];
				var pj = polygonArray[j];

				if (Math.Abs(pj.Longitude - pi.Longitude) > 180)
				{
					if (pj.Longitude > pi.Longitude)
						pi.Longitude += 360;
					else
						pj.Longitude += 360;
				}

				if (pi.Latitude > normalizedLat != pj.Latitude > normalizedLat &&
					normalizedLon < (pj.Longitude - pi.Longitude) *
					 (normalizedLat - pi.Latitude) /
					 (pj.Latitude - pi.Latitude) + pi.Longitude)
				{
					inside = !inside;
				}
			}

			return inside;
		}
		private static double NormalizeLongitude(double longitude)
		{
			while (longitude < -180) longitude += 360;
			while (longitude > 180) longitude -= 360;
			return longitude;
		}

	}
}
