using HtmlAgilityPack;
using Microsoft.Extensions.Logging;
using Microsoft.AspNetCore.WebUtilities;
using Microsoft.Extensions.Primitives;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Zoobee.Infrastructure.Parsers.Core.Enums;
using Zoobee.Infrastructure.Parsers.Core.Transformation;
using Zoobee.Infrastructure.Parsers.Interfaces.Transformation;
using Zoobee.Infrastructure.Parsers.Parsers.Zoobazar.Services;

namespace Zoobee.Infrastructure.Parsers.Parsers.Zoobazar.Handlers
{
	public class ZoobazarCatalogHandler : IResourceHandler
	{
		public string TargetSourceName => "Zoobazar";

		ILogger<ZoobazarCatalogHandler> logger;

		public ZoobazarCatalogHandler(ILogger<ZoobazarCatalogHandler> logger)
		{
			this.logger = logger;
		}
		//TODO Переписать CanHandle?, поскольку все товары в url тоже содержат c atalog, требуется уточнить
		public bool CanHandle(ScrapingTaskType taskType, string content, string url)
		{
			if (taskType == ScrapingTaskType.Catalog) return true;
			bool catalog_main = content.Contains("<div class=\"catalog__main\">");
			bool catalog_head = content.Contains("<div class=\"catalog__heading\">");

			return catalog_head && catalog_main;
		}

		public async Task<TransformationResult> HandleAsync(string content, string url)
		{
			var sw = System.Diagnostics.Stopwatch.StartNew();
			logger.LogInformation("Начат процесс трансформации в хендлере. Url: {Url}", url);

			var transformResult = new TransformationResult
			{
				IsSuccess = true,
				ExtractedData = (null, null),
				NewTasks = new List<(string Url, ScrapingTaskType Type)>(),
				UpdatedTaskType = ScrapingTaskType.Catalog
			};

			try
			{
				var links = await ZoobazarLinksExtractor.ExtractLinksAsync(content, "https://zoobazar.by");
				int totalLinks = links?.Count() ?? 0;
				logger.LogInformation("Извлечено ссылок: {Count} для Url: {Url}", totalLinks, url);

				if (totalLinks > 0)
				{
					foreach (var link in links)
					{
						if (link.Contains("://zoobazar"))
						{
							transformResult.NewTasks.Add(new(link, ScrapingTaskType.Unknown));
							logger.LogDebug("Добавлена новая задача: Url={Link}, Type={Type}", link, ScrapingTaskType.Unknown);
						}
						else
						{
							logger.LogDebug("Пропущена ссылка (не соответствует хосту): {Link}", link);
						}
					}
				}
				else
				{
					logger.LogWarning("Не найдены ссылки на странице. Url: {Url}", url);
				}

				// Determine current page from PAGEN_2 if present
				var match = Regex.Match(url, @"PAGEN_2=(\d+)", RegexOptions.IgnoreCase);
				int page = match.Success ? int.Parse(match.Groups[1].Value) : 1;
				logger.LogDebug("Текущий номер страницы (parsed): {Page} (match.Success={MatchSuccess})", page, match.Success);

				int newPageNumber = page + 1;

				// Build next catalog link using a dedicated method that never uses regex replacements.
				// This guarantees we don't accidentally interpret replacement patterns like "$13".
				var nextCatalogLink = BuildNextCatalogLink(url, newPageNumber);

				if (match.Success)
				{
					logger.LogDebug("Обновлён параметр PAGEN_2 в Url. Новая ссылка: {NextCatalogLink}", nextCatalogLink);
				}
				else
				{
					logger.LogDebug("PAGEN_2 не найден в Url. Сформирована следующая страница: {NextCatalogLink}", nextCatalogLink);
				}

				transformResult.NewTasks.Add(new(nextCatalogLink, ScrapingTaskType.Catalog));
				logger.LogInformation("Добавлена задача листинга для следующей страницы: {NextCatalogLink}", nextCatalogLink);

				sw.Stop();
				logger.LogInformation("Обработка {Url} завершена. Время выполнения: {ElapsedMs} ms. Всего задач: {TaskCount}",
					url, sw.ElapsedMilliseconds, transformResult.NewTasks.Count);


				return transformResult;
			}
			catch (Exception ex)
			{
				sw.Stop();
				logger.LogError(ex, "Ошибка при обработке Url: {Url}. Время до ошибки: {ElapsedMs} ms", url, sw.ElapsedMilliseconds);
				transformResult.IsSuccess = false;
				return transformResult;
			}
		}
		private static string BuildNextCatalogLink(string url, int newPageNumber)
		{
			// Preserve fragment (#...) if present
			var baseUri = url;
			var fragment = "";
			var fragIndex = url.IndexOf('#');
			if (fragIndex >= 0)
			{
				fragment = url.Substring(fragIndex);
				baseUri = url.Substring(0, fragIndex);
			}

			// Separate path and existing query
			var pathIndex = baseUri.LastIndexOf('/');
			var paramIndex = baseUri.IndexOf('?');
			var path = pathIndex >= 0 ? baseUri.Substring(0, pathIndex+1) : baseUri;
			var existingQuery = paramIndex >= 0 ? baseUri.Substring(paramIndex) : "";

			// ParseQuery accepts a string that may start with '?'
			var parsed = QueryHelpers.ParseQuery(existingQuery);

			// Copy into mutable dictionary<string, string>
			var dict = parsed.ToDictionary(k => k.Key, k => (string)k.Value.ToString(), StringComparer.OrdinalIgnoreCase);

			// Set or overwrite PAGEN_2
			dict["PAGEN_2"] = newPageNumber.ToString();

			// Build final URL and append preserved fragment
			var next = QueryHelpers.AddQueryString(path, dict) + fragment;

			return next;
		}
	}
}
