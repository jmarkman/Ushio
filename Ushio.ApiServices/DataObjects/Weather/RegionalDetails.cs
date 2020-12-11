using Newtonsoft.Json;

namespace Ushio.ApiServices.DataObjects.Weather
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
