using System.Collections.Generic;
using Ushio.Core.ParsingRules;
using Ushio.Data.YouTube;

namespace Ushio.Core
{
    /// <summary>
    /// Houses the logic for parsing fighting game vod titles for
    /// player and character information
    /// </summary>
    public class VodTitleParser
    {
        private readonly List<ITitleParseRule> parsingRules;

        public VodTitleParser()
        {
            parsingRules = new List<ITitleParseRule>()
            {
                new FightingGameVillageTitleParserRule(),
                new GuiltyGearStriveMoviesTitleParseRule(),
                new HuaWenSolAndGamestorageChTitleParserRule()
            };
        }

        /// <summary>
        /// Parses a player from the vod title and returns it as a string.
        /// </summary>
        /// <param name="ytVideo">The youtube video object containing the source
        /// channel and the title of the video</param>
        /// <param name="parsePlayer2">If true, will parse player 2 out of the title.
        /// The rightmost player in the title is considered to be player 2.</param>
        /// <returns>The name of the specified player</returns>
        public string ParsePlayerFromVideoTitle(YouTubeVideo ytVideo, bool parsePlayer2 = false)
        {
            foreach (var rule in parsingRules)
            {
                var playerName = rule.ParsePlayer(ytVideo, parsePlayer2);
                if (playerName is not null)
                {
                    return playerName;
                }
            }

            return null;
        }

        /// <summary>
        /// Parses a character from the vod title and returns it as a string.
        /// </summary>
        /// <param name="ytVideo">The youtube video object containing the source
        /// channel and the title of the video</param>
        /// <param name="parsePlayer2Char">If true, will parse player 2's character
        /// out of the title. The rightmost player in the title is considered to be
        /// player 2.</param>
        /// <returns>The character the specified player is playing as</returns>
        public string ParseCharacterFromVideoTitle(YouTubeVideo ytVideo, bool parsePlayer2Char = false)
        {
            foreach (var rule in parsingRules)
            {
                var characterName = rule.ParseCharacter(ytVideo, parsePlayer2Char);
                if (characterName is not null)
                {
                    return characterName;
                }
            }

            return null;
        }
    }
}
