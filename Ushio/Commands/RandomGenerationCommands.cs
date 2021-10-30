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
        private readonly string DDDHonkSlot = "<a:DDDHonkSlot:903452126490017792>";
        private readonly string DDDHonkSlot2 = "<a:DDDHonkSlot2:903453676750921768>";
        private readonly string DDDHonkSlot3 = "<a:DDDHonkSlot3:903454356517560340>";

        public RandomGenerationCommands(UshioConstants constants)
        {
            _constants = constants.InitializeFortunes();
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

        [Command("slots")]
        public async Task KingDededeRoulette()
        {
            List<Emote> dddSlots = new()
            {
                Emote.Parse(DDDHonkSlot),
                Emote.Parse(DDDHonkSlot2),
                Emote.Parse(DDDHonkSlot3)
            };

            StringBuilder sb = new();
            sb.Append(dddSlots.ElementAt(_rnd.Next(0, dddSlots.Count)).ToString());
            sb.Append(dddSlots.ElementAt(_rnd.Next(0, dddSlots.Count)).ToString());
            sb.Append(dddSlots.ElementAt(_rnd.Next(0, dddSlots.Count)).ToString());

            await ReplyAsync(sb.ToString());
        }
    }
}
