using Microsoft.Extensions.Logging;
using System;
using System.IO;

namespace Ushio.Core.Logging
{
    public class UshioLogger : ILogger
    {
        private readonly string _outputDirectory;
        private readonly string _categoryName;

        private string _logFile => Path.Combine(_outputDirectory, CreateLogFilename(DateTime.Now));

        public UshioLogger(string categoryName)
        {
            _categoryName = categoryName;
            _outputDirectory = Path.Combine(AppContext.BaseDirectory, "Logs");
        }

        public bool IsEnabled(LogLevel logLevel)
        {
            return true;
        }

        public void Log<TState>(LogLevel logLevel, EventId eventId, TState state, Exception exception, Func<TState, Exception, string> formatter)
        {
            var logDetail = new UshioLogDetail
            {
                LogLevel = logLevel,
                Location = _categoryName,
                Message = formatter(state, exception)
            };

            var logText = logDetail.ToString();

            if (!Directory.Exists(_outputDirectory))
            {
                Directory.CreateDirectory(_outputDirectory);
            }

            var logFileInfo = new FileInfo(_logFile);

            using var logWriter = logFileInfo.AppendText();
            logWriter.WriteLine(logText);

            LogToConsole(logDetail);
        }

        public IDisposable BeginScope<TState>(TState state)
        {
            return null;
        }

        private void LogToConsole(UshioLogDetail logDetail)
        {
            ConsoleColor levelColor = ConsoleColor.White;
            ConsoleColor messageColor = ConsoleColor.White;

            Console.WriteLine(logDetail.Timestamp.ToString(), ConsoleColor.Gray);
            
            switch (logDetail.LogLevel)
            {
                case LogLevel.Trace:
                case LogLevel.Debug:
                case LogLevel.Information:
                    levelColor = ConsoleColor.Green;
                    break;
                case LogLevel.Warning:
                    levelColor = ConsoleColor.Yellow;
                    break;
                case LogLevel.Error:
                case LogLevel.Critical:
                    levelColor = ConsoleColor.Red;
                    break;
                case LogLevel.None:
                default:
                    break;
            }

            Console.Write($"[{logDetail.LogLevel}] - ", levelColor);
            Console.Write($"({logDetail.Location}): ", messageColor);
            Console.Write($"{logDetail.Message}", messageColor);
            Console.WriteLine();
        }

        private string CreateLogFilename(DateTime dt)
        {
            return $"UshioLog-{dt:MMM dd yyyy, HH.mm.ss}.txt";
        }
    }
}
