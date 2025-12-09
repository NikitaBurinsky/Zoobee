# Архитектура и Потоки данных (Workflows)

## 1. Общий цикл парсинга (The Scraping Loop)

Система работает по циклическому принципу. Задачи (`ScrapingTask`) никогда не удаляются, они лишь обновляют свой статус и время следующего запуска.

## 2. Жизненный цикл задачи (Task Lifecycle)
Каждый URL проходит через следующие этапы:

Seeding / Discovery: URL попадает в базу (статус Pending, тип Sitemap или ListingPage).

Processing: Воркер берет задачу, когда наступает время NextTryAt.

Execution:

Успех (200 OK): Сохраняется snapshot HTML. Статус обновляется, NextTryAt сдвигается вперед (например, на 24 часа).

Ошибка (4xx, 5xx): Счетчик попыток (AttemptCount) растет. NextTryAt сдвигается на короткий промежуток (Exponential Backoff).

Transformation: Если скачан новый контент, запускается анализ:

Если это Sitemap: извлекаются новые ссылки -> создаются новые ScrapingTask.

Если это ProductPage: извлекаются данные о товаре -> обновляется основной каталог.

## 3. Разделение ответственности
ScrapingRepository: Только работа с БД (сохранение, выборка очереди). Не знает бизнес-логики.

ScrapingSeeder: Отвечает только за инициализацию начальных данных при старте.

```mermaid
graph TD
    A[Start / Config] -->|Seeder| B(Database: ScrapingTasks)
    B -->|GetPendingTasks| C{Scraping Worker}
    C -->|Download HTML| D[ScrapingData History]
    D -->|Update Task Status| B
    
    subgraph Transformation Logic
    D -->|New Content Found| E[Transformation Worker]
    E -->|Extract Links| B
    E -->|Extract Product Info| F[Main Database]
    end