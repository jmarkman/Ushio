using Newtonsoft.Json;

namespace Ushio.Data.Weather
{
    public class WeatherDescription
    {
        [JsonProperty(PropertyName = "id")]
        public int WeatherConditionId { get; set; }
        [JsonProperty(PropertyName = "main")]
        public string WeatherParameters { get; set; }
        [JsonProperty(PropertyName = "description")]
        public string WeatherCondition { get; set; }
        [JsonProperty(PropertyName = "icon")]
        public string WeatherConditionIcon { get; set; }
    }
}
