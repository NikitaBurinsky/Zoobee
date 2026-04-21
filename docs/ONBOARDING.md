# Onboarding Path (Human + AI)

Связанные записи: [ADR-0001](./ADR/0001-documentation-system.md), backlog `DOC-001`, `DOC-002`.

## Цель

За 60-90 минут новый участник должен:

- понять текущий runtime-контур,
- объяснить пайплайн парсинга от скачивания до сохранения,
- знать, где и как фиксировать решения/долги,
- уметь безопасно вносить изменения с учетом docs-gate.

## Обязательный маршрут

1. Прочитать [System Map](./SYSTEM_MAP.md) и [Assumptions And Constraints](./ASSUMPTIONS_AND_CONSTRAINTS.md).
2. Прочитать [Runtime Contour](./RUNTIME_CONTOUR.md).
3. Прочитать [Parsers Pipeline](./PARSERS_PIPELINE.md).
4. Прочитать [Conventions](./CONVENTIONS/NAMING.md) и [Errors And Messages](./CONVENTIONS/ERRORS_AND_MESSAGES.md).
5. Прочитать [Risks And Debt](./RISKS_AND_DEBT.md) и [Documentation Backlog](./BACKLOG/DOCUMENTATION_BACKLOG.md).
6. Прочитать [Documentation Policy](./GOVERNANCE/DOCUMENTATION_POLICY.md) и [ADR README](./ADR/README.md).

## Минимальный чек после чтения

- Назвать текущий entrypoint приложения (`Zoobee.0.1/Program.cs`).
- Назвать где зарегистрированы hosted workers парсеров.
- Назвать 3 текущих технических риска из [RISKS_AND_DEBT.md](./RISKS_AND_DEBT.md).
- Создать или обновить backlog item при найденной неясности.

## Для AI-агентов

Стартовая точка: [../AGENT_GUIDE.md](../AGENT_GUIDE.md).
