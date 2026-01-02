// Путь: Zoobee.Infrastructure/Services/Products/Matching/Normalization/StringNormalizer.cs
using System.Text.RegularExpressions;

namespace Zoobee.Infrastructure.Services.Products.Matching.Normalization
{
	public class StringNormalizer
	{
		public string Normalize(string input)
		{
			if (string.IsNullOrWhiteSpace(input))
				return string.Empty;

			// Приводим к нижнему регистру
			var normalized = input.ToLowerInvariant();

			// Оставляем только буквы (кириллица/латиница) и цифры. 
			// Убираем спецсимволы, чтобы "Pro-Plan" и "Pro Plan" стали "proplan"
			// Это агрессивная нормализация для сравнения брендов и типов
			normalized = Regex.Replace(normalized, @"[^\w\dа-яА-ЯёЁ]", "");

			return normalized;
		}
	}
}