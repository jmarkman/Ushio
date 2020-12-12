using Discord;
using Discord.Commands;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Ushio.ApiServices;
using Ushio.ApiServices.DataObjects.Weather;

namespace Ushio.Commands
{
    [Name("WeatherForecast")]
    public class WeatherCommands : ModuleBase<SocketCommandContext>
    {
        private readonly WeatherApiService weatherApi;

        public WeatherCommands(WeatherApiService weather)
        {
            weatherApi = weather;
        }

        /// <summary>
        /// Retrieves the current weather forecast for the provided postal (ZIP) code.
        /// ZIP codes are only supported in the United States
        /// </summary>
        /// <param name="postalCode">The postal (ZIP) code, a 5-digit integer</param>
        [Command("weather")]
        public async Task GetWeatherFor(int postalCode)
        {
            var weatherData = await weatherApi.GetWeatherByZipCodeAsync(postalCode);

            var weatherEmbed = new EmbedBuilder
            {
                Title = $"Weather for {postalCode}",
                Fields = CreateWeatherEmbedFields(weatherData)
            }
            .WithFooter(new EmbedFooterBuilder { Text = DateTimeOffset.FromUnixTimeSeconds(weatherData.UnixTimeWhenGathered).ToString()})
            .Build();

            await ReplyAsync(embed: weatherEmbed);


        }

        /// <summary>
        /// Retrieves the current weather forecast for the provided location.
        /// </summary>
        /// <param name="location"></param>
        /// <returns></returns>
        public async Task GetWeatherFor(string location)
        {

        }

        private List<EmbedFieldBuilder> CreateWeatherEmbedFields(WeatherApiResponse weatherData)
        {
            List<EmbedFieldBuilder> weatherInfoFields = new List<EmbedFieldBuilder>()
                {
                    new EmbedFieldBuilder
                    {
                        Name = "City Name",
                        Value = weatherData.CityName
                    },
                    new EmbedFieldBuilder
                    {
                        Name = "Current Temperature",
                        Value = weatherData.TemperatureAndPressure.Temperature
                    },
                    new EmbedFieldBuilder
                    {
                        Name = "Current Weather Condition",
                        Value = weatherData.Weather.FirstOrDefault().WeatherCondition
                    }
                };

            return weatherInfoFields;
        }
    }
}
