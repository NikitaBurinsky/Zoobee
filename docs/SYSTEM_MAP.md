# System Map (Active/Legacy/Stale/Experimental/Planned)

Связанные записи: [ADR-0001](./ADR/0001-documentation-system.md), backlog `DOC-003`, `DOC-012`.

## Карта подсистем

| Подсистема | Статус | Назначение | Подтверждение |
| --- | --- | --- | --- |
| `Zoobee.0.1` | `Active` | Web host + API + startup orchestration | `Zoobee.0.1/Program.cs` |
| `Zoobee.Application` | `Active` | Application layer, DTO, contracts, validators | `Zoobee.Application/*` |
| `Zoobee.Application.Shared` | `Active` | Shared DTO/constants/contracts | `Zoobee.Application.Shared/*` |
| `Zoobee.Core` | `Active` | Domain entities/primitives/localization resources | `Zoobee.Core/*` |
| `Zoobee.Infrastructure` | `Active` | EF Core, repositories, services, product matching | `Zoobee.Infrastructure/*` |
| `Zoobee.Infrastructure.Parsers` | `Active + Experimental` | Scraping/transformation workers и parser data model | `Zoobee.Infrastructure.Parsers/*` |
| Parser failed-task handling | `Experimental` | Ручное/полуавтоматическое восстановление failed parsing | `Zoobee.Infrastructure.Parsers/Services/System/FailedParsingTasksService.cs` |
| Admin CRUD API | `Legacy + WIP` | Часть CRUD и parsing admin endpoints | `Zoobee.0.1/Area/Admin/*` |
| `Zoobee.Test` | `Active (partial coverage)` | Unit + integration tests | `Zoobee.Test/*` |
| `Zoobee.Infrastructure.Parsers/ZoobeeParsers.sln` | `Stale` | Исторический solution с устаревшими путями | `Zoobee.Infrastructure.Parsers/ZoobeeParsers.sln` |
| Корневая папка `Zoobee.Web` | `Legacy + Stale` | Исторический артефакт (`obj`), не основной runtime | `Zoobee.Web/obj/*` |
| Namespace `ZooStores.*` в части контроллеров | `Legacy` | След после rename, не синхронно с `Zoobee.*` | `Zoobee.0.1/Area/*` |

## Planned (зафиксированные направления)

- Выделение `Infrastructure.Parsers` в отдельный сервисный контур.
- Завершение типизированного контура обновления продуктов (WIP-методы).

## Явно неканоничные источники

- Любые документы вне `/docs` считаются entrypoint/legacy, если не указано обратное.
