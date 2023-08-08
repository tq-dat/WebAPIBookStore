using Microsoft.AspNetCore.Mvc;
using WebAPIBookStore.Dto;
using WebAPIBookStore.UseCase;

namespace WebAPIBookStore.Controllers
{
    [Route("api/Product")]
    [ApiController]
    public class ProductController : ControllerBase
    {
        private readonly ProductUseCase _productUseCase;

        public ProductController(ProductUseCase productUseCase)
        {
            _productUseCase = productUseCase;
        }

        [HttpGet]
        public IActionResult GetProducts()
        {
            var output = _productUseCase.Get();
            return output.Error == "404" ? NotFound(output) : Ok(output);
        }

        [HttpGet("{id}")]
        public IActionResult GetProduct([FromRoute] int id)
        {
            var output = _productUseCase.GetById(id);
            return output.Error == "404" ? NotFound(output) : Ok(output);
        }

        [HttpGet("Category")]
        public IActionResult GetProductsByCategoryId([FromQuery] int categoryId)
        {
            var output = _productUseCase.GetByCategory(categoryId);
            return output.Error == "404" ? NotFound(output) : Ok(output);
        }

        [HttpGet("Search")]
        public IActionResult GetProduct([FromQuery] string name)
        {
            var output = _productUseCase.GetByName(name);
            return output.Error == "404" ? NotFound(output) : Ok(output);   
        }

        [HttpPost]
        public IActionResult CreateProduct([FromBody] ProductCreate productCreate)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var output = _productUseCase.Post(productCreate);
            if (output.Success == false)
            {
                if (output.Error == "404")
                    return NotFound(output);

                if (output.Error == "422")
                    return StatusCode(422, output);

                if (output.Error == "500")
                    return BadRequest(output);
            }

            return Ok(output);
        }

        [HttpPut]
        public IActionResult UpdateProduct([FromBody] ProductDto product)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var output = _productUseCase.Put(product);
            if (output.Success == false)
            {
                if (output.Error == "404")
                    return NotFound(output);

                if (output.Error == "500")
                    return BadRequest(output);
            }

            return Ok(output);
        }

        [HttpDelete("{id:int}")]
        public IActionResult DeleteProduct([FromRoute] int id)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var output = _productUseCase.Delete(id);
            if (output.Success == false)
            {
                if (output.Error == "404")
                    return NotFound(output);

                if (output.Error == "500")
                    return BadRequest(output);
            }

            return Ok(output);
        }
    }
}
