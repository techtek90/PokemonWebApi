using PokemonReviewApp.Models;

namespace PokemonReviewApp.Interfaces
{
    public interface IPokemonRepository
    {   
        ICollection<Pokemon> GetPokemons();

        Pokemon GetPokemon(int kimlikNo);
        Pokemon GetPokemon(string isim);
        Decimal GetPokemonRating(int pokeId);
        bool PokemonExist(int pokeId);
        bool CreatePokemon(int ownerId, int categoryId, Pokemon pokemon);
        bool Save();
        bool UpdatePokemon(int ownerId, int categoryId, Pokemon pokemon);
        bool DeletePokemon(Pokemon pokemon);

    }
}
