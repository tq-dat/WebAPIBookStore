using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using WebAPIBookStore.Dto;
using WebAPIBookStore.Interfaces;
using WebAPIBookStore.Models;
using WebAPIBookStore.Usecase;

namespace WebAPIBookStore.Controllers
{
    [Route("api/Product")]
    [ApiController]
    public class ProductController : ControllerBase
    {
        private readonly ProductUsecase _productUsecase;

        public ProductController(ProductUsecase productUsecase)
        {
            _productUsecase = productUsecase;
        }

        [HttpGet]
        public IActionResult GetProducts()
        {
            var output = _productUsecase.Get();
            if (output.Error == "404")
                return NotFound(output);

            return ModelState.IsValid ? Ok(output) : BadRequest(ModelState);
        }

        [HttpGet("{id}")]
        public IActionResult GetProduct([FromRoute] int id)
        {
            var output = _productUsecase.GetById(id);
            if (output.Error == "404")
                return NotFound(output);

            return ModelState.IsValid ? Ok(output) : BadRequest(ModelState);
        }

        [HttpGet("Category")]
        public IActionResult GetProductsByCategoryId([FromQuery] int categoryId)
        {
            var output = _productUsecase.GetByCategory(categoryId);
            if (output.Error == "404")
                return NotFound(output);

            return ModelState.IsValid ? Ok(output) : BadRequest(ModelState);
        }

        [HttpGet("Search")]
        public IActionResult GetProduct([FromQuery] string name)
        {
            var output = _productUsecase.GetByName(name);
            if (output.Error == "404")
                return NotFound(output);

            return ModelState.IsValid ? Ok(output) : BadRequest(ModelState);
        }

        [HttpPost]
        public IActionResult CreateProduct([FromBody] ProductCreate productCreate)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var output = _productUsecase.Post(productCreate.CategoryId, productCreate.ProductDto);
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

            var output = _productUsecase.Put(product.Id, product);
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

            var output = _productUsecase.Delete(id);
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
