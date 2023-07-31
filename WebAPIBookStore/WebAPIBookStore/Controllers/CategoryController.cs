using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using WebAPIBookStore.Dto;
using WebAPIBookStore.Interfaces;
using WebAPIBookStore.Models;
using WebAPIBookStore.Repository;

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
        public IActionResult Getcategories()
        {
            var categories = _mapper.Map<List<CategoryDto>>(_categoryRepository.GetCategories());
            if (categories.Count() <= 0)
                return NotFound();

            return ModelState.IsValid ? Ok(categories) : BadRequest(ModelState);
        }

        [HttpGet("id")]
        public IActionResult GetCategory([FromQuery] int id)
        {
            if (!_categoryRepository.CategoryExists(id))
                return NotFound("Not found category");

            var category = _mapper.Map<CategoryDto>(_categoryRepository.GetCategory(id));
            return ModelState.IsValid ? Ok(category) : BadRequest(ModelState);
        }

        [HttpPost]
        public IActionResult CreateCategory([FromBody] CategoryDto categoryDto)
        {
            if (categoryDto == null)
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
        public IActionResult UpdateCategory([FromQuery] int id,[FromQuery] string name)
        {
            var category = _categoryRepository.GetCategory(id);
            if (category == null)
                return NotFound("Not founf category");

            if (!_categoryRepository.UpdateCategory(category,name))
            {
                ModelState.AddModelError("", "Something went wrong while savin");
                return StatusCode(500, ModelState);
            }

            return Ok("Successfully updated");
        }

        [HttpDelete("id")]
        public IActionResult DeleteCategory([FromQuery] int id)
        {
            var deleteCategory = _categoryRepository.GetCategory(id);
            if (deleteCategory == null)
                return NotFound("Not founf category");

            if (!_categoryRepository.DeleteCategory(deleteCategory))
            {
                ModelState.AddModelError("", "Something went wrong while savin");
                return StatusCode(500, ModelState);
            }

            return Ok("Successfully deleted");
        }
    }
}
