using Microsoft.AspNetCore.Mvc;
using WebAPIBookStore.Consts;
using WebAPIBookStore.Dto;
using WebAPIBookStore.UseCase;

namespace WebAPIBookStore.Controllers
{
    [Route("api/Category")]
    [ApiController]
    public class CategoryController : ControllerBase
    {
        private readonly CategoryUseCase _categoryUseCase;
        public CategoryController(CategoryUseCase categoryUseCase)
        {
            _categoryUseCase = categoryUseCase;
        }

        [HttpGet]
        public IActionResult GetCategories()
        {
            var output = _categoryUseCase.Get();
            return output.Error != StatusCodeAPI.NotFound ? Ok(output) : NotFound(output); 
        }

        [HttpGet("{id}")]
        public IActionResult GetCategory([FromRoute] int id)
        {
            var output = _categoryUseCase.GetById(id);
            return output.Error != StatusCodeAPI.NotFound ? Ok(output) : NotFound(output);
        }

        [HttpPost]
        public IActionResult CreateCategory([FromBody] CategoryDto categoryDto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var output = _categoryUseCase.Post(categoryDto);
            if (!output.Success)
            {
                switch (output.Error)
                {
                    case StatusCodeAPI.UnprocessableEntity:
                        return StatusCode(422, output);

                    case StatusCodeAPI.InternalServer:
                        return BadRequest(output);
                }
            }

            return Ok(output);
        }

        [HttpPut]
        public IActionResult UpdateCategory([FromBody] CategoryDto categoryDto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var output = _categoryUseCase.Put(categoryDto);
            if (!output.Success)
            {
                switch (output.Error)
                {
                    case StatusCodeAPI.NotFound:
                        return NotFound(output);

                    case StatusCodeAPI.InternalServer:
                        return BadRequest(output);
                }
            }

            return Ok(output);
        }

        [HttpDelete("{id:int}")]
        public IActionResult DeleteCategory([FromRoute] int id)
        {
            var output = _categoryUseCase.Delete(id);
            if (!output.Success)
            {
                switch (output.Error)
                {
                    case StatusCodeAPI.NotFound:
                        return NotFound(output);

                    case StatusCodeAPI.InternalServer:
                        return BadRequest(output);
                }
            }

            return Ok(output);
        }
    }
}