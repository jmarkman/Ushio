using Discord;
using Discord.Commands;
using Discord.WebSocket;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace Ushio.Services
{
    public class LoggingService
    {
        private readonly DiscordSocketClient _client;
        private readonly CommandService _cmdService;

        public LoggingService(DiscordSocketClient client, CommandService commands)
        {
            _cmdService = commands;
            _client = client;
        }


        public void Start()
        {
            _cmdService.Log += OnLogAsync;
            _client.Log += OnLogAsync;
        }

        private Task OnLogAsync(LogMessage msg)
        {
            if (msg.Exception is CommandException commandException)
            {
                Console.WriteLine($"[Command | {msg.Severity}] {commandException.Command.Aliases[0]} failed to execute in {commandException.Context.Channel}");
                Console.WriteLine(commandException);
            }    
            else
            {
                Console.WriteLine($"[Command | {msg.Severity}] {msg}");
            }

            return Task.CompletedTask;
        }
    }
}
