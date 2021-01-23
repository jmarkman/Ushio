using Discord;
using System;
using System.Collections.Generic;
using System.Text;

namespace Ushio
{
    public class EmbedConstructor
    {
        public EmbedConstructor() { }

        /// <summary>
        /// Generates an embed for the 8-ball fortune
        /// </summary>
        /// <param name="question"></param>
        /// <param name="response"></param>
        /// <returns>The 8-ball embed as a <see cref="Embed"/> object</returns>
        public Embed CreateFortuneEmbed(string question, string response)
        {
            var fortuneEmbed = new EmbedBuilder()
                .WithTitle("8ball")
                .AddField("Question", $"{question}")
                .AddField("Response?", $"{response}")
                .Build();

            return fortuneEmbed;
        }
    }
}
