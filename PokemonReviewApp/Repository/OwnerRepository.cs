using PokemonReviewApp.Data;
using PokemonReviewApp.Interfaces;
using PokemonReviewApp.Models;

namespace PokemonReviewApp.Repository
{
    public class OwnerRepository : IOwnerRepository
    {
        private readonly DataContext _dataContext;
        public OwnerRepository(DataContext dataContext)
        {
            _dataContext = dataContext;
        }

        public bool CreateOwner(Owner owner)
        {
           _dataContext.Add(owner);
            return Save();
        }

        public bool DeleteOwner(Owner owner)
        {
            _dataContext.Remove(owner);
            return Save();
        }

        public Owner GetOwner(int ownerId)
        {
            return _dataContext.Owner.Where(o => o.Id == ownerId).FirstOrDefault();
        }

        public ICollection<Owner> GetOwners()
        {
            return _dataContext.Owner.ToList();
        }

        public ICollection<Owner> GetOwnersOfAPokemon(int pokeID)
        {
            return _dataContext.PokemonOwners.Where(p => p.PokemonId == pokeID).Select(o => o.Owner).ToList();
        }

        public ICollection<Pokemon> GetPokemonsByOwner(int ownerId)
        {
            return _dataContext.PokemonOwners.Where(p => p.OwnerId == ownerId).Select(p => p.Pokemon).ToList();
        }

        public bool OwnerExist(int OwnerId)
        {
            return _dataContext.Owner.Any(o => o.Id == OwnerId);
        }

        public bool Save()
        {
            var saved= _dataContext.SaveChanges();
            return saved > 0 ? true : false;
        }

        public bool UpdateOwner(Owner owner)
        {
            _dataContext.Update(owner);
            return Save();
        }
    }
}
