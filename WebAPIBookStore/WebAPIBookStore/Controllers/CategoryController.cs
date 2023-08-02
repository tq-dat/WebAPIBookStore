using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using WebAPIBookStore.Dto;
using WebAPIBookStore.Interfaces;
using WebAPIBookStore.Models;

namespace WebAPIBookStore.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CategoryController : ControllerBase
    {
        private readonly ICategoryRepository _categoryRepository;
        private readonly IMapper _mapper;

        public CategoryController(ICategoryRepository categoryRepository, IMapper mapper) 
        {
            _categoryRepository = categoryRepository;
            _mapper = mapper;
        }

        [HttpGet]
        public IActionResult GetCategories()
        {
            var categories = _categoryRepository.GetCategories();
            if (categories.Count <= 0)
                return NotFound();

            var categoryMaps = _mapper.Map<List<CategoryDto>>(categories);

            return ModelState.IsValid ? Ok(categoryMaps) : BadRequest(ModelState);
        }

        [HttpGet("{id}")]
        public IActionResult GetCategory(int id)
        {
            var category = _categoryRepository.GetCategory(id);
            if (category == null)
                return NotFound("Not found category");

            var categoryMap = _mapper.Map<CategoryDto>(category);
            return ModelState.IsValid ? Ok(categoryMap) : BadRequest(ModelState);
        }

        [HttpPost]
        public IActionResult CreateCategory([FromBody] CategoryDto categoryDto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var category = _categoryRepository.GetCategories().FirstOrDefault(c => c.Name.Trim().ToUpper() == categoryDto.Name.Trim().ToUpper());
            if (category != null)
            {
                ModelState.AddModelError("", "Category already exists");
                return StatusCode(422, ModelState);
            }

            var categoryMap = _mapper.Map<Category>(categoryDto);
            if (!_categoryRepository.CreateCategory(categoryMap))
            {
                ModelState.AddModelError("", "Something went wrong while savin");
                return StatusCode(500, ModelState);
            }

            return Ok("Successfully created");
        }

        [HttpPut]
        public IActionResult UpdateCategory([FromBody] CategoryDto categoryDto)
        {
            var category = _categoryRepository.GetCategory(categoryDto.Id);
            if (category == null)
                return NotFound("Not found category");

            if (!_categoryRepository.UpdateCategory(category,categoryDto.Name))
            {
                ModelState.AddModelError("", "Something went wrong while savin");
                return StatusCode(500, ModelState);
            }

            return Ok("Successfully updated");
        }

        [HttpDelete("{id}")]
        public IActionResult DeleteCategory(int id)
        {
            var deleteCategory = _categoryRepository.GetCategory(id);
            if (deleteCategory == null)
                return NotFound("Not found category");

            if (!_categoryRepository.DeleteCategory(deleteCategory))
            {
                ModelState.AddModelError("", "Something went wrong while savin");
                return StatusCode(500, ModelState);
            }

            return Ok("Successfully deleted");
        }
    }
}
