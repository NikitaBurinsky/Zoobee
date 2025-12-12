using Microsoft.Extensions.Logging;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Zoobee.Infrastructure.Parsers.Core.Enums;
using Zoobee.Infrastructure.Parsers.Core.Transformation;
using Zoobee.Infrastructure.Parsers.Interfaces.Transformation;

namespace Zoobee.Infrastructure.Parsers.Parsers.Zoobazar
{
	public class ZoobazarTransformer : BaseWebPageTransformer
	{
		private readonly IEnumerable<IResourceHandler> _handlers;

		public ZoobazarTransformer(
			ILogger<ZoobazarTransformer> logger,
			IEnumerable<IResourceHandler> handlers)
			: base(logger)
		{
			// Забираем только "своих" работников
			_handlers = handlers.Where(h => h.TargetSourceName == "Zoobazar");
		}

		protected override string TargetSourceName => "Zoobazar";

		protected override async Task<TransformationResult> TransformInternalAsync(string content, string url, ScrapingTaskType taskType)
		{
			// Ищем подходящего исполнителя
			// Важно: порядок регистрации или проверки может иметь значение, если условия пересекаются
			var handler = _handlers.FirstOrDefault(h => h.CanHandle(taskType, content));

			if (handler == null)
			{
				_logger.LogWarning("No handler found for TaskType: {Type}", taskType);
				// Можно вернуть пустой успех, чтобы не считать это сбоем парсинга, или ошибку
				return new TransformationResult
				{
					IsSuccess = true,
					ErrorMessage = "No suitable handler found (skipped)"
				};
			}

			_logger.LogInformation("Delegating to handler: {Handler}", handler.GetType().Name);
			return await handler.HandleAsync(content, url);
		}
	}
}