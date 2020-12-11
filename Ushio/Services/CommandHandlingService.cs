using Discord.Commands;
using Discord.WebSocket;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Ushio.Services
{
    public class CommandHandlingService
    {
        private readonly CommandService _commandService;
        private readonly IServiceProvider _serviceProvider;
        private readonly DiscordSocketClient _discordClient;

        public CommandHandlingService(DiscordSocketClient client, CommandService cmdSvc, IServiceProvider svcProvider)
        {
            _discordClient = client;
            _commandService = cmdSvc;
            _serviceProvider = svcProvider;
        }

        public void Start()
        {
            _discordClient.MessageReceived += OnMessageRecievedAsync;
        }

        public void Stop()
        {
            _discordClient.MessageReceived -= OnMessageRecievedAsync;
        }

        private async Task OnMessageRecievedAsync(SocketMessage socketMessage)
        {
            if (!(socketMessage is SocketUserMessage msg))
            {
                return;
            }

            if (!(socketMessage.Channel is SocketGuildChannel))
            {
                return;
            }

            int argPos = 0;
            var msgContext = new SocketCommandContext(_discordClient, msg);
        }
    }
}
