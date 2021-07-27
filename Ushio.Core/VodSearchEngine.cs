using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Ushio.ApiServices;
using Ushio.Data;
using Ushio.Data.DatabaseModels;
using Ushio.Data.YouTube;
using Ushio.Infrastructure.Database.Repositories;

namespace Ushio.Core
{
    /// <summary>
    /// This class handles input from the FightingGameVodCommands class,
    /// passes that input to the <see cref="YouTubeApiService"/> for searches,
    /// archives the results, and returns vods back to the vod command class
    /// so it can be displayed to end users
    /// </summary>
    public class VodSearchEngine
    {
        private readonly YouTubeApiService _youTubeApiService;
        private readonly FightingGameVodRepository _fightingGameVodRepository;
        private readonly Random _rnd;
        private readonly VodTitleParser _vodTitleParser;

        public VodSearchEngine(FightingGameVodRepository repo, YouTubeApiService apiSvc)
        {
            _fightingGameVodRepository = repo;
            _youTubeApiService = apiSvc;
            _rnd = new Random();
            _vodTitleParser = new VodTitleParser();
        }

        /// <summary>
        /// Retrieve a vod based on the provided game and search terms.
        /// </summary>
        /// <param name="game">The enum parsed and sent from the command module</param>
        /// <param name="searchTerms">Either the character, the player, or both terms to filter on</param>
        /// <param name="getNewClips">If true, will pull videos from the YouTube API and populate the database</param>
        /// <returns>A random vod from the filtered results</returns>
        public async Task<YouTubeVideo> GetVodFor(FightingGameName game, VodSearchTerms searchTerms, bool getNewClips = false)
        {
            if (getNewClips)
            {
                return await GetVodFromYouTubeApi(game, searchTerms);
            }
            else
            {
                return await GetVodFromDatabase(game, searchTerms);
            }
        }

        /// <summary>
        /// Retrieves a vod from the database based on the game and search terms provided. This will be the
        /// "default" method to avoid leaning on the YouTube API quota too hard.
        /// </summary>
        /// <param name="gameName">The name of the desired game</param>
        /// <param name="searchTerms">A POCO that will hold either a character name, player name, or both</param>
        /// <returns>A singular <see cref="YouTubeVideo"/> randomly chosen from the search results</returns>
        private async Task<YouTubeVideo> GetVodFromDatabase(FightingGameName gameName, VodSearchTerms searchTerms)
        {
            Expression<Func<FightingGameVod, bool>> filterAsFunc = (vod) => false;

            filterAsFunc = GenerateFilterExpression(searchTerms, filterAsFunc);

            var databaseResults = await _fightingGameVodRepository.FindAsync(filterAsFunc) as List<FightingGameVod>;

            var randomSelectedVod = databaseResults[_rnd.Next(databaseResults.Count)];

            return new YouTubeVideo { Title = randomSelectedVod.OriginalTitle, Id = randomSelectedVod.VideoId };

            // Builds the LINQ expression used to comb the database for the desired vod
            Expression<Func<FightingGameVod, bool>> GenerateFilterExpression(VodSearchTerms searchTerms, Expression<Func<FightingGameVod, bool>> filterAsFunc)
            {
                if (searchTerms.Character != null && searchTerms.Player == null)
                {
                    filterAsFunc = vod => (vod.CharacterP1 == searchTerms.Character || vod.CharacterP2 == searchTerms.Character) && (vod.GameName == gameName);
                }
                else if (searchTerms.Character == null && searchTerms.Player != null)
                {
                    filterAsFunc = vod => (vod.Player1 == searchTerms.Player || vod.Player2 == searchTerms.Player) && (vod.GameName == gameName);
                }
                else if (searchTerms.Character != null && searchTerms.Player != null)
                {
                    filterAsFunc = vod => ((vod.CharacterP1 == searchTerms.Character || vod.CharacterP2 == searchTerms.Character) && (vod.Player1 == searchTerms.Player || vod.Player2 == searchTerms.Player)) && (vod.GameName == gameName);
                }

                return filterAsFunc;
            }
        }

        /// <summary>
        /// If requested by the user, will hit the YouTube API to search for a vod and return it to the end user.
        /// This method will incur a performance hit as not only will it hit the API, but it will upload the results
        /// to the vod database.
        /// </summary>
        /// <param name="gameName">The name of the desired game</param>
        /// <param name="searchTerms">A POCO that will hold either a character name, player name, or both</param>
        /// <returns>A singular <see cref="YouTubeVideo"/> randomly chosen from the search results</returns>
        private async Task<YouTubeVideo> GetVodFromYouTubeApi(FightingGameName gameName, VodSearchTerms searchTerms)
        {
            var vods = await _youTubeApiService.GetSpecifiedVodsAsync(gameName, searchTerms);

            List<FightingGameVod> vodsToStoreInDatabase = new();

            foreach (var vod in vods)
            {
                var fgVod = CreateFightingGameVodObj(vod);
                vodsToStoreInDatabase.Add(fgVod);
            }

            await _fightingGameVodRepository.AddRangeAsync(vodsToStoreInDatabase);
            await _fightingGameVodRepository.SaveChangesAsync();

            return vods[_rnd.Next(vods.Count)];
        }

        /// <summary>
        /// Creates a <see cref="FightingGameVod"/> object from a <see cref="YouTubeVideo"/>
        /// for insertion into the database
        /// </summary>
        /// <param name="ytVideo">The fighting game vod details encased in a <see cref="YouTubeVideo"/> object</param>
        /// <returns>A <see cref="FightingGameVod"/> based on the contents of the <see cref="YouTubeVideo"/></returns>
        private FightingGameVod CreateFightingGameVodObj(YouTubeVideo ytVideo)
        {
            FightingGameVod fgVod = new()
            {
                OriginalTitle = ytVideo.Title,
                VideoId = ytVideo.Id,
                GameName = ytVideo.GameName,
                SourceChannel = ytVideo.SourceChannel,
                Player1 = _vodTitleParser.ParsePlayerFromVideoTitle(ytVideo),
                Player2 = _vodTitleParser.ParsePlayerFromVideoTitle(ytVideo, parsePlayer2: true),
                CharacterP1 = _vodTitleParser.ParseCharacterFromVideoTitle(ytVideo),
                CharacterP2 = _vodTitleParser.ParseCharacterFromVideoTitle(ytVideo, parsePlayer2Char: true),
                DateUploaded = ytVideo.DateUploaded,
                DateAddedToRepo = DateTimeOffset.Now
            };

            return fgVod;
        }
    }
}
