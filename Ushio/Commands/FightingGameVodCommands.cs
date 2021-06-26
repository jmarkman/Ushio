using Discord.Commands;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Ushio.Commands.NamedArgs;
using Ushio.Core;

namespace Ushio.Commands
{
    public class FightingGameVodCommands : ModuleBase<SocketCommandContext>
    {
        private readonly GameAbbreviations abbreviations;

        public FightingGameVodCommands(GameAbbreviations a)
        {
            abbreviations = a;
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
        }

        /// <summary>
        /// Gets a specific clip (i.e., clip####) from the 3rd STRIKE channel on YouTube
        /// </summary>
        /// <param name="clipNumber">The number part of the title</param>
        [Command("3s")]
        public async Task GetThirdStrikeClip(string clipNumber = "")
        {

        }

        private string GetFullGameName(string game)
        {
            var fullGameName = string.Empty;

            fullGameName = abbreviations.List.Where(g => g.Name == game).Select(game => game.Name).FirstOrDefault();

            if (string.IsNullOrWhiteSpace(fullGameName))
            {
                fullGameName = abbreviations.List.Where(g => g.Aliases.Contains(game)).Select(game => game.Name).FirstOrDefault();
            }

            return fullGameName;
        }
    }
}
