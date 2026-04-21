# Configuration Convention

Связанные записи: [ADR-0001](../ADR/0001-documentation-system.md), backlog `DOC-015`, `DOC-016`, `DOC-026`.

## Основные секции конфигурации (текущее)

- `ConnectionStrings`
- `Serilog`
- `Parsers:Scraping`
- `FileStorage`
- `ScrapingSeeding`
- `APIs:YandexGeoCoder`
- `CountryBorders`

## Правила

- Локальная и средовая конфигурация разделяются (`appsettings.*` + env vars).
- Секреты не хранятся в repo-config.
- Для новых секций обязательно:
  - краткое назначение;
  - обязательные ключи;
  - дефолт;
  - пример env override.

## Критичные наблюдения

- В текущем `appsettings.json` присутствуют чувствительные значения.
- Startup зависит от `Admin:Email` и `Admin:Password`, но эти ключи не задокументированы в конфиге по умолчанию.
- Есть абсолютные пути, завязанные на локальную машину.
