using System.Text.RegularExpressions;
using Ushio.Data.YouTube;

namespace Ushio.Core.ParsingRules
{
    public class GuiltyGearStriveMoviesTitleParseRule : ITitleParseRule
    {
        public string ParseCharacter(YouTubeVideo ytVideo, bool parsePlayer2)
        {
            if (ytVideo.SourceChannel.ToLower() == "guilty gear strive movies")
            {
                Regex ggsmCharacterRegex = new(@"(?<=\s)\((.+) vs (.+)\)");
                var charNameMatch = ggsmCharacterRegex.Match(ytVideo.Title);

                if (charNameMatch.Success)
                {
                    string extractedContent;

                    if (parsePlayer2)
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

                    return characterInNamecase;
                }
                else
                {
                    /*
                     * The above if statement should cover almost all of the vods on the
                     * Guilty Gear Strive MOVIES channel. If we hit this else statement,
                     * it means that we're dealing with an older vod that followed the 
                     * [GGST] [player](character in cjk/eng) vs [player](character in cjk/eng)
                     * pattern.
                     * 
                     * While it's possible to write a regex that will match both characters
                     * in one pass, doing so loses all context of which player is what
                     * character. Both regexes have different lookbehind assertions, but
                     * they both follow the same pattern: after the space and player name
                     * (\s, [a-zA-Z0-9]*), move past the opening parenthesis and whatever
                     * CJK characters that might be encountered until the forward slash is
                     * reached (\(, .*, \/).
                     */
                    Match oldStyleCharNameMatch;

                    if (parsePlayer2)
                    {
                        ggsmCharacterRegex = new Regex(@"(?<=vs\s[a-zA-Z0-9]*\(.*\/).*?(?=\))");
                        oldStyleCharNameMatch = ggsmCharacterRegex.Match(ytVideo.Title);
                    }
                    else
                    {
                        ggsmCharacterRegex = new Regex(@"(?<=]\s[a-zA-Z0-9]*\(.*\/).*?(?=\)\svs)");
                        oldStyleCharNameMatch = ggsmCharacterRegex.Match(ytVideo.Title);
                    }

                    if (oldStyleCharNameMatch.Success)
                    {
                        // Return the character's name in namecase, not all caps
                        var namecase = oldStyleCharNameMatch.Value[0] + oldStyleCharNameMatch.Value[1..].ToLower();
                        return namecase.Trim();
                    }
                }
            }

            return null;
        }


        public string ParsePlayer(YouTubeVideo ytVideo, bool parsePlayer2)
        {
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
                    ggstmPlayerRegex = new Regex(@"(?<=vs\s).*(?=\()");
                }
                else
                {
                    // For the GGSM channel, older videos weren't prefixed with [GGST], but
                    // the owner of the channel changed to using the [GGST] pattern to denote
                    // what videos were for Strive. This would cause the regex for this channel
                    // to fail as it was originally written to support the [GGST] prefix format.
                    if (!ytVideo.Title.StartsWith("["))
                    {
                        ggstmPlayerRegex = new Regex(@"^.*?(?=vs)");
                    }
                    else
                    {
                        // The lookahead contains an "or" (the pipe character), allowing us to
                        // parse Bleed (ぶりーど, Buriido) out of "ぶりーど vs Kazunoko" and
                        // SonicFox out of "SonicFox(レオ/LEO) vs Verix777(名残雪/NAGORIYUKI)"
                        ggstmPlayerRegex = new Regex(@"(?<=]\s).*?(?=\(|\svs)");
                    }
                }

                var ggstmPlayerRgxMatch = ggstmPlayerRegex.Match(ytVideo.Title);

                if (ggstmPlayerRgxMatch.Success)
                {
                    return ggstmPlayerRgxMatch.Value.Trim();
                }
            }
            return null;
        }
    }
}
