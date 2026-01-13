
using System;
using System.Collections.Generic;
using System.IO;
using System.Text.Json;
using Zoobee.Application.Interfaces.Repositories.Environtment.Manufactures;
using Zoobee.Domain.DataEntities.Environment.Creators;
namespace Zoobee.Web.ProgramConfigurators.App_PreRun
{
	public static class CountriesSeeder
	{
		public static bool SeedCountries(this WebApplication app, string filepath)
		{
			var clist = GetCountriesFromFile(filepath);
			using (var s = app.Services.CreateScope())
			{
				var countriesRepository = s.ServiceProvider.GetRequiredService<ICreatorCountriesRepository>();

				foreach (var country in clist)
					if (countriesRepository.CreateAsync(new CreatorCountryEntity
					{
						CountryNameRus = country.RussianName,
						CountryNameEng = country.EnglishName,
					}).Result.Failed)
						return false;
				countriesRepository.SaveChanges();	
			}
			return true;
		}



		private static List<(string RussianName, string EnglishName)> GetCountriesFromFile(string filePath)
		{
			try
			{
				// Читаем JSON файл
				string jsonContent = File.ReadAllText(filePath);

				// Десериализуем JSON
				var options = new JsonSerializerOptions
				{
					PropertyNameCaseInsensitive = true,
					Encoder = System.Text.Encodings.Web.JavaScriptEncoder.UnsafeRelaxedJsonEscaping
				};

				var jsonObject = JsonSerializer.Deserialize<Dictionary<string, Dictionary<string, string>>>(jsonContent, options);

				// Извлекаем словарь стран
				if (jsonObject != null && jsonObject.TryGetValue("countries", out var countriesDict))
				{
					var result = new List<(string, string)>();

					foreach (var pair in countriesDict)
					{
						result.Add((pair.Key, pair.Value));
					}

					return result;
				}

				return new List<(string, string)>();
			}
			catch (Exception ex)
			{
				Console.WriteLine($"Ошибка при чтении файла: {ex.Message}");
				return new List<(string, string)>();
			}
		}
	}

}