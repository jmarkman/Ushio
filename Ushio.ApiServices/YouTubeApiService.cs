using Google.Apis.Services;
using Google.Apis.YouTube.v3;
using Google.Apis.YouTube.v3.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Ushio.Data;
using Ushio.Data.YouTube;

namespace Ushio.ApiServices
{
    /// <summary>
    /// This API service allows for interacting with the YouTube API.
    /// Currently focuses on combing fighting game channels for vods.
    /// </summary>
    public class YouTubeApiService
    {
        private readonly string _apiKey;
        private readonly YouTubeService _ytService;

        public YouTubeApiService(string key, UshioConstants ushioConstants)
        {
            _apiKey = key;
            _ytService = CreateYouTubeApiService();
        }

        /// <summary>
        /// For usage later in development. Gets all of the playlists for a given channel, used for
        /// vod channels that organize vods by primary character.
        /// </summary>
        /// <param name="channelId">The unique Id for the vod channel</param>
        /// <returns>A list of <see cref="YouTubePlaylist"/> objects representing the playlists made for the channel</returns>
        public async Task<List<YouTubePlaylist>> GetPlaylistsForChannelAsync(string channelId)
        {
            var playlistRequest = _ytService.Playlists.List("snippet");
            playlistRequest.ChannelId = channelId;
            playlistRequest.MaxResults = 50;

            var playlistResponse = await playlistRequest.ExecuteAsync();

            return playlistResponse.Items.Select(x => new YouTubePlaylist { Title = x.Snippet.Title, Id = x.Id }).ToList();
        }

        /// <summary>
        /// Populates the provided list with video clips from a given channel or playlist. 
        /// </summary>
        /// <remarks>In YouTube API lexicon, a user's regular unorganized uploads are a master
        /// playlist for that user's channel, so YouTube recognizes everything as a playlist.
        /// This is why the parameter name for the playlist ID can be either a channel ID
        /// or a playlist ID.</remarks>
        /// <param name="playlistId">The playlist to retrieve videos from</param>
        /// <param name="titleRegex">Regex filter</param>
        public async Task<List<PlaylistItem>> GetVideosFromPlaylistAsync(Regex titleRegex, string playlistId)
        {
            List<PlaylistItem> playlistItems = new();
            var nextPageToken = string.Empty;

            while (nextPageToken != null)
            {
                var videoRequest = _ytService.PlaylistItems.List("snippet,contentDetails");
                videoRequest.PlaylistId = playlistId;
                videoRequest.PageToken = nextPageToken;
                videoRequest.MaxResults = 50;

                var videoResponse = await videoRequest.ExecuteAsync();

                var currVideos = videoResponse.Items;

                var clips = currVideos.Where(x => titleRegex.IsMatch(x.Snippet.Title)).ToList();

                playlistItems.AddRange(clips);

                nextPageToken = videoResponse.NextPageToken;
            }

            return playlistItems;
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
