namespace Zoobee.Application.Interfaces.Services.GeoServices.GeoLocationService.GeoCoderApiClient.GeoCoderResponse
{
	using Newtonsoft.Json;
	using System.Collections.Generic;

	public class GeoCoderResponse
	{
		[JsonProperty("response")]
		public Response Response { get; set; }
	}

	public class GeocoderResponseMetaData
	{
		[JsonProperty("request")]
		public string Request { get; set; }

		[JsonProperty("found")]
		public string Found { get; set; }

		[JsonProperty("results")]
		public string Results { get; set; }
	}

	public class MetaDataProperty
	{
		[JsonProperty("GeocoderResponseMetaData")]
		public GeocoderResponseMetaData GeocoderResponseMetaData { get; set; }
	}

	public class Component
	{
		[JsonProperty("kind")]
		public string Kind { get; set; }

		[JsonProperty("name")]
		public string Name { get; set; }
	}

	public class Address
	{
		[JsonProperty("country_code")]
		public string CountryCode { get; set; }

		[JsonProperty("postal_code")]
		public string PostalCode { get; set; }

		[JsonProperty("formatted")]
		public string Formatted { get; set; }

		[JsonProperty("Components")]
		public List<Component> Components { get; set; }
	}

	public class PostalCode
	{
		[JsonProperty("PostalCodeNumber")]
		public string PostalCodeNumber { get; set; }
	}

	public class Premise
	{
		[JsonProperty("PremiseNumber")]
		public string PremiseNumber { get; set; }

		[JsonProperty("PostalCode")]
		public PostalCode PostalCode { get; set; }
	}

	public class Thoroughfare
	{
		[JsonProperty("ThoroughfareName")]
		public string ThoroughfareName { get; set; }

		[JsonProperty("Premise")]
		public Premise Premise { get; set; }
	}

	public class Locality
	{
		[JsonProperty("LocalityName")]
		public string LocalityName { get; set; }

		[JsonProperty("Thoroughfare")]
		public Thoroughfare Thoroughfare { get; set; }
	}

	public class AdministrativeArea
	{
		[JsonProperty("AdministrativeAreaName")]
		public string AdministrativeAreaName { get; set; }

		[JsonProperty("Locality")]
		public Locality Locality { get; set; }
	}

	public class Country
	{
		[JsonProperty("AddressLine")]
		public string AddressLine { get; set; }

		[JsonProperty("CountryNameCode")]
		public string CountryNameCode { get; set; }

		[JsonProperty("CountryName")]
		public string CountryName { get; set; }

		[JsonProperty("AdministrativeArea")]
		public AdministrativeArea AdministrativeArea { get; set; }
	}

	public class AddressDetails
	{
		[JsonProperty("Country")]
		public Country Country { get; set; }
	}

	public class GeocoderMetaData
	{
		[JsonProperty("kind")]
		public string Kind { get; set; }

		[JsonProperty("text")]
		public string Text { get; set; }

		[JsonProperty("precision")]
		public string Precision { get; set; }

		[JsonProperty("Address")]
		public Address Address { get; set; }

		[JsonProperty("AddressDetails")]
		public AddressDetails AddressDetails { get; set; }
	}

	public class GeoObjectMetaDataProperty
	{
		[JsonProperty("GeocoderMetaData")]
		public GeocoderMetaData GeocoderMetaData { get; set; }
	}

	public class Envelope
	{
		[JsonProperty("lowerCorner")]
		public string LowerCorner { get; set; }

		[JsonProperty("upperCorner")]
		public string UpperCorner { get; set; }
	}

	public class BoundedBy
	{
		[JsonProperty("Envelope")]
		public Envelope Envelope { get; set; }
	}

	public class Point
	{
		[JsonProperty("pos")]
		public string Pos { get; set; }
	}

	public class GeoObject
	{
		[JsonProperty("metaDataProperty")]
		public GeoObjectMetaDataProperty MetaDataProperty { get; set; }

		[JsonProperty("description")]
		public string Description { get; set; }

		[JsonProperty("name")]
		public string Name { get; set; }

		[JsonProperty("boundedBy")]
		public BoundedBy BoundedBy { get; set; }

		[JsonProperty("Point")]
		public Point Point { get; set; }
	}

	public class FeatureMember
	{
		[JsonProperty("GeoObject")]
		public GeoObject GeoObject { get; set; }
	}

	public class GeoObjectCollection
	{
		[JsonProperty("metaDataProperty")]
		public MetaDataProperty MetaDataProperty { get; set; }

		[JsonProperty("featureMember")]
		public List<FeatureMember> FeatureMember { get; set; }
	}

	public class Response
	{
		[JsonProperty("GeoObjectCollection")]
		public GeoObjectCollection GeoObjectCollection { get; set; }
	}

}
