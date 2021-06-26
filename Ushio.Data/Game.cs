using Newtonsoft.Json;

namespace Ushio.Data
{
    public class Game
    {
        [JsonProperty(PropertyName = "name")]
        public string Name { get; set; }

        [JsonProperty(PropertyName = "aliases")]
        public string[] Aliases { get; set; }
    }
}
