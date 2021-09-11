using Ushio.Data.YouTube;

namespace Ushio.Core
{
    public interface ITitleParseRule
    {
        string ParseCharacter(YouTubeVideo ytVideo, bool parsePlayer2);

        string ParsePlayer(YouTubeVideo ytVideo, bool parsePlayer2);
    }
}
