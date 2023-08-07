using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using WebAPIBookStore.Dto;
using WebAPIBookStore.Interfaces;
using WebAPIBookStore.Models;
namespace WebAPIBookStore.Controllers
{
    [Route("api/Category")]
    [ApiController]
    public class CategoryController : ControllerBase
    {
        private readonly ICategoryRepository _categoryRepository;
        private readonly IMapper _mapper;
        public CategoryController(
            ICategoryRepository categoryRepository,
            IMapper mapper)
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
            return _categoryRepository.CreateCategory(categoryMap) ? Ok(categoryMap) : BadRequest(ModelState);
        }

        [HttpPut]
        public IActionResult UpdateCategory([FromBody] CategoryDto categoryDto)
        {
            var category = _categoryRepository.GetCategory(categoryDto.Id);
            if (category == null)
                return NotFound("Not found category");

            return _categoryRepository.UpdateCategory(category, categoryDto.Name) ? Ok(category) : BadRequest(ModelState);
        }

        [HttpDelete("{id:int}")]
        public IActionResult DeleteCategory([FromRoute] int id)
        {
            var deleteCategory = _categoryRepository.GetCategory(id);
            if (deleteCategory == null)
                return NotFound("Not found category");

            return _categoryRepository.DeleteCategory(deleteCategory) ? Ok(deleteCategory) : BadRequest(ModelState);
        }
    }
}   