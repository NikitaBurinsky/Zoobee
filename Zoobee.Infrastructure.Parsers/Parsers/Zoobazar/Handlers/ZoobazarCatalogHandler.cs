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
using System.Globalization;

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
			if (!url.Contains("zoobazar.by/catalog")) return false;
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
				List<string> otherLinks, linksFromCatalog;
				ExtractLinksFromCatalogAsync(content, "zoobazar.by", out linksFromCatalog, out otherLinks);

				int totalLinks = linksFromCatalog?.Count() + otherLinks?.Count() ?? 0;

				if (totalLinks > 0)
				{
					
					var addedCount = SaveNewLinksTasks(transformResult, linksFromCatalog, ScrapingTaskType.ProductPage);
					logger.LogInformation("Добавлено ссылок на продукты в каталоге: {AddedCount}", addedCount);
					logger.LogTrace(addedCount > 0 ? "Ссылки на продукты: {Links}" : "Нет ссылок на продукты", string.Join(",\n ", linksFromCatalog));	
					addedCount = SaveNewLinksTasks(transformResult, otherLinks, ScrapingTaskType.Unknown);
					logger.LogInformation("Добавлено иных ссылок: {AddedCount}", addedCount);
					logger.LogTrace(addedCount > 0 ? "Ссылки на иные страницы: {Links}" : "Нет иных ссылок", string.Join(",\n ", otherLinks));	

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

		private int SaveNewLinksTasks(TransformationResult transformResult, List<string> linksFromCatalog, ScrapingTaskType currentTaskType)
		{
			int c = 0;
			foreach (var link in linksFromCatalog)
			{
				//TODO Здесь отсеиваются ссылки по путям. В будущем расширить и\или перенести фильтрацию ссылок в другое место
				//Также здесь и отсеиваются ссылки на shops и прочие, нужные в будущем приблуды
				if (link.Contains("://zoobazar.by/catalog"))
				{
					transformResult.NewTasks.Add(new(link, currentTaskType));
					++c;
					logger.LogDebug("В результат транформации добавлена новая задача: Url={Link}, Type={Type}", link, currentTaskType);
				}
				else
				{
					//logger.LogDebug("Пропущена ссылка (не соответствует хосту): {Link}", link);
				}
			}
			return c;
		}

		private void ExtractLinksFromCatalogAsync(string content, string baseUrl, out List<string> catalogLinks, out List<string> otherLinks)
		{
			catalogLinks = new List<string>();
			otherLinks = new List<string>();

			if (string.IsNullOrWhiteSpace(content))
			{
				logger.LogDebug("Empty content passed to ExtractLinksFromCatalogAsync");
				return;
			}

			// Parse document
			var doc = new HtmlDocument();
			doc.LoadHtml(content);

			// Build a base Uri that can be used to resolve relative links.
			// If baseUrl contains no scheme, default to https.
			Uri baseUri;
			try
			{
				if (Uri.TryCreate(baseUrl, UriKind.Absolute, out baseUri) == false)
				{
					// treat as host (e.g. "zoobazar.by") or path
					var normalized = baseUrl.StartsWith("http://", StringComparison.OrdinalIgnoreCase) ||
									 baseUrl.StartsWith("https://", StringComparison.OrdinalIgnoreCase)
						? baseUrl
						: "https://" + baseUrl.Trim();
					baseUri = new Uri(normalized);
				}
			}
			catch (Exception ex)
			{
				logger.LogWarning(ex, "Failed to create base URI from '{BaseUrl}', fallback to https://zoobazar.by", baseUrl);
				baseUri = new Uri("https://zoobazar.by/");
			}

			// Extract product links (only anchors that are inside a div.product)
			var productHrefSet = ExtractProductLinksFromDocument(doc, baseUri);

			// Extract all other links and exclude product links
			var allHrefSet = ExtractAllLinksFromDocument(doc, baseUri);

			// Remove product links from other links
			allHrefSet.ExceptWith(productHrefSet);

			catalogLinks = productHrefSet.ToList();
			otherLinks = allHrefSet.ToList();

			logger.LogDebug("ExtractLinksFromCatalogAsync -> product links: {ProductCount}, other links: {OtherCount}", catalogLinks.Count, otherLinks.Count);
		}

		/// <summary>
		/// Extracts absolute product links from the document. Only anchors that are descendants of a node
		/// with class 'product' are considered product links.
		/// Returns deduplicated set of absolute URLs.
		/// </summary>
		private HashSet<string> ExtractProductLinksFromDocument(HtmlDocument doc, Uri baseUri)
		{
			var result = new HashSet<string>(StringComparer.OrdinalIgnoreCase);

			// Регулярное выражение, которое ищет div с классом product и извлекает href из ссылок внутри него
			string pattern = @"<div\s+[^>]*class\s*=\s*['""]?product['""]?[^>]*>.*?<a\s+[^>]*href\s*=\s*['""]([^'""]+)['""][^>]*>.*?</div>";
			var matches = Regex.Matches(doc.Text, pattern, RegexOptions.IgnoreCase | RegexOptions.Singleline);

			foreach (Match match in matches)
			{
				if (match.Groups.Count > 1 && !string.IsNullOrWhiteSpace(match.Groups[1].Value))
				{
					var href = match.Groups[1].Value.Trim();
					var resolved = TryResolveHref(href, baseUri);
					if (resolved != null)
					{
						result.Add(resolved);
					}
				}
			}

			return result;
		}

		/// <summary>
		/// Extracts absolute links from all anchors in the document.
		/// Returns deduplicated set of absolute URLs.
		/// </summary>
		private HashSet<string> ExtractAllLinksFromDocument(HtmlDocument doc, Uri baseUri)
		{
			var result = new HashSet<string>(StringComparer.OrdinalIgnoreCase);

			var anchors = doc.DocumentNode.SelectNodes("//a[@href]");
			if (anchors == null) return result;

			foreach (var a in anchors)
			{
				var href = a.GetAttributeValue("href", null);
				if (string.IsNullOrWhiteSpace(href)) continue;

				var resolved = TryResolveHref(href, baseUri);
				if (resolved != null)
				{
					result.Add(resolved);
				}
			}

			return result;
		}

		/// <summary>
		/// Resolves an href value against the provided baseUri and normalizes results.
		/// Returns null for non-http(s) schemes like javascript:, mailto:, or pure fragments.
		/// </summary>
		private static string? TryResolveHref(string href, Uri baseUri)
		{
			href = href.Trim();

			// Skip fragments, javascript, mailto and data URIs
			if (href.StartsWith("#", StringComparison.Ordinal) ||
				href.StartsWith("javascript:", StringComparison.OrdinalIgnoreCase) ||
				href.StartsWith("mailto:", StringComparison.OrdinalIgnoreCase) ||
				href.StartsWith("data:", StringComparison.OrdinalIgnoreCase))
			{
				return null;
			}

			// Protocol-relative (//host/path) -> add scheme from baseUri
			if (href.StartsWith("//", StringComparison.Ordinal))
			{
				return $"{baseUri.Scheme}:{href}";
			}

			// Absolute URI
			if (Uri.TryCreate(href, UriKind.Absolute, out var abs))
			{
				// only accept http/https absolute links
				if (abs.Scheme == Uri.UriSchemeHttp || abs.Scheme == Uri.UriSchemeHttps)
				{
					return abs.GetLeftPart(UriPartial.Path) + (string.IsNullOrEmpty(abs.Query) ? "" : abs.Query) + (string.IsNullOrEmpty(abs.Fragment) ? "" : abs.Fragment);
				}
				return null;
			}

			// Relative URI -> resolve against baseUri
			try
			{
				if (Uri.TryCreate(baseUri, href, out var resolved))
				{
					if (resolved.Scheme == Uri.UriSchemeHttp || resolved.Scheme == Uri.UriSchemeHttps)
					{
						return resolved.GetLeftPart(UriPartial.Path) + (string.IsNullOrEmpty(resolved.Query) ? "" : resolved.Query) + (string.IsNullOrEmpty(resolved.Fragment) ? "" : resolved.Fragment);
					}
				}
			}
			catch
			{
				// ignore resolution failures
			}

			return null;
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
