using Google.Apis.Services;
using Google.Apis.YouTube.v3;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Ushio.Core;
using Ushio.Data.YouTube;

namespace Ushio.ApiServices
{
    public class YouTubeApiService
    {
        private readonly string _apiKey;
        private readonly YouTubeService _ytService;
        private readonly UshioConstants _ushioConstants;

        public YouTubeApiService(string key, UshioConstants ushioConstants)
        {
            _apiKey = key;
            _ytService = CreateYouTubeApiService();
            _ushioConstants = ushioConstants;
        }

        public async Task<YouTubeVideo> GetRandomThirdStrikeClip()
        {
            List<YouTubeVideo> thirdStrikeClips = new List<YouTubeVideo>();
            var clipRegex = new Regex(@"clip[0-9]{1,4}", RegexOptions.IgnoreCase);
            var thirdStrikeChannelId = _ushioConstants.VodChannels.Where(x => x.Name.ToLower() == "3rd strike").Select(y => y.Id).FirstOrDefault();
            var nextPageToken = string.Empty;

            while (nextPageToken != null)
            {
                var videoRequest = _ytService.PlaylistItems.List("snippet");
                videoRequest.PlaylistId = thirdStrikeChannelId;
                videoRequest.PageToken = nextPageToken;
                videoRequest.MaxResults = 50;

                var videoResponse = await videoRequest.ExecuteAsync();

                var currVideos = videoResponse.Items;
                var clips = currVideos.Where(x => clipRegex.IsMatch(x.Snippet.Title))
                                         .Select(y => new YouTubeVideo
                                         {
                                             Title = y.Snippet.Title,
                                             Id = y.Snippet.ResourceId.VideoId
                                         })
                                         .ToList();

                thirdStrikeClips.AddRange(clips);

                nextPageToken = videoResponse.NextPageToken;
            }


            var random = new Random();

            return thirdStrikeClips[random.Next(thirdStrikeClips.Count)];
        }

        private YouTubeService CreateYouTubeApiService()
        {
            var ytSvc = new YouTubeService(new BaseClientService.Initializer()
            {
                ApiKey = _apiKey,
                ApplicationName = "UshioDiscordBot"
            });

            return ytSvc;
        }
    }
}
