using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Zoobee.Application.DTOs.Products.Base;
using Zoobee.Application.Interfaces.Repositories.UnitsOfWork;
using Zoobee.Domain.DataEntities.Products;
using Zoobee.Infrastructure.Services.Products.Matching.Fingerprinting;

namespace Zoobee.Infrastructure.Services.Products.Matching
{
	public class ProductInfoMatcher
	{
		private readonly IProductsUnitOfWork _productsUnitOfWork;
		private readonly FingerprintBuilder _fingerprintBuilder;
		private readonly ILogger<ProductInfoMatcher> _logger;

		public ProductInfoMatcher(
			IProductsUnitOfWork productsUnitOfWork,
			FingerprintBuilder fingerprintBuilder,
			ILogger<ProductInfoMatcher> logger)
		{
			_productsUnitOfWork = productsUnitOfWork;
			_fingerprintBuilder = fingerprintBuilder;
			_logger = logger;
		}

		public async Task<BaseProductEntity?> FindMatchAsync(BaseProductDto dto)
		{
			// Используем Scope, чтобы все логи внутри этого блока имели привязку к имени товара
			using var scope = _logger.BeginScope("Сопоставление товара: {DtoName}", dto.Name);

			// 1. Быстрый выход и валидация
			if (string.IsNullOrWhiteSpace(dto.BrandName) || string.IsNullOrWhiteSpace(dto.PetKind))
			{
				_logger.LogWarning("Сопоставление ПРОПУЩЕНО. Причина: Отсутствуют обязательные метаданные. Бренд: '{Brand}', Вид питомца: '{PetKind}'",
					dto.BrandName, dto.PetKind);
				return null;
			}
			try
			{
				// 2. Создаем фингерпринт искомого товара
				var targetFingerprint = _fingerprintBuilder.BuildFromDto(dto);

				_logger.LogDebug("Сгенерирован целевой отпечаток (Fingerprint): [Бренд: {NormBrand}, Вид: {NormKind}, Метрика: {Metric}]",
					targetFingerprint.NormalizedBrandName,
					targetFingerprint.NormalizedPetKind,
					targetFingerprint.MetricValue);

				// 3. Формируем запрос к БД (Broad Filter)
				var query = _productsUnitOfWork.AllProducts.GetAll()
					.Include(p => p.Brand)
					.Include(p => p.PetKind)
					.AsNoTracking();

				// Фильтрация на уровне БД (чтобы не тянуть всю таблицу)
				query = query.Where(p =>
					p.PetKind.PetKindName == dto.PetKind ||
					p.Brand.BrandName.Contains(dto.BrandName) ||
					dto.BrandName.Contains(p.Brand.BrandName));

				var candidates = await query.ToListAsync();

				if (candidates.Count == 0)
				{
					_logger.LogInformation("Сопоставление НЕ УДАЛОСЬ. Причина: В БД не найдено кандидатов по фильтру Бренд/Вид питомца.");
					return null;
				}

				_logger.LogDebug("Найдено {Count} потенциальных кандидатов в БД. Запуск глубокого анализа...", candidates.Count);

				// 4. Точный матчинг в памяти
				foreach (var candidate in candidates)
				{
					var candidateFingerprint = _fingerprintBuilder.BuildFromEntity(candidate);

					// А. Проверка Бренда
					if (targetFingerprint.NormalizedBrandName != candidateFingerprint.NormalizedBrandName)
					{
						// Доп. проверка на вхождение для страховки
						if (!candidateFingerprint.NormalizedBrandName.Contains(targetFingerprint.NormalizedBrandName) &&
							!targetFingerprint.NormalizedBrandName.Contains(candidateFingerprint.NormalizedBrandName))
						{
							_logger.LogTrace("Кандидат '{CandName}' ({Id}) ОТКЛОНЕН. Причина: Несовпадение бренда ('{TargetB}' != '{CandB}')",
								candidate.Name, candidate.Id, targetFingerprint.NormalizedBrandName, candidateFingerprint.NormalizedBrandName);
							continue;
						}
					}

					// Б. Проверка Вида животного
					if (targetFingerprint.NormalizedPetKind != candidateFingerprint.NormalizedPetKind)
					{
						_logger.LogTrace("Кандидат '{CandName}' ({Id}) ОТКЛОНЕН. Причина: Несовпадение вида питомца ('{TargetK}' != '{CandK}')",
							candidate.Name, candidate.Id, targetFingerprint.NormalizedPetKind, candidateFingerprint.NormalizedPetKind);
						continue;
					}

					// В. Проверка Веса/Объема (Критический атрибут)
					if (targetFingerprint.MetricValue.HasValue && candidateFingerprint.MetricValue.HasValue)
					{
						if (targetFingerprint.MetricValue.Value != candidateFingerprint.MetricValue.Value)
						{
							_logger.LogDebug("Кандидат '{CandName}' ({Id}) ОТКЛОНЕН. Причина: Несовпадение веса/объема (Цель: {TargetM}, Кандидат: {CandM})",
								candidate.Name, candidate.Id, targetFingerprint.MetricValue, candidateFingerprint.MetricValue);
							continue;
						}
					}
					else if (targetFingerprint.MetricValue.HasValue && !candidateFingerprint.MetricValue.HasValue)
					{
						// Опционально: можно логировать предупреждение
						_logger.LogTrace("Кандидат '{CandName}' ({Id}) пропущен при проверке метрики (метрика кандидата null). Потенциально слабое совпадение.", candidate.Name, candidate.Id);
					}

					// Если дошли сюда — это УСПЕХ
					_logger.LogInformation("Сопоставление УСПЕШНО! DTO '{DtoName}' привязан к сущности '{EntityName}' ({Id}). Оценка: Высокая (Совпадение по отпечатку)",
						dto.Name, candidate.Name, candidate.Id);

					return candidate;
				}

				_logger.LogInformation("Сопоставление НЕ УДАЛОСЬ. Проанализировано {Count} кандидатов, но точных совпадений не найдено.", candidates.Count);
				return null;
			}
			catch (Exception ex)
			{
				_logger.LogError(ex, "КРИТИЧЕСКАЯ ОШИБКА при сопоставлении товара '{DtoName}'", dto.Name);
				throw;
			}
		}
	}
}