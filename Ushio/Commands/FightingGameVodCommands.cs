using Discord.Commands;
using System;
using System.Linq;
using System.Threading.Tasks;
using Ushio.ApiServices;
using Ushio.Core;
using Ushio.Data;
using Ushio.Data.NamedArgs;
using Ushio.Data.YouTube;
using Ushio.Infrastructure.Database.Repositories;

namespace Ushio.Commands
{
    public class FightingGameVodCommands : ModuleBase<SocketCommandContext>
    {
        private readonly UshioConstants ushioConstants;
        private readonly VodSearchEngine vodSearchEngine;
        private readonly string CouldNotFindVod = "A vod could not be found with the provided search terms";

        public FightingGameVodCommands(FightingGameVodRepository fgVodRepo, YouTubeApiService ytApiSvc, UshioConstants constants)
        {
            vodSearchEngine = new VodSearchEngine(fgVodRepo, ytApiSvc, constants);
            ushioConstants = constants;
        }

        /// <summary>
        /// Gets a vod for a specific game, and if provided, a filter for getting either 
        /// vods for a certain player, character, or both
        /// </summary>
        /// <param name="game">The game to look up</param>
        /// <param name="filter">An object containing named parameters for the command</param>
        [Command("vod")]
        public async Task GetVod(string game, VodFilter filter)
        {
            var searchTerms = new VodSearchTerms { Character = filter.Character, Player = filter.Player };
            YouTubeVideo vod = null;

            try
            {
                FightingGameName gameName = GetFullGameName(game);

                vod = await vodSearchEngine.GetVodFor(gameName, searchTerms, filter.GetNewClips);
            }
            catch (InvalidOperationException invalidOpEx)
            {
                await ReplyAsync(invalidOpEx.Message);
            }

            await ReplyAsync((vod != null) ? vod.GetVideoUrl() : CouldNotFindVod);
        }

        /// <summary>
        /// Gets either a specific clip (i.e., clip####) or a random clip from the 
        /// 3rd STRIKE channel on YouTube. If no number is provided, retrieves a
        /// random clip.
        /// </summary>
        /// <param name="clipNumber">The number part of the title</param>
        [Command("3s")]
        public async Task GetThirdStrikeClip(string clipNumber = "")
        {
            await ReplyAsync("Not yet implemented");
        }


        /// <summary>
        /// Retrieves the enum associated with the provided full game name, shorthand,
        /// or abbreviation of the game name
        /// </summary>
        /// <param name="game">The abbreviated or shorthand name for the game</param>
        /// <returns>The full name of the game as a string</returns>
        private FightingGameName GetFullGameName(string game)
        {
            var gameNameEnumString = string.Empty;

            gameNameEnumString = ushioConstants.GameAbbreviations.Where(g => g.Name == game)
                                                            .Select(game => game.GetEnumFromName())
                                                            .FirstOrDefault();

            if (string.IsNullOrWhiteSpace(gameNameEnumString))
            {
                gameNameEnumString = ushioConstants.GameAbbreviations.Where(g => g.Aliases.Contains(game))
                                                                .Select(game => game.GetEnumFromName())
                                                                .FirstOrDefault();
            }

            return ParseGameNameEnumFromInput(gameNameEnumString);

            FightingGameName ParseGameNameEnumFromInput(string game)
            {
                try
                {
                    return Enum.Parse<FightingGameName>(game);
                }
                catch (ArgumentException)
                {
                    throw new InvalidOperationException($"Failed to find a suitable enum for the game '{game}'.");
                }
            }
        }
    }
}
