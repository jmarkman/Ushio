using Discord;
using Discord.Commands;
using System.Threading.Tasks;
using Ushio.ApiServices;

namespace Ushio.Commands
{
    [Name("Pokemon")]
    public class PokemonCommands : ModuleBase<SocketCommandContext>
    {
        private readonly PokemonApiService pokemonApi;

        public PokemonCommands(PokemonApiService pkmnApiService)
        {
            pokemonApi = pkmnApiService;
        }

        [Command("pokedex")]
        public async Task GetPokedexEntryFor(string pokemon)
        {
            var entry = await pokemonApi.GetPokedexEntryAsync(pokemon);

            var pokedexEntry = new EmbedBuilder
            {
                ThumbnailUrl = entry.SpriteUrl,
                Title = $"Pokedex Entry for: {entry.PokemonName}",
                Color = Color.Red,
                Description = entry.FlavorText
            }
            .WithFooter(footer => footer.Text = $"Game Version: Pokemon {entry.CameFrom}")
            .WithCurrentTimestamp()
            .Build();

            await ReplyAsync(embed: pokedexEntry);
        }
    }
}
