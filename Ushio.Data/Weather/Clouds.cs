using Newtonsoft.Json;

namespace Ushio.Data.Weather
{
    public class Clouds
    {
        [JsonProperty(PropertyName = "all")]
        public int CloudCoveragePercent { get; set; }
    }
}
