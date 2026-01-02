// Путь: Zoobee.Infrastructure/Services/Products/Matching/Fingerprinting/ProductFingerprint.cs
namespace Zoobee.Infrastructure.Services.Products.Matching.Fingerprinting
{
	public record ProductFingerprint(
		string NormalizedBrandName,
		string NormalizedPetKind,
		int? MetricValue // Вес/Объем
	);
}