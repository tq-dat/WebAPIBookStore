using Microsoft.AspNetCore.Mvc;
using WebAPIBookStore.Consts;
using WebAPIBookStore.Dto;
using WebAPIBookStore.Enum;
using WebAPIBookStore.Input;
using WebAPIBookStore.UseCase;

namespace WebAPIBookStore.Controllers
{
    [Route("api/Product")]
    [ApiController]
    public class ProductController : ControllerBase
    {
        private readonly ProductUseCase _productUseCase;
        public ProductController(
            ProductUseCase productUseCase)
        {
            _productUseCase = productUseCase;
        }

        [HttpGet]
        public IActionResult GetProducts()
        {
            var output = _productUseCase.Get();
            return output.Error != StatusCodeAPI.NotFound ? Ok(output) : NotFound(output);
        }

        [HttpGet("{id}")]
        public IActionResult GetProduct([FromRoute] int id)
        {
            var output = _productUseCase.GetById(id);
            return output.Error != StatusCodeAPI.NotFound ? Ok(output) : NotFound(output);
        }

        [HttpGet("Category")]
        public IActionResult GetProductsByCategoryId([FromQuery] int categoryId)
        {
            var output = _productUseCase.GetByCategory(categoryId);
            return output.Error != StatusCodeAPI.NotFound ? Ok(output) : NotFound(output);
        }

        [HttpGet("Search")]
        public IActionResult SearchProdcutByOption([FromQuery] string input, [FromQuery] int? limit, [FromQuery] int option)
        {
            var output = _productUseCase.SearchByOption(input, limit, option); // option 1 : authot, option 2 : puslishYera, default : name
            return output.Error != StatusCodeAPI.NotFound ? Ok(output) : NotFound(output);
        }

        [HttpGet("Sort")]
        public IActionResult Sort([FromBody] SortInput sortInput)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);  

            var output = _productUseCase.Sort(sortInput);
            if (output.Error == StatusCodeAPI.InternalServer)
                return BadRequest(output);

            return Ok(output);
        }

        [HttpPost]
        public IActionResult CreateProduct([FromBody] AddProductInput addProductInput)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var output = _productUseCase.Post(addProductInput);
            if (!output.Success)
            {
                switch (output.Error)
                {
                    case StatusCodeAPI.NotFound:
                        return NotFound(output);

                    case StatusCodeAPI.UnprocessableEntity:
                        return StatusCode(422, output);

                    case StatusCodeAPI.InternalServer:
                        return BadRequest(output);
                }
            }

            return Ok(output);
        }

        [HttpPut]
        public IActionResult UpdateProduct([FromBody] UpdateProductInput updateProductInput)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var output = _productUseCase.Put(updateProductInput);
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
        public IActionResult DeleteProduct([FromRoute] int id)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var output = _productUseCase.Delete(id);
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
