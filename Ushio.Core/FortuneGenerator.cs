using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;

namespace Ushio.Core
{
    public class FortuneGenerator
    {
        private readonly List<string> _fortunes;

        public FortuneGenerator()
        {
            string fortuneFilePath = Path.Combine(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location), @"DataSources\Fortunes.txt");
            _fortunes = File.ReadAllLines(fortuneFilePath).ToList();
        }

        public string GetFortune()
        {
            return _fortunes.Random();
        }
    }
}
