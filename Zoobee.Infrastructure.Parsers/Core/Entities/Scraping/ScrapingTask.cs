using System;
using System.Collections.Generic;
using Zoobee.Application.DtoTypes.Base;
using Zoobee.Infrastructure.Parsers.Core.Enums; // Добавили namespace

namespace Zoobee.Infrastructure.Parsers.Core.Entities
{
	public class ScrapingTask : BaseEntity
	{
		public Guid Id { get; set; }
		public string Url { get; set; }
		public string SourceName { get; set; }

		public RawPageStatus Status { get; set; }

		// Новое поле: Тип задачи
		public ScrapingTaskType Type { get; set; } = ScrapingTaskType.Unknown;

		public DateTime NextTryAt { get; set; } = DateTime.UtcNow;
		public int AttemptCount { get; set; }
		public int? CustomFrequencyHours { get; set; }

		public ICollection<ScrapingData> History { get; set; }
	}
}