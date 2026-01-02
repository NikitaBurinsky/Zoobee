# Общая иерархия трансформеров

	- TransformationWorker - Хост процесса трансформации скачанных данных в требуемый нам вид
	- IWebPageTransformer's - Классы обработки конкретных сайтов (прим. ZoobazarTransformer)
	- IResourceHandler's - Обработчики конкретных типов страницы внутри трансформеров сайтов. Они и занимаются выделением полезной информации со страниц, а именно
		информации о продуктах, информацию об офферах, и ссылок на другие страницы сайта, для дальнейшей их обработки (прим. ZoobazarTransformer -> ZoobazarFoodHandler, ZoobazarSitemapHandler...) 

	- TransformationService - сервис, инкапсулирующий и обьединяющий работу всего преобразования
	- TransformationResolver - сервис, используемый TransformationService для выбора соответсвующего странице IWebPageTransformer'a


# Как добавить новый сайт в систему Scraping

Откройте appsettings.json.

Найдите массив Sources.

Добавьте новый объект с именем источника и ссылками.

Перезапустите приложение. ScrapingSeeder проверит наличие ссылок в БД и добавит только отсутствующие. Дубликаты игнорируются.

# Как добавить новый сайт и страницы в систему Transformation

Создать класс реализующий интерфейс IWebPageTransformer для конкретного сайта. В свойстве string TargetSourceName указать название сайта. 
Внутри конструктора выбрать из DI контейнера IResourceHandler для этого же сайта (прим. _handlers = handlers.Where(h => h.TargetSourceName == "Zoobazar");)

Создать классы реализующие интерфейсы IResourceHandler для каждого типа страницы на сайте (такие как товар-корм, sitemap, товар-туалет...). В свойстве TargetSourceName указать название сайта

Зарегистрировать трансформеров и обработчиков в InfrastructureParsersLayerBuilding.cs;
