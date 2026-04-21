# Future Foundations

Связанные записи: [Decision Log](../DECISION_LOG.md), backlog `DOC-031`, `DOC-032`, `DOC-033`.

## Фундамент, заложенный в текущем пакете docs

- Централизованный docs hub + трассируемость.
- Формализованный ADR процесс.
- Риск-регистр и backlog для управляемой эволюции.
- Системная карта с lifecycle-статусами.

## Ближайшие foundation-workstreams

1. Стабилизация parser failure recovery и типизированного product update.
2. Приведение identity/auth контракта к единому состоянию (roles/policies/users).
3. Перенос секретов в безопасное хранилище и чистка конфигов.
4. Выравнивание legacy namespaces/путей после rename.

## Условие readiness к активному росту

- Стабильная CI валидация build/test/docs.
- Закрыты high/critical риски `R-001...R-007`.
- Для ключевых модулей есть актуальные runbook + ownership.
