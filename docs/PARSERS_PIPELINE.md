# Parsers Pipeline (Wave 1 Priority)

Связанные записи: [ADR-0001](./ADR/0001-documentation-system.md), backlog `DOC-005`, `DOC-014`, `DOC-016`.

## Контур

- Scraping выполняется `ScrapingWorker`.
- Transformation выполняется `TransformationWorker`.
- Storage:
  - очереди/история scraping — `ParsersDbContext` (`ScrapingTasks`, `ScrapingDatas`);
  - сохранение в основной каталог — через сервисы `Infrastructure` и `IProductsUnitOfWork`.

## E2E поток

```mermaid
flowchart LR
    A[ScrapingSeeder] --> B[ScrapingTasks: Pending]
    B --> C[ScrapingWorker]
    C --> D[DownloadResult]
    D --> E[ScrapingDatas history]
    D --> F[Task status update]
    F --> G[Downloaded tasks]
    G --> H[TransformationWorker]
    H --> I[TransformerResolver]
    I --> J[IWebPageTransformer + IResourceHandler]
    J --> K[Extracted Product/Slot/New URLs]
    K --> L[BulkAddTasksAsync]
    K --> M[ProductsInfoService + SellingSlotsInfoService]
    M --> N[Main DB]
    M --> O[FailedParsingTasksService]
```

## Фактические статусы по подсистемам

- `Scraping loop`: `Active`.
- `Transformation routing`: `Active`.
- `Failed parsing recovery`: `Experimental/WIP`.

## Наблюдаемые пробелы (зафиксировано как долг)

- Сигнатуры вызовов `CreateResolvationTask_*` в transformation и сервисе failure handling не полностью согласованы.
- В create-методах failure service создание сущностей не всегда заканчивается явным persist через repository.
- Не все типизированные ветки product update завершены (`NotImplementedException` в основном потоке).

## Операционный вывод

Для MVP pipeline пригоден как основа, но контур восстановления ошибок и типизированного update должен считаться нестабильным и быть под ручным контролем.
