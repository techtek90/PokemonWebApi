using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using PokemonReviewApp.DTO;
using PokemonReviewApp.Interfaces;
using PokemonReviewApp.Models;

namespace PokemonReviewApp.Controllers
{

    [Route("api/[controller]")]
    [ApiController]
    public class ReviewController : Controller
    {
        private readonly IReviewRepository _reviewRepository;
        public readonly IPokemonRepository _pokemonRepository;
        public readonly IReviewerRepository _reviewerRepository;
        private readonly IMapper _mapper;

        public ReviewController(IReviewRepository reviewRepository,IPokemonRepository pokemonRepository, IReviewerRepository reviewerRepository, IMapper mapper)
        {
            _reviewRepository = reviewRepository;
            _pokemonRepository = pokemonRepository;
            _reviewerRepository = reviewerRepository;

            _mapper = mapper;
        }

        [HttpGet("{reviewId}")]
        [ProducesResponseType(200, Type = typeof(Review))]
        public IActionResult GetReview(int reviewId)
        {
            if (!_reviewRepository.ReviewsExist(reviewId))
                return NotFound();
            var review = _mapper.Map<ReviewDto>(_reviewRepository.GetReview(reviewId));
            if(!ModelState.IsValid)
                return BadRequest(ModelState);
            return Ok(review);
        }

        [HttpGet]
        [ProducesResponseType(200, Type = typeof(IEnumerable<Review>))]
        [ProducesResponseType(400)]
        public IActionResult GetReviews()
        {
            var reviews = _mapper.Map<List<ReviewDto>>(_reviewRepository.GetReviews());
            if(!ModelState.IsValid)
                return BadRequest(ModelState);
            return Ok(reviews);
        }

        [HttpGet("pokemon/{pokeId}")]
        [ProducesResponseType(200, Type = typeof(IEnumerable<Review>))]
        [ProducesResponseType(400)]
        public IActionResult GetReviewsOfaPokemon(int pokeId)
        {
             var reviews = _mapper.Map<List<ReviewDto>>(_reviewRepository.GetReviewsOfaPokemon(pokeId));
            if (!ModelState.IsValid)
                return BadRequest(ModelState);
            return Ok(reviews);
        }

        [HttpPost]
        [ProducesResponseType(200)]
        [ProducesResponseType(400)]
        public IActionResult CreateReview ([FromQuery] int reviewerId, [FromQuery] int pokemonId, [FromBody] ReviewDto newReview)
        {
            if(newReview == null)
            {
                ModelState.AddModelError("", "Review is null");
                return StatusCode(442,ModelState);
            }

            var reviewMatched = _reviewRepository.GetReviews().Where(r => r.Title.Trim().ToUpper() == newReview.Title.Trim().ToUpper()).FirstOrDefault();
            if(reviewMatched != null)
            {
                ModelState.AddModelError("", "Review already exist.");
                return StatusCode(442, ModelState);
            }

            var reviewMapped = _mapper.Map<Review>(newReview);
            reviewMapped.Pokemon = _pokemonRepository.GetPokemon(pokemonId);
            reviewMapped.Reviewer = _reviewerRepository.GetReviewer(reviewerId);

            if(!_reviewRepository.CreateReview(reviewMapped))
            {
                ModelState.AddModelError("", "Sorry something went wrong.");
                return StatusCode(442, ModelState);
            }
            return Ok("Ekleme Başarılı");
        }

        [HttpPut("{reviewId}")]
        [ProducesResponseType(200)]
        [ProducesResponseType(400)]
        public IActionResult UpdateReview (int reviewId, [FromBody] ReviewDto review)
        {
            if (review == null)
                return BadRequest();

            if(reviewId != review.Id)
                return BadRequest();

            var  reviewMapped = _mapper.Map<Review> (review);
            if(!_reviewRepository.UpdateReview(reviewMapped))
                return BadRequest();    
            return Ok("Basarılı...");
        }

        [HttpDelete("{reviewId}")]
        [ProducesResponseType(200)]
        [ProducesResponseType(400)]

        public IActionResult DeleteReview(int reviewId)
        {
            if(!_reviewRepository.ReviewsExist(reviewId))
                return NotFound();

            var reviewToDelete = _reviewRepository.GetReview(reviewId);
            if (!ModelState.IsValid)
                return BadRequest();
            if(!_reviewRepository.DeleteReview(reviewToDelete))
                ModelState.AddModelError("", "Something went wrong while deleting");
               
            return Ok("Silindi...");
        }
    }
}
