// Путь: Zoobee.Infrastructure/Services/Products/Matching/Fingerprinting/FingerprintBuilder.cs
using Zoobee.Application.DTOs.Products.Base;
using Zoobee.Domain.DataEntities.Products;
using Zoobee.Infrastructure.Services.Products.Matching.Attributes;
using Zoobee.Infrastructure.Services.Products.Matching.Normalization;

namespace Zoobee.Infrastructure.Services.Products.Matching.Fingerprinting
{
	public class FingerprintBuilder
	{
		private readonly AttributeExtractor _attributeExtractor;
		private readonly StringNormalizer _normalizer;

		public FingerprintBuilder(AttributeExtractor attributeExtractor, StringNormalizer normalizer)
		{
			_attributeExtractor = attributeExtractor;
			_normalizer = normalizer;
		}

		// Строим слепок из входящего DTO
		public ProductFingerprint BuildFromDto(BaseProductDto dto)
		{
			var metricValue = _attributeExtractor.ExtractMetricValue(dto.Name);
			var normBrand = _normalizer.Normalize(dto.BrandName);
			var normPet = _normalizer.Normalize(dto.PetKind);

			return new ProductFingerprint(normBrand, normPet, metricValue);
		}

		// Строим слепок из существующей сущности БД для сравнения
		public ProductFingerprint BuildFromEntity(BaseProductEntity entity)
		{
			// У сущности берем Name для веса, и навигационные свойства для бренда/вида
			// Важно: предполагается, что entity подгружена с Include(Brand).Include(PetKind)
			var metricValue = _attributeExtractor.ExtractMetricValue(entity.Name);

			// Если навигационное свойство null (битая база), возвращаем пустую строку, чтобы не упасть
			var brandName = entity.Brand?.BrandName ?? string.Empty;
			var petKindName = entity.PetKind?.PetKindName ?? string.Empty;

			var normBrand = _normalizer.Normalize(brandName);
			var normPet = _normalizer.Normalize(petKindName);

			return new ProductFingerprint(normBrand, normPet, metricValue);
		}
	}
}