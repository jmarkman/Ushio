using Newtonsoft.Json;

namespace Ushio.Data.Weather
{
    public class TemperatureAndPressure
    {
        [JsonProperty(PropertyName = "temp")]
        public float Temperature { get; set; }
        [JsonProperty(PropertyName = "pressure")]
        public int Pressure { get; set; }
        [JsonProperty(PropertyName = "humidity")]
        public int Humidity { get; set; }
        [JsonProperty(PropertyName = "temp_min")]
        public float MinimumTemperature { get; set; }
        [JsonProperty(PropertyName = "temp_max")]
        public float MaximumTemperature { get; set; }
    }
}
