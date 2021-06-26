using Discord.Commands;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Ushio.ApiServices;
using Ushio.Commands.NamedArgs;
using Ushio.Core;

namespace Ushio.Commands
{
    public class FightingGameVodCommands : ModuleBase<SocketCommandContext>
    {
        private readonly UshioConstants ushioConstants;
        private readonly YouTubeApiService youtubeApiSvc;

        public FightingGameVodCommands(YouTubeApiService ytApiSvc, UshioConstants constants)
        {
            youtubeApiSvc = ytApiSvc;
            ushioConstants = constants;
        }

        /// <summary>
        /// Gets a vod for a specific game, and if provided, a filter for getting either 
        /// vods for a certain player, character, or both
        /// </summary>
        /// <param name="game">The game to look up</param>
        /// <param name="filter">An object containing named parameters for the command</param>
        [Command("vod")]
        public async Task GetVod(string game, VodFilter filter = null)
        {
            var fullGameName = GetFullGameName(game);

            await ReplyAsync("Not Implemented");
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
            var thirdStrikeClip = await youtubeApiSvc.GetRandomThirdStrikeClip();

            if (thirdStrikeClip != null)
            {
                await ReplyAsync(thirdStrikeClip.GetVideoUrl());
            }
        }

        /// <summary>
        /// Retrieves the full name of the game to source vods for from an abbreviation
        /// or shorthand way for referring to the game (i.e., sf5 for Street Fighter 5)
        /// </summary>
        /// <param name="game">The abbreviated or shorthand name for the game</param>
        /// <returns>The full name of the game as a string</returns>
        private string GetFullGameName(string game)
        {
            var fullGameName = string.Empty;

            fullGameName = ushioConstants.GameAbbreviations.Where(g => g.Name == game)
                                                            .Select(game => game.Name)
                                                            .FirstOrDefault();

            if (string.IsNullOrWhiteSpace(fullGameName))
            {
                fullGameName = ushioConstants.GameAbbreviations.Where(g => g.Aliases.Contains(game))
                                                                .Select(game => game.Name)
                                                                .FirstOrDefault();
            }

            return fullGameName;
        }
    }
}
