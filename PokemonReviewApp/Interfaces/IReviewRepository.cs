using PokemonReviewApp.Models;

namespace PokemonReviewApp.Interfaces
{
    public interface IReviewRepository
    {
        ICollection<Review> GetReviews();
        Review GetReview(int reviewId);
        ICollection<Review> GetReviewsOfaPokemon(int pokeID);
        bool ReviewsExist (int reviewId);
        bool CreateReview(Review review);
        bool Save();
        bool UpdateReview(Review review);
        bool DeleteReview(Review review);
        bool DeleteListOfReviews (List<Review> reviews);
    }
}
