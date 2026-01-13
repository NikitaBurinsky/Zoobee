namespace Zoobee.Infrastructure.Parsers.Core.Enums
{
	public enum ScrapingTaskType
	{
		Unknown = 0,
		ProductPage = 1,  // Страница товара
		Catalog = 2,  // Категория/Листинг товаров
		Sitemap = 3       // XML или HTML карта сайта
	}
}