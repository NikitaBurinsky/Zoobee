# Logging And Telemetry

Связанные записи: [ADR-0001](../ADR/0001-documentation-system.md), backlog `DOC-009`, `DOC-015`.

## Текущее состояние

- Используется Serilog через `ReadFrom.Configuration`.
- Настроены source-specific override уровни для parser subsystem.
- File sink указывает на абсолютный путь в локальной машине.

## Базовые правила логирования

- Для бизнес-событий использовать структурированные параметры (`{Key}`), не конкатенацию строк.
- Ошибки всегда логировать с exception объектом.
- Для long-running workers писать старт/стоп и ключевые transition-события.

## Минимальный telemetry baseline

- У каждого критичного workflow есть:
  - `start` log;
  - `success`/`failed` log;
  - correlation marker (URL/task id/entity id).

## Долги

- Абсолютный путь file sink снижает переносимость.
- Нет зафиксированных SLO/SLI и единой схемы correlation-id.
