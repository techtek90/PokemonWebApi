using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using PokemonReviewApp.DTO;
using PokemonReviewApp.Interfaces;
using PokemonReviewApp.Models;

namespace PokemonReviewApp.Controllers
{

    [Route("api/[controller]")]
    [ApiController]
    public class ReviewerController : Controller
    {
        private readonly IReviewerRepository _reviewerRepository;
        private readonly IMapper _mapper;
        public ReviewerController(IReviewerRepository reviewerRepository, IReviewRepository reviewRepository, IMapper mapper)
        {
            _reviewerRepository = reviewerRepository;
            _mapper = mapper;
        }

        [HttpGet]
        [ProducesResponseType(200, Type = typeof(IEnumerable<Reviewer>))]
        [ProducesResponseType(400)]
        public IActionResult GetReviewers()
        {
            var reviewers = _mapper.Map<List<Reviewer>>(_reviewerRepository.GetReviewers());
            if (!ModelState.IsValid)
                return BadRequest(ModelState);
            return Ok(reviewers);
        }

        [HttpGet("{reviewerId}")]
        [ProducesResponseType(200, Type = typeof(Reviewer))]
        [ProducesResponseType(400)]
        public IActionResult GetReviewer(int reviewerId)
        {

            if (!_reviewerRepository.ReviewerExist(reviewerId))
                return NotFound();
            var reviewer = _mapper.Map<ReviewerDto>(_reviewerRepository.GetReviewer(reviewerId));
            if (!ModelState.IsValid)
                return BadRequest(ModelState);
            return Ok(reviewer);
        }

        [HttpGet("{reviewerId}/reviews")]
        public IActionResult GetReviewsbyReviewer(int reviewerId)
        {
            if (!_reviewerRepository.ReviewerExist(reviewerId))
                return NotFound();
            var reviews = _mapper.Map<List<ReviewerDto>>(_reviewerRepository.GetReviewsbyReviewer(reviewerId));
            if (!ModelState.IsValid)
                return BadRequest(ModelState);
            return Ok(reviews);
        }

        [HttpPost]
        [ProducesResponseType(200)]
        [ProducesResponseType(400)]
        public IActionResult CreateReviewer([FromBody] ReviewerDto reviewer)
        {
            if (reviewer == null)
            {
                return BadRequest();
            }
            var reviewerMatched = _reviewerRepository.GetReviewers().Where(r => r.FirstName.Trim().ToUpper() == reviewer.FirstName.Trim().ToUpper()).FirstOrDefault();
            if (reviewerMatched != null)
            {
                ModelState.AddModelError("", "Reviewer already exist.");
                return StatusCode(442, ModelState);
            }

            var reviewerMapped = _mapper.Map<Reviewer>(reviewer);
            if (!_reviewerRepository.CreateReviewer(reviewerMapped))
            {
                return BadRequest();
            }
            return Ok("Succesfully created");
        }

        [HttpPut("{reviewerId}")]
        [ProducesResponseType(200)]
        [ProducesResponseType(400)]

        public IActionResult UpdateReviewer (int reviewerId, [FromBody] ReviewerDto updatedReviewer)
        {
            if(reviewerId == null)
                return BadRequest();

            if (reviewerId != updatedReviewer.Id)
                return BadRequest();

            var reviewerMapped = _mapper.Map<Reviewer>(updatedReviewer);
            if(!_reviewerRepository.UpdateReviewer(reviewerMapped))
                return BadRequest();

            return Ok("islem basarılı.. ");
        }

        [HttpDelete ("{reviewerId}")]
        [ProducesResponseType(200)]
        [ProducesResponseType(400)]
        public IActionResult DeleteReviewer ( int reviewerId)
        {
            if(!_reviewerRepository.ReviewerExist(reviewerId))
                return NotFound();
            if (!ModelState.IsValid)
                return BadRequest();
            var reviewerTodelete = _reviewerRepository.GetReviewer(reviewerId);
            if(!_reviewerRepository.DeleteReviewer(reviewerTodelete))
                ModelState.AddModelError("", "Something went wrong while deleting..");
            return Ok("Deleted..");

        }
    }
}
