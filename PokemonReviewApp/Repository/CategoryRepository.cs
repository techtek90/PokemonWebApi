using PokemonReviewApp.Data;
using PokemonReviewApp.Interfaces;
using PokemonReviewApp.Models;

namespace PokemonReviewApp.Repository
{
    public class CategoryRepository : ICategoryRepository
    {
        public readonly DataContext _dataContext;
        public CategoryRepository(DataContext datacontext)
        {
            _dataContext = datacontext;
        }
        public bool CategoryExist(int id)
        {
            return _dataContext.Categories.Any(c => c.Id == id);
        }

        public bool CreateCategory(Category category)
        {
            // change tracker will insert it when savechanges methos is called
            _dataContext.Add(category);
            return Save();
        }

        public bool DeleteCategory(Category category)
        {
            _dataContext.Remove(category);
            return Save();
        }

        public ICollection<Category> GetCategories()
        {
           return _dataContext.Categories.ToList();
        }

        public Category GetCategory(int id)
        {
            return _dataContext.Categories.Where(c => c.Id == id).FirstOrDefault();
        }

        public ICollection<Pokemon> GetPokemonByCategory(int id)
        {
            return _dataContext.PokemonCategories.Where(c => c.CategoryId == id).Select(e => e.Pokemon).ToList();
        }

        public bool Save()
        {
            var saved = _dataContext.SaveChanges();
            return saved > 0 ? true : false;
        }

        public bool UpdateCategory(Category category)
        {
            _dataContext.Update(category);
            return Save();
        }
    }    
}
