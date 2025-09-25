using Microsoft.Extensions.Configuration;
using System.Globalization;
using System.Net;
using System.Text.Json;
using Zoobee.Application.DTOs.Environtment.Geography;
using Zoobee.Application.Interfaces.Services.GeoServices.GeoLocationService.GeoCoderApiClient;
using Zoobee.Application.Interfaces.Services.GeoServices.GeoLocationService.GeoCoderApiClient.GeoCoderResponse;
using Zoobee.Domain;
using Zoobee.Domain.DataEntities.Environment.Geography;

namespace Zoobee.Infrastructure.Services.GeoServices.GeoLocationService.GeoCoderApiClient
{
    public class GeoCoderApiClient : IGeoCoderApiClient
    {
        private GeoCoderApiClient() { }
        string APIKEY { get; set; }
        string APIURL { get; set; }
        IHttpClientFactory httpClientFactory;
        JsonSerializerOptions SerializerOptions = new JsonSerializerOptions
        {
            PropertyNameCaseInsensitive = true,
        };

        public GeoCoderApiClient(IConfiguration configuration, IHttpClientFactory? clientFactory)
        {
            httpClientFactory = clientFactory;
            APIKEY = configuration.GetSection("APIs:YandexGeoCoder:KEY").Value;
            APIURL = configuration.GetSection("APIs:YandexGeoCoder:URL").Value;
            if (string.IsNullOrEmpty(APIKEY) || string.IsNullOrEmpty(APIURL))
                throw new ArgumentException("TODO GeoCoder API KEY Values are not set");
        }

        public async Task<OperationResult<LocationDto>> GetLocationByGeoPoint(GeoPoint geoPoint)
        {
            //var client = httpClientFactory.CreateClient();
            var client = new HttpClient();
            var Params = new Dictionary<string, string>()
            {
                ["apikey"] = APIKEY,
                ["geocode"] = $"{geoPoint.Longitude}, {geoPoint.Latitude}",
                ["sco"] = "longlat",
                ["format"] = "json",
                ["kind"] = "house",
                ["lang"] = "ru_RU",
            };
            var urlContent = new FormUrlEncodedContent(Params);
            string queryParamsString = await urlContent.ReadAsStringAsync();
            string url = $"{APIURL}?{queryParamsString}";
            HttpResponseMessage response = await client.GetAsync(url);
            string responseBody = await response.Content.ReadAsStringAsync();
            if (!response.IsSuccessStatusCode)
                return await CreateGeoApiError<LocationDto>(response);
            var resultLocation = CreateFromJsonBody(responseBody);
            return resultLocation;
        }

        public async Task<OperationResult<LocationDto>> GetLocationByAddressAsync(string addressToFind)
        {
            //var client = httpClientFactory.CreateClient();
            var client = new HttpClient();
            var Params = new Dictionary<string, string>()
            {
                ["apikey"] = APIKEY,
                ["format"] = "json",
                ["geocode"] = $"{addressToFind}",
                ["sco"] = "longlat",
                ["kind"] = "house",
                ["lang"] = "ru_RU",
            };

            var urlContent = new FormUrlEncodedContent(Params);
            string queryParamsString = await urlContent.ReadAsStringAsync();
            string url = $"{APIURL}?{queryParamsString}";
            HttpResponseMessage response = await client.GetAsync(url);
            string responseBody = await response.Content.ReadAsStringAsync();
            if (!response.IsSuccessStatusCode)
                return await CreateGeoApiError<LocationDto>(response);
            var resultLocation = CreateFromJsonBody(responseBody);
            return resultLocation;

        }

        private async Task<OperationResult<T>> CreateGeoApiError<T>(HttpResponseMessage response)
        {
            var responseBody = await response.Content.ReadAsStringAsync();
            var problem = JsonSerializer.Deserialize<GeoCoderErrorResponse>(responseBody);
            if (problem == null)
                throw new InvalidOperationException("TODO PARSING FAILED : 3513");
            return OperationResult<T>.Error(problem.message, (HttpStatusCode)problem.statusCode);
        }

        private OperationResult<LocationDto> CreateFromJsonBody(string body)
        {
            var Result = JsonSerializer
                .Deserialize<GeoCoderResponse>(body, SerializerOptions);
            if (Result == null || Result.Response == null)
            {
                return OperationResult<LocationDto>.Error("TODO Parsing null failure", HttpStatusCode.InternalServerError);
            }
            var geocoderMeta = Result.Response.GeoObjectCollection.FeatureMember[0].GeoObject.MetaDataProperty.GeocoderMetaData;
            var cityString = geocoderMeta.Address.Components[2].Name;
            var addressString = geocoderMeta.Address.Formatted;

            string[] longlat = Result.Response.GeoObjectCollection.FeatureMember[0].GeoObject.Point.Pos.Split(' ');

            if (double.TryParse(longlat[0], NumberStyles.Any, CultureInfo.InvariantCulture, out double longitude) &&
                double.TryParse(longlat[1], NumberStyles.Any, CultureInfo.InvariantCulture, out double latitude))
            {
                return OperationResult<LocationDto>.Success(new LocationDto
                {
                    Address = addressString,
                    City = cityString,
                    GeoPoint = new GeoPoint(longitude, latitude)
                });
            }
            else
                return OperationResult<LocationDto>.Error("TODO Parsing LL Failure", HttpStatusCode.InternalServerError);
        }

    }
}
