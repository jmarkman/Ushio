﻿using Discord;
using Discord.Commands;
using Discord.WebSocket;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.IO;
using System.Reflection;
using System.Threading.Tasks;
using Ushio.ApiServices;
using Ushio.Core;
using Ushio.Infrastructure.Database;
using Ushio.Infrastructure.Database.Repositories;
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
            }));

            services.AddSingleton(new CommandService(new CommandServiceConfig
            {
                CaseSensitiveCommands = false,
                IgnoreExtraArgs = false,
                LogLevel = LogSeverity.Verbose,
                DefaultRunMode = RunMode.Async
            }));

            services.AddSingleton<CommandHandlingService>();
            services.AddSingleton<PokemonApiService>();
            services.AddSingleton(new WeatherApiService(_config["ApiKeys:OpenWeatherMap"]));
            services.AddSingleton<VideoClipRepository>();
            services.AddSingleton<FortuneGenerator>();
            services.AddDbContext<UshioDbContext>(options => options.UseNpgsql(_config["ConnectionStrings:Database"]));
            services.AddSingleton(_config);
        }
    }
}
