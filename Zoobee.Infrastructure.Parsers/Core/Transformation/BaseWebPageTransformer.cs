using Microsoft.Extensions.Logging;
using System;
using System.Threading.Tasks;
using Zoobee.Infrastructure.Parsers.Core.Enums;
using Zoobee.Infrastructure.Parsers.Interfaces.Transformation;

namespace Zoobee.Infrastructure.Parsers.Core.Transformation
{
	public abstract class BaseWebPageTransformer : IWebPageTransformer
	{
		protected readonly ILogger _logger;

		protected BaseWebPageTransformer(ILogger logger)
		{
			_logger = logger;
		}

		protected abstract string TargetSourceName { get; }

		public virtual bool CanTransform(string sourceName)
		{
			return TargetSourceName == "*" || sourceName.Equals(TargetSourceName, StringComparison.OrdinalIgnoreCase);
		}

		public async Task<TransformationResult> TransformAsync(string content, string url, ScrapingTaskType taskType)
		{
			using (_logger.BeginScope("Transform: {Source} [{Type}] {Url}", TargetSourceName, taskType, url))
			{
				try
				{
					if (string.IsNullOrWhiteSpace(content))
					{
						return new TransformationResult { IsSuccess = false, ErrorMessage = "Content is empty" };
					}

					return await TransformInternalAsync(content, url, taskType);
				}
				catch (Exception ex)
				{
					_logger.LogError(ex, "Error in transformation pipeline");
					return new TransformationResult { IsSuccess = false, ErrorMessage = ex.Message };
				}
			}
		}

		protected abstract Task<TransformationResult> TransformInternalAsync(string content, string url, ScrapingTaskType taskType);
	}
}