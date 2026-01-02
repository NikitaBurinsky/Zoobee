// Путь: Zoobee.Infrastructure/Services/Products/Matching/Attributes/AttributeExtractor.cs
using System.Text.RegularExpressions;

namespace Zoobee.Infrastructure.Services.Products.Matching.Attributes
{
	public class AttributeExtractor
	{
		// Регулярка для вытаскивания веса/объема из строки названия
		// Поддерживает: "15кг", "1.5 кг", "400g", "100мл"
		private static readonly Regex WeightRegex = new Regex(@"(?<value>\d+([.,]\d+)?)\s*(?<unit>kg|кг|g|г|gr|гр|l|л|ml|мл)", RegexOptions.IgnoreCase | RegexOptions.Compiled);

		public int? ExtractMetricValue(string productName)
		{
			if (string.IsNullOrWhiteSpace(productName)) return null;

			var match = WeightRegex.Match(productName);
			if (match.Success)
			{
				var valueStr = match.Groups["value"].Value.Replace(',', '.');
				var unitStr = match.Groups["unit"].Value;

				if (double.TryParse(valueStr, System.Globalization.NumberStyles.Any, System.Globalization.CultureInfo.InvariantCulture, out double value))
				{
					return UnitConverter.ConvertToBaseValue(value, unitStr);
				}
			}

			return null;
		}
	}
}