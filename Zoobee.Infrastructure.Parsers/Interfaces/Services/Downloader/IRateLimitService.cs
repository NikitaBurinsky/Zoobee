using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Zoobee.Infrastructure.Parsers.Interfaces.Services.Downloader
{
	public interface IRateLimitService
	{
		// Блокирует выполнение (await), пока не пройдет нужное время для этого домена
		Task WaitForDelayAsync(string urlOrDomain, CancellationToken ct);

		// Сообщает сервису, что запрос был сделан (чтобы обновить таймер последнего запроса)
		void TrackRequest(string urlOrDomain);
	}
}
