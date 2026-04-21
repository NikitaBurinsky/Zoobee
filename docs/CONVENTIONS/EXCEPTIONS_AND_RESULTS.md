# Exceptions And OperationResult

Связанные записи: [ADR-0001](../ADR/0001-documentation-system.md), backlog `DOC-008`, `DOC-019`.

## Контракт слоя

- Ожидаемые бизнес-ошибки: `OperationResult` / `OperationResult<T>`.
- Исключения: только для действительно нештатных, инфраструктурных или фатальных ситуаций.

## Правила

- В сервисах уровня `Application/Infrastructure` не использовать `throw NotImplementedException()` в runtime-путях.
- Вызов, который может валиться по валидации/данным, возвращает `OperationResult`.
- Exception message не должен быть пользовательским контрактом API.

## Текущие исключения (зафиксированный долг)

- `ProductsInfoService.UpdateOrAddProductInfoOfType` бросает `NotImplementedException`.
- `ToiletProductUpdateSpecInfoProfile.UpdateSpecificInfo` бросает `NotImplementedException`.
- В ряде мест есть `throw new ArgumentException("TODO ...")`.
