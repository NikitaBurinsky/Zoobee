// Путь: Zoobee.Infrastructure/Services/Products/Matching/Attributes/UnitConverter.cs
namespace Zoobee.Infrastructure.Services.Products.Matching.Attributes
{
	public static class UnitConverter
	{
		// Приводим все к граммам или миллилитрам
		public static int? ConvertToBaseValue(double value, string unit)
		{
			var cleanUnit = unit.ToLowerInvariant().Trim().Trim('.');

			return cleanUnit switch
			{
				"kg" or "кг" => (int)(value * 1000),
				"g" or "г" or "gr" or "гр" => (int)value,
				"l" or "л" => (int)(value * 1000),
				"ml" or "мл" => (int)value,
				_ => null
			};
		}
	}
}