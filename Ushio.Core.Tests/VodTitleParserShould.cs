using Ushio.Data.YouTube;
using Xunit;

namespace Ushio.Core.Tests
{
    public class VodTitleParserShould
    {
        [Fact]
        public void ShouldParseCharacter_FromTitleCorrectly()
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

        /// <summary>
        /// Fighting Game Village switched title formats as Strive went from brand new to
        /// established game. The parser should be able to retrieve content from both the
        /// old title format and the new title format.
        /// </summary>
        [Fact]
        public void ShouldParseContent_FromOldAndNewFightingGameVillageTitleNamingSchemes()
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

        /// <summary>
        /// Guilty Gear Strive MOVIES has an older title format where the vod title didn't have
        /// [GGST] prefixing the rest of the title. There was an issue with parsing CJK names
        /// from this older format where the regex wasn't finding any characters despite the
        /// regex using lazy capture of all characters. This test ensures that the parsing
        /// method will retrieve the player name from the older format, focusing on CJK names.
        /// </summary>
        [Fact]
        public void ShouldParse_CJKPlayerName_FromOldGuiltyGearStriveMOVIESTitleNamingScheme()
        {
            YouTubeVideo vod = new()
            {
                Title = "レオ vs Punk (レオ/LEO vs ミリア/MILLIA)[GGST/GUILTY GEAR STRIVE/ギルティギアストライヴ]",
                SourceChannel = "Guilty Gear Strive MOVIES"
            };

            var expectedCJKName = "レオ";

            var vtpUnderTest = new VodTitleParser();

            var result = vtpUnderTest.ParsePlayerFromVideoTitle(vod);

            Assert.Equal(expectedCJKName, result);
        }

        [Fact]
        public void ShouldCorrectParse_CharactersAndPlayers_WithNonCJKNames_FromOldGuiltyGearStriveMOVIESTitleNamingScheme()
        {
            YouTubeVideo vod = new()
            {
                Title = "[GGST] SonicFox(レオ/LEO) vs Verix777(名残雪/NAGORIYUKI) [GUILTY GEAR STRIVE/ギルティギアストライヴ]",
                SourceChannel = "Guilty Gear Strive MOVIES"
            };

            var expectedPlayer1 = "SonicFox";
            var expectedPlayer1Character = "Leo";
            var expectedPlayer2 = "Verix777";
            var expectedPlayer2Character = "Nagoriyuki";

            var vtpUnderTest = new VodTitleParser();

            var player1 = vtpUnderTest.ParsePlayerFromVideoTitle(vod);
            var player2 = vtpUnderTest.ParsePlayerFromVideoTitle(vod, true);

            var p1Character = vtpUnderTest.ParseCharacterFromVideoTitle(vod);
            var p2Character = vtpUnderTest.ParseCharacterFromVideoTitle(vod, true);

            Assert.Equal(expectedPlayer1, player1);
            Assert.Equal(expectedPlayer2, player2);
            Assert.Equal(expectedPlayer1Character, p1Character);
            Assert.Equal(expectedPlayer2Character, p2Character);
        }

        [Fact]
        public void ShouldParsePlayer1_FromTitle()
        {
            var vtpUnderTest = new VodTitleParser();

            var testData = TestCaseGenerator.GetParseFirstPlayerFromTitleTestData();

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

        [Fact]
        public void ShouldParsePlayer2_FromTitle()
        {
            var vtpUnderTest = new VodTitleParser();

            var testData = TestCaseGenerator.GetParseSecondPlyerFromTitleTestData();

            foreach (var mock in testData)
            {
                YouTubeVideo ytVideo = new()
                {
                    Title = mock.Title,
                    SourceChannel = mock.Channel
                };

                var returnPlayerName = vtpUnderTest.ParsePlayerFromVideoTitle(ytVideo, parsePlayer2: true);

                Assert.Equal(mock.SearchTerm, returnPlayerName);
            }
        }
    }
}
