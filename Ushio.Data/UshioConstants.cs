using Newtonsoft.Json;
using System.Collections.Generic;
using System.IO;
using System.Reflection;

namespace Ushio.Data
{
    public class UshioConstants
    {
        private readonly string _abbrFilePath;
        private readonly string _vodChannelFilePath;
        private readonly string _fortunesPath;

        public List<VideoGameNameData> GameAbbreviations { get; private set; }
        public List<VodChannel> VodChannels { get; private set; }
        public List<string> Fortunes { get; private set; }

        public UshioConstants()
        {
            _abbrFilePath = Path.Combine(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location), Path.Join("DataSources", "gameAbbreviations.json"));
            _vodChannelFilePath = Path.Combine(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location), Path.Join("DataSources","vodChannels.json"));
            _fortunesPath = Path.Combine(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location), Path.Join("DataSources", "Fortunes.txt"));
        }

        public UshioConstants InitializeGameAbbreviations()
        {
            var json = File.ReadAllText(_abbrFilePath);

            GameAbbreviations = JsonConvert.DeserializeObject<List<VideoGameNameData>>(json);
            return this;
        }

        public UshioConstants InitializeVodChannelInfo()
        {
            var json = File.ReadAllText(_vodChannelFilePath);

            VodChannels = JsonConvert.DeserializeObject<List<VodChannel>>(json);
            return this;
        }

        public UshioConstants InitializeFortunes()
        {
            Fortunes = new List<string>(File.ReadAllLines(_fortunesPath));

            return this;
        }
    }
}
