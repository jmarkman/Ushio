using System;

namespace Ushio.Data.YouTube
{
    public class YouTubeVideo
    {
        public string Title { get; set; }
        public string Id { get; set; }
        public string SourceChannel { get; set; }
        public FightingGameName GameName { get; set; }
        public DateTimeOffset DateUploaded { get; set; }

        public YouTubeVideo() { }

        public string GetVideoUrl()
        {
            return $"https://www.youtube.com/watch?v={Id}";
        }
    }
}
