using PokemonReviewApp.Models;

namespace PokemonReviewApp.Interfaces
{
    public interface IOwnerRepository
    {
        ICollection<Owner> GetOwners();
        Owner GetOwner(int ownerId);
        ICollection<Owner>GetOwnersOfAPokemon(int pokeID);
        ICollection<Pokemon> GetPokemonsByOwner(int pokeId);
      
        bool OwnerExist(int OwnerId);
        bool CreateOwner(Owner owner);
        bool Save();
        bool UpdateOwner(Owner owner);
        bool DeleteOwner (Owner owner);
    }
}
