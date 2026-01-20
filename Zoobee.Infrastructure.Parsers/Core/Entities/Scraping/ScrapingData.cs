using System;
using Zoobee.Application.DtoTypes.Base;

namespace Zoobee.Infrastructure.Parsers.Core.Entities
{
	public class ScrapingData : BaseEntity
	{
		public Guid Id { get; set; }
		public Guid ScrapingTaskId { get; set; }
		public ScrapingTask ScrapingTask { get; set; }

		// Контент может быть null, если была ошибка сети, но мы хотим записать факт попытки
		public string? Content { get; set; }

		public int HttpStatusCode { get; set; }
		public string? ErrorMessage { get; set; }

		// CreatedAt наследуется от BaseEntity - это и есть дата снапшота
	}
}