using Ushio.Data.YouTube;
using Xunit;

namespace Ushio.Core.Tests
{
    public class VodTitleParserShould
    {
        [Fact]
        public void ParseCharacterFromTitleCorrectly()
        {
            //TODO: Extract test data creation to separate class
            // Guilty Gear Strive MOVIES
            var ggstMOVIESExampleTitle = "[GGST] ぶりーど vs Kazunoko (レオ/LEO vs チップ/CHIPP) [GUILTY GEAR STRIVE/ギルティギアストライヴ]";
            var ggstMOVIESChannelName = "Guilty Gear Strive MOVIES";
            var ggstMOVIESExpectedCharacterName = "Leo";

            YouTubeVideo ggstMOVIESChannelVod = new()
            {
                Title = ggstMOVIESExampleTitle,
                SourceChannel = ggstMOVIESChannelName
            };

            // Fighting Game Village
            var fgvExampleTitle = "Daru(イノ/Ino) vs T5M7(レオ/Leo) Guilty Gear Strive 天上階対戦【GGST】60fps";
            var fgvChannelName = "Kakuto gemu-mura (Fighting Game Village)";
            var fgvExpectedCharacterName = "Ino";

            YouTubeVideo fgvChannelVod = new()
            {
                Title = fgvExampleTitle,
                SourceChannel = fgvChannelName
            };

            // Gamestorage Ch
            var gschExampleTitle = "【Guilty Gear Strive】Domi(Anji) vs GNT(Leo) High Level Gameplay【GGST】【PS4pro/60FPS】";
            var gschChannelName = "GameStorage Ch";
            var gschExpectedCharacterName = "Anji";

            YouTubeVideo gschChannelVod = new()
            {
                Title = gschExampleTitle,
                SourceChannel = gschChannelName
            };

            var vtpUnderTest = new VodTitleParser();

            var ggstMOVIESReturnedCharacterName = vtpUnderTest.ParseCharacterFromVideoTitle(ggstMOVIESChannelVod);
            var fgvReturnedCharacterName = vtpUnderTest.ParseCharacterFromVideoTitle(fgvChannelVod);
            var gschReturnedCharacterName = vtpUnderTest.ParseCharacterFromVideoTitle(gschChannelVod);

            Assert.Equal(ggstMOVIESExpectedCharacterName, ggstMOVIESReturnedCharacterName);
            Assert.Equal(fgvExpectedCharacterName, fgvReturnedCharacterName);
            Assert.Equal(gschExpectedCharacterName, gschReturnedCharacterName);
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
            var ggstMOVIESExampleTitle = "[GGST] ぶりーど vs Kazunoko (レオ/LEO vs チップ/CHIPP) [GUILTY GEAR STRIVE/ギルティギアストライヴ]";
            var ggstMOVIESChannelName = "Guilty Gear Strive MOVIES";
            var ggstMOVIESExpectedPlayerName = "ぶりーど";

            YouTubeVideo ggstMOVIESChannelVod = new()
            {
                Title = ggstMOVIESExampleTitle,
                SourceChannel = ggstMOVIESChannelName
            };

            var vtpUnderTest = new VodTitleParser();

            var ggstMOVIESReturnedPlayerName = vtpUnderTest.ParsePlayerFromVideoTitle(ggstMOVIESChannelVod);

            Assert.Equal(ggstMOVIESExpectedPlayerName, ggstMOVIESReturnedPlayerName);
        }
    }
}
