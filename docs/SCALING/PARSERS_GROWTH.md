# Scaling Playbook: Parsers Growth

Связанные записи: [Parsers Pipeline](../PARSERS_PIPELINE.md), [System Map](../SYSTEM_MAP.md), backlog `DOC-029`, `DOC-030`.

## Масштабирование источников (текущий контур)

1. Добавить источник в `ScrapingSeeding` (`SourceName`, `Sitemaps`, `StartUrls`).
2. Добавить/обновить `IWebPageTransformer` для нового source.
3. Добавить `IResourceHandler` для типов страниц.
4. Зарегистрировать обработчики и трансформер в `InfrastructureParsersLayerBuilding`.
5. Прогнать smoke tests на sample URLs.

## Масштабирование нагрузкой

- Тюнить scraping delays (`RequestDelayMs`, `NoTasksDelayMs`).
- Контролировать рост очереди `ScrapingTasks`.
- Отдельно мониторить долю failed transformations.

## Переход к отдельному сервису (стратегический путь)

Текущий путь:

- `Zoobee.0.1` host держит parser workers in-process.

Целевой путь (Planned):

1. Вынести parser host и `ParsersDbContext` в отдельный deployable service.
2. Ввести контракт обмена (events/API) между parser service и main catalog.
3. Разделить observability и operational runbooks по сервисам.
