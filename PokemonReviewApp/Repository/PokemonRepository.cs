
using PokemonReviewApp.Data;
using PokemonReviewApp.Interfaces;
using PokemonReviewApp.Models;

namespace PokemonReviewApp.Repository
{
    public class PokemonRepository : IPokemonRepository
    {
        public readonly DataContext _context;
        public PokemonRepository(DataContext context)
        {
            _context = context;
        }

        public bool CreatePokemon(int ownerId, int categoryId, Pokemon pokemon)
        {
            var pokemonOwnerEntity = _context.Owner.Where(x => x.Id == ownerId).FirstOrDefault();
            var pokemonCategoryEntity = _context.Categories.Where( c => c.Id == categoryId).FirstOrDefault();

            var pokemonOwner = new PokemonOwner() { Owner = pokemonOwnerEntity, Pokemon = pokemon };
            var pokemonCategory = new PokemonCategory() { Category= pokemonCategoryEntity, Pokemon= pokemon };
            
            _context.Add(pokemonOwner);
            _context.Add(pokemonCategory);
            _context.Add(pokemon);
            return Save();
        }

        public bool DeletePokemon(Pokemon pokemon)
        {
            _context.Remove(pokemon);
            return Save();
        }

        public Pokemon GetPokemon(int kimlikNo)
        {
            return _context.Pokemon.Where(p=> p.ID == kimlikNo).FirstOrDefault();
        }

        public Pokemon GetPokemon(string isim)
        {
            return _context.Pokemon.Where(p => p.Name == isim).FirstOrDefault();
        }

        public decimal GetPokemonRating(int pokeId)
        {
            var reviews = _context.Reviews.Where(p => p.Pokemon.ID == pokeId);
            if(reviews.Count() <= 0)
            {
                return 0;
            }
            return ((decimal)reviews.Sum(r => r.Rating/ reviews.Count()));              
        }

        public ICollection<Pokemon> GetPokemons()
        {
            var pokemons = _context.Pokemon.OrderBy(p => p.ID).ToList();
            return pokemons;
        }

        public bool PokemonExist(int pokeId)
        {
            return _context.Pokemon.Any(p=> p.ID==pokeId);
        }

        public bool Save()
        {
            var saved = _context.SaveChanges();
            return saved>0 ? true : false;
        }

        public bool UpdatePokemon(int ownerId, int categoryId, Pokemon pokemon)
        {
            _context.Update(pokemon);
            return Save();
        }
    }
}
