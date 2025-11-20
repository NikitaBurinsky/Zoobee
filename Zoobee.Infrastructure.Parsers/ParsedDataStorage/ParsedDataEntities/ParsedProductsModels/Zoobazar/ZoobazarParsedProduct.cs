using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json;
using System.Text.Json.Serialization;
using Zoobee.Infrastructure.Parsers.ParsedDataStorage.ValueConverters.JsonElementToString;

namespace Zoobee.Infrastructure.Parsers.Storage.ParsedDataEntities.ParsedProductsModels.Zoobazar
{
	/// <summary>
	///TODO
	///МЕНЯЕМ ВСЕ С OWNED НА ENTITIES
	/// </summary>
	public class ZoobazarParsedProduct
	{
		[Key]
		[JsonIgnore(Condition = JsonIgnoreCondition.Always)]
		public Guid Id {  get; set; }

		[JsonPropertyName("haveOffers")]
		public bool? HaveOffers { get; set; }

		[JsonPropertyName("currentLocation")]
		public CurrentLocation? CurrentLocation { get; set; }

		[JsonPropertyName("user")]
		public User User { get; set; }

		[JsonPropertyName("siteId")]
		public string? SiteId { get; set; }

		[JsonPropertyName("productId")]
		public int? ProductId { get; set; }

		[JsonPropertyName("productXmlId")]
		public string? ProductXmlId { get; set; }

		[JsonPropertyName("productName")]
		public string? ProductName { get; set; }

		[JsonPropertyName("sectionId")]
		public string? SectionId { get; set; }

		[JsonPropertyName("sectionName")]
		public string? SectionName { get; set; }

		[JsonPropertyName("sectionPath")]
		public string? SectionPath { get; set; }

		[JsonPropertyName("rating")]
		public Rating Rating { get; set; }

		[JsonPropertyName("composition")]
		public string? Composition { get; set; }

		[JsonPropertyName("brandId")]
		public string? BrandId { get; set; }

		[JsonPropertyName("brand")]
		public Brand Brand { get; set; }

		[JsonPropertyName("advantages")]
		public string? Advantages { get; set; }

		[JsonPropertyName("tabs")]
		public List<Tab> Tabs { get; set; }

		[JsonPropertyName("offersProperty")]
		public string? OffersProperty { get; set; }

		[JsonPropertyName("test")]
		public TestData? Test { get; set; } = new TestData();

		[JsonPropertyName("reviews")]
		public Reviews Reviews { get; set; }

		[JsonPropertyName("offers")]
		public List<Offer> Offers { get; set; }
	}

	public class Tab
	{
		[Key]
		[JsonIgnore(Condition = JsonIgnoreCondition.Always)]
		public Guid Id { get; set; }

		[JsonPropertyName("name")]
		public string? Name { get; set; }

		[JsonPropertyName("code")]
		public string? Code { get; set; }

		[JsonPropertyName("sort")]
		public string? Sort { get; set; }

		[JsonPropertyName("value")]
		public string? Value { get; set; }
	}

	//TODO //TEST
	//В отдельную таблицу этот клас и все нижние
	[Owned]
	public class CurrentLocation
	{
		[JsonPropertyName("ID")]
		public int Id { get; set; }

		[JsonPropertyName("CODE")]
		public string? Code { get; set; }

		[JsonPropertyName("NAME")]
		public string? Name { get; set; }

		[JsonPropertyName("STORE")]
		public List<int> Store { get; set; }

		[JsonPropertyName("LOCATION")]
		public List<string> Location { get; set; }

		[JsonPropertyName("FREE_DELIVERY")]
		public int? FreeDelivery { get; set; }

		[JsonPropertyName("HOURS")]
		public string? Hours { get; set; }

		[JsonPropertyName("PICKUP_ONLY")]
		public bool? PickupOnly { get; set; }
	}
	//TODO //TEST
	[Owned]
	public class User
	{
		[JsonPropertyName("id")]
		public int? Id { get; set; }

		[JsonPropertyName("personalPhone")]
		public string? PersonalPhone { get; set; }

		[JsonPropertyName("name")]
		public string? Name { get; set; }
	}
	//TODO //TEST
	[Owned]
	public class Rating
	{
		[JsonPropertyName("value")]
		public double? Value { get; set; }

		[JsonPropertyName("count")]
		public int? Count { get; set; }
	}
	//TODO //TEST
	[Owned]
	public class Brand
	{
		[JsonPropertyName("id")]
		public string? Id { get; set; }

		[JsonPropertyName("name")]
		public string? Name { get; set; }

		[JsonPropertyName("link")]
		public string? Link { get; set; }
	}
	//TODO //TEST
	[Owned]
	public class TestData
	{
		[JsonPropertyName("BRAND")]
		public PropertyData? Brand { get; set; }

		[JsonPropertyName("GALLERY")]
		public PropertyData? Gallery { get; set; }

		// Добавьте другие свойства по необходимости
	}
	//TODO //TEST
	[Owned]
	public class PropertyData
	{
		[JsonPropertyName("ID")]
		public string? Id { get; set; }

		[JsonPropertyName("IBLOCK_ID")]
		public string? IblockId { get; set; }

		[JsonPropertyName("ACTIVE")]
		public string? Active { get; set; }

		[JsonPropertyName("NAME")]
		public string? Name { get; set; }

