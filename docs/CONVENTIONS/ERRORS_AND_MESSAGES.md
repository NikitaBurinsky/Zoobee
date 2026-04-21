# Errors And Messages Convention

Связанные записи: [ADR-0001](../ADR/0001-documentation-system.md), backlog `DOC-008`, `DOC-025`.

## Формат ключей ошибок

Целевой формат:

`Error.{DomainContext}.{OptionalField}.{ErrorName}`

Примеры из кода:

- `Error.CreatorCompanies.CreatorCompanyNotFound`
- `Error.MediaStorage.FileNotFound`
- `Error.FailedParsedTasks.TaskNotFound`

## Правила

- Пользовательские ошибки возвращаются через `OperationResult.Error(...)`.
- Внешние ответы API не должны содержать отладочные/нецензурные тексты.
- Для новых сообщений:
  - сначала ключ ресурса,
  - затем локализованное значение,
  - затем mapping к HTTP status.

## Текущий долг

- Есть `TODO`/raw messages в production-коде (например в `ProductsInfoService`).
- Есть смешение локализованных и нелокализованных сообщений.
- Для MVP fallback ключей допустим временно, но новые критичные ошибки должны иметь ресурсный ключ.
