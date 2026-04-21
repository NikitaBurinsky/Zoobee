# Documentation Backlog

Формат item: `ID | Priority | Risk | Effort | Owner | Dependencies | DoD | Status`.

## Items

| ID | Title | Priority | Risk | Effort | Owner | Dependencies | DoD | Status |
| --- | --- | --- | --- | --- | --- | --- | --- | --- |
| DOC-001 | Централизованный docs hub и навигация | P0 | High | M | TBD | - | `/docs/README.md` как единая точка входа | Done |
| DOC-002 | Onboarding path + glossary + AI entrypoint linkage | P0 | High | S | TBD | DOC-001 | Новый участник проходит маршрут без устных пояснений | Done |
| DOC-003 | Полная system map со статусами жизненного цикла | P0 | High | M | TBD | DOC-001 | Все подсистемы помечены `Active/Legacy/Stale/Experimental/Planned` | Done |
| DOC-004 | Документация runtime contour | P0 | High | S | TBD | DOC-001 | Есть точная схема и startup sequence | Done |
| DOC-005 | E2E документация parser pipeline | P0 | High | M | TBD | DOC-004 | Путь scrape->transform->save/failure описан и проверяем | Done |
| DOC-006 | Документация допущений и ограничений | P1 | Medium | S | TBD | DOC-001 | Зафиксированы locked defaults и constraints | Done |
| DOC-007 | Формализация naming conventions | P1 | Medium | S | TBD | DOC-001 | Новые изменения следуют единому naming-контракту | Done |
| DOC-008 | Формализация errors/messages/results conventions | P0 | High | S | TBD | DOC-001 | Есть единый контракт ошибок и правил `OperationResult` | Done |
| DOC-009 | Logging/telemetry conventions | P1 | Medium | S | TBD | DOC-001 | Есть базовый logging standard и known gaps | Done |
| DOC-010 | Назначение владельцев документации | P1 | Medium | S | Project Owner | DOC-001 | Поле owner заполнено не `TBD` для критичных областей | Todo |
| DOC-011 | Local startup runbook | P0 | High | S | TBD | DOC-004 | Запуск и базовый smoke описаны пошагово | Done |
| DOC-012 | Перевод модульных docs в thin entrypoints | P0 | Medium | S | TBD | DOC-001 | Legacy docs ссылаются на канонические страницы | Todo |
| DOC-013 | Уточнение runtime startup cleanup задач | P1 | Medium | S | TBD | DOC-004 | Startup debt items отражены и приоритизированы | Done |
| DOC-014 | Диагностика нестабильного build/restore поведения | P0 | High | M | TBD | - | Выявлена и документирована воспроизводимая причина/фикс | Todo |
| DOC-015 | Удаление абсолютных путей из startup/logging config | P0 | High | M | TBD | DOC-014 | Startup и logging работают переносимо | Todo |
| DOC-016 | Секьюрная переработка конфигурации и секретов | P0 | Critical | L | TBD | - | Секреты вынесены из repo-config, выполнена ротация | Todo |
| DOC-017 | Выравнивание PolicyNames и authorization контракта | P0 | High | S | TBD | - | Нет рассогласования policy constants и использования | Todo |
| DOC-018 | Решение по `OrganisationUser` (roadmap/implementation) | P0 | High | M | TBD | - | Принят и зафиксирован целевой identity-контракт | Todo |
| DOC-019 | Завершение типизированного product update path | P0 | High | L | TBD | DOC-017 | Убраны `NotImplementedException` из runtime пути | Todo |
| DOC-020 | Стабилизация failed parsing recovery | P0 | High | L | TBD | DOC-019 | Failure tasks корректно создаются/сохраняются/перезапускаются | Todo |
| DOC-021 | Минимальный бизнес-контекст в docs | P2 | Medium | S | TBD | DOC-001 | Есть документ продукта/потоков/ограничений | Done |
| DOC-022 | Environment matrix (с `N/A`) | P1 | Medium | S | TBD | DOC-001 | Матрица окружений заполнена и поддерживается | Done |
| DOC-023 | План cleanup legacy `ZooStores` namespaces | P1 | Medium | M | TBD | DOC-003 | Согласован plan и этапы миграции namespace | Todo |
| DOC-024 | Cleanup stale solutions/entrypoints | P1 | Medium | M | TBD | DOC-003 | Оставлены только актуальные solution entrypoints | Todo |
| DOC-025 | MVP localization policy и debt tracking | P1 | Medium | S | TBD | DOC-008 | Политика локализации закреплена и соблюдается | Done |
| DOC-026 | Документированный контракт admin seeding credentials | P0 | High | S | TBD | DOC-016 | `Admin:*` контракт задокументирован и верифицируем | Done |
| DOC-027 | Incident triage runbook | P1 | Medium | S | TBD | DOC-011 | Есть стандарт triage + postmortem flow | Done |
| DOC-028 | Playbook масштабирования mapping profiles | P1 | Medium | S | TBD | DOC-019 | Новый тип продукта добавляется по стандартизованному процессу | Done |
| DOC-029 | Playbook добавления новых parser sources | P1 | Medium | S | TBD | DOC-005 | Новый source подключается без ad-hoc решений | Done |
| DOC-030 | Roadmap выноса parser subsystem в сервис | P1 | Medium | M | TBD | DOC-029 | Есть фазовый migration path | Done |
| DOC-031 | Базовый CI pipeline (build/test/docs checks) | P0 | High | M | TBD | DOC-014 | CI стабильно валидирует изменения | Todo |
| DOC-032 | Документация foundation под будущие системы | P2 | Medium | S | TBD | DOC-001 | Зафиксированы foundation-workstreams | Done |
| DOC-033 | SLI/SLO и observability contract | P1 | Medium | M | TBD | DOC-009 | Определены метрики и алерты по критичным потокам | Todo |
| DOC-034 | Политика документации (обязательные правила) | P0 | High | S | TBD | DOC-001 | Док-политика утверждена и применяется | Done |
| DOC-035 | Ambiguity protocol governance | P0 | High | S | TBD | DOC-034 | Неясности обрабатываются через block-and-ask | Done |
| DOC-036 | Lightweight security baseline | P0 | High | S | TBD | DOC-016 | Базовые security-практики зафиксированы | Done |
| DOC-037 | Ежемесячный stale-doc review процесс | P1 | Medium | S | TBD | DOC-034 | Регулярный review проводится и логируется | Done |
