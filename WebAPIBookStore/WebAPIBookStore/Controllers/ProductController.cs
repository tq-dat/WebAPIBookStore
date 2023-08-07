﻿using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using WebAPIBookStore.Dto;
using WebAPIBookStore.Interfaces;
using WebAPIBookStore.Models;
namespace WebAPIBookStore.Controllers
{
    [Route("api/Product")]
    [ApiController]
    public class ProductController : ControllerBase
    {
        private readonly IProductRepository _productRepository;
        private readonly ICategoryRepository _categoryRepository;
        private readonly IMapper _mapper;
        public ProductController(
            IProductRepository productRepository,
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

        [HttpGet("{id}")]
        public IActionResult GetProduct([FromRoute] int id)
        {
            var product = _productRepository.GetProduct(id);
            if (product == null)
                return NotFound();

            var productMap = _mapper.Map<ProductDto>(product);
            return ModelState.IsValid ? Ok(productMap) : BadRequest(ModelState);
        }

        [HttpGet("Category")]
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

        [HttpGet("Search")]
        public IActionResult GetProduct([FromQuery] string name)
        {
            var products = _productRepository.GetProductsByName(name);
            if (products.Count <= 0)
                return NotFound();

            var productMaps = _mapper.Map<List<ProductDto>>(products);
            return ModelState.IsValid ? Ok(productMaps) : BadRequest(ModelState);
        }

        [HttpPost]
        public IActionResult CreateProduct([FromBody] ProductCreate productCreate)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            if (!_categoryRepository.CategoryExists(productCreate.CategoryId))
                return NotFound("Not found category");

            var product = _productRepository.GetProducts().FirstOrDefault(c => c.Name.Trim().ToUpper() == productCreate.ProductDto.Name.Trim().ToUpper());
            if (product != null)
            {
                ModelState.AddModelError("", "Product already exists");
                return StatusCode(422, ModelState);
            }

            var productMap = _mapper.Map<Product>(productCreate.ProductDto);
            return _productRepository.CreateProduct(productCreate.CategoryId, productMap) ? Ok(productMap) : BadRequest(ModelState);
        }

        [HttpPut]
        public IActionResult UpdateProduct([FromBody] ProductDto product)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var productUpdate = _productRepository.GetProduct(product.Id);
            if (productUpdate == null)
                return NotFound("Not found product");

            var productMap = _mapper.Map<Product>(product);
            return _productRepository.UpdateProduct(productUpdate, productMap) ? Ok(productUpdate) : BadRequest(ModelState);
        }

        [HttpDelete("{id:int}")]
        public IActionResult DeleteProduct([FromRoute] int id)
        {
            var product = _productRepository.GetProduct(id);
            if (product == null)
                return NotFound("Not found product");

            return _productRepository.DeleteProduct(product) ? Ok(product) : BadRequest(ModelState);
        }
    }
}
