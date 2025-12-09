using System.Collections.Generic;
using Zoobee.Application.DtoTypes.Base;
using Zoobee.Infrastructure.Parsers.Core.Enums;

namespace Zoobee.Infrastructure.Parsers.Core.Entities
{
    public class ScrapingTask : BaseEntity
    {
		public Guid Id { get; set; }
        public string Url { get; set; }
        public string SourceName { get; set; } // "Zoobazar", "Wildberries"

        public RawPageStatus Status { get; set; }

        // Планирование
        public DateTime NextTryAt { get; set; } = DateTime.UtcNow;
        public int AttemptCount { get; set; }

        // Индивидуальная частота для этого URL (если 0 - берем из конфига)
        // Это позволит для важных товаров ставить обновление раз в час, а для остальных - раз в день
        public int? CustomFrequencyHours { get; set; }

        // Связь с историей
        public ICollection<ScrapingData> History { get; set; }
    }
}