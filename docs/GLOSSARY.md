# Glossary

Связанные записи: [ADR-0001](./ADR/0001-documentation-system.md), backlog `DOC-002`.

| Термин | Значение в контексте Zoobee |
| --- | --- |
| `OperationResult` | Стандартный результат операции (успех/ошибка/сообщение/код). |
| `DTO` | Объект обмена между слоями и API/внутренними сервисами. |
| `Mapping Profile` | Компонент преобразования DTO <-> Entity или похожих контрактов. |
| `ScrapingTask` | Единица работы crawler pipeline для конкретного URL. |
| `ScrapingData` | История скачанного контента по задаче. |
| `Transformation` | Преобразование сырых страниц в продуктовые данные и новые задачи. |
| `Failed Parsing Task` | Задача ручного/повторного восстановления после ошибки сохранения parsed данных. |
| `System Map Status` | Метка жизненного цикла подсистемы (`Active/Legacy/Stale/Experimental/Planned`). |
| `ADR` | Architecture Decision Record (формальный артефакт решения). |
| `Docs Impact` | Обязательная оценка влияния PR на документацию. |
