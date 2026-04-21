# Naming Conventions

Связанные записи: [ADR-0001](../ADR/0001-documentation-system.md), backlog `DOC-007`, `DOC-023`.

## Базовые правила

- Сущности БД: `*Entity` (пример: `FoodProductEntity`).
- DTO: `*Dto` (пример: `FoodProductDto`).
- Профили маппинга: `*MappingProfile` / `*UpdateSpecInfoProfile`.
- Репозитории: `*Repository`, UoW: `*UnitOfWork`.
- Константы ролей/политик: только через `Zoobee.Application.Shared.Constants`.

## Правило для новых модулей

- Новые namespaces только `Zoobee.*`.
- Новые файлы и папки без опечаток и без смешения `Environtment/Environment`, `Repositoties/Repositories`, `Autorization/Authorization`.

## Текущее состояние (долг)

- В коде присутствуют legacy опечатки и rename-следы:
  - `ZooStores.*` namespaces;
  - `Environtment`, `Repositoties`, `Autorization` в путях/типах.

До полного cleanup эти отклонения документируются как `Legacy` и не используются как шаблон для нового кода.

## Legacy entrypoint

Исторический документ: `Zoobee.Core/docs/Naming-Policy.md` (теперь только ссылка на этот файл).
