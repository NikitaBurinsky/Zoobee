using System;
using System.Linq;
using System.Threading.Tasks;
using System.Xml.Linq;
using Zoobee.Infrastructure.Parsers.Core.Enums;
using Zoobee.Infrastructure.Parsers.Core.Transformation;
using Zoobee.Infrastructure.Parsers.Interfaces.Transformation;

namespace Zoobee.Infrastructure.Parsers.Parsers.Zoobazar.Handlers
{
	public class ZoobazarSitemapHandler : IResourceHandler
	{
		public string TargetSourceName => "Zoobazar";

		public bool CanHandle(ScrapingTaskType taskType, string content)
		{
			return taskType == ScrapingTaskType.Sitemap;
		}

		public Task<TransformationResult> HandleAsync(string content, string url)
		{
			var result = new TransformationResult();
			try
			{
				var xDoc = XDocument.Parse(content);
				var ns = xDoc.Root.Name.Namespace;
				var isIndex = xDoc.Root.Name.LocalName.Equals("sitemapindex", StringComparison.OrdinalIgnoreCase);

				var links = xDoc.Descendants(ns + "loc").Select(x => x.Value).ToList();

				foreach (var link in links)
				{
					// Если это индекс -> создаем задачи типа Sitemap, иначе -> ProductPage
					var type = isIndex ? ScrapingTaskType.Sitemap : ScrapingTaskType.ProductPage;
					result.NewTasks.Add((link, type));
				}
			}
			catch (Exception ex)
			{
				result.IsSuccess = false;
				result.ErrorMessage = $"XML Error: {ex.Message}";
			}

			return Task.FromResult(result);
		}
	}
}