using Discord;
using Discord.Commands;
using System;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using Ushio.ApiServices;
using Ushio.ApiServices.DataObjects.Weather;

namespace Ushio.Commands
{
    [Name("WeatherForecast")]
    public class WeatherCommands : ModuleBase<SocketCommandContext>
    {
        private const string weatherIconUrl = "http://openweathermap.org/img/w/";
        private readonly WeatherApiService weatherApi;

        public WeatherCommands(WeatherApiService weather)
        {
            weatherApi = weather;
        }

        /// <summary>
        /// Retrieves the current weather forecast for the provided location data
        /// </summary>
        /// <param name="input">This method should receive either a 5-digit zip code
        /// or a location string as input</param>
        [Command("weather")]
        public async Task GetWeatherFor(string input)
        {
            WeatherApiResponse weatherData;

            try
            {
                weatherData = await weatherApi.GetCurrentWeatherAsync(input);

                await ReplyAsync(embed: CreateWeatherEmbed(weatherData));
            }
            catch (WebException httpGetExc)
            {
                await ReplyAsync(httpGetExc.Message);
            }
        }

        /// <summary>
        /// Creates the embed used for displaying the weather data to the end user
        /// </summary>
        /// <param name="weatherData">The weather data as a <see cref="WeatherApiResponse"/> object</param>
        /// <returns>A discord <see cref="Embed"/> object</returns>
        private Embed CreateWeatherEmbed(WeatherApiResponse weatherData)
        {
            var weatherEmbed = new EmbedBuilder()
            .WithTitle($"Current weather for {weatherData.CityName} ({weatherData.RegionInfo.CountryCode})")
            .WithImageUrl($"{weatherIconUrl}{weatherData.Weather.FirstOrDefault().WeatherConditionIcon}.png")
            .AddField("Temperature", (int)weatherData.TemperatureAndPressure.Temperature, true)
            .AddField("Condition", weatherData.Weather.FirstOrDefault().WeatherCondition, true)
            .AddField("Humidity", $"{weatherData.TemperatureAndPressure.Humidity}%", true)
            .AddField("High", (int)weatherData.TemperatureAndPressure.MaximumTemperature, true)
            .AddField("Low", (int)weatherData.TemperatureAndPressure.MinimumTemperature, true)
            .AddField("Cloud Coverage", $"{weatherData.CloudInfo.CloudCoveragePercent}%", true)
            .WithFooter(new EmbedFooterBuilder { Text = DateTime.Now.ToString("f")})
            .Build();

            return weatherEmbed;
        }
    }
}
