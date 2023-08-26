using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using PokemonReviewApp.DTO;
using PokemonReviewApp.Interfaces;
using PokemonReviewApp.Models;

namespace PokemonReviewApp.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class OwnerController : Controller
    {
        private readonly IOwnerRepository _ownerRepository;
        private readonly IMapper _mapper;
        private readonly ICountryRepository _countryRepository;
        public OwnerController(IOwnerRepository ownerRepository, ICountryRepository countryRepository, IMapper mapper)
        {
            _ownerRepository = ownerRepository;
            _mapper = mapper;
            _countryRepository = countryRepository;
        }

        [HttpGet]
        [ProducesResponseType(200, Type = typeof(IEnumerable<Owner>))]
        public IActionResult GetOwners()
        {
            var owners = _mapper.Map<List<OwnerDto>>(_ownerRepository.GetOwners());
            if (!ModelState.IsValid)
                return BadRequest(ModelState);
            return Ok(owners);
        }

        [HttpGet("{ownerId}")]
        [ProducesResponseType(200, Type = typeof(Owner))]
        [ProducesResponseType(400)]
        public IActionResult GetOwner(int ownerId)
        {
            if (!_ownerRepository.OwnerExist(ownerId))
                return NotFound();
            var owner = _mapper.Map<OwnerDto>(_ownerRepository.GetOwner(ownerId));
            if(!ModelState.IsValid)
                return BadRequest(ModelState);
            return Ok(owner);
        }

        [HttpGet("{ownerId}/pokemon")]
        [ProducesResponseType(200, Type = typeof(Owner))]
        [ProducesResponseType(400)]
        public IActionResult GetPokemonsByOwner(int ownerId)
        {
            if (!_ownerRepository.OwnerExist(ownerId))
                return NotFound();
            var pokemon = _mapper.Map<List<PokemonDto>>(_ownerRepository.GetPokemonsByOwner(ownerId));
            if (!ModelState.IsValid)
                return BadRequest(ModelState);
            return Ok(pokemon);
        }  

        [HttpPost]
        [ProducesResponseType(200)]
        [ProducesResponseType(400)]

        public IActionResult CreateOwner([FromQuery]int countryId, [FromBody] OwnerDto newOwner)
        {
            // check if it is null
            if(newOwner == null)
            {
                return BadRequest(ModelState);
            }

            //check if it exists
            var owner = _ownerRepository.GetOwners().Where(o => o.LastName.Trim().ToUpper() == newOwner.LastName.TrimEnd().ToUpper()).FirstOrDefault();
            if(owner != null)
            {
                ModelState.AddModelError("", "Owner already exist.");
                return StatusCode(442, ModelState);
            }

            var ownerMapped = _mapper.Map<Owner>(newOwner);
            ownerMapped.Country = _countryRepository.GetCountry(countryId);
            if(!_ownerRepository.CreateOwner(ownerMapped))
            {
                ModelState.AddModelError("", "Sorry something went wrong.");
                return StatusCode(442, ModelState);
            }
            return Ok("Succesfully created");
        }

        [HttpPut]
        [ProducesResponseType(200)]
        [ProducesResponseType(400)]
        public IActionResult UpdateOwner(int ownerId, [FromBody]OwnerDto updatedOwner)
        {
            if(updatedOwner == null)
            {
                return BadRequest();
            }
            if(ownerId != updatedOwner.Id)
            {
                return BadRequest();
            }
            if(!_ownerRepository.OwnerExist(ownerId))
            {
                return NotFound();
            }
            var ownerMapped = _mapper.Map<Owner>(updatedOwner);
            if(!_ownerRepository.UpdateOwner(ownerMapped))
            {
                return BadRequest();
            }
            return Ok("Successfully created.");
        }
        
        [HttpDelete("{ownerId}")]
        [ProducesResponseType(200)]
        [ProducesResponseType(400)]
        public IActionResult DeleteOwner(int ownerId)
        {
            if (ownerId == null)
                return BadRequest();
            if(!_ownerRepository.OwnerExist(ownerId))
                return NotFound();
            var ownerToDelete = _ownerRepository.GetOwner(ownerId);

            if(!_ownerRepository.DeleteOwner(ownerToDelete))
                return BadRequest();
            return Ok("Silindi..");
        }
    }
}
