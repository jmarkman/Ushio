using Discord.Commands;

namespace Ushio.Data.NamedArgs
{
    [NamedArgumentType]
    public class VodFilter
    {
        public string Character { get; set; }
        public string Player { get; set; }
        public bool GetNewClips { get; set; }
    }
}
