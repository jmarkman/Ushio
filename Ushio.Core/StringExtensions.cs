using System.Linq;

namespace Ushio.Core
{
    public static class StringExtensions
    {
        /// <summary>
        /// Determines if the provided string contains any non-ASCII characters.
        /// </summary>
        /// <remarks>This method is for handling any CJK unicode characters
        /// that are used alongside/in fighting game character or player names.
        /// This might be an issue if the player's name is in CJK chars and
        /// this method forces parsing to skip the player name.</remarks>
        /// <param name="input"></param>
        /// <returns>True if the provided string contains any unicode characters</returns>
        public static bool ContainsUnicodeCharacters(this string input)
        {
            const int asciiLimit = 255;

            return input.Any(c => c > asciiLimit);
        }
    }
}
