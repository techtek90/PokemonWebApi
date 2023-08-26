using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using PokemonReviewApp.DTO;
using PokemonReviewApp.Interfaces;
using PokemonReviewApp.Models;

namespace PokemonReviewApp.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PokemonController : Controller
    {
        public readonly IPokemonRepository _pokemonRepository;
        public readonly IReviewRepository _reviewRepository;
        public readonly IMapper _mapper;
        public PokemonController(IPokemonRepository pokemonRepository,IReviewRepository reviewRepository, IMapper mapper ) //dependency injection
        {
            _pokemonRepository = pokemonRepository;
            _reviewRepository = reviewRepository;
            _mapper = mapper;
        }

        [HttpGet]
        [ProducesResponseType(200, Type = typeof(IEnumerable<Pokemon>))]
        public IActionResult GetPokemons()
        {
            var pokemons = _mapper.Map<List<PokemonDto>>(_pokemonRepository.GetPokemons());
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            return Ok(pokemons);
        }

        [HttpGet("{pokeId}")]
        [ProducesResponseType(200, Type = typeof(Pokemon))]
        [ProducesResponseType(400)]
        public IActionResult GetPokemon(int pokeId)
        {
            if (!_pokemonRepository.PokemonExist(pokeId))
                return NotFound();
            var pokemon = _mapper.Map<PokemonDto>(_pokemonRepository.GetPokemon(pokeId));
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            return Ok(pokemon);
        }

        [HttpGet("{pokeId}/rating")]
        [ProducesResponseType(200, Type = typeof(decimal))]
        [ProducesResponseType(400)]
        public IActionResult GetPokemonRating(int pokeId)
        {
            if (!_pokemonRepository.PokemonExist(pokeId))
                return NotFound();

            var rating = _pokemonRepository.GetPokemonRating(pokeId);
            if (!ModelState.IsValid)            
                return BadRequest();           
            return Ok(rating);
        }

        [HttpPost]
        [ProducesResponseType(200)]
        [ProducesResponseType(400)]
        public IActionResult CreatePokemon([FromQuery] int ownerId, [FromQuery] int categoryId, [FromBody] PokemonDto newPokemon)
        {
            if(newPokemon == null)
            {
                return BadRequest(ModelState);
            }

            var pokemons = _pokemonRepository.GetPokemons().Where(p => p.Name.Trim().ToUpper() == newPokemon.Name.Trim().ToUpper()).FirstOrDefault();
            if(pokemons != null)
            {
                ModelState.AddModelError("","Pokemon already exist");
                return StatusCode(442, ModelState);
            }                                                                                                                                          
            var pokemonMapped = _mapper.Map<Pokemon>(newPokemon);
            if(!_pokemonRepository.CreatePokemon(ownerId, categoryId, pokemonMapped))
            {                                                                                                                                       
                ModelState.AddModelError("", "Something went wrong.");
                return StatusCode(442, ModelState);
            }
            return Ok("Successfully created.");
        }

        [HttpPut("{pokeId}")]
        [ProducesResponseType(200)]
        [ProducesResponseType(400)]
        public IActionResult UpdatePokemon(int pokeId, [FromQuery] int ownerId, [FromQuery] int catId, [FromBody] PokemonDto updatedPokemon)
        {
            if (updatedPokemon == null)
                return BadRequest(ModelState);
            if(pokeId != updatedPokemon.ID )
                return BadRequest(ModelState);
            if(!_pokemonRepository.PokemonExist(pokeId))
                return NotFound();
             
            var mappedPokemon = _mapper.Map<Pokemon>(updatedPokemon);
            if(!_pokemonRepository.UpdatePokemon(ownerId, catId, mappedPokemon))
                return BadRequest(ModelState);
            return Ok("Successfully created.");
        }

        [HttpDelete("{pokeId}")]
        [ProducesResponseType(200)]
        [ProducesResponseType(400)]
        public IActionResult DeletePokemon(int pokeId)
        {
            if (pokeId == null)
                return BadRequest();
            if (!_pokemonRepository.PokemonExist(pokeId))
                return NotFound();

            var reviewsTodelete = _reviewRepository.GetReviewsOfaPokemon(pokeId);
            var pokeToDelete = _pokemonRepository.GetPokemon(pokeId);
            
            if (!_reviewRepository.DeleteListOfReviews(reviewsTodelete.ToList()))
                return BadRequest();
            if (!_pokemonRepository.DeletePokemon(pokeToDelete))
                return BadRequest();
            return Ok("Silindi..");
        }
    }  
}

