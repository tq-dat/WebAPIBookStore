using Microsoft.AspNetCore.Mvc;
using WebAPIBookStore.Consts;
using WebAPIBookStore.Dto;
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
        public IActionResult GetProduct([FromQuery] string name)
        {
            var output = _productUseCase.GetByName(name);
            return output.Error != StatusCodeAPI.NotFound ? Ok(output) : NotFound(output);   
        }

        [HttpPost]
        public IActionResult CreateProduct([FromBody] ProductCreate productCreate)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var output = _productUseCase.Post(productCreate);
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
        public IActionResult UpdateProduct([FromBody] ProductDto product)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var output = _productUseCase.Put(product);
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
