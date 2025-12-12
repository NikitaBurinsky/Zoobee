using System.Threading.Tasks;
using Zoobee.Infrastructure.Parsers.Core.Enums;
using Zoobee.Infrastructure.Parsers.Core.Transformation;

namespace Zoobee.Infrastructure.Parsers.Interfaces.Transformation
{
	public interface IResourceHandler
	{
		// К какому сайту относится этот хендлер (для DI фильтрации)
		string TargetSourceName { get; }

		// Метод проверки: подходит ли этот хендлер для текущей задачи и контента?
		// Мы передаем и тип задачи (из БД), и контент (для анализа HTML)
		bool CanHandle(ScrapingTaskType taskType, string content);

		// Основная логика обработки
		Task<TransformationResult> HandleAsync(string content, string url);
	}
}