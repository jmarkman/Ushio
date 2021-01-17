using System;

namespace Ushio.Data.Pokemon
{
    public class PokedexEntry
    {
        /// <summary>
        /// The URL to the Pokemon's sprite hosted by the PokeAPI github repo
        /// </summary>
        public string SpriteUrl { get; set; }

        /// <summary>
        /// The name of the Pokemon
        /// </summary>
        public string PokemonName { get; set; }

        /// <summary>
        /// The associated Pokedex entry that Pokemon has
        /// </summary>
        public string FlavorText { get; set; }

        /// <summary>
        /// The game where said Pokedex entry came from
        /// </summary>
        public string CameFrom { get; set; }

        public override string ToString()
        {
            return $"Pokemon: {PokemonName}{Environment.NewLine}Pokedex Entry: {FlavorText}{Environment.NewLine}(This entry came from {CameFrom})";
        }
    }
}
