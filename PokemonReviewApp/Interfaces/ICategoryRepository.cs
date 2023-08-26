using PokemonReviewApp.Models;

namespace PokemonReviewApp.Interfaces
{
    public interface ICategoryRepository
    {
        ICollection<Category> GetCategories();
        Category GetCategory(int id);
        ICollection<Pokemon> GetPokemonByCategory(int id);
        bool CategoryExist(int id);
        bool CreateCategory(Category category);
        bool Save();
        bool UpdateCategory(Category category);
        bool DeleteCategory(Category category);
    }
}
