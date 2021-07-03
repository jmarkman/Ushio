using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Ushio.ApiServices;
using Ushio.Data;
using Ushio.Data.YouTube;
using Ushio.Infrastructure.Database.Repositories;

namespace Ushio.Core
{
    public class VodSearchEngine
    {
        private readonly YouTubeApiService _youTubeApiService;
        private readonly UshioConstants _ushioConstants;
        private readonly FightingGameVodRepository _fightingGameVodRepository;
        private readonly Random _rnd;

        public VodSearchEngine(FightingGameVodRepository repo, YouTubeApiService apiSvc, UshioConstants constants)
        {
            _fightingGameVodRepository = repo;
            _youTubeApiService = apiSvc;
            _ushioConstants = constants;
            _rnd = new Random();
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

        private async Task<YouTubeVideo> GetVodFromDatabase(FightingGameName gameName, VodSearchTerms searchTerms)
        {
            /*
             * if just char
             * else if just player
             * else if both
             * else return null
             */ 

            if (searchTerms.Character != null && searchTerms.Player == null)
            {
                var databaseResults = await _fightingGameVodRepository.FindAsync(vod => vod.CharacterP1 == searchTerms.Character || vod.CharacterP2 == searchTerms.Character);

            }
            else if (searchTerms.Character == null && searchTerms.Player != null)
            {

            }
            else if (searchTerms.Character != null && searchTerms.Player != null)
            {

            }
            else
            {
                return null;
            }
        }

        private async Task<YouTubeVideo> GetVodFromYouTubeApi(FightingGameName gameName, VodSearchTerms searchTerms)
        {

        }
    }
}
