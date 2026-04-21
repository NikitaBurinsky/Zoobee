# Scaling Playbook: Mapping Profiles

Связанные записи: [Runtime Contour](../RUNTIME_CONTOUR.md), [Exceptions And Results](../CONVENTIONS/EXCEPTIONS_AND_RESULTS.md), backlog `DOC-019`, `DOC-028`.

## Когда использовать

Когда добавляется новый тип продукта/DTO или расширяется контракт existing DTO.

## Шаги расширения (обязательные)

1. Добавить DTO и Entity типы.
2. Добавить mapping profile в `Zoobee.Application/Mapping Profiles/*`.
3. Добавить update profile в `Zoobee.Infrastructure/UpdateProductsSpecificInfoProfiles/*`.
4. Зарегистрировать профили в DI:
   - `AddApplicationMappingProfiles(...)`;
   - `AddProductInfoUpdateProfiles(...)`.
5. Зарегистрировать новый mapping в `ProductTypeRegistry`.
6. Добавить/обновить tests и docs.

## Критичная оговорка текущей версии

Контур `UpdateOrAddProductInfoOfType` в текущем состоянии WIP и содержит `NotImplementedException`; расширение типов без закрытия этого долга запрещено для production rollout.

## Definition of Done

- Нет runtime `NotImplementedException` на новом пути.
- Новый тип проходит E2E путь mapping -> save -> retrieval.
- Обновлены `RISKS_AND_DEBT`, backlog и (при необходимости) ADR.
