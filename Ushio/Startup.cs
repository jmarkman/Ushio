using Discord.Commands;
using Discord.WebSocket;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.IO;
using System.Threading.Tasks;

namespace Ushio
{
    public class Startup
    {
        private IConfigurationRoot _config;

        public Startup(string[] args)
        {
            var cfgBuilder = new ConfigurationBuilder()
                .SetBasePath(Path.Combine(AppContext.BaseDirectory, "Configuration"))
                .AddJsonFile("appSettings.json", optional: false, reloadOnChange: true);

            _config = cfgBuilder.Build();
        }

        public async Task StartAsync()
        {

        }

        private void ConfigureServices(ServiceCollection services)
        {

        }
    }
}
