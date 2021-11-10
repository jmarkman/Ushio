using Discord.Commands;
using System;
using System.Collections.Generic;
using Ushio.Core;
using System.Threading.Tasks;
using Discord;
using System.Text;
using System.Linq;
using Ushio.Data;

namespace Ushio.Commands
{
    [Name("RNG")]
    public class RandomGenerationCommands : ModuleBase<SocketCommandContext>
    {
        private readonly UshioConstants _constants;
        private readonly Random _rnd;

        public RandomGenerationCommands(UshioConstants constants)
        {
            _constants = constants.InitializeFortunes().InitializeEmotes();
            _rnd = new Random();
        }

        [Command("choose")]
        public async Task ChooseRandomItem(params string[] items)
        {
            await ReplyAsync($"{items.Random()}");
        }

        [Command("8ball")]
        public async Task GenerateFortuneForQuestion([Remainder]string question)
        {
            EmbedConstructor constructor = new();
            var fortune = _constants.Fortunes.ElementAt(_rnd.Next(0, _constants.Fortunes.Count));

            await ReplyAsync(embed: constructor.CreateFortuneEmbed(question, fortune));
        }

        /// <summary>
        /// Sends a "slot machine" message using all available roulette emotes
        /// </summary>
        [Command("slots", ignoreExtraArgs: true)]
        public async Task EmoticonRoulette()
        {
            var allEmotes = _constants.Emotes;
            await BuildAndSendEmoteString(allEmotes);
        }

        /// <summary>
        /// Sends a "slot machine" message using the available "Blini"/CatJazz roulette emotes
        /// </summary>
        [Command("blinislots")]
        public async Task BliniRoulette()
        {
            var bliniEmotes = _constants.Emotes.Where(e => e.Name.Contains("Blini")).ToList();
            await BuildAndSendEmoteString(bliniEmotes);
        }

        /// <summary>
        /// Sends a "slot machine" message using the available King Dedede roulette emotes
        /// </summary>
        [Command("dddslots")]
        public async Task KingDededeRoulette()
        {
            var dddEmotes = _constants.Emotes.Where(e => e.Name.Contains("DDD")).ToList();
            await BuildAndSendEmoteString(dddEmotes);
        }

        /// <summary>
        /// Generates a "slot machine" effect with three different emotes
        /// </summary>
        /// <param name="emotes">A list of <see cref="DiscordEmote"/> objects containing pertinent
        /// emote information</param>
        /// <returns>A string representing a "slot machine"</returns>
        private async Task BuildAndSendEmoteString(List<DiscordEmote> emotes)
        {
            StringBuilder sb = new();

            for (int i = 0; i < 2; i++)
            {
                int rndIndex = _rnd.Next(0, emotes.Count);
                var rouletteEmote = Emote.Parse(emotes[rndIndex].Identifier);
                sb.Append(rouletteEmote.ToString());
            }

            await ReplyAsync(sb.ToString());
        }
    }
}