		//Несколько возможных значений, назначение пока не понятно
		//TODO
		[NotMapped]
		//[JsonPropertyName("VALUE")]
		public object? Value { get; set; }

		// Добавьте другие свойства по необходимости
	}
	//TODO //TEST
	[Owned]
	public class Reviews
	{
		[JsonPropertyName("statistic")]
		public ReviewStatistic Statistic { get; set; }
	}
	//TODO //TEST
	[Owned]
	public class ReviewStatistic
	{
		[JsonPropertyName("count")]
		public int? Count { get; set; }

		[JsonPropertyName("vote")]
		public double? Vote { get; set; }

		[JsonPropertyName("allow")]
		public bool? Allow { get; set; }

		[JsonPropertyName("isExists")]
		public bool? IsExists { get; set; }
	}
	//TODO //TEST
	public class Offer
	{
		[JsonIgnore(Condition = JsonIgnoreCondition.Always)]
		[Key]
		public Guid? Id { get; set; } = Guid.NewGuid();

		[JsonPropertyName("id")]
		public int? SiteId { get; set; }

		[JsonPropertyName("xmlId")]
		public string? XmlId { get; set; }

		[JsonPropertyName("mindboxId")]
		public string? MindboxId { get; set; }

		[JsonPropertyName("selected")]
		public bool? Selected { get; set; }

		[JsonPropertyName("name")]
		public string? Name { get; set; }

		[JsonPropertyName("label")]
		public string? Label { get; set; }

		[JsonPropertyName("link")]
		public string? Link { get; set; }

		[JsonPropertyName("description")]
		public string? Description { get; set; }

		[JsonPropertyName("sku")]
		public string? Sku { get; set; }

		[JsonPropertyName("internalCode")]
		public string? InternalCode { get; set; }

		[JsonPropertyName("available")]
		public bool? Available { get; set; }

		[JsonPropertyName("quantity")]
		public int? Quantity { get; set; }

		[JsonPropertyName("dateDelivery")]
		public string? DateDelivery { get; set; }

		[JsonPropertyName("weight")]
		public string? Weight { get; set; }

		[JsonPropertyName("price")]
		public Price? Price { get; set; }

		[JsonPropertyName("discountPoints")]
		public DiscountPoints? DiscountPoints { get; set; }

		/*
         //TODO 
		Недостаточно инфы, что приходит отсюда. Нужно узнать что за тип данных.
         */
		[Column(TypeName = "jsonb")]
		[JsonPropertyName("sale")]
		public JsonElement Sale { get; set; }

		[JsonPropertyName("isMedicine")]
		public bool? IsMedicine { get; set; }

		[JsonPropertyName("isAvailDelivery")]
		public bool? IsAvailDelivery { get; set; }

		[JsonPropertyName("tags")]
		public List<Tag> Tags { get; set; }

		[JsonPropertyName("properties")]
		public List<Property> Properties { get; set; }
	}
	//TODO //TEST
	[Owned]
	public class Price
	{

		[JsonIgnore(Condition = JsonIgnoreCondition.Always)]
		public Guid Id { get; set; }

		[JsonPropertyName("base")]
		public string? Base { get; set; }

		[JsonPropertyName("result")]
		public string? Result { get; set; }

		[JsonPropertyName("discountPercent")]
		public int? DiscountPercent { get; set; }

		[JsonPropertyName("baseRaw")]
		public double? BaseRaw { get; set; }

		[JsonPropertyName("resultRaw")]
		public double? ResultRaw { get; set; }

		[JsonPropertyName("currency")]
		public string? Currency { get; set; }

		[JsonPropertyName("subscriptionRaw")]
		public double? SubscriptionRaw { get; set; }

		[JsonPropertyName("format")]
		public PriceFormat? Format { get; set; }
	}
	//TODO //TEST
	[Owned]
	public class PriceFormat
	{
		[JsonPropertyName("pattern")]
		public string? Pattern { get; set; }

		[JsonPropertyName("decPoint")]
		public string? DecimalPoint { get; set; }

		[JsonPropertyName("thousandsSep")]
		public string? ThousandsSeparator { get; set; }

		[JsonPropertyName("decimals")]
		public int? Decimals { get; set; }
	}
	//TODO //TEST
	[Owned]
	public class DiscountPoints
	{
		[JsonPropertyName("raw")]
		public double? Raw { get; set; }

		[JsonPropertyName("formatted")]
		public string? Formatted { get; set; }
	}
	//TODO //TEST
	public class Tag
	{
		[Key]
		[JsonIgnore(Condition = JsonIgnoreCondition.Always)]
		public Guid TagId { get; set; }

		[Column(TypeName = "jsonb")]
		[JsonPropertyName("id")]
		public JsonElement? Id { get; set; }

		[JsonPropertyName("title")]
		public string? Title { get; set; }

		[JsonPropertyName("name")]
		public string? Name { get; set; }

		[JsonPropertyName("link")]
		public string? Link { get; set; }
	}
	//TODO //TEST
	public class Property
	{
		[Key]
		[JsonIgnore(Condition = JsonIgnoreCondition.Always)]
		public Guid PropertyId { get; set; }

		[JsonPropertyName("id")]
		public string? Id { get; set; }

		[JsonPropertyName("name")]
		public string? Name { get; set; }

		[JsonPropertyName("value")]
		public string? Value { get; set; }
	}
}
