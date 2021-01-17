using Newtonsoft.Json;

namespace Ushio.Data.Weather
{
    public class GeographicCoordinates
    {
        [JsonProperty(PropertyName = "lon")]
        public float Longitude { get; set; }
        [JsonProperty(PropertyName = "lat")]
        public float Latitude { get; set; }
    }
}
