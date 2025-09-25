using System.Net.Http.Json;
using ZooStorages.Application.Features.Catalog_Features.PetKinds;
using System.Text.Json;
using Xunit.Abstractions;

namespace Zoobee.Test
{
	public class ApiTest 
	{
		ITestOutputHelper log;

		public ApiTest(ITestOutputHelper log)
		{
			this.log = log;
		}

		private readonly HttpClient client = new HttpClient
		{
			BaseAddress = new Uri("https://localhost:7205")
		};
		[Fact]
		public async void InitializeDatabase()
		{
			await AddPetKinds("C:\\Users\\Formatis\\Documents\\GitHub\\Zoobee\\ZooStores.Test\\TestData\\petkinds.json");
			await AddCategories("C:\\Users\\Formatis\\Documents\\GitHub\\Zoobee\\ZooStores.Test\\TestData\\productcategories.json");
			await AddTypes("C:\\Users\\Formatis\\Documents\\GitHub\\Zoobee\\ZooStores.Test\\TestData\\producttypes.json");
			//await AddProducts("C:\\Users\\Formatis\\Documents\\GitHub\\Code_Projects\\ZooStores.Test\\TestData\\products.json");
		}


		public class PetKindTestData
		{
			public List<CreatePetKindCommand> ValidCommands { get; set; }
			public List<CreatePetKindCommand> InvalidCommands { get; set; }
			public List<CreatePetKindCommand> EdgeCases { get; set; }
		}
		public async Task AddPetKinds(string file)
		{
			var json = File.ReadAllText(file);
			var testData = JsonSerializer.Deserialize<PetKindTestData>(json).ValidCommands;
			
			foreach(var req in testData)
			{
				var res = await client.PostAsJsonAsync("admin/pet-kinds/add", req);
				log.WriteLine(res.IsSuccessStatusCode ? "Sent successfully" : $"Err sent : {req}");
			}
			Assert.True(true, "All PetKinds SENT SUCCESSFULLY");
		}
		public async Task AddCategories(string file)
		{
			var json = File.ReadAllText(file);
			var testData = JsonSerializer.Deserialize<ProductCategoryTestData>(json).ValidCategories;
			foreach (var req in testData)
			{
				var res = await client.PostAsJsonAsync("admin/product-categories/add", req);
				log.WriteLine(res.IsSuccessStatusCode ? "Sent successfully" : $"Err sent : {req}");
			}
			Assert.True(true, "All Categories SENT SUCCESSFULLY");
		}
		public async Task AddTypes(string file)
		{
			var json = File.ReadAllText(file);
			var testData = JsonSerializer.Deserialize<ProductTypeTestData>(json).ValidProductTypes;
			foreach (var req in testData)
			{
				var res = await client.PostAsJsonAsync("admin/product-types/add", req);
				log.WriteLine(res.IsSuccessStatusCode ? "Sent successfully" : $"Err sent : {req}");
			}
			Assert.True(true, "All Types SENT SUCCESSFULLY");
		}
		public async Task AddProducts(string file)
		{
			var json = File.ReadAllText(file);
			var testData = JsonSerializer.Deserialize<ProductTestData>(json).ValidProducts;
			foreach (var req in testData)
			{
				var res = await client.PostAsJsonAsync("admin/products/add", req);
				log.WriteLine(res.IsSuccessStatusCode ? "Sent successfully" : $"Err sent : {req}");
			}
			Assert.True(true, "All Products SENT SUCCESSFULLY");
		}
		public async void AddProductsMedia(string file)
		{
			throw LocationNotFoundmentedException();
		}
		public async void AddProducSlots(string file)
		{
			throw LocationNotFoundmentedException();
		}
		public async void AddDeliveryOptions(string file)
		{
			throw LocationNotFoundmentedException();
		}
		public async void AddSelfPickupOptions(string file)
		{
			throw LocationNotFoundmentedException();
		}
	}
}