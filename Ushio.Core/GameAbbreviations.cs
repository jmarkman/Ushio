using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using Ushio.Data;

namespace Ushio.Core
{
    public class GameAbbreviations
    {
        public List<Game> List { get; private set; }

        public GameAbbreviations()
        {
            var abbrFilePath = Path.Combine(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location), @"DataSources\gameAbbreviations.json");
            var json = File.ReadAllText(abbrFilePath);

            List = JsonConvert.DeserializeObject<List<Game>>(json);
        }
    }
}
