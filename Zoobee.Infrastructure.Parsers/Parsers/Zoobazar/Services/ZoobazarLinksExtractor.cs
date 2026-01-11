using HtmlAgilityPack;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Zoobee.Infrastructure.Parsers.Parsers.Zoobazar.Services
{
	public static class ZoobazarLinksExtractor
	{
		public static Task<List<string>> ExtractLinksAsync(string content, string baseurl)
		{
			if (string.IsNullOrWhiteSpace(content))
				return Task.FromResult(new List<string>());

			var uniqueUrls = new HashSet<string>();
			var doc = new HtmlDocument();
			doc.LoadHtml(content);

			var nodes = doc.DocumentNode.SelectNodes("//a[@href]");

			if (nodes == null)
				return Task.FromResult(new List<string>());

			// Создаем объект Uri из базового URL для корректного объединения
			// Если baseurl пришел без протокола (напр. "zoobazar.by"), добавляем https://
			if (!baseurl.StartsWith("http", StringComparison.OrdinalIgnoreCase))
			{
				baseurl = "https://" + baseurl;
			}

			if (!Uri.TryCreate(baseurl, UriKind.Absolute, out Uri baseUri))
			{
				// Если базовый URL некорректен, мы не сможем восстановить относительные ссылки
				return Task.FromResult(new List<string>());
			}

			foreach (var node in nodes)
			{
				string href = node.GetAttributeValue("href", null);

				if (string.IsNullOrWhiteSpace(href)) continue;

				// Декодируем HTML сущности (например, &amp; -> &)
				href = System.Net.WebUtility.HtmlDecode(href.Trim());

				// Игнорируем ссылки-якоря (#), javascript, mailto и tel
				if (href.StartsWith("#") ||
					href.StartsWith("javascript:", StringComparison.OrdinalIgnoreCase) ||
					href.StartsWith("mailto:", StringComparison.OrdinalIgnoreCase) ||
					href.StartsWith("tel:", StringComparison.OrdinalIgnoreCase))
				{
					continue;
				}

				try
				{
					// 2. Магия класса Uri: объединяем базу и найденную ссылку
					// Это автоматически обрабатывает:
					// - Абсолютные ссылки (https://google.com) -> остаются как есть
					// - Относительные от корня (/catalog) -> https://base.com/catalog
					// - Относительные от текущей папки (page.html) -> https://base.com/sub/page.html
					Uri combinedUri = new Uri(baseUri, href);

					uniqueUrls.Add(combinedUri.AbsoluteUri);
				}
				catch (UriFormatException)
				{
					// Игнорируем битые URL
				}
			}

			return Task.FromResult(uniqueUrls.ToList());
		}
	}






}

