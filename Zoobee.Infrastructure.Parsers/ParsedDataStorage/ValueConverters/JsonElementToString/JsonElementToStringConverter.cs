using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace Zoobee.Infrastructure.Parsers.ParsedDataStorage.ValueConverters.JsonElementToString
{
	public class JsonElementToStringConverter : ValueConverter<JsonElement, string>
	{

		public JsonElementToStringConverter()
			: base(
				v => v.GetRawText(),
				v => string.IsNullOrEmpty(v)
				? JsonDocument.Parse("{}", new JsonDocumentOptions{ AllowTrailingCommas = true }).RootElement
				: JsonDocument.Parse(v, new JsonDocumentOptions { AllowTrailingCommas = true }).RootElement)
		{
		}
	}
}
