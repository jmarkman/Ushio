using Newtonsoft.Json;
using System.Collections.Generic;
using System.IO;
using System.Reflection;

namespace Ushio.Data
{
    public class UshioConstants
    {
        private readonly string abbrFilePath;
        private readonly string vodChannelFilePath;
        private readonly string fortunesPath;
        private readonly string emotesPath;

        public List<VideoGameNameData> GameAbbreviations { get; private set; }
        public List<VodChannel> VodChannels { get; private set; }
        public List<string> Fortunes { get; private set; }
        public List<DiscordEmote> Emotes { get; set; }

        public UshioConstants()
        {
            var executingAssemblyLocation = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);

            abbrFilePath = Path.Combine(executingAssemblyLocation, "DataSources", "gameAbbreviations.json");
            vodChannelFilePath = Path.Combine(executingAssemblyLocation, "DataSources", "vodChannels.json");
            fortunesPath = Path.Combine(executingAssemblyLocation, "DataSources", "Fortunes.txt");
            emotesPath = Path.Combine(executingAssemblyLocation, "DataSources", "emoticons.json");
        }

        public UshioConstants InitializeGameAbbreviations()
        {
            var json = File.ReadAllText(abbrFilePath);

            GameAbbreviations = JsonConvert.DeserializeObject<List<VideoGameNameData>>(json);

            return this;
        }

        public UshioConstants InitializeVodChannelInfo()
        {
            var json = File.ReadAllText(vodChannelFilePath);

            VodChannels = JsonConvert.DeserializeObject<List<VodChannel>>(json);

            return this;
        }

        public UshioConstants InitializeFortunes()
        {
            Fortunes = new List<string>(File.ReadAllLines(fortunesPath));

            return this;
        }

        public UshioConstants InitializeEmotes()
        {
            var json = File.ReadAllText(emotesPath);

            Emotes = JsonConvert.DeserializeObject<List<DiscordEmote>>(json);

            return this;
        }
    }
}
