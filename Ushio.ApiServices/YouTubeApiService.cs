using Google.Apis.Services;
using Google.Apis.YouTube.v3;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Ushio.Core;
using Ushio.Data.NamedArgs;
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

        public async Task<YouTubeVideo> GetGuiltyGearStriveVod(VodFilter searchTerms)
        {
            List<YouTubeVideo> striveVods = new();
            var random = new Random();
            Regex vodRegex;

            if (searchTerms.Character != null && searchTerms.Player ==  null)
            {
                vodRegex = new Regex($@"\({searchTerms.Character}\)", RegexOptions.IgnoreCase);
            }
            else if (searchTerms.Character == null && searchTerms.Player != null)
            {
                vodRegex = new Regex($@"{searchTerms.Player}", RegexOptions.IgnoreCase);
            }
            else
            {
                vodRegex = new Regex($@"{searchTerms.Player}\({searchTerms.Character}\)", RegexOptions.IgnoreCase);
            }

            var striveChannelId = _ushioConstants.VodChannels.Where(x => x.Name.ToLower().Contains("village")).Select(y => y.Id).FirstOrDefault();

            await PopulateVideoCollection(striveVods, striveChannelId, vodRegex);

            return striveVods[random.Next(striveVods.Count)];
        }

        public async Task<YouTubeVideo> GetRandomThirdStrikeClip()
        {
            List<YouTubeVideo> thirdStrikeClips = new();
            var random = new Random();
            var clipRegex = new Regex(@"clip[0-9]{1,4}", RegexOptions.IgnoreCase);
            var thirdStrikeChannelId = _ushioConstants.VodChannels.Where(x => x.Name.ToLower() == "3rd strike").Select(y => y.Id).FirstOrDefault();
            
            await PopulateVideoCollection(thirdStrikeClips, thirdStrikeChannelId, clipRegex);

            return thirdStrikeClips[random.Next(thirdStrikeClips.Count)];
        }

        /// <summary>
        /// Populates the provided list with video clips from a given channel or playlist. 
        /// </summary>
        /// <remarks>In YouTube API lexicon, a user's regular unorganized uploads are a master
        /// playlist for that user's channel, so YouTube recognizes everything as a playlist.
        /// This is why the parameter name for the playlist ID can be either a channel ID
        /// or a playlist ID.</remarks>
        /// <param name="clipCollection">The <see cref="List{YouTubeVideo}"/> holding the videos</param>
        /// <param name="playlistId">The playlist to retrieve videos from</param>
        /// <param name="clipRegex">Regex filter</param>
        private async Task PopulateVideoCollection(List<YouTubeVideo> clipCollection, string playlistId, Regex clipRegex)
        {
            var nextPageToken = string.Empty;

            while (nextPageToken != null)
            {
                var videoRequest = _ytService.PlaylistItems.List("snippet");
                videoRequest.PlaylistId = playlistId;
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

                clipCollection.AddRange(clips);

                nextPageToken = videoResponse.NextPageToken;
            }
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
