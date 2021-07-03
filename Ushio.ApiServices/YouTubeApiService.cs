using Google.Apis.Services;
using Google.Apis.YouTube.v3;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Ushio.Data;
using Ushio.Data.YouTube;

namespace Ushio.ApiServices
{
    public class YouTubeApiService
    {
        private readonly string _apiKey;
        private readonly YouTubeService _ytService;
        private readonly UshioConstants _ushioConstants;
        private readonly Random _rnd;

        public YouTubeApiService(string key, UshioConstants ushioConstants)
        {
            _apiKey = key;
            _ytService = CreateYouTubeApiService();
            _ushioConstants = ushioConstants;
            _rnd = new Random();
        }

        /// <summary>
        /// Given an object containing search terms, retrieves a Guilty Gear Strive 
        /// vod from a randomly selected Strive channel
        /// </summary>
        /// <param name="searchTerms">A DTO containing search terms</param>
        /// <returns>The <see cref="YouTubeVideo"/> object for the (psuedo)randomly chosen vod</returns>
        public async Task<YouTubeVideo> GetGuiltyGearStriveVod(VodSearchTerms searchTerms)
        {
            List<YouTubeVideo> striveVods = new();
            Regex vodRegex;

            var striveChannels = _ushioConstants.VodChannels.Where(x => x.Game.ToLower() == "guilty gear strive").Select(y => y.Id).ToList();
            var channel = striveChannels[_rnd.Next(striveChannels.Count)];

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

            await PopulateVideoCollection(striveVods, channel, vodRegex);

            return striveVods[_rnd.Next(striveVods.Count)];
        }

        /// <summary>
        /// Retrieves a random Third Strike clip from the 3rd STRIKE channel on YouTube. These clips are in the
        /// format "clip####", sometimes with a space and additional text after the number.
        /// </summary>
        /// <returns>The <see cref="YouTubeVideo"/> object for the (psuedo)randomly chosen clip</returns>
        public async Task<YouTubeVideo> GetRandomThirdStrikeClip()
        {
            List<YouTubeVideo> thirdStrikeClips = new();
            var clipRegex = new Regex(@"clip[0-9]{1,4}", RegexOptions.IgnoreCase);
            var thirdStrikeChannelId = _ushioConstants.VodChannels.Where(x => x.Name.ToLower() == "3rd strike").Select(y => y.Id).FirstOrDefault();
            
            await PopulateVideoCollection(thirdStrikeClips, thirdStrikeChannelId, clipRegex);

            return thirdStrikeClips[_rnd.Next(thirdStrikeClips.Count)];
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

        /// <summary>
        /// Constructs the YouTube API service for querying playlists and channels
        /// </summary>
        /// <returns><see cref="YouTubeService"/></returns>
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
