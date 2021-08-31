using System;
using System.Linq;
using System.Text.RegularExpressions;
using Ushio.Data.YouTube;

namespace Ushio.Core
{
    /// <summary>
    /// Houses the logic for parsing fighting game vod titles for
    /// player and character information
    /// </summary>
    public class VodTitleParser
    {
        private const char RightBlackLenticularBracket = '】';
        private const char LeftBlackLenticularBracket = '【';

        /// <summary>
        /// Parses a player from the vod title and returns it as a string.
        /// </summary>
        /// <param name="ytVideo">The youtube video object containing the source
        /// channel and the title of the video</param>
        /// <param name="parsePlayer2">If true, will parse player 2 out of the title.
        /// The rightmost player in the title is considered to be player 2.</param>
        /// <returns>The name of the player as a string</returns>
        public string ParsePlayerFromVideoTitle(YouTubeVideo ytVideo, bool parsePlayer2 = false)
        {
            var playerName = string.Empty;

            if (ytVideo.SourceChannel.ToLower() == "guilty gear strive movies")
            {
                Regex ggstmPlayerRegex;
                /*
                 * (?<=)
                 *    Positive lookbehind. For player 2, we want to start at
                 *    the "vs" towards the start of the string. For player 1,
                 *    we want to start at the first closing bracket.
                 *    
                 * ?(?=)
                 *    Non-greedy lookahead. Player 1 will stop at the first "vs" and
                 *    player 2 will stop at the first opening parenthesis. Player 1 
                 *    needs a non-greedy lookahead because the regex will capture 
                 *    everything to the very last "vs" it encounters (i.e., the "vs"
                 *    that separates the characters that the players are using).
                 *    
                 *  This is using a blanket capture because regex doesn't play nicely
                 *  with Japanese characters. This might be a roadblock in the future.
                 */
                if (parsePlayer2)
                {
                    ggstmPlayerRegex = new Regex(@"(?<=vs).*(?=\()");
                }
                else
                {
                    ggstmPlayerRegex = new Regex(@"(?<=]).*?(?=vs)");
                }

                var ggstmPlayerRgxMatch = ggstmPlayerRegex.Match(ytVideo.Title);

                if (ggstmPlayerRgxMatch.Success)
                {
                    playerName = ggstmPlayerRgxMatch.Value.Trim();
                }
            }
            else if (ytVideo.SourceChannel.Contains("Kakuto"))
            {
                string playerAndCharacter;

                if (parsePlayer2)
                {
                    playerAndCharacter = FightingGameVillageTitleParser(ytVideo.Title, parsePlayer2);
                }
                else
                {
                    playerAndCharacter = FightingGameVillageTitleParser(ytVideo.Title);
                }

                var openingParenthesisIdx = playerAndCharacter.IndexOf('(');
                playerName = playerAndCharacter.Substring(0, openingParenthesisIdx);
            }
            else if (ytVideo.SourceChannel.ToLower() == "gamestorage ch" || ytVideo.SourceChannel.ToLower() == "huawen sol")
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
                    playerName = string.Empty;
                }
                else
                {
                    var openingParenthesisIdx = playerAndCharacter.IndexOf('(');
                    playerName = playerAndCharacter.Substring(0, openingParenthesisIdx);
                }
            }

