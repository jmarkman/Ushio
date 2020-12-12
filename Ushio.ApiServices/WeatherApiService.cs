using Newtonsoft.Json;
using System;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using Ushio.ApiServices.DataObjects.Weather;

namespace Ushio.ApiServices
{
    public class WeatherApiService
    {
        private const string ImperialMeasurement = "imperial";
        private const string MetricMeasurement = "metric";
        private readonly string _apiKey;
        private readonly static HttpClient httpClient = CreateHttpClient();

        public WeatherApiService(string key)
        {
            _apiKey = key;
        }

        /// <summary>
        /// Get the weather for the specified location via postal (ZIP) code.
        /// </summary>
        /// <param name="zipCode">The postal code for the area</param>
        /// <param name="useImperial">Uses imperial measurements by default, specify false to this parameter to use metric</param>
        /// <returns></returns>
        public async Task<WeatherApiResponse> GetWeatherByZipCodeAsync(int zipCode, bool useImperial = true)
        {
            StringBuilder urlParamsBuilder = new StringBuilder();
            urlParamsBuilder.Append($"weather?zip={zipCode},us");

            if (useImperial)
            {
                urlParamsBuilder.Append($"&units={ImperialMeasurement}");
            }
            else
            {
                urlParamsBuilder.Append($"&units={MetricMeasurement}");
            }

            urlParamsBuilder.Append($"&appid={_apiKey}");

            return await GetWeatherAsync(urlParamsBuilder.ToString());
        }

        /// <summary>
        /// Get the weather for the specified location. Input is flexible and allows the user to specify either
        /// [city]/[city],[state]/[city],[state],[country] as the location.
        /// </summary>
        /// <param name="location">The location as a singular code or series of codes</param>
        /// <param name="useImperial">Uses imperial measurements by default, specify false to this parameter to use metric</param>
        /// <returns>The weather for the specified named location as a <see cref="WeatherApiResponse"/></returns>
        public async Task<WeatherApiResponse> GetWeatherByNamedLocationAsync(string location, bool useImperial = true)
        {
            StringBuilder urlParamsBuilder = new StringBuilder();
            urlParamsBuilder.Append($"weather?q={location}");

            if (useImperial)
            {
                urlParamsBuilder.Append($"&units={ImperialMeasurement}");
            }
            else
            {
                urlParamsBuilder.Append($"&units={MetricMeasurement}");
            }

            urlParamsBuilder.Append($"&appid={_apiKey}");

            return await GetWeatherAsync(urlParamsBuilder.ToString());
        }

        /// <summary>
        /// Performs a HTTP GET request to the OpenWeatherMap API using the supplied query
        /// </summary>
        /// <param name="query">The url parameters that make up the weather data query</param>
        /// <returns>The weather for the specified region as a <see cref="WeatherApiResponse"/></returns>
        private async Task<WeatherApiResponse> GetWeatherAsync(string query)
        {
            WeatherApiResponse weatherData;

            var response = await httpClient.GetAsync(query);
            if (response.IsSuccessStatusCode)
            {
                var data = await response.Content.ReadAsStringAsync();
                weatherData = JsonConvert.DeserializeObject<WeatherApiResponse>(data);
            }
            else
            {
                var idx = query.LastIndexOf("&");
                var errQuery = query.Substring(0, idx);
                throw new WebException($"[{(int)response.StatusCode} {response.ReasonPhrase}] The weather query failed to return valid data.{Environment.NewLine}{Environment.NewLine}Query: '{errQuery}'");
            }

            return weatherData;
        }

        /// <summary>
        /// Constructs the HttpClient to use for the life of the service
        /// </summary>
        private static HttpClient CreateHttpClient()
        {
            var client = new HttpClient();
            client.BaseAddress = new Uri("https://api.openweathermap.org/data/2.5/weather");
            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

            return client;
        }
    }
}
