using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Localization; // Для IStringLocalizer
using Microsoft.Extensions.Logging;
using NUnit.Framework;
using Serilog;
using Zoobee.Application.Interfaces.Repositories.Products_Repositories;
using Zoobee.Application.Interfaces.Repositories.UnitsOfWork;
using Zoobee.Domain.DataEntities.Environment.Creators;
using Zoobee.Domain.DataEntities.Products;
using Zoobee.Infrastructure.Parsers.Core.Enums;
using Zoobee.Infrastructure.Parsers.Core.Transformation;
using Zoobee.Infrastructure.Parsers.Interfaces.Services.Downloader;
using Zoobee.Infrastructure.Parsers.Interfaces.Transformation;
using Zoobee.Infrastructure.Parsers.Parsers.Zoobazar;
using Zoobee.Infrastructure.Parsers.Parsers.Zoobazar.Handlers;
using Zoobee.Infrastructure.Parsers.Services.Downloader;
using Zoobee.Infrastructure.Parsers.Services.Transformation;
using Zoobee.Infrastructure.Repositoties;
using Zoobee.Infrastructure.Repositoties.Products; // Для конкретных классов репозиториев
using Zoobee.Infrastructure.Repositoties.UnitsOfWork;
using Zoobee.Infrastructure.Services.Products.Matching;
using Zoobee.Infrastructure.Services.Products.Matching.Attributes;
using Zoobee.Infrastructure.Services.Products.Matching.Fingerprinting;
using Zoobee.Infrastructure.Services.Products.Matching.Normalization;

namespace Zoobee.Test.IntegrationTests.Pipeline
{
	[TestFixture]
	public class ParsingPipelineDebugTests
	{
		private ServiceProvider _serviceProvider;
		private ILogger<ParsingPipelineDebugTests> _logger;

		[OneTimeSetUp]
		public void Setup()
		{
			// 1. Настройка Логгера (Пишем в консоль)
			Log.Logger = new LoggerConfiguration()
				.MinimumLevel.Verbose()
				.WriteTo.Console(outputTemplate: "[{Timestamp:HH:mm:ss} {Level:u3}] {Message:lj}{NewLine}{Exception}")
				.CreateLogger();

			var services = new ServiceCollection();

			services.AddLogging(builder =>
			{
				builder.ClearProviders();
				builder.AddSerilog();
			});

			// 2. База данных (InMemory)
			services.AddDbContext<ZoobeeAppDbContext>(options =>
				options.UseInMemoryDatabase($"Debug_Db_{Guid.NewGuid()}"));

			// 3. --- ИСПРАВЛЕНИЕ: Заглушка для Локализации (нужна репозиториям) ---
			services.AddSingleton(typeof(IStringLocalizer<>), typeof(DummyStringLocalizer<>));

			// 4. --- ИСПРАВЛЕНИЕ: Регистрация Репозиториев (зависимости UnitOfWork) ---
			services.AddScoped<IFoodProductsRepository, FoodProductRepository>();
			services.AddScoped<IToiletProductsRepository, ToiletProductRepository>();
			services.AddScoped<IBaseProductsRepository, BaseProductsRepository>();

			// 5. Регистрация UnitOfWork
			services.AddScoped<IProductsUnitOfWork, ProductsUnitOfWork>();

			// 6. Регистрация Матчера
			services.AddSingleton<StringNormalizer>();
			services.AddSingleton<AttributeExtractor>();
			services.AddTransient<FingerprintBuilder>();
			services.AddScoped<ProductInfoMatcher>();

			// 7. Регистрация Парсинга
			services.AddHttpClient();
			services.AddTransient<IHtmlDownloader, HttpHtmlDownloader>();
			services.AddTransient<ITransformerResolver, TransformerResolver>();

			// Парсеры Zoobazar
			services.AddTransient<ZoobazarTransformer>();
			services.AddTransient<IResourceHandler, ZoobazarFoodHandler>();
			services.AddTransient<IResourceHandler, ZoobazarSitemapHandler>();

			// Регистрируем резолвер стратегий
			services.AddTransient<IEnumerable<IWebPageTransformer>>(sp =>
				new List<IWebPageTransformer> { sp.GetRequiredService<ZoobazarTransformer>() });

			_serviceProvider = services.BuildServiceProvider();
			_logger = _serviceProvider.GetRequiredService<ILogger<ParsingPipelineDebugTests>>();
		}

		[OneTimeTearDown]
		public void TearDown()
		{
			_serviceProvider?.Dispose();
			Log.CloseAndFlush();
		}

