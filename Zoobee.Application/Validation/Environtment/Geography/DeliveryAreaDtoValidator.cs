using FluentValidation;
using Microsoft.Extensions.Localization;
using Zoobee.Application.DTOs.Environtment.Geography;
using Zoobee.Application.Interfaces.Repositories.Environtment.Geography;
using Zoobee.Application.Interfaces.Repositories.Environtment.Manufactures;
using Zoobee.Application.Interfaces.Services.GeoServices;
using Zoobee.Domain.DataEntities.Environment.Geography;
using Zoobee.Domain.Localization;

namespace Zoobee.Application.Validation.Environtment.Geography
{
	public class DeliveryAreaDtoValidator : BaseDtoValidator<DeliveryAreaDto>
	{
		private const int MAX_POINTS = 50;
		private const int MIN_POINTS = 3;
		private const double MAX_AREA_KM2 = 400000;

		ISellerCompanyRepository sellerCompanyRepository;
		ICountryBorderService borderService;
		IDeliveryAreaRepository deliveryAreaRepository;

		public DeliveryAreaDtoValidator(IStringLocalizer<Validations> localizer,
			ISellerCompanyRepository sellerCompany,
			ICountryBorderService borderService,
			IDeliveryAreaRepository deliveryAreaRepository)
			: base(localizer)
		{
			sellerCompanyRepository = sellerCompany;
			this.borderService = borderService;
			this.deliveryAreaRepository = deliveryAreaRepository;

			// 1. ВАЛИДАЦИЯ ОСНОВНЫХ ПОЛЕЙ
			ValidateBasicFields();

			// 2. ВАЛИДАЦИЯ СТРУКТУРЫ ПОЛИГОНА
			ValidatePolygonStructure();

			// 3. ВАЛИДАЦИЯ КООРДИНАТ ТОЧЕК
			ValidateCoordinates();

			// 4. ГЕОМЕТРИЧЕСКАЯ ВАЛИДАЦИЯ
			ValidateGeometry();

			// 5. БИЗНЕС-ЛОГИКА ВАЛИДАЦИИ
			ValidateBusinessRules();
		}
		/*
		 * isT  Company	Valid
		 * 0	0		false
		 * 0	1		true
		 * 1	0		true
		 * 1	1		false
		 * 
		 */


		private void ValidateBasicFields()
		{
			RuleFor(x => x.GeoArea)
				.NotNull()
					.WithMessage(ValidationMessage("GeoArea.MustBeNotNull"));

			RuleFor(x => x.AreaName)
				.MaximumLength(40)
					.WithMessage(ValidationMessage("AreaName.MaxLength40"));

			RuleFor(x => x)
				.Must(x => x.IsTemplate && x.SellerCompanyName == null)
					.WithMessage(ValidationMessage("SellerCompanyName.TemplateAreasCannotHaveCompany"))
					.When(x => x.IsTemplate == true);

			RuleFor(x => x)
				.Must(x => !x.IsTemplate && x.SellerCompanyName != null)
					.WithMessage(ValidationMessage("SellerCompanyName.NonTemplateAreasMustHaveCompany"))
					.When(x => x.IsTemplate == false);

			RuleFor(x => x.SellerCompanyName)
				.MaximumLength(60)
					.WithMessage(ValidationMessage("SellerCompanyName.MaxLength60"))
				.Must(sellerCompanyRepository.IsEntityExists)
					.WithMessage(ValidationMessage("SellerCompanyName.SellerCompanyNotFound"))
				.When(x => x.SellerCompanyName != null);
		}

		private void ValidatePolygonStructure()
		{
			// Пункт 2.1: Проверка минимального количества точек
			RuleFor(x => x.GeoArea)
				.Must(polygon => polygon.Count >= MIN_POINTS)
					.WithMessage(ValidationMessage($"GeoArea.MinCount3"))
				.When(x => x.GeoArea != null);

			// Пункт 2.2: Проверка максимального количества точек
			RuleFor(x => x.GeoArea)
				.Must(polygon => polygon.Count <= MAX_POINTS)
					.WithMessage(ValidationMessage($"GeoArea.MaxCount50"))
				.When(x => x.GeoArea != null);

		}

		private void ValidateCoordinates()
		{
			// Пункт 3.1: Проверка что все точки валидны (не null)
			RuleFor(x => x.GeoArea)
				.Must(polygon => polygon.All(p => p != null))
				.WithMessage(ValidationMessage("GeoArea.AllMustBeNotNullOrEmpty"))
				.When(x => x.GeoArea != null);

			// Пункт 3.2: Проверка диапазона долготы для каждой точки
			RuleForEach(x => x.GeoArea)
				.Must((request, point) => point.Longitude >= -180 && point.Longitude <= 180)
				.WithMessage(ValidationMessage("GeoArea.LongitudeMustBeInRangeFrom-180To180"))
				.When(x => x.GeoArea != null);

			// Пункт 3.3: Проверка диапазона широты для каждой точки
			RuleForEach(x => x.GeoArea)
				.Must((request, point) => point.Latitude >= -90 && point.Latitude <= 90)
					.WithMessage(ValidationMessage("GeoArea.LatitudeMustBeInRangeFrom-180To180"))
				.When(x => x.GeoArea != null);

			// Пункт 3.4: Проверка на уникальность точек (исключая дубликаты)
			RuleFor(x => x.GeoArea)
				.Must(polygon => polygon.Distinct(new GeoPointComparer()).Count() == polygon.Count)
				.WithMessage(ValidationMessage("GeoArea.CannotContainDuplicates"))
				.When(x => x.GeoArea != null);
		}

		private void ValidateGeometry()
		{
			// Пункт 4.1: Проверка на самопересечения полигона
			/* //TODO 
			RuleFor(x => x.GeoArea)
				.Must(polygon => !HasSelfIntersections(polygon))
					.WithMessage(ValidationMessage("GeoArea.CannotContainSelfIntersections"))
				.When(x => x.GeoArea != null);
			*/
			// Пункт 4.2: Проверка что полигон не слишком маленький
			RuleFor(x => x.GeoArea)
				.Must(polygon => CalculateSphericalArea(polygon) > 0.01) // Минимум 0.01 км²
					.WithMessage(ValidationMessage("GeoArea.MinAreaSize0.01km^2"))
				.When(x => x.GeoArea != null);

			// Пункт 4.3: Проверка что полигон не слишком большой
			RuleFor(x => x.GeoArea)
				.Must(polygon => CalculateSphericalArea(polygon) <= MAX_AREA_KM2)
					.WithMessage("GeoArea.MaxAreaSize400000km^2")
				.When(x => x.GeoArea != null);
		}

		private void ValidateBusinessRules()
		{
			// Пункт 5.1: Проверка что область находится в допустимом регионе
			RuleFor(x => x.GeoArea)
				.Must(borderService.IsPolygonInCountry)
					.WithMessage(ValidationMessage("GeoArea.AreaMustBePartOfBelarus"));

			//!!!!!!
			//Проверка на существование таких же
			RuleFor(x => x)
				.Must(dto => !deliveryAreaRepository.IsTemplateAreaWithNameExists(dto.AreaName))
					.WithMessage(ValidationMessage("AreaName.TemplateAreaWithSimilarNameExists"))
				.When(x => x.IsTemplate == true);
			RuleFor(x => x)
				.Must(dto => !deliveryAreaRepository.IsCompanyAreaWithNameExists(dto.AreaName, dto.SellerCompanyName))
					.WithMessage(ValidationMessage("AreaName.CompanyContainsAreaWithSimilarName"))
				.When(x => x.IsTemplate == false);
		}
		// ВСПОМОГАТЕЛЬНЫЕ МЕТОДЫ

		public class GeoPointComparer : IEqualityComparer<GeoPoint>
		{
			public bool Equals(GeoPoint p1, GeoPoint p2)
			{
				if (ReferenceEquals(p1, p2)) return true;
				if (p1 is null || p2 is null) return false;
				return Math.Abs(p1.Longitude - p2.Longitude) < 1e-10 &&
					   Math.Abs(p1.Latitude - p2.Latitude) < 1e-10;
			}

			public int GetHashCode(GeoPoint obj)
			{
				return HashCode.Combine(
					Math.Round(obj.Longitude, 6),
					Math.Round(obj.Latitude, 6)
				);
			}
		}

		private bool DoSegmentsIntersect(GeoPoint a1, GeoPoint a2, GeoPoint b1, GeoPoint b2)
		{
			double d = (a2.Longitude - a1.Longitude) * (b2.Latitude - b1.Latitude)
					 - (a2.Latitude - a1.Latitude) * (b2.Longitude - b1.Longitude);

			if (Math.Abs(d) < 1e-10) return false;

			double t = ((b1.Longitude - a1.Longitude) * (b2.Latitude - b1.Latitude)
					  - (b1.Latitude - a1.Latitude) * (b2.Longitude - b1.Longitude)) / d;

			double u = ((b1.Longitude - a1.Longitude) * (a2.Latitude - a1.Latitude)
					  - (b1.Latitude - a1.Latitude) * (a2.Longitude - a1.Longitude)) / d;

			return t >= 0 && t <= 1 && u >= 0 && u <= 1;
		}

		private double CalculateSphericalArea(ICollection<GeoPoint> polygon)
		{
			var points = polygon.Take(polygon.Count - 1).ToArray(); // Исключаем дубликат
			double area = 0;
			int n = points.Length;

			for (int i = 0; i < n; i++)
			{
				var p1 = points[i];
				var p2 = points[(i + 1) % n];

				area += ToRadians(p2.Longitude - p1.Longitude) *
					   (2 + Math.Sin(ToRadians(p1.Latitude)) + Math.Sin(ToRadians(p2.Latitude)));
			}

			return Math.Abs(area * 6371 * 6371 / 2);
		}

		private double CalculateAngle(GeoPoint a, GeoPoint b, GeoPoint c)
		{
			var ba = new GeoPoint(a.Longitude - b.Longitude, a.Latitude - b.Latitude);
			var bc = new GeoPoint(c.Longitude - b.Longitude, c.Latitude - b.Latitude);

			double dot = ba.Longitude * bc.Longitude + ba.Latitude * bc.Latitude;
			double magBa = Math.Sqrt(ba.Longitude * ba.Longitude + ba.Latitude * ba.Latitude);
			double magBc = Math.Sqrt(bc.Longitude * bc.Longitude + bc.Latitude * bc.Latitude);

			double angleRad = Math.Acos(dot / (magBa * magBc));
			return angleRad * 180.0 / Math.PI;
		}

		private double ToRadians(double degrees) => degrees * Math.PI / 180.0;
	}
}
