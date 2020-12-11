using Newtonsoft.Json;

namespace Ushio.ApiServices.DataObjects.Weather
{
    public class GeographicCoordinates
    {
        [JsonProperty(PropertyName = "lon")]
        public float Longitude { get; set; }
        [JsonProperty(PropertyName = "lat")]
        public float Latitude { get; set; }
    }
}
