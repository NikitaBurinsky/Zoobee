# Risks And Debt Register

Связанные записи: [ADR-0001](./ADR/0001-documentation-system.md), backlog `DOC-014`..`DOC-020`.

## Реестр

| ID | Риск / долг | Подтверждение | Влияние | Приоритет | Текущий статус | Связанный backlog |
| --- | --- | --- | --- | --- | --- | --- |
| R-001 | `dotnet build` может падать без явных CS/MSB ошибок в summary | `dotnet build Zoobee.0.1/Zoobee.0.3.sln` | Невозможность надежной валидации изменений | High | Open | `DOC-014` |
| R-002 | Startup содержит `#define DEV` и абсолютный путь до `countries.json` | `Zoobee.0.1/Program.cs` | Непереносимый запуск | High | Open | `DOC-015` |
| R-003 | Секреты и креды в `appsettings.json` | `Zoobee.0.1/appsettings.json` | Утечка секретов/риск компрометации | Critical | Open | `DOC-016` |
| R-004 | Несоответствие policy constants (`DatabaseAdministration` vs `DatabaseAdmin`) | `PolicyNames.cs`, `AuthorizationPoliciesExtensions.cs` | Потенциальная ошибка авторизации/политик | High | Open | `DOC-017` |
| R-005 | `OrganisationUser` используется в конфигурации/discriminator, но тип не найден в core users | `BaseApplicationUser.cs`, `ZoobeeAppDbContext.cs` | Риск миграций и identity контракта | High | Planned (postponed) | `DOC-018` |
| R-006 | Типизированный update продукта содержит `NotImplementedException` | `ProductsInfoService.cs`, `ToiletProductUpdateSpecInfoProfile.cs` | Неустойчивый pipeline сохранения данных | High | Open (WIP) | `DOC-019` |
| R-007 | Failed parsing recovery контур частично несогласован | `TransformationService.cs`, `FailedParsingTasksService.cs` | Потери данных/ручные сбои при восстановлении | High | Experimental | `DOC-020` |
| R-008 | Legacy namespace `ZooStores.*` в действующих контроллерах | `Zoobee.0.1/Area/*` | Путаница при онбординге и рефакторинге | Medium | Open | `DOC-023` |
| R-009 | Наличие stale solution `ZoobeeParsers.sln` с историческими путями | `Zoobee.Infrastructure.Parsers/ZoobeeParsers.sln` | Ошибочные точки входа для новых участников | Medium | Open | `DOC-024` |
| R-010 | Локализационные ключи/ресурсы не покрывают все вызовы; fallback допустим только временно | `OperationResult.Error(...)`, `LocalizationResources/*.resx` | Нестабильные клиентские сообщения | Medium | Accepted for MVP | `DOC-025` |
| R-011 | В `IdentityRolesSeeding` обязательны `Admin:Email/Password`, но секция отсутствует в `appsettings.json` | `IdentityRolesSeeding.cs`, `appsettings.json` | Падение старта в окружениях без env overrides | High | Open | `DOC-026` |

## Правило работы с долгом

- Новый выявленный долг сразу получает `ID` и backlog item.
- Закрытие долга требует: изменение кода + update docs + статус в backlog.
