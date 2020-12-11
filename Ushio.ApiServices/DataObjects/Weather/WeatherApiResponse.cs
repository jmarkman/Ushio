using Newtonsoft.Json;

namespace Ushio.ApiServices.DataObjects.Weather
{
    public class WeatherApiResponse
    {
        [JsonProperty(PropertyName = "coord")]
        public GeographicCoordinates Coordinates { get; set; }
        [JsonProperty(PropertyName = "clouds")]
        public Clouds CloudInfo { get; set; }
        [JsonProperty(PropertyName = "weather")]
        public WeatherDescription[] Weather { get; set; }
        [JsonProperty(PropertyName = "main")]
        public TemperatureAndPressure TemperatureAndPressure { get; set; }
        [JsonProperty(PropertyName = "visibility")]
        public int Visibility { get; set; }
        [JsonProperty(PropertyName = "wind")]
        public WindDetails Wind { get; set; }
        [JsonProperty(PropertyName = "dt")]
        public int UnixTimeWhenGathered { get; set; }
        [JsonProperty(PropertyName = "sys")]
        public RegionalDetails RegionInfo { get; set; }
        [JsonProperty(PropertyName = "id")]
        public int ID { get; set; }
        [JsonProperty(PropertyName = "name")]
        public string CityName { get; set; }
    }
}
