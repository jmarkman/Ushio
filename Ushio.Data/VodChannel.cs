using Newtonsoft.Json;
using System;
using System.Linq;

namespace Ushio.Data
{
    public class VodChannel
    {
        [JsonProperty(PropertyName = "name")]
        public string Name { get; set; }

        [JsonProperty(PropertyName = "game")]
        public string Game { get; set; }

        [JsonProperty(PropertyName = "channelId")]
        public string Id { get; set; }

        [JsonProperty(PropertyName = "channelUrl")]
        public Uri Url { get; set; }

        public string GetEnumFromGame()
        {
            return Name switch
            {
                "Guilty Gear XX Accent Core +R" => "GuiltyGearXXACPlusR",
                _ => new string(Game.ToCharArray().Where(c => !char.IsWhiteSpace(c)).ToArray()),
            };
        }
    }
}
