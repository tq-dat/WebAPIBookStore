using AutoMapper;
using WebAPIBookStore.Dto;
using WebAPIBookStore.Interfaces;
using WebAPIBookStore.Models;
using WebAPIBookStore.Usecase;

namespace WebAPIBookStore.UseCase
{
    public class ProductUseCase
    {
        private readonly IProductRepository _productRepository;
        private readonly ICategoryRepository _categoryRepository;
        private readonly IMapper _mapper;
        private readonly IUseCaseOutput _useCaseOutput;

        public ProductUseCase(
            IProductRepository productRepository,
            ICategoryRepository categoryRepository,
            IMapper mapper,
            IUseCaseOutput useCaseOutput)
        {
            _productRepository = productRepository;
            _categoryRepository = categoryRepository;
            _mapper = mapper;
            _useCaseOutput = useCaseOutput;
        }

        public Output Get()
        {
            var products = _productRepository.GetProducts();
            if (!products.Any())
            {
                return _useCaseOutput.NotFound("Not found product");
            }

            return _useCaseOutput.Success(_mapper.Map<List<ProductDto>>(products));
        }

        public Output GetById(int id)
        {
            var product = _productRepository.GetProduct(id);
            if (product == null)
            {
                return _useCaseOutput.NotFound("Not found productId");
            }

            return _useCaseOutput.Success(_mapper.Map<ProductDto>(product));
        }

        public Output GetByCategory(int categoryId)
        {
            if (!_categoryRepository.CategoryExists(categoryId))
            {
                return _useCaseOutput.NotFound("Not found categoryId");
            }

            var products = _productRepository.GetProductsByCategory(categoryId);
            if (!products.Any())
            {
                return _useCaseOutput.NotFound("Not found product");
            }

            return _useCaseOutput.Success(_mapper.Map<List<ProductDto>>(products));
        }

        public Output GetByName(string name)
        {
            var products = _productRepository.GetProductsByName(name);
            if (!products.Any())
            {
                return _useCaseOutput.NotFound("Not found product");
            }

            return _useCaseOutput.Success(_mapper.Map<List<ProductDto>>(products));
        }

        public Output Post(ProductCreate productCreate)
        {
            if (!_categoryRepository.CategoryExists(productCreate.CategoryId))
            {
                return _useCaseOutput.NotFound("Not found categoryId");
            }

            var product = _productRepository.GetProducts().FirstOrDefault(c => c.Name.Trim().ToUpper() == productCreate.ProductDto.Name.Trim().ToUpper());
            if (product != null)
            {
                return _useCaseOutput.UnprocessableEntity("Product already exists");
            }

            var productMap = _mapper.Map<Product>(productCreate.ProductDto);
            if (!_productRepository.CreateProduct(productCreate.CategoryId, productMap))
            {
                return _useCaseOutput.InternalServer("Something went wrong while saving");
            }

            return _useCaseOutput.Success(_mapper.Map<ProductDto>(productMap));
        }

        public Output Put(ProductDto product) 
        {
            var productUpdate = _productRepository.GetProduct(product.Id);
            if (productUpdate == null)
            {
                return _useCaseOutput.NotFound("Not found productId");
            }

            var productMap = _mapper.Map<Product>(product);
            if (!_productRepository.UpdateProduct(productUpdate, productMap))
            {
                return _useCaseOutput.InternalServer("Something went wrong while saving");
            }

            return _useCaseOutput.Success(_mapper.Map<ProductDto>(productMap));
        }

        public Output Delete(int id)
        {
            var product = _productRepository.GetProduct(id);
            if (product == null)
            {
                return _useCaseOutput.Success("Not found productId");
            }

            if (!_productRepository.DeleteProduct(product))
            {
                return _useCaseOutput.InternalServer("Something went wrong while saving");
            }

            return _useCaseOutput.Success(_mapper.Map<ProductDto>(product));
        }
    }
}
