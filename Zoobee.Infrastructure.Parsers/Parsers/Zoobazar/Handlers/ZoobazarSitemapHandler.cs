using System;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Xml.Linq;
using Zoobee.Infrastructure.Parsers.Core.Enums;
using Zoobee.Infrastructure.Parsers.Core.Transformation;
using Zoobee.Infrastructure.Parsers.Interfaces.Transformation;
using Zoobee.Infrastructure.Parsers.Parsers.Zoobazar.Services;

namespace Zoobee.Infrastructure.Parsers.Parsers.Zoobazar.Handlers
{
	public class ZoobazarSitemapHandler : IResourceHandler
	{
		public string TargetSourceName => "Zoobazar";

		public bool CanHandle(ScrapingTaskType taskType, string content, string url)
		{
			return url.Contains("sitemap") || taskType == ScrapingTaskType.Sitemap;
		}

		public async Task<TransformationResult> HandleAsync(string content, string url)
		{
			var result = new TransformationResult() { 
			UpdatedTaskType = ScrapingTaskType.Sitemap,
			};
			try
			{
				var links = await ZoobazarLinksExtractor.ExtractLinksAsync(content, url);
				foreach (var link in links)
				{
					if (link.Contains("://zoobazar"))
					{
						bool isSitemap = link.Contains("sitemap");
						var type = isSitemap ? ScrapingTaskType.Sitemap : ScrapingTaskType.ProductPage;
						result.NewTasks.Add((link, type));
					}
				}
			}
			catch (Exception ex)
			{
				result.IsSuccess = false;
				result.ErrorMessage = $"XML Error: {ex.Message}";
			}

			return result;
		}
	}
}