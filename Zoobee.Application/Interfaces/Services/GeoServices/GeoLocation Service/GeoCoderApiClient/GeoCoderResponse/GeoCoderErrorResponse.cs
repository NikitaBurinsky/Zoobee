namespace Zoobee.Application.Interfaces.Services.GeoServices.GeoLocationService.GeoCoderApiClient.GeoCoderResponse
{
	public class GeoCoderErrorResponse
	{
		public int statusCode { get; set; }
		public string error { get; set; }
		public string message { get; set; }
	}
}
