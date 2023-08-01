using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using WebAPIBookStore.Dto;
using WebAPIBookStore.Interfaces;
using WebAPIBookStore.Models;
namespace WebAPIBookStore.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductController : ControllerBase
    {
        private readonly IProductRepository _productRepository;
        private readonly ICategoryRepository _categoryRepository;
        private readonly IMapper _mapper;
        public ProductController(IProductRepository productRepository,
                                 ICategoryRepository categoryRepository,
                                 IMapper mapper) 
        {
            _productRepository = productRepository;
            _categoryRepository = categoryRepository;
            _mapper = mapper;
        }
        [HttpGet]
        public IActionResult GetProducts()
        {
            var products = _productRepository.GetProducts();
            if (products.Count <= 0)
                return NotFound();
            
            var productMaps = _mapper.Map<List<ProductDto>>(products);
            return ModelState.IsValid ? Ok(productMaps) : BadRequest(ModelState);
        }
        
        [HttpGet("id")]
        public IActionResult GetProduct([FromQuery] int id)
        {
            var product = _productRepository.GetProduct(id);
            if (product == null)
                return NotFound();
            
            var productMap = _mapper.Map<ProductDto>(product);
            return ModelState.IsValid ? Ok(productMap) : BadRequest(ModelState);
        }
        
        [HttpGet("Category/categoryId")]
        public IActionResult GetProductsByCategoryId([FromQuery] int categoryId)
        {
            if (!_categoryRepository.CategoryExists(categoryId))
                return NotFound("Not found category");
            
            var products = _productRepository.GetProductsByCategory(categoryId);
            if (products.Count <= 0)
                return NotFound();
            
            var productMaps = _mapper.Map<List<ProductDto>>(products);
            return ModelState.IsValid ? Ok(productMaps) : BadRequest(ModelState);
        }
        
        [HttpGet("name")]
        public IActionResult GetProduct([FromQuery] string name)
        {
            var products = _productRepository.GetProductsByName(name);
            if (products.Count <= 0)
                return NotFound();
            
            var productMaps = _mapper.Map<List<ProductDto>>(products);
            return ModelState.IsValid ? Ok(productMaps) : BadRequest(ModelState);
        }
        [HttpPost]
        public IActionResult CreateProduct([FromQuery] int categoryId,[FromBody] ProductDto productCreate)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);
            
            if (!_categoryRepository.CategoryExists(categoryId))
                return NotFound("Not found category");
            
            var product = _productRepository.GetProducts().FirstOrDefault(c => c.Name.Trim().ToUpper() == productCreate.Name.Trim().ToUpper());
            if (product != null)
            {
                ModelState.AddModelError("", "Product already exists");
                return StatusCode(422, ModelState);
            }
            
            var productMap = _mapper.Map<Product>(productCreate);
            if (!_productRepository.CreateProduct(categoryId, productMap))
            {
                ModelState.AddModelError("", "Something went wrong while savin");
                return StatusCode(500, ModelState);
            }
            
            return Ok("Successfully created");
        }
        [HttpPut]
        public IActionResult UpdateProduct(int prodId, ProductDto product)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);
            
            var productUpdate = _productRepository.GetProduct(prodId);
            if (productUpdate == null)
                return NotFound("Not found product");
            
            var productMap = _mapper.Map<Product>(product);
            if (!_productRepository.UpdateProduct(productUpdate, productMap))
            {
                ModelState.AddModelError("", "Something went wrong while savin");
                return StatusCode(500, ModelState);
            }
            
            return Ok("Successfully updated");
        }
        
        [HttpDelete("id")]
        public IActionResult DeleteProduct([FromQuery] int id)
        {
            var product = _productRepository.GetProduct(id);
            if (product == null)
                return NotFound("Not found product");
            
            if (!_productRepository.DeleteProduct(product))
            {
                ModelState.AddModelError("", "Something went wrong while savin");
                return StatusCode(500, ModelState);
            }
            
            return Ok("Successfully deleted");
        }
    }
}
