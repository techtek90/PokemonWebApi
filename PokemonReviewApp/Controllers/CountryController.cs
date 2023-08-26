using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using PokemonReviewApp.DTO;
using PokemonReviewApp.Interfaces;
using PokemonReviewApp.Models;
using System.Collections.Generic;

namespace PokemonReviewApp.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CountryController: Controller
    {
        private readonly ICountryRepository _countryRepository;
        private readonly IMapper _mapper;
        public CountryController(ICountryRepository countryRepository, IMapper mapper)
        {
            _countryRepository = countryRepository;
            _mapper = mapper;
        }

        [HttpGet]
        [ProducesResponseType(200, Type = typeof(IEnumerable<Country>))]       
        public IActionResult GetCountries()
        {
            var countries= _mapper.Map<List<CountryDto>>(_countryRepository.GetCountries());
            if(!ModelState.IsValid)
                return BadRequest(ModelState);
            return Ok(countries);
        }

        [HttpGet("{countryId}")]
        [ProducesResponseType(200, Type = typeof(Country))]
        [ProducesResponseType(400)]
        public IActionResult GetCountry( int countryId)
        {
            if(!_countryRepository.CountryExist(countryId))
                return NotFound();
            var country = _mapper.Map<CountryDto>(_countryRepository.GetCountry(countryId));
            if (!ModelState.IsValid)
                return BadRequest(ModelState);
            return Ok(country);
        }

        [HttpGet("/owners/{ownerId}")]
        [ProducesResponseType(200, Type = typeof(IEnumerable<Country>))]
        [ProducesResponseType(400)]
 
        public IActionResult GetCountryByOwner(int ownerId)
        {
            var country =_mapper.Map<CountryDto>(_countryRepository.GetCountryByOwner(ownerId));
            if (!ModelState.IsValid)
                return BadRequest(ModelState);
            return Ok(country);
        }

        [HttpPost]
        [ProducesResponseType(204)]
        [ProducesResponseType(400)]

        public IActionResult CreateCountry([FromBody] CountryDto newCountry)
        {
            if(newCountry == null)
            {
                ModelState.AddModelError("", "Country is null");
                return StatusCode(442, ModelState);
            }

            var country = _countryRepository.GetCountries().Where(c => c.Name.Trim().ToUpper() == newCountry.Name.TrimEnd().ToUpper()).FirstOrDefault();

            if(country != null)
            {
                ModelState.AddModelError("", "Country already exist.");
                return StatusCode(442, ModelState);
            }
            var countryMapped = _mapper.Map<Country>(newCountry);
            return Ok("Successfully created.");
        }

        [HttpPut]
        [ProducesResponseType(200)]
        [ProducesResponseType(400)]
        public IActionResult UpdateCountry(int countryId, [FromBody] CountryDto updatedCountry)
        {
            if(updatedCountry == null)
            {
                ModelState.AddModelError("", "Country is null");
                return StatusCode(442, ModelState);
            }

            if(countryId != updatedCountry.Id)
            {
                ModelState.AddModelError("", "Country is not the same");
                return StatusCode(442, ModelState);
            }
            if(!_countryRepository.CountryExist(countryId))
            {
                return NotFound();
            }

            var countryMapped = _mapper.Map<Country>(updatedCountry);
            if(!_countryRepository.UpdateCountry(countryMapped))
            {
                ModelState.AddModelError("", "Something went wrong");
                return StatusCode(442, ModelState);
            }
            return Ok("Succesfully created.");            
        }

        [HttpDelete ("{countryId}")]
        [ProducesResponseType(200)]
        [ProducesResponseType(400)]
        public IActionResult DeleteCountry (int countryId)
        {
            if (!_countryRepository.CountryExist(countryId))
                return NotFound();
            var countryToDelete = _countryRepository.GetCountry(countryId);
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            if (!_countryRepository.DeleteCountry(countryToDelete))
                 ModelState.AddModelError("","Something went wrong while deleting..");
            return Ok("Silindi..");             
        }
    }
}
