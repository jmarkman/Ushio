using Newtonsoft.Json;
using System;

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
    }
}
