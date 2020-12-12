using Discord;
using Discord.Commands;
using Discord.WebSocket;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.IO;
using System.Reflection;
using System.Threading.Tasks;
using Ushio.ApiServices;
using Ushio.Services;

namespace Ushio
{
    public class Startup
    {
        private IConfigurationRoot _config;

        public Startup(string[] args)
        {
            var cfgBuilder = new ConfigurationBuilder()
                .SetBasePath(Path.Combine(AppContext.BaseDirectory, "Common"))
                .AddJsonFile("appSettings.json", optional: false, reloadOnChange: true);

            _config = cfgBuilder.Build();
        }

        public async Task StartAsync()
        {
            var services = new ServiceCollection();
            ConfigureServices(services);

            var provider = services.BuildServiceProvider();

            var discord = provider.GetRequiredService<DiscordSocketClient>();
            await discord.LoginAsync(TokenType.Bot, _config["ApiKeys:Discord"]);
            await discord.StartAsync();

            var commands = provider.GetRequiredService<CommandService>();
            await commands.AddModulesAsync(Assembly.GetExecutingAssembly(), provider);

            await provider.GetRequiredService<CommandHandlingService>().StartAsync();

            await Task.Delay(-1);   
        }

        private void ConfigureServices(ServiceCollection services)
        {
            services.AddSingleton(new DiscordSocketClient(new DiscordSocketConfig
            {
                LogLevel = LogSeverity.Verbose,
                MessageCacheSize = 1000
            }))
            .AddSingleton(new CommandService(new CommandServiceConfig
            {
                CaseSensitiveCommands = false,
                IgnoreExtraArgs = false,
                LogLevel = LogSeverity.Verbose,
                DefaultRunMode = RunMode.Async
            }))
            .AddSingleton<CommandHandlingService>()
            .AddSingleton<PokemonApiService>()
            .AddSingleton(new WeatherApiService(_config["ApiKeys:OpenWeatherMap"]))
            .AddSingleton(_config);
        }
    }
}
