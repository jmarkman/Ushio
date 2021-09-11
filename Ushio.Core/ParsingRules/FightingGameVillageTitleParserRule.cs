using System;
using System.Text.RegularExpressions;
using Ushio.Data.YouTube;

namespace Ushio.Core.ParsingRules
{
    public class FightingGameVillageTitleParserRule : ITitleParseRule
    {
        private const char LeftBlackLenticularBracket = '【';

        public string ParseCharacter(YouTubeVideo ytVideo, bool parsePlayer2)
        {
            if (ytVideo.SourceChannel.Contains("Kakuto"))
            {
                string playerAndCharacter;
                string character = null;

                if (parsePlayer2)
                {
                    playerAndCharacter = FightingGameVillageTitleParser(ytVideo.Title, parsePlayer2);
                }
                else
                {
                    playerAndCharacter = FightingGameVillageTitleParser(ytVideo.Title);
                }

                if (playerAndCharacter.ContainsUnicodeCharacters())
                {
                    var backslashIdx = playerAndCharacter.IndexOf('/') + 1;
                    character = playerAndCharacter[backslashIdx..].Replace(")", string.Empty);
                }
                else
                {
                    // Add 1 to the index since we want to start the substring after the opening parenthesis
                    var openParenthesisIndex = playerAndCharacter.IndexOf('(') + 1;
                    character = playerAndCharacter[openParenthesisIndex..].Replace(")", string.Empty);
                }

                return character.Trim();
            }

            return null;
        }

        public string ParsePlayer(YouTubeVideo ytVideo, bool parsePlayer2)
        {
            if (ytVideo.SourceChannel.Contains("Kakuto"))
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
                var parsedPlayer = playerAndCharacter.Substring(0, openingParenthesisIdx);

                // TODO: Implement a result class system (see ParseResults class, FluentResults)
                var player = !string.IsNullOrWhiteSpace(parsedPlayer) ? parsedPlayer : null;

                return player;
            }

            return null;
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
        }
        private static int ParsePlayer1ContentFromTitle(string title, int pointer)
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

        private static int ParsePlayer2ContentFromTitle(string title, int pointer)
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
}
