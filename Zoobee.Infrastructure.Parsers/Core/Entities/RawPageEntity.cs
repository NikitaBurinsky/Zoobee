using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Zoobee.Infrastructure.Parsers.Core.Entities
{
	using global::Zoobee.Application.DtoTypes.Base;
	using global::Zoobee.Infrastructure.Parsers.Core.Enums;
	using System;

	namespace Zoobee.Infrastructure.Parsers.Core.Entities
	{
		public class RawPageEntity : BaseEntity
		{
			public Guid Id { get; set; }
			public string Url { get; set; }
			public string SourceName { get; set; } // Например "Zoobazar"

			// Сюда пишем HTML. Для Postgres это text/jsonb, для SQL Server nvarchar(max)
			public string Content { get; set; }

			public int? HttpStatusCode { get; set; }
			public string ErrorMessage { get; set; }

			public RawPageStatus Status { get; set; }

			// Для механизма повторных попыток (Retry)
			public DateTime NextTryAt { get; set; } = DateTime.UtcNow;
			public int AttemptCount { get; set; } = 0;
		}
	}
}
