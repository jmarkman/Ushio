using System;
using System.Text.RegularExpressions;
using Ushio.Data.YouTube;

namespace Ushio.Core.ParsingRules
{
    public class HuaWenSolAndGamestorageChTitleParserRule : ITitleParseRule
    {
        private const char RightBlackLenticularBracket = '】';

        public string ParseCharacter(YouTubeVideo ytVideo, bool parsePlayer2)
        {
            if (ytVideo.SourceChannel.ToLower() == "gamestorage ch" || ytVideo.SourceChannel.ToLower() == "huawen sol")
            {
                string playerAndCharacter;

                if (parsePlayer2)
                {
                    playerAndCharacter = LenticularBracketTitleParser(ytVideo.Title, parsePlayer2);
                }
                else
                {
                    playerAndCharacter = LenticularBracketTitleParser(ytVideo.Title);
                }

                // HuaWen Sol uploads videos that highlight one single player and their character,
                // so cover these instances by ensuring that the character name will be an empty string
                if (parsePlayer2 && string.IsNullOrWhiteSpace(playerAndCharacter))
                {
                    return null;
                }
                else
                {
                    // Add 1 to the index since we want to start the substring after the opening parenthesis
                    var openParenthesisIndex = playerAndCharacter.IndexOf('(') + 1;
                    var character = playerAndCharacter[openParenthesisIndex..].Replace(")", string.Empty);

                    return character.Trim();
                }
            }

            return null;
        }

        public string ParsePlayer(YouTubeVideo ytVideo, bool parsePlayer2)
        {
            if (ytVideo.SourceChannel.ToLower() == "gamestorage ch" || ytVideo.SourceChannel.ToLower() == "huawen sol")
            {
                string playerAndCharacter;

                if (parsePlayer2)
                {
                    playerAndCharacter = LenticularBracketTitleParser(ytVideo.Title, parsePlayer2);
                }
                else
                {
                    playerAndCharacter = LenticularBracketTitleParser(ytVideo.Title);
                }

                // HuaWen Sol uploads videos that highlight one single player and their character,
                // so cover these instances by ensuring that the player name will be an empty string
                if (parsePlayer2 && string.IsNullOrWhiteSpace(playerAndCharacter))
                {
                    return null;
                }
                else
                {
                    var openingParenthesisIdx = playerAndCharacter.IndexOf('(');
                    return playerAndCharacter.Substring(0, openingParenthesisIdx);
                }
            }

            return null;
        }

        /// <summary>
        /// Given a video title from any channel that uses lenticular brackets, get the character
        /// and player from the video title
        /// </summary>
        /// <param name="title">The title of the YouTube video</param>
        /// <param name="getPlayer2">If true, will get the information for player 2. Default is false (player 1)</param>
        /// <returns>The desired player and their character from the title as a string</returns>
        private string LenticularBracketTitleParser(string title, bool getPlayer2 = false)
        {
            Regex gamestorageTitleRegex = new($@"(?<={RightBlackLenticularBracket})(.*)vs(.*\))");
            string player1;
            string player2;
            var playerTitleSegment = gamestorageTitleRegex.Match(title);

            if (playerTitleSegment.Success)
            {
                player1 = playerTitleSegment.Groups[1].Value.Trim();
                player2 = playerTitleSegment.Groups[2].Value.Trim();

                if (getPlayer2)
                {
                    return player2;
                }
                else
                {
                    return player1;
                }
            }

            return null;
        }
    }
}
