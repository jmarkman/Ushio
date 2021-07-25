using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;

namespace Ushio.Core.Logging
{
    public class UshioLogDetail
    {
        public DateTime Timestamp { get; set; }
        public LogLevel LogLevel { get; set; }
        public string Message { get; set; }
        public string Location { get; set; }
        private string ReadableTimestamp => Timestamp.ToString("MM/dd/yy HH:mm:ss");
        private string AbbreviatedLogLevel => LogLevel.ToString().Substring(0, 4);

        public UshioLogDetail()
        {
            Timestamp = DateTime.Now;
        }

        public override string ToString() => $"{ReadableTimestamp} [{AbbreviatedLogLevel}] {Location}: {Message}";


    }
}
