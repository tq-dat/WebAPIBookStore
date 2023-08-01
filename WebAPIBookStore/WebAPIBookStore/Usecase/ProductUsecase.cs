using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using NuGet.Protocol;
using WebAPIBookStore.Dto;
using WebAPIBookStore.Interfaces;
using WebAPIBookStore.Models;

namespace WebAPIBookStore.Usecase
{
    public class ProductUsecase
    {
        private readonly IProductRepository _productRepository;
        private readonly ICategoryRepository _categoryRepository;
        private readonly IMapper _mapper;

        public ProductUsecase(IProductRepository productRepository,
                              ICategoryRepository categoryRepository,
                              IMapper mapper)
        {
            _productRepository = productRepository;
            _categoryRepository = categoryRepository;
            _mapper = mapper;
        }

        public List<ProductDto>? Get()
        {
            var products = _productRepository.GetProducts();
            if (products.Count() <= 0)
                return null;

            var productMaps = _mapper.Map<List<ProductDto>>(products);
            return productMaps;
        }

        public ProductDto? GetById(int id)
        {
            var product = _productRepository.GetProduct(id);
            if (product == null)
                return null;

            var productMap = _mapper.Map<ProductDto>(product);
            return productMap;
        }

        public List<ProductDto>? GetByCategory(int categoryId)
        {
            if (!_categoryRepository.CategoryExists(categoryId))
                return null;

            var products = _productRepository.GetProductsByCategory(categoryId);
            if (products.Count() <= 0)
                return null;

            var productMaps = _mapper.Map<List<ProductDto>>(products);
            return productMaps;
        }

        public List<ProductDto>? GetByName(string name)
        {
            var products = _productRepository.GetProductsByName(name);
            if (products.Count() <= 0)
                return null;

            var productMaps = _mapper.Map<List<ProductDto>>(products);
            return productMaps;
        }

        public int Post(int categoryId, ProductDto productCreate)
        {
            if (!_categoryRepository.CategoryExists(categoryId))
                return 404;

            var product = _productRepository.GetProducts().FirstOrDefault(c => c.Name.Trim().ToUpper() == productCreate.Name.Trim().ToUpper());
            if (product != null)
            {
                return 422;
            }

            var productMap = _mapper.Map<Product>(productCreate);
            if (!_productRepository.CreateProduct(categoryId, productMap))
            {
                return 500;
            }

            return 200;
        }

        public int Put(int prodId, ProductDto product) 
        {
            var productUpdate = _productRepository.GetProduct(prodId);
            if (productUpdate == null)
                return 404;

            var productMap = _mapper.Map<Product>(product);
            if (!_productRepository.UpdateProduct(productUpdate, productMap))
            {
                return 500;
            }

            return 200;
        }

        public int Delete(int id)
        {
            var product = _productRepository.GetProduct(id);
            if (product == null)
                return 404;

            if (!_productRepository.DeleteProduct(product))
            {
                return 500;
            }

            return 200;
        }
    }
}
