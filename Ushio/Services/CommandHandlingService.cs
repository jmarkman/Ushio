using Discord.Commands;
using Discord.WebSocket;
using Microsoft.Extensions.Configuration;
using System;
using System.Reflection;
using System.Threading.Tasks;
using Ushio.Commands.TypeReaders;

namespace Ushio.Services
{
    public class CommandHandlingService
    {
        private readonly CommandService _commandService;
        private readonly IServiceProvider _serviceProvider;
        private readonly DiscordSocketClient _discordClient;
        private readonly IConfigurationRoot _config;

        public CommandHandlingService(DiscordSocketClient client, CommandService cmdSvc, IServiceProvider svcProvider, IConfigurationRoot cfg)
        {
            _discordClient = client;
            _commandService = cmdSvc;
            _serviceProvider = svcProvider;
            _config = cfg;
        }

        public async Task StartAsync()
        {
            _discordClient.MessageReceived += OnMessageReceivedAsync;
            _commandService.AddTypeReader(typeof(int), new IntTypeReader());

            await _commandService.AddModulesAsync(Assembly.GetEntryAssembly(), _serviceProvider);
        }

        public void Stop()
        {
            _discordClient.MessageReceived -= OnMessageReceivedAsync;
        }

        private async Task OnMessageReceivedAsync(SocketMessage socketMessage)
        {
            if (socketMessage is not SocketUserMessage msg)
            {
                return;
            }

            if (socketMessage.Channel is not SocketGuildChannel)
            {
                return;
            }

            int argPos = 0;
            var msgContext = new SocketCommandContext(_discordClient, msg);

            if (msg.HasStringPrefix(_config["Prefix"], ref argPos) && msg.Content.Length > 1)
            {
                var result = await _commandService.ExecuteAsync(msgContext, argPos, _serviceProvider);

                if (!result.IsSuccess)
                {
                    await msgContext.Channel.SendMessageAsync(result.ToString());
                }
            }
        }
    }
}
