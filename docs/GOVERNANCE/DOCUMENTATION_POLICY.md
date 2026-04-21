# Documentation Policy

Связанные записи: [ADR-0001](../ADR/0001-documentation-system.md), backlog `DOC-034`.

## Обязательные правила

- `/docs` — единственный канонический источник документации.
- Любой PR с изменением API/архитектуры/конвенций обязан обновить docs.
- Для значимых решений обязателен ADR.
- Нельзя закрывать PR, если docs-impact section не заполнен.

## Что считается docs-impact change

- Изменение public endpoint/контракта DTO.
- Изменение runtime wiring/DI/startup.
- Изменение conventions (errors/logging/config/security).
- Изменение operational поведения (workers, retries, seed logic).

## Минимальный комплект обновлений при docs-impact

1. Изменить релевантную страницу в `/docs`.
2. Обновить backlog status или создать новый item.
3. Если решение архитектурное — создать ADR.
