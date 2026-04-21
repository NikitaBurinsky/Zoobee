# AGENT_GUIDE (Zoobee)

Этот документ — входная точка для AI-агентов.

## 1) Обязательный старт

1. Прочитать [docs/README.md](./docs/README.md).
2. Прочитать [docs/ONBOARDING.md](./docs/ONBOARDING.md).
3. Прочитать [docs/SYSTEM_MAP.md](./docs/SYSTEM_MAP.md).
4. Прочитать [docs/RISKS_AND_DEBT.md](./docs/RISKS_AND_DEBT.md).
5. Проверить [docs/BACKLOG/DOCUMENTATION_BACKLOG.md](./docs/BACKLOG/DOCUMENTATION_BACKLOG.md).

## 2) Контракты работы

- Source of truth: только `/docs`.
- Для неясностей: [Ambiguity Protocol](./docs/GOVERNANCE/AMBIGUITY_PROTOCOL.md).
- Для значимых решений: ADR в классическом формате.
- Для API/архитектурных изменений: обязательный docs-impact update.

## 3) Быстрые рецепты

### Добавить новый тип продукта (mapping path)

1. Обновить типы DTO/Entity.
2. Добавить mapping profile.
3. Добавить update profile.
4. Обновить DI регистрации и `ProductTypeRegistry`.
5. Обновить тесты + docs + backlog.

См.: [docs/SCALING/MAPPING_PROFILES.md](./docs/SCALING/MAPPING_PROFILES.md)

### Добавить новый источник парсинга

1. Добавить source в `ScrapingSeeding`.
2. Добавить transformer/handlers.
3. Зарегистрировать в parser DI.
4. Обновить runbook и system map.

См.: [docs/SCALING/PARSERS_GROWTH.md](./docs/SCALING/PARSERS_GROWTH.md)

### Разбор инцидента

См.: [docs/RUNBOOKS/INCIDENT_TRIAGE.md](./docs/RUNBOOKS/INCIDENT_TRIAGE.md)

## 4) Что нельзя пропускать

- Нельзя закрывать PR без обновления docs-impact части.
- Нельзя «угадывать» историческое решение без фиксации и подтверждения.
- Нельзя добавлять секреты в документацию или репозиторий.
