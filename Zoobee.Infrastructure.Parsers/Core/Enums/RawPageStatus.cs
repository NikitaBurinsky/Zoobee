using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Zoobee.Infrastructure.Parsers.Core.Enums
{
	public enum RawPageStatus
	{
		Pending = 0,        // Ждет скачивания
		Downloaded = 1,     // Скачано, ждет парсинга
		Processed = 2,      // Данные извлечены
		Failed = 3,         // Ошибка скачивания
		NotFound = 4        // 404 ошибка
	}
}
