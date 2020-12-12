using Discord.Commands;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Ushio.ApiServices;

namespace Ushio.Commands
{
    public class WeatherCommands : ModuleBase<SocketCommandContext>
    {
        private readonly WeatherApiService weatherApi;

        public WeatherCommands(WeatherApiService weather)
        {
            weatherApi = weather;
        }

        public async Task GetWeatherFor(string location)
        {

        }
    }
}