            return playerName;
        }

        public string ParseCharacterFromVideoTitle(YouTubeVideo ytVideo, bool parsePlayer2Char = false)
        {
            var characterName = string.Empty;

            if (ytVideo.SourceChannel.ToLower() == "guilty gear strive movies")
            {
                Regex ggsmCharacterRegex = new(@"(?<=\s)\((.+) vs (.+)\)");
                var charNameMatch = ggsmCharacterRegex.Match(ytVideo.Title);

                if (charNameMatch.Success)
                {
                    string extractedContent;

                    if (parsePlayer2Char)
                    {                       
                        extractedContent = charNameMatch.Groups[2].Value;
                    }
                    else
                    {
                        extractedContent = charNameMatch.Groups[1].Value;
                    }

                    // Want to split the Japanese glyphs for the character name
                    // (usually katakana) from the English character name, then
                    // give the English name proper name casing instead of all caps.
                    // Example: メイ/MAY --> MAY --> May
                    var englishName = extractedContent.Split('/')[1];
                    var characterInNamecase = englishName[0] + englishName[1..].ToLower();

                    characterName = characterInNamecase;
                }
            }
            else if (ytVideo.SourceChannel.Contains("Kakuto"))
            {
                string playerAndCharacter;

                if (parsePlayer2Char)
                {
                    playerAndCharacter = FightingGameVillageTitleParser(ytVideo.Title, parsePlayer2Char);
                }
                else
                {
                    playerAndCharacter = FightingGameVillageTitleParser(ytVideo.Title);
                }

                if (ContainsUnicodeCharacters(playerAndCharacter))
                {
                    var backslashIdx = playerAndCharacter.IndexOf('/') + 1;
                    characterName = playerAndCharacter[backslashIdx..].Replace(")", string.Empty);
                }
                else
                {
                    // Add 1 to the index since we want to start the substring after the opening parenthesis
                    var openParenthesisIndex = playerAndCharacter.IndexOf('(') + 1;
                    characterName = playerAndCharacter[openParenthesisIndex..].Replace(")", string.Empty);
                }
            }
            else if (ytVideo.SourceChannel.ToLower() == "gamestorage ch" || ytVideo.SourceChannel.ToLower() == "huawen sol")
            {
                string playerAndCharacter;

                if (parsePlayer2Char)
                {
                    playerAndCharacter = LenticularBracketTitleParser(ytVideo.Title, parsePlayer2Char);
                }
                else
                {
                    playerAndCharacter = LenticularBracketTitleParser(ytVideo.Title);
                }

                // HuaWen Sol uploads videos that highlight one single player and their character,
                // so cover these instances by ensuring that the character name will be an empty string
                if (parsePlayer2Char && string.IsNullOrWhiteSpace(playerAndCharacter))
                {
                    characterName = string.Empty;
                }
                else
                {
                    // Add 1 to the index since we want to start the substring after the opening parenthesis
                    var openParenthesisIndex = playerAndCharacter.IndexOf('(') + 1;
                    characterName = playerAndCharacter[openParenthesisIndex..].Replace(")", string.Empty);
                }
            }

            return characterName.Trim();
        }

        /// <summary>
        /// Given a video title from Fighting Game Village's channel, get the character and the player
        /// from the title
        /// </summary>
        /// <param name="title">The title of the YouTube video</param>
        /// <param name="getPlayer2">If true, will get the information for player 2. Default is false (player 1)</param>
        /// <returns>The desired player and their character from the title as a string</returns>
        private string FightingGameVillageTitleParser(string title, bool getPlayer2 = false)
        {
            string data = string.Empty;
            Regex updatedFGVTitleRegex = new(@"^(.*)(?=vs)(.*)(?=\))");

            if (title.Contains(','))
            {
                int commaIdx = title.LastIndexOf(',');
                int pointer = commaIdx;

                if (getPlayer2)
                {
                    // We need to move forward from the comma
                    pointer = ParsePlayer2ContentFromTitle(title, pointer);

                    data = title.AsSpan(commaIdx + 1, pointer - commaIdx).ToString().Trim();
                }
                else
                {
                    // We need to move backwards from the comma
                    pointer = ParsePlayer1ContentFromTitle(title, pointer);

                    data = title.AsSpan(pointer, commaIdx - pointer).ToString().Trim();
                }
            }
            else
            {
                var fgvMatch = updatedFGVTitleRegex.Match(title);

                if (fgvMatch.Success)
                {
                    if (getPlayer2)
                    {
                        data = fgvMatch.Groups[2].Value;
                    }
                    else
                    {
                        data = fgvMatch.Groups[1].Value;
                    }
                }
            }

            return data;

            int ParsePlayer1ContentFromTitle(string title, int pointer)
            {
                while (true)
                {
                    --pointer;
                    if (pointer < 0 || title[pointer] == ' ')
                    {
                        break;
                    }
                }

                return pointer;
            }

            int ParsePlayer2ContentFromTitle(string title, int pointer)
            {
                while (true)
                {
                    ++pointer;
                    if (pointer > title.Length || title[pointer] == LeftBlackLenticularBracket)
                    {
                        pointer--;
                        break;
                    }
                }

                return pointer;
            }
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

            return string.Empty;
        }

        /// <summary>
        /// Determines if the provided string contains any non-ASCII characters.
        /// </summary>
        /// <remarks>This method is for handling any CJK unicode characters
        /// that are used alongside/in fighting game character or player names.
        /// This might be an issue if the player's name is in CJK chars and
        /// this method forces parsing to skip the player name.</remarks>
        /// <param name="input"></param>
        /// <returns>True if the provided string contains any unicode characters</returns>
        private bool ContainsUnicodeCharacters(string input)
        {
            const int asciiLimit = 255;

            return input.Any(c => c > asciiLimit);
        }
    }
}
