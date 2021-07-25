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
        private const char RightBlackLenticularBracket = '】';
        private const char LeftBlackLenticularBracket = '【';
        private readonly YouTubeApiService _youTubeApiService;
        private readonly FightingGameVodRepository _fightingGameVodRepository;
        private readonly Random _rnd;

        public VodSearchEngine(FightingGameVodRepository repo, YouTubeApiService apiSvc)
        {
            _fightingGameVodRepository = repo;
            _youTubeApiService = apiSvc;
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
                Player1 = ParsePlayerFromVideoTitle(ytVideo),
                Player2 = ParsePlayerFromVideoTitle(ytVideo, parsePlayer2: true),
                CharacterP1 = ParseCharacterFromVideoTitle(ytVideo),
                CharacterP2 = ParseCharacterFromVideoTitle(ytVideo, parsePlayer2Char: true),
                DateUploaded = ytVideo.DateUploaded,
                DateAddedToRepo = DateTimeOffset.Now
            };

            return fgVod;
        }

        private string ParsePlayerFromVideoTitle(YouTubeVideo ytVideo, bool parsePlayer2 = false)
        {
            var playerName = string.Empty;
            
            if (ytVideo.SourceChannel.ToLower() == "guilty gear strive movies")
            {
                Regex ggstmPlayerRegex;
                /*
                 * (?<=)
                 *    Positive lookbehind. For player 2, we want to start at
                 *    the "vs" towards the start of the string. For player 1,
                 *    we want to start at the first closing bracket.
                 *    
                 * ?(?=)
                 *    Non-greedy lookahead. Player 1 will stop at the first "vs" and
                 *    player 2 will stop at the first opening parenthesis. Player 1 
                 *    needs a non-greedy lookahead because the regex will capture 
                 *    everything to the very last "vs" it encounters (i.e., the "vs"
                 *    that separates the characters that the players are using).
                 *    
                 *  This is using a blanket capture because regex doesn't play nicely
                 *  with Japanese characters. This might be a roadblock in the future.
                 */
                if (parsePlayer2)
                {
                    ggstmPlayerRegex = new Regex(@"(?<=vs).*(?=\()");
                }
                else
                {
                    ggstmPlayerRegex = new Regex(@"(?<=]).*?(?=vs)");
                }

                var ggstmPlayerRgxMatch = ggstmPlayerRegex.Match(ytVideo.Title);

                if (ggstmPlayerRgxMatch.Success)
                {
                    playerName = ggstmPlayerRgxMatch.Value.Trim();
                }
            }
            else if (ytVideo.SourceChannel.Contains("Kakuto"))
            {
                string playerAndCharacter;

                if (parsePlayer2)
                {
                    playerAndCharacter = FightingGameVillageTitleParser(ytVideo.Title, parsePlayer2);
                }
                else
                {
                    playerAndCharacter = FightingGameVillageTitleParser(ytVideo.Title);
                }

                var openingParenthesisIdx = playerAndCharacter.IndexOf('(');
                playerName = playerAndCharacter.Substring(0, openingParenthesisIdx);
            }
            else if (ytVideo.SourceChannel.ToLower() == "gamestorage ch")
            {
                string playerAndCharacter;

                if (parsePlayer2)
                {
                    playerAndCharacter = GamestorageChTitleParser(ytVideo.Title, parsePlayer2);
                }
                else
                {
                    playerAndCharacter = GamestorageChTitleParser(ytVideo.Title);
                }

                var openingParenthesisIdx = playerAndCharacter.IndexOf('(');
                playerName = playerAndCharacter.Substring(0, openingParenthesisIdx);
            }

            return playerName;
        }

        private string ParseCharacterFromVideoTitle(YouTubeVideo ytVideo, bool parsePlayer2Char = false)
        {
            var characterName = string.Empty;

            if (ytVideo.SourceChannel.ToLower() == "guilty gear strive movies")
            {
                Regex ggsmCharacterRegex = new(@"(?<=\s)\((.+) vs (.+)\)");
                var charNameMatch = ggsmCharacterRegex.Match(ytVideo.Title);

                if (charNameMatch.Success)
                {
                    if (parsePlayer2Char)
                    {
                        characterName = charNameMatch.Groups[2].Value;
                    }
                    else
                    {
                        characterName = charNameMatch.Groups[1].Value;
                    }
                }
            }
            else if (ytVideo.SourceChannel.Contains("Kakuto"))
            {
                string playerAndCharacter;

                if (parsePlayer2Char)
                {
                    playerAndCharacter = FightingGameVillageTitleParser(ytVideo.Title, parsePlayer2Char);
                }
                else
                {
                    playerAndCharacter = FightingGameVillageTitleParser(ytVideo.Title);
                }

                // Add 1 to the index since we want to start the substring after the opening parenthesis
                var openParenthesisIndex = playerAndCharacter.IndexOf('(') + 1;
                characterName = playerAndCharacter[openParenthesisIndex..].Replace(")", string.Empty);
            }
            else if (ytVideo.SourceChannel.ToLower() == "gamestorage ch")
            {
                string playerAndCharacter;

                if (parsePlayer2Char)
                {
                    playerAndCharacter = GamestorageChTitleParser(ytVideo.Title, parsePlayer2Char);
                }
                else
                {
                    playerAndCharacter = GamestorageChTitleParser(ytVideo.Title);
                }

                // Add 1 to the index since we want to start the substring after the opening parenthesis
                var openParenthesisIndex = playerAndCharacter.IndexOf('(') + 1;
                characterName = playerAndCharacter[openParenthesisIndex..].Replace(")", string.Empty);
            }

            return characterName;
        }

        /// <summary>
        /// Given a video title from Fighting Game Village's channel, get the character and the player
        /// from the title
        /// </summary>
        /// <param name="title">The title of the YouTube video</param>
        /// <param name="getPlayer2">If true, will get the information for player 2. Default is false (player 1)</param>
        /// <returns>The desired player and their character from the title as a string</returns>
        private string FightingGameVillageTitleParser(string title, bool getPlayer2 = false)
        {
            int commaIdx = title.LastIndexOf(',');
            int pointer = commaIdx;
            string data;

            if (getPlayer2)
            {
                // We need to move forward from the comma
                pointer = ParsePlayer2ContentFromTitle(title, pointer);

                data = title.AsSpan(commaIdx + 1, pointer - commaIdx).ToString().Trim();
            }
            else
            {
                // We need to move backwards from the comma
                pointer = ParsePlayer1ContentFromTitle(title, pointer);

                data = title.AsSpan(pointer, commaIdx - pointer).ToString().Trim();
            }

            return data;

            int ParsePlayer1ContentFromTitle(string title, int pointer)
            {
                while (true)
                {
                    --pointer;
                    if (pointer < 0 || title[pointer] == ' ')
                    {
                        break;
                    }
                }

                return pointer;
            }

            int ParsePlayer2ContentFromTitle(string title, int pointer)
            {
                while (true)
                {
                    ++pointer;
                    if (pointer > title.Length || title[pointer] == LeftBlackLenticularBracket)
                    {
                        pointer--;
                        break;
                    }
                }

                return pointer;
            }
        }

        /// <summary>
        /// Given a video title from Gamestorage Ch, get the character and player from the title
        /// </summary>
        /// <param name="title">The title of the YouTube video</param>
        /// <param name="getPlayer2">If true, will get the information for player 2. Default is false (player 1)</param>
        /// <returns>The desired player and their character from the title as a string</returns>
        private string GamestorageChTitleParser(string title, bool getPlayer2 = false)
        {
            Regex gamestorageTitleRegex = new($@"(?<={RightBlackLenticularBracket})(.*)vs(.*\))");
            string player1;
            string player2;
            var playerTitleSegment = gamestorageTitleRegex.Match(title);

            if (playerTitleSegment.Success)
            {
                player1 = playerTitleSegment.Groups[1].Value.Trim();
                player2 = playerTitleSegment.Groups[2].Value.Trim();

                if (getPlayer2)
                {
                    return player2;
                }
                else
                {
                    return player1;
                }
            }

            return string.Empty;
        }
    }
}
