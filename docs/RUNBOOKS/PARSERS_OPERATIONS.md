# Runbook: Parsers Operations

Связанные записи: [Parsers Pipeline](../PARSERS_PIPELINE.md), [Parsers Growth](../SCALING/PARSERS_GROWTH.md), backlog `DOC-005`, `DOC-020`.

## Цель

Операционно контролировать scraping/transformation loop и восстанавливать сбои.

## Нормальный цикл

1. `ScrapingWorker` берет `Pending` задачи, скачивает страницу, пишет history.
2. `TransformationWorker` обрабатывает `Downloaded` задачи.
3. Новые URL добавляются через `BulkAddTasksAsync`.
4. Product/slot сохраняются в основной каталог.

## Минимальные operational checks

- В логах есть циклы `Worker started`/обработка URL.
- В `ScrapingTasks` обновляются `Status`, `NextTryAt`, `AttemptCount`.
- В `ScrapingDatas` появляются новые snapshots.

## При росте очереди и падении throughput

- Проверить `Parsers:Scraping:RequestDelayMs` и `NoTasksDelayMs`.
- Проверить долю задач со статусами `Failed`/`NotFound`.
- Проверить узкие места в transformation handlers.

## Ограничение текущей версии

Контур failed parsing recovery помечен `Experimental`; при массовых ошибках требуется ручной контроль и фиксация инцидента в backlog.