		[TestCase("https://zoobazar.by/catalog/koshki/sukhie_korma/super-premium-klassa/korm_royal_canin_dlya_sterilizovannykh_koshek_sterilised_37_2_kg/")]
		public async Task Debug_FullPipeline_UrlToMatch(string url)
		{
			using var scope = _serviceProvider.CreateScope();
			var dbContext = scope.ServiceProvider.GetRequiredService<ZoobeeAppDbContext>();
			var matcher = scope.ServiceProvider.GetRequiredService<ProductInfoMatcher>();
			var downloader = scope.ServiceProvider.GetRequiredService<IHtmlDownloader>();
			var resolver = scope.ServiceProvider.GetRequiredService<ITransformerResolver>();

			// ========================================================================
			// ШАГ 0: Подготовка БД (Seeding)
			// ========================================================================
			_logger.LogInformation("--- STEP 0: Seeding InMemory DB ---");

			var testBrand = new BrandEntity
			{
				Id = Guid.NewGuid(),
				BrandName = "Royal Canin",
				NormalizedBrandName = "royalcanin", // REQUIRED
				Description = "Test Brand Desc",    // REQUIRED
				DeleteData = new Domain.DataEntities.Base.SoftDelete.ISoftDeletableEntity.SoftDeleteData { IsDeleted = false }
			};

			var testPet = new PetKindEntity
			{
				Id = Guid.NewGuid(),
				PetKindName = "Кошки",
				NormalizedPetKindName = "кошки", // REQUIRED
				DeleteData = new Domain.DataEntities.Base.SoftDelete.ISoftDeletableEntity.SoftDeleteData { IsDeleted = false }
			};

			// Создаем товар-кандидат
			var existingProduct = new BaseProductEntity
			{
				Id = Guid.NewGuid(),
				Name = "Royal Canin Sterilised",
				NormalizedName = "royalcaninsterilised", // REQUIRED
				Description = "Test Product Desc",       // REQUIRED

				Brand = testBrand,
				PetKind = testPet,
				DeleteData = new Domain.DataEntities.Base.SoftDelete.ISoftDeletableEntity.SoftDeleteData
				{
					IsDeleted = false
				},
				// Инициализируем пустой словарь для SiteArticles, чтобы не упал ValueConverter
				SiteArticles = new Dictionary<string, string>()
			};

			await dbContext.Brands.AddAsync(testBrand);
			await dbContext.PetKinds.AddAsync(testPet);
			await dbContext.Products.AddAsync(existingProduct);
			await dbContext.SaveChangesAsync();

			_logger.LogInformation("Seeded Product: '{Name}' (ID: {Id})", existingProduct.Name, existingProduct.Id);

			_logger.LogInformation("Seeded Product: '{Name}' (ID: {Id})", existingProduct.Name, existingProduct.Id);
			// ========================================================================
			// ШАГ 1: Скачивание (Crawling)
			// ========================================================================
			_logger.LogInformation("\n--- STEP 1: Downloading URL: {Url} ---", url);

			var taskType = ScrapingTaskType.ProductPage;
			var html = await downloader.DownloadAsync(url, new CancellationToken(false));

			if (!html.IsSuccess || string.IsNullOrEmpty(html.Content))
			{
				Assert.Fail("HTML is empty. Download failed or blocked.");
			}
			_logger.LogInformation("Download Success. HTML Length: {Length}", html.Content.Length);

			// ========================================================================
			// ШАГ 2: Трансформация (Parsing)
			// ========================================================================
			_logger.LogInformation("\n--- STEP 2: Transformation ---");

			var transformer = resolver.GetTransformer("Zoobazar", taskType);
			if (transformer == null) Assert.Fail("Transformer for 'Zoobazar' not found.");

			var transformationResult = await transformer.TransformAsync(html.Content, url, taskType);

			_logger.LogInformation("Transformation Finished. Extracted Data: {@ExtractedData}", transformationResult.ExtractedData);

			if (transformationResult.ExtractedData.ProductInfo == null && transformationResult.ExtractedData.ProductSlot == null)
			{
				// Если парсер ничего не вернул, тест "не прошел" в плане полезности, но не упал с ошибкой
				_logger.LogWarning("Parser returned NO items. Check handlers.");
			}

			// ========================================================================
			// ШАГ 3: Матчинг (Matching)
			// ========================================================================
			_logger.LogInformation("\n--- STEP 3: Product Matching ---");
			var itemDto = transformationResult.ExtractedData.ProductInfo;
			if (itemDto is not Application.DTOs.Products.Base.BaseProductDto productDto)
			{
				_logger.LogWarning("Item is not a BaseProductDto. Type: {Type}", itemDto.GetType().Name);
				return;
			}

			_logger.LogInformation(">>> Processing DTO: '{Name}'", productDto.Name);

			var matchResult = await matcher.FindMatchAsync(productDto);

			if (matchResult != null)
			{
				_logger.LogInformation("✅ MATCHED! Linked to DB Entity: '{Name}' ({Id})", matchResult.Name, matchResult.Id);
				Assert.That(matchResult.Id, Is.EqualTo(existingProduct.Id), "Matcher found WRONG product!");
			}
			else
			{
				_logger.LogWarning("❌ NO MATCH FOUND.");
			}
		}
	}

	// Заглушка для локализации, чтобы тесты не падали на IStringLocalizer
	public class DummyStringLocalizer<T> : IStringLocalizer<T>
	{
		public LocalizedString this[string name] => new LocalizedString(name, name);
		public LocalizedString this[string name, params object[] arguments] => new LocalizedString(name, string.Format(name, arguments));
		public IEnumerable<LocalizedString> GetAllStrings(bool includeParentCultures) => Enumerable.Empty<LocalizedString>();
	}
}