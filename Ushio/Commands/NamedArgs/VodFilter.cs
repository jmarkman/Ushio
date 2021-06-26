using Discord.Commands;
using System;
using System.Collections.Generic;
using System.Text;

namespace Ushio.Commands.NamedArgs
{
    [NamedArgumentType]
    public class VodFilter
    {
        public string Character { get; set; }
        public string Player { get; set; }
    }
}
