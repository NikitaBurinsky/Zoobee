using System.Collections.Generic;
using Zoobee.Infrastructure.Parsers.Core.Enums;

namespace Zoobee.Infrastructure.Parsers.Core.Transformation
{
	public class TransformationResult
	{
		public bool IsSuccess { get; set; } = true;
		public string ErrorMessage { get; set; }

		/// <summary>
		/// Новые задачи для краулера (например, найденные ссылки в sitemap или пагинации).
		/// </summary>
		public List<(string Url, ScrapingTaskType Type)> NewTasks { get; set; } = new();

		/// <summary>
		/// Извлеченные бизнес-данные.
		/// Может быть BaseProductDto, FoodProductDto, List<TagDto> и т.д.
		/// </summary>
		public object? ExtractedData { get; set; }
	}
}