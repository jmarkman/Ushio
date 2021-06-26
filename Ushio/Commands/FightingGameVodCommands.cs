using Discord.Commands;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Ushio.Commands.NamedArgs;

namespace Ushio.Commands
{
    public class FightingGameVodCommands : ModuleBase<SocketCommandContext>
    {
        public FightingGameVodCommands()
        {

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

        }

        /// <summary>
        /// Gets a specific clip (i.e., clip####) from the 3rd STRIKE channel on YouTube
        /// </summary>
        /// <param name="clipNumber">The number part of the title</param>
        [Command("3s")]
        public async Task GetThirdStrikeClip(string clipNumber = "")
        {

        }
    }
}
