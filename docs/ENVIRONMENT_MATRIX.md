# Environment Matrix

Связанные записи: [ADR-0001](./ADR/0001-documentation-system.md), backlog `DOC-022`.

## Матрица

| Контур | Статус | Runtime | DB | Конфиг/секреты | Наблюдения |
| --- | --- | --- | --- | --- | --- |
| Local (Windows) | `Active` | .NET SDK 9.0.308, target `net8.0` | PostgreSQL (по connection string) | В `appsettings.json` есть значения, требующие выноса в secrets | Build/restore может завершаться с кодом ошибки без явных CS-error строк |
| CI | `N/A` | `N/A` | `N/A` | `N/A` | В репозитории не обнаружен активный CI workflow |
| Staging | `N/A` | `N/A` | `N/A` | `N/A` | Данные/регламенты отсутствуют |
| Production | `N/A` | `N/A` | `N/A` | `N/A` | Данные/регламенты отсутствуют |

## Что нужно дополнить

- Явный CI pipeline для build/test/docs checks.
- Отдельные `appsettings.*` профили и секрет-менеджер.
- Runbook для staging/prod deployment.
