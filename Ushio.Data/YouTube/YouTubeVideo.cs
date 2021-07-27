using System;

namespace Ushio.Data.YouTube
{
    /// <summary>
    /// Represents the data of a YouTube video that we're interested in
    /// using for organizing fighting game YouTube vods
    /// </summary>
    public class YouTubeVideo
    {
        /// <summary>
        /// The title of the video as a user would see it on YouTube
        /// </summary>
        public string Title { get; set; }

        /// <summary>
        /// The id snippet of the video URL
        /// </summary>
        public string Id { get; set; }

        /// <summary>
        /// The channel where this video originated from
        /// </summary>
        public string SourceChannel { get; set; }

        /// <summary>
        /// The game this video is about
        /// </summary>
        public FightingGameName GameName { get; set; }

        /// <summary>
        /// The date this video was uploaded to YouTube
        /// </summary>
        public DateTimeOffset DateUploaded { get; set; }

        public YouTubeVideo() { }

        /// <summary>
        /// Builds a URL for the video using the <see cref="Id"/> property
        /// </summary>
        /// <returns>A string representation of the video's URL</returns>
        public string GetVideoUrl()
        {
            return $"https://www.youtube.com/watch?v={Id}";
        }
    }
}
