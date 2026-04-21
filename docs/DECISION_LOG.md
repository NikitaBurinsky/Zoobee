# Decision Log

Связанные записи: [ADR Index](./ADR/INDEX.md), [Documentation Backlog](./BACKLOG/DOCUMENTATION_BACKLOG.md).

## Принятые решения (2026-04-21)

| ID | Решение | Статус |
| --- | --- | --- |
| D-001 | Документируем факт + долг (не скрываем расхождения) | Accepted |
| D-002 | Основной язык документации: русский | Accepted |
| D-003 | Решения и backlog хранятся в репозитории | Accepted |
| D-004 | Системная карта со статусами `Active/Legacy/Stale/Experimental/Planned` | Accepted |
| D-005 | Governance: MVP-balance (docs update обязателен для API/арх-изменений; ADR для значимых решений) | Accepted |
| D-006 | AI-документация прагматичная (`AGENT_GUIDE.md` + рецепты) | Accepted |
| D-007 | Первая волна: сначала Systems (`Runtime + Parsers`) | Accepted |
| D-008 | Для неясностей действует `block-and-ask` | Accepted |
| D-009 | В документации не храним секреты; фиксируем remediation plan | Accepted |
| D-010 | Канонический runtime сейчас: монолит + parsers в том же host (`Zoobee.0.1`) | Accepted |
| D-011 | `ZoobeeParsers.sln`, `Zoobee.Web`, `ZooStores`-следы — legacy после rename | Accepted |
| D-012 | `OrganisationUser` — отложенный roadmap item | Accepted |
| D-013 | Минимальный бизнес-контекст обязателен в docs | Accepted |
| D-014 | Практические runbook’и обязательны | Accepted |
| D-015 | Локализация «as-is» | Superseded by D-030/D-031 |
| D-016 | Для MVP допустим fallback ключей в ответах | Accepted |
| D-017 | `UpdateOrAddProductInfoOfType` и toilet update profile — WIP, не production-ready | Accepted |
| D-018 | Failed parsing tasks subsystem — Experimental WIP | Accepted |
| D-019 | Admin CRUD API — partly legacy/WIP | Accepted |
| D-020 | Формат архитектурных решений — Classic ADR | Accepted |
| D-021 | Формат диаграмм — Mermaid (Markdown) | Accepted |
| D-022 | Схема backlog: priority, risk, effort, owner, deps, DoD, status | Accepted |
| D-023 | Топология docs: централизованный `/docs` + тонкие module entrypoints | Accepted |
| D-024 | PR docs-impact checklist обязателен | Accepted |
| D-025 | AI entrypoint filename: `AGENT_GUIDE.md` | Accepted |
| D-026 | Включаем lightweight security baseline | Accepted |
| D-027 | В docs включаем environment matrix (`N/A`, если нет данных) | Accepted |
| D-028 | Review cadence документации: ежемесячно | Accepted |
| D-029 | Стратегически сохраняем возможность выноса parser subsystem в микросервис | Accepted |
| D-030 | Для MVP временно поддерживаем режим «без локалей» | Accepted |
| D-031 | Текущие ru/be ресурсы маркируем как low-priority legacy для MVP | Accepted |
| D-032 | Отдельные `TODO_DOCS.*` файлы не ведем; используем structured docs/backlog | Accepted |
