using Zoobee.Infrastructure.Parsers.Core.Enums;

namespace Zoobee.Infrastructure.Parsers.Interfaces.Transformation
{
	public interface ITransformerResolver
	{
		/// <summary>
		/// Находит подходящий трансформер для указанного источника и типа задачи.
		/// </summary>
		/// <returns>Экземпляр трансформера или null, если не найден.</returns>
		IWebPageTransformer GetTransformer(string sourceName, ScrapingTaskType taskType);
	}
}