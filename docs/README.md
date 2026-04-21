# Документация Zoobee (Source of Truth)

Этот каталог — канонический источник документации проекта.

## Принципы

- Язык по умолчанию: русский.
- Подход: фиксируем фактическое состояние системы + явно ведем реестр долгов/рисков.
- Для значимых решений: обязательный ADR в классическом формате.
- Для неясностей: `block-and-ask` (останавливаемся и уточняем у владельца решения).
- Все изменения API/архитектуры/конвенций требуют обновления документации.

## Статусы систем

- `Active` — используется в текущем runtime.
- `Legacy` — историческая часть, может использоваться частично.
- `Stale` — устаревший артефакт, не является текущим источником истины.
- `Experimental` — WIP/не стабилизировано.
- `Planned` — запланировано, но не внедрено.

## Быстрый вход

1. [Onboarding Path](./ONBOARDING.md)
2. [System Map](./SYSTEM_MAP.md)
3. [Runtime Contour](./RUNTIME_CONTOUR.md)
4. [Parsers Pipeline](./PARSERS_PIPELINE.md)
5. [Conventions](./CONVENTIONS/NAMING.md)
6. [Risks And Debt Register](./RISKS_AND_DEBT.md)
7. [Decision Log](./DECISION_LOG.md)
8. [Documentation Backlog](./BACKLOG/DOCUMENTATION_BACKLOG.md)
9. [ADR Index](./ADR/INDEX.md)

## Навигация

- Бизнес-контекст: [BUSINESS_CONTEXT.md](./BUSINESS_CONTEXT.md)
- Допущения и ограничения: [ASSUMPTIONS_AND_CONSTRAINTS.md](./ASSUMPTIONS_AND_CONSTRAINTS.md)
- Матрица окружений: [ENVIRONMENT_MATRIX.md](./ENVIRONMENT_MATRIX.md)
- Конвенции:
  - [Naming](./CONVENTIONS/NAMING.md)
  - [Errors And Messages](./CONVENTIONS/ERRORS_AND_MESSAGES.md)
  - [Exceptions And Results](./CONVENTIONS/EXCEPTIONS_AND_RESULTS.md)
  - [Logging And Telemetry](./CONVENTIONS/LOGGING_AND_TELEMETRY.md)
  - [Configuration](./CONVENTIONS/CONFIGURATION.md)
  - [Localization Policy](./CONVENTIONS/LOCALIZATION_POLICY.md)
- Runbooks:
  - [Local Startup](./RUNBOOKS/LOCAL_STARTUP.md)
  - [Parsers Operations](./RUNBOOKS/PARSERS_OPERATIONS.md)
  - [Incident Triage](./RUNBOOKS/INCIDENT_TRIAGE.md)
- Масштабирование:
  - [Mapping Profiles](./SCALING/MAPPING_PROFILES.md)
  - [Parsers Growth](./SCALING/PARSERS_GROWTH.md)
  - [Future Foundations](./SCALING/FUTURE_FOUNDATIONS.md)
- Управление документацией:
  - [Documentation Policy](./GOVERNANCE/DOCUMENTATION_POLICY.md)
  - [Ambiguity Protocol](./GOVERNANCE/AMBIGUITY_PROTOCOL.md)
  - [Security Baseline](./GOVERNANCE/SECURITY_BASELINE.md)
  - [Stale Doc Review](./GOVERNANCE/STALE_DOC_REVIEW.md)

## Трассируемость

Для каждой архитектурной/конвенционной страницы обязательны ссылки:

- на ADR (`docs/ADR/*`),
- на backlog item (`docs/BACKLOG/DOCUMENTATION_BACKLOG.md`),
- на подтверждающие артефакты кода (путь в репозитории).

## Владение

Матрица владельцев и ответственных ролей: [OWNERSHIP.md](./OWNERSHIP.md).
