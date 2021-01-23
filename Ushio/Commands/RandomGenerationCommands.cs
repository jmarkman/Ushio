using Discord.Commands;
using System;
using System.Collections.Generic;
using Ushio.Core;
using System.Threading.Tasks;

namespace Ushio.Commands
{
    [Name("RNG")]
    public class RandomGenerationCommands : ModuleBase<SocketCommandContext>
    {
        private readonly FortuneGenerator _fortuneGenerator;

        public RandomGenerationCommands(FortuneGenerator generator)
        {
            _fortuneGenerator = generator;
        }

        [Command("choose")]
        public async Task ChooseRandomItem(params string[] items)
        {
            await ReplyAsync($"{items.Random()}");
        }

        [Command("8ball")]
        public async Task GenerateFortuneForQuestion([Remainder]string question)
        {
            EmbedConstructor constructor = new EmbedConstructor();

            await ReplyAsync(embed: constructor.CreateFortuneEmbed(question, _fortuneGenerator.GetFortune()));
        }
    }
}
