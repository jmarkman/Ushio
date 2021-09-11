using Newtonsoft.Json;
using System.Collections.Generic;
using System.IO;

namespace Ushio.Core.Tests
{
    internal static class TestCaseGenerator
    {
        private static readonly string ParseCharacterFromTitleDataPath = "Data\\ParseCharacterFromTitle.json";
        private static readonly string ParseFirstPlayerFromTitleDataPath = "Data\\ParseFirstPlayerFromTitle.json";
        private static readonly string ParseSecondPlayerFromTitleDataPath = "Data\\ParseSecondPlayerFromTitle.json";

        public static List<ParseTestCase> GetParseCharacterFromTitleTestData()
        {
            var json = File.ReadAllText(ParseCharacterFromTitleDataPath);
            return JsonConvert.DeserializeObject<List<ParseTestCase>>(json);
        }

        public static List<ParseTestCase> GetParseFirstPlayerFromTitleTestData()
        {
            var json = File.ReadAllText(ParseFirstPlayerFromTitleDataPath);
            return JsonConvert.DeserializeObject<List<ParseTestCase>>(json);
        }
        
        public static List<ParseTestCase> GetParseSecondPlyerFromTitleTestData()
        {
            var json = File.ReadAllText(ParseSecondPlayerFromTitleDataPath);
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
