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
    /// <summary>
    /// This API service allows for interacting with the YouTube API.
    /// Currently focuses on combing fighting game channels for vods.
    /// </summary>
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
        /// Retrieves a list of vods from a randomly chosen vod channel based on the provided game and
        /// search terms
        /// </summary>
        /// <param name="gameName">An enum representing the desired game</param>
        /// <param name="searchTerms">A POCO that will hold either a character name, player name, or both</param>
        /// <returns>A list of <see cref="YouTubeVideo"/> objects representing vods, filtered by the <see cref="VodSearchTerms"/> object</returns>
        public async Task<List<YouTubeVideo>> GetSpecifiedVodsAsync(FightingGameName gameName, VodSearchTerms searchTerms)
        {
            List<YouTubeVideo> vods = new();
            var vodChannelsForSpecifiedGame = _ushioConstants.VodChannels.Where(ch => Enum.Parse<FightingGameName>(ch.GetEnumFromGame()) == gameName).ToList();
            var channel = vodChannelsForSpecifiedGame[_rnd.Next(vodChannelsForSpecifiedGame.Count)];

            Regex vodRegex = GenerateSearchRegexForChannel(channel.Name, searchTerms);

            await PopulateVideoCollectionAsync(vods, vodRegex, gameName, channel.Id, channel.Name);

            return vods;
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
            
            //await PopulateVideoCollectionAsync(thirdStrikeClips, thirdStrikeChannelId, clipRegex);

            return thirdStrikeClips[_rnd.Next(thirdStrikeClips.Count)];
        }

        /// <summary>
        /// For usage later in development. Gets all of the playlists for a given channel, used for
        /// vod channels that organize vods by primary character.
        /// </summary>
        /// <param name="channelId">The unique Id for the vod channel</param>
        /// <returns>A list of <see cref="YouTubePlaylist"/> objects representing the playlists made for the channel</returns>
        private async Task<List<YouTubePlaylist>> GetPlaylistsForChannelAsync(string channelId)
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
        /// <param name="vodCollection">The <see cref="List{YouTubeVideo}"/> holding the videos</param>
        /// <param name="playlistId">The playlist to retrieve videos from</param>
        /// <param name="channelName">The channel name associated with the video</param>
        /// <param name="vodRegex">Regex filter</param>
        private async Task PopulateVideoCollectionAsync(List<YouTubeVideo> vodCollection, Regex vodRegex, FightingGameName gameName, string playlistId, string channelName)
        {
            var nextPageToken = string.Empty;

            while (nextPageToken != null)
            {
                var videoRequest = _ytService.PlaylistItems.List("snippet,contentDetails");
                videoRequest.PlaylistId = playlistId;
                videoRequest.PageToken = nextPageToken;
                videoRequest.MaxResults = 50;

                var videoResponse = await videoRequest.ExecuteAsync();

                var currVideos = videoResponse.Items;

                var clips = currVideos.Where(x => vodRegex.IsMatch(x.Snippet.Title))
                                         .Select(y => new YouTubeVideo
                                         {
                                             Title = y.Snippet.Title,
                                             Id = y.Snippet.ResourceId.VideoId,
                                             SourceChannel = channelName,
                                             GameName = gameName,
                                             DateUploaded = new DateTimeOffset(y.ContentDetails.VideoPublishedAt.Value)
                                         })
                                         .ToList();

                vodCollection.AddRange(clips);

                nextPageToken = videoResponse.NextPageToken;
            }
        }

        /// <summary>
        /// Based on the channel name and search terms, constructs a regex to use when filtering
        /// vods by title.
        /// </summary>
        /// <param name="channelName">The name of the channel that will be combed for vods</param>
        /// <param name="searchTerms">The vod filtering POCO</param>
        /// <returns>A regex that works for the current channel's naming scheme for vod titles</returns>
        private Regex GenerateSearchRegexForChannel(string channelName, VodSearchTerms searchTerms)
        {
            Regex rgx = null;

            if (channelName.Contains("Kakuto") || channelName.ToLower() == "gamestorage ch")
            {
                if (searchTerms.Character != null && searchTerms.Player == null)
                {
                    rgx = new Regex($@"\({searchTerms.Character}\)", RegexOptions.IgnoreCase);
                }
                else if (searchTerms.Character == null && searchTerms.Player != null)
                {
                    rgx = new Regex($@"{searchTerms.Player}", RegexOptions.IgnoreCase);
                }
                else
                {
                    rgx = new Regex($@"{searchTerms.Player}\({searchTerms.Character}\)", RegexOptions.IgnoreCase);
                }
            }
            else if (channelName.ToLower() == "guilty gear strive movies")
            {
                if (searchTerms.Character != null && searchTerms.Player == null)
                {
                    rgx = new Regex($@"\/{searchTerms.Character}", RegexOptions.IgnoreCase);
                }
                else if (searchTerms.Character == null && searchTerms.Player != null)
                {
                    rgx = new Regex($@"{searchTerms.Player}", RegexOptions.IgnoreCase);
                }
                else
                {
                    rgx = new Regex($@"{searchTerms.Player}.*?\b(?<=\/){searchTerms.Character}\b", RegexOptions.IgnoreCase);
                }
            }

            return rgx;
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
