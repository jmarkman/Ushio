using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;

namespace Ushio.ApiServices
{
    public class WeatherApiService
    {
        private readonly string _apiKey;
        private readonly static HttpClient httpClient = CreateHttpClient();

        public WeatherApiService(string key)
        {
            _apiKey = key;
        }

        private static HttpClient CreateHttpClient()
        {
            var client = new HttpClient();
            client.BaseAddress = new Uri("https://api.openweathermap.org/data/2.5/weather");
            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

            return client;
        }
    }
}
