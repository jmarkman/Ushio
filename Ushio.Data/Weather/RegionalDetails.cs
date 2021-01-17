using Newtonsoft.Json;

namespace Ushio.Data.Weather
{
    public class RegionalDetails
    {
        [JsonProperty(PropertyName = "country")]
        public string CountryCode { get; set; }
        [JsonProperty(PropertyName = "sunrise")]
        public int SunriseTime { get; set; }
        [JsonProperty(PropertyName = "sunset")]
        public int SunsetTime { get; set; }
    }
}
