# Руководство по конфигурации (Seeding)

Система поддерживает автоматический "посев" (Seeding) начальных ссылок через конфигурационный файл `appsettings.json`. Это позволяет добавлять новые магазины без перекомпиляции кода.

## Структура конфигурации

Настройки находятся в секции `ScrapingSeeding`.

```json
"ScrapingSeeding": {
  "Sources": [
    {
      "SourceName": "Zoobazar",
      "Sitemaps": [
        "[https://zoobazar.by/sitemap.xml](https://zoobazar.by/sitemap.xml)",
        "[https://zoobazar.by/sitemap-products.xml](https://zoobazar.by/sitemap-products.xml)"
      ],
      "StartUrls": [
        "[https://zoobazar.by/catalog/cats](https://zoobazar.by/catalog/cats)",
        "[https://zoobazar.by/catalog/dogs](https://zoobazar.by/catalog/dogs)"
      ]
    },
    {
      "SourceName": "AnotherStore",
      "Sitemaps": [], 
      "StartUrls": [ "[https://anotherstore.by/new-items](https://anotherstore.by/new-items)" ]
    }
  ]
}
```

# Как добавить новый сайт

Откройте appsettings.json.

Найдите массив Sources.

Добавьте новый объект с именем источника и ссылками.

Перезапустите приложение. ScrapingSeeder проверит наличие ссылок в БД и добавит только отсутствующие. Дубликаты игнорируются.