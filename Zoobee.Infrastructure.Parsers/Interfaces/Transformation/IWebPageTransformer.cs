using System.Threading.Tasks;
using Zoobee.Infrastructure.Parsers.Core.Enums;
using Zoobee.Infrastructure.Parsers.Core.Transformation;

namespace Zoobee.Infrastructure.Parsers.Interfaces.Transformation
{
	public interface IWebPageTransformer
	{
		// Выбираем трансформер по имени сайта (Source)
		bool CanTransform(string sourceName);

		// Передаем тип задачи (Sitemap/Product), чтобы трансформер знал, какой хендлер запустить
		Task<TransformationResult> TransformAsync(string content, string url, ScrapingTaskType taskType);
	}
}