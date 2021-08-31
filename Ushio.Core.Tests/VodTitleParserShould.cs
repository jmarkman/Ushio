using Ushio.Data.YouTube;
using Xunit;

namespace Ushio.Core.Tests
{
    public class VodTitleParserShould
    {
        [Fact]
        public void ParseCharacterFromTitleCorrectly()
        {
            var vtpUnderTest = new VodTitleParser();

            var testData = TestCaseGenerator.GetParseCharacterFromTitleTestData();

            foreach (var mock in testData)
            {
                YouTubeVideo ytVideo = new()
                {
                    Title = mock.Title,
                    SourceChannel = mock.Channel
                };

                var returnedCharacterName = vtpUnderTest.ParseCharacterFromVideoTitle(ytVideo);

                Assert.Equal(mock.SearchTerm, returnedCharacterName);
            }
        }

        [Fact]
        public void ParseContentFromOldAndNewFightingGameVillageTitleNamingSchemes()
        {
            var channelName = "Kakuto gemu-mura (Fighting Game Village)";

            var newTitleFormat = "Daru(イノ/Ino) vs T5M7(レオ/Leo) Guilty Gear Strive 天上階対戦【GGST】60fps";
            var oldTitleFormat = "コイチ(イノ) vs MSY(名残雪) Guilty Gear Strive 天上階対戦 Koichi(Ino),MSY(Nagoriyuki)【GGST】60fps";
            var expectedCharacter = "Ino";

            YouTubeVideo newTitleFormatVod = new()
            {
                Title = newTitleFormat,
                SourceChannel = channelName
            };

            YouTubeVideo oldTitleFormatVod = new()
            {
                Title = oldTitleFormat,
                SourceChannel = channelName
            };

            var vtpUnderTest = new VodTitleParser();

            var resultFromOld = vtpUnderTest.ParseCharacterFromVideoTitle(oldTitleFormatVod);
            var resultFromNew = vtpUnderTest.ParseCharacterFromVideoTitle(newTitleFormatVod);

            Assert.Equal(expectedCharacter, resultFromOld);
            Assert.Equal(expectedCharacter, resultFromNew);
        }

        [Fact]
        public void ParsePlayer1FromTitleCorrectly()
        {
            var vtpUnderTest = new VodTitleParser();

            var testData = TestCaseGenerator.GetParsePlayerFromTitleTestData();

            foreach (var mock in testData)
            {
                YouTubeVideo ytVideo = new()
                {
                    Title = mock.Title,
                    SourceChannel = mock.Channel
                };

                var returnPlayerName = vtpUnderTest.ParsePlayerFromVideoTitle(ytVideo);

                Assert.Equal(mock.SearchTerm, returnPlayerName);
            }
        }
    }
}
