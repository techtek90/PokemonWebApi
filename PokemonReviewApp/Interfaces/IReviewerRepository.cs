using PokemonReviewApp.Models;

namespace PokemonReviewApp.Interfaces
{
    public interface IReviewerRepository
    {
        Reviewer GetReviewer(int reviewerId);
        ICollection<Reviewer> GetReviewers();

        ICollection<Review>GetReviewsbyReviewer(int reviewerId);
        bool ReviewerExist(int reviewerId);
        bool CreateReviewer (Reviewer reviewer);
        bool Save();
        bool UpdateReviewer(Reviewer reviewer);
        bool DeleteReviewer(Reviewer reviewer);
    }
}
