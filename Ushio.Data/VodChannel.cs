using Newtonsoft.Json;

namespace Ushio.Data
{
    public class VodChannel
    {
        [JsonProperty(PropertyName = "name")]
        public string Name { get; set; }

        [JsonProperty(PropertyName = "channelId")]
        public string Id { get; set; }
    }
}
