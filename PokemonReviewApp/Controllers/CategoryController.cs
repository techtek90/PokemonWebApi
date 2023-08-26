using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using PokemonReviewApp.DTO;
using PokemonReviewApp.Interfaces;
using PokemonReviewApp.Models;

namespace PokemonReviewApp.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CategoryController:Controller
    {
        private readonly ICategoryRepository _categoryRepository;
        private readonly IMapper _mapper;
        public CategoryController(ICategoryRepository categoryRepository, IMapper mapper)
        {
            _categoryRepository = categoryRepository;
            _mapper = mapper;
        }

        [HttpGet]
        [ProducesResponseType(200, Type = typeof(IEnumerable<Category>))]

        public IActionResult GetCategories()
        {
            var categories = _mapper.Map<List<CategoryDto>>(_categoryRepository.GetCategories());
            if(!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            return Ok(categories);
        }

        [HttpGet("{categoryId}")]
        [ProducesResponseType(200, Type = typeof(Category))]
        [ProducesResponseType(400)]        
        public IActionResult GetCategory(int categoryId)
        {
            if(!_categoryRepository.CategoryExist(categoryId))
                return NotFound();

            var category = _mapper.Map<CategoryDto>(_categoryRepository.GetCategory(categoryId));
            if (!ModelState.IsValid)            
                return BadRequest(ModelState);
            
            return Ok(category);
        }

        [HttpGet ("pokemon/{categoryId}")]
        [ProducesResponseType(200, Type = typeof(IEnumerable<Category>))]
        [ProducesResponseType(400)]
        public IActionResult GetPokemonByCategory(int categoryId)
        {
            var pokemons = _mapper.Map<List<PokemonDto>>(_categoryRepository.GetPokemonByCategory(categoryId));
            if(!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            return Ok(pokemons);
        }

        [HttpPost]
        [ProducesResponseType(204)]
        [ProducesResponseType(400)]
        public IActionResult CreateCategory([FromBody] CategoryDto newCategory)
        {
            // check if it is null
            if (newCategory == null)
                return BadRequest(ModelState);

            //check if it exists
            var category = _categoryRepository.GetCategories().Where(c => c.Name.Trim().ToUpper() == newCategory.Name.Trim().ToUpper()).FirstOrDefault();
            if (category != null)
            {
                ModelState.AddModelError("", "Category already exist.");
                return StatusCode(422, ModelState);
            }
            
            // create the category and map it
            var categoryMapped = _mapper.Map<Category>(newCategory);
            if(!_categoryRepository.CreateCategory(categoryMapped))
            {
                ModelState.AddModelError("", "Something went wrong while saving..");
                return StatusCode(500, ModelState);
            }
            return Ok("Succesfully created.");
        }
        
        [HttpPut]
        [ProducesResponseType(200)]
        [ProducesResponseType(400)]
        public IActionResult UpdateCategory(int categoryId, [FromBody]CategoryDto updatedCategory)
        {
            if(updatedCategory == null)
            {
                return BadRequest(ModelState);
            }

            if(categoryId != updatedCategory.Id)
            {
                return BadRequest(ModelState);
            }

            var categoryMapped = _mapper.Map<Category>(updatedCategory);
            if(!_categoryRepository.UpdateCategory(categoryMapped))
            {
                return BadRequest();
            }
            return Ok("Successful");
        }

        [HttpDelete ("{categoryId}")]
        [ProducesResponseType(200)]
        [ProducesResponseType(400)]

        public IActionResult DeleteCategory(int categoryId)
        {
            if (!_categoryRepository.CategoryExist(categoryId))
                return BadRequest();
            var catToDelete = _categoryRepository.GetCategory(categoryId);                      
            if(!_categoryRepository.DeleteCategory(catToDelete))
                return BadRequest();
            return Ok("Silindi");
        }
    }
}
