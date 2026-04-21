# Runbook: Local Startup

Связанные записи: [Runtime Contour](../RUNTIME_CONTOUR.md), [Configuration](../CONVENTIONS/CONFIGURATION.md), backlog `DOC-011`, `DOC-014`, `DOC-026`.

## Предпосылки

- Windows + .NET SDK (проект target `net8.0`).
- Доступ к PostgreSQL для `DefaultConnection` и `ParsersDatabaseConnection`.
- Заполненные `Admin:Email` и `Admin:Password` через env vars/секреты.

## Рекомендуемая последовательность

1. Проверить SDK:
   - `dotnet --info`
2. Верифицировать конфиг:
   - connection strings доступны;
   - admin credentials заданы;
   - чувствительные ключи не в явном виде в commit.
3. Запустить:
   - `dotnet run --project Zoobee.0.1/Zoobee.Web.csproj`
4. Проверить health smoke:
   - swagger endpoint;
   - логи старта `RolesSeeding` и parser workers.

## Известные особенности

- В `Program.cs` есть `#define DEV` и seed countries через абсолютный путь.
- В текущем окружении `dotnet build` может завершаться ошибкой без явных CS-errors в summary (см. risk `R-001`).

## Что делать при падении на старте

- Проверить наличие `Admin:Email`/`Admin:Password`.
- Проверить доступность обеих БД.
- Проверить абсолютные пути в startup и Serilog file sink.
