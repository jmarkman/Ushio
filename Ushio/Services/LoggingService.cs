using Discord;
using Discord.Commands;
using Discord.WebSocket;
using Microsoft.Extensions.Logging;
using System.Threading.Tasks;

namespace Ushio.Services
{
    public class LoggingService
    {
        private readonly ILoggerFactory _factory;
        private readonly DiscordSocketClient _socketClient;
        private readonly CommandService _cmdService;

        public LoggingService(DiscordSocketClient client, CommandService commands, ILoggerFactory factory)
        {
            _factory = factory;
            _cmdService = commands;
            _socketClient = client;
        }


        public void Start()
        {
            _cmdService.Log += OnLogAsync;
            _socketClient.Log += OnLogAsync;
        }

        private Task OnLogAsync(LogMessage msg)
        {
            var logger = _factory.CreateLogger(msg.Source);
            var textMsg = msg.Exception?.ToString() ?? msg.Message;

            switch (msg.Severity)
            {
                case LogSeverity.Critical:
                case LogSeverity.Error:
                    logger.LogError(textMsg);
                    break;
                case LogSeverity.Warning:
                    logger.LogWarning(textMsg);
                    break;
                case LogSeverity.Info:
                case LogSeverity.Verbose:
                case LogSeverity.Debug:
                    logger.LogInformation(textMsg);
                    break;
                default:
                    break;
            }

            return Task.CompletedTask;
        }
    }
}
