using Newtonsoft.Json;

namespace Ushio.ApiServices.DataObjects.Weather
{
    public class Clouds
    {
        [JsonProperty(PropertyName = "all")]
        public int CloudCoveragePercent { get; set; }
    }
}
