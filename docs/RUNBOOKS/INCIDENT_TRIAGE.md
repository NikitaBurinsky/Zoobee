# Runbook: Incident Triage

Связанные записи: [Risks And Debt](../RISKS_AND_DEBT.md), [Documentation Backlog](../BACKLOG/DOCUMENTATION_BACKLOG.md), backlog `DOC-027`.

## Severity

- `SEV-1`: сервис недоступен или потеря данных.
- `SEV-2`: критичный бизнес-поток деградирован.
- `SEV-3`: частичный сбой/локальная деградация.

## Шаги triage

1. Зафиксировать:
   - время инцидента;
   - affected контур;
   - симптом.
2. Проверить логи host + workers.
3. Проверить состояние БД (задачи, ключевые таблицы).
4. Определить, можно ли безопасно смягчить (config rollback/restart/feature toggle).
5. Создать backlog item и ссылку на risk ID (или новый risk ID).

## Post-incident требования

- Обновить документацию по фактической причине и workaround.
- Если решение архитектурное — оформить ADR.
- Если найден новый системный риск — добавить в `RISKS_AND_DEBT.md`.
