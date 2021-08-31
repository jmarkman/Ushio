using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ushio.Core.Tests
{
    internal static class TestCaseGenerator
    {
        private static readonly string ParseCharacterFromTitleDataPath = "Data\\ParseCharacterFromTitle.json";
        private static readonly string ParsePlayerFromTitleDataPath = "Data\\ParsePlayerFromTitle.json";

        public static List<ParseTestCase> GetParseCharacterFromTitleTestData()
        {
            var json = File.ReadAllText(ParseCharacterFromTitleDataPath);
            return JsonConvert.DeserializeObject<List<ParseTestCase>>(json);
        }

        public static List<ParseTestCase> GetParsePlayerFromTitleTestData()
        {
            var json = File.ReadAllText(ParsePlayerFromTitleDataPath);
            return JsonConvert.DeserializeObject<List<ParseTestCase>>(json);
        }
    }

    internal class ParseTestCase
    {
        public string Title { get; set; }
        public string Channel { get; set; }
        public string SearchTerm { get; set; }
    }
}
