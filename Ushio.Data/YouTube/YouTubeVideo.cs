using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ushio.Data.YouTube
{
    public class YouTubeVideo
    {
        public string Title { get; set; }
        public string Id { get; set; }

        public YouTubeVideo() { }

        public string GetVideoUrl()
        {
            return $"https://www.youtube.com/watch?v={Id}";
        }
    }
}
