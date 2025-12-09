using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Zoobee.Infrastructure.Parsers.Core.Entities;
using Zoobee.Infrastructure.Parsers.Core.Entities.Zoobee.Infrastructure.Parsers.Core.Entities;
using Zoobee.Infrastructure.Parsers.Interfaces.Services.Downloader;

namespace Zoobee.Infrastructure.Parsers.Interfaces.Services.Scheduling
{
	public interface IDownloadSchedulingService
	{
		// Получает пачку URL, которые пора качать (Status = Pending и NextTry <= Now)
		Task<IEnumerable<ScrapingTask>> GetNextBatchAsync(int batchSize, CancellationToken ct);

		// Обновляет статус страницы после попытки скачивания
		Task HandleDownloadResultAsync(ScrapingTask page, DownloadResult result, CancellationToken ct);
	}
}
