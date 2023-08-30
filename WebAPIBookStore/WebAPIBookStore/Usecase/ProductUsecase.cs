using AutoMapper;
using Google.Protobuf.WellKnownTypes;
using Newtonsoft.Json.Linq;
using WebAPIBookStore.Dto;
using WebAPIBookStore.Enum;
using WebAPIBookStore.Input;
using WebAPIBookStore.Interfaces;
using WebAPIBookStore.Models;
using WebAPIBookStore.Result;
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

            return _useCaseOutput.Success(_mapper.Map<List<ProductOutput>>(products));
        }

        public Output GetById(int id)
        {
            var product = _productRepository.GetProductReturnProductOutput(id);
            if (product == null)
            {
                return _useCaseOutput.NotFound("Not found productId");
            }

            return _useCaseOutput.Success(product);
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

        public Output Post(AddProductInput addProductInput)
        {
            foreach (var id in addProductInput.CategoryIds)
            {
                if (!_categoryRepository.CategoryExists(id))
                {
                    return _useCaseOutput.NotFound("Not found categoryId");
                }
            }

            var product = _productRepository.GetProducts().FirstOrDefault(c => c.Name.Trim().ToUpper() == addProductInput.Name.Trim().ToUpper());
            if (product != null)
            {
                return _useCaseOutput.UnprocessableEntity("Product already exists");
            }

            if (!_productRepository.CreateProduct(addProductInput))
            {
                return _useCaseOutput.InternalServer("Something went wrong while saving");
            }

            return _useCaseOutput.Success(addProductInput);
        }

        public Output Put(UpdateProductInput updateProductInput)
        {
            var productUpdate = _productRepository.GetProductReturnProduct(updateProductInput.Id);
            if (productUpdate == null)
            {
                return _useCaseOutput.NotFound("Not found productId");
            }

            if (!_productRepository.UpdateProduct(productUpdate, updateProductInput))
            {
                return _useCaseOutput.InternalServer("Something went wrong while saving");
            }

            return _useCaseOutput.Success(_mapper.Map<ProductDto>(productUpdate));
        }

        public Output Delete(int id)
        {
            var product = _productRepository.GetProductReturnProduct(id);
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

        public Output Sort(SortInput sortInput)
        {
            var products = _useCaseOutput.Success(Sort(sortInput));
            if (products == null)
                return _useCaseOutput.InternalServer("Input not true");

            return _useCaseOutput.Success(products);
        }

        public Output SearchByOption(string input, int? limit, int option)
        {
            var products = _productRepository.SearchByOption(input, limit, option);
            if (products.Any())
                return _useCaseOutput.NotFound("Not found product");

            return _useCaseOutput.Success(products);
        }
    }
}
