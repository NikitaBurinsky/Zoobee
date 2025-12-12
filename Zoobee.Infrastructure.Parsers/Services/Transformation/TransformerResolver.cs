using Microsoft.Extensions.Logging;
using System.Collections.Generic;
using System.Linq;
using Zoobee.Infrastructure.Parsers.Core.Enums;
using Zoobee.Infrastructure.Parsers.Interfaces.Transformation;

namespace Zoobee.Infrastructure.Parsers.Services.Transformation
{
	public class TransformerResolver : ITransformerResolver
	{
		private readonly IEnumerable<IWebPageTransformer> _transformers;
		private readonly ILogger<TransformerResolver> _logger;

		// DI автоматически внедрит сюда коллекцию всех классов, реализующих IWebPageTransformer
		public TransformerResolver(IEnumerable<IWebPageTransformer> transformers, ILogger<TransformerResolver> logger)
		{
			_transformers = transformers;
			_logger = logger;
		}

		public IWebPageTransformer GetTransformer(string sourceName, ScrapingTaskType taskType)
		{
			var transformer = _transformers.FirstOrDefault(t => t.CanTransform(sourceName));
			if (transformer == null)
			{
				_logger.LogWarning("No transformer found for Source: {Source}, Type: {Type}", sourceName, taskType);
			}

			return transformer;
		}
	}
}