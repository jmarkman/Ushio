using Newtonsoft.Json;
using System.Linq;

namespace Ushio.Data
{
    public class VideoGameNameData
    {
        [JsonProperty(PropertyName = "name")]
        public string Name { get; set; }

        [JsonProperty(PropertyName = "aliases")]
        public string[] Aliases { get; set; }

        public string GetEnumFromName()
        {
            return Name switch
            {
                "Guilty Gear XX Accent Core +R" => "GuiltyGearXXACPlusR",
                _ => new string(Name.ToCharArray().Where(c => !char.IsWhiteSpace(c)).ToArray()),
            };
        }
    }
}
