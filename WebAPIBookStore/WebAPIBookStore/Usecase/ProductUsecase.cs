using AutoMapper;
using WebAPIBookStore.Dto;
using WebAPIBookStore.Interfaces;
using WebAPIBookStore.Models;

namespace WebAPIBookStore.UseCase
{
    public class ProductUseCase
    {
        private readonly IProductRepository _productRepository;
        private readonly ICategoryRepository _categoryRepository;
        private readonly IMapper _mapper;
        private readonly Const _const;

        public ProductUseCase(
            IProductRepository productRepository,
            ICategoryRepository categoryRepository,
            IMapper mapper,
            Const @const)
        {
            _productRepository = productRepository;
            _categoryRepository = categoryRepository;
            _mapper = mapper;
            _const = @const;
        }

        public Output Get()
        {
            var products = _productRepository.GetProducts();
            var output = new Output();
            if (!products.Any())
            {
                output.Success = false;
                output.Error = _const.NotFoundCode;
                output.Message = "Not found product";
                return output;
            }

            output.Success = true;
            output.Data = _mapper.Map<List<ProductDto>>(products);
            return output;
        }

        public Output GetById(int id)
        {
            var product = _productRepository.GetProduct(id);
            var output = new Output();
            if (product == null)
            {
                output.Success = false;
                output.Error = _const.NotFoundCode;
                output.Message = "Not found productId";
                return output;
            }

            output.Success = true;
            output.Data = _mapper.Map<ProductDto>(product);
            return output;
        }

        public Output GetByCategory(int categoryId)
        {
            var output = new Output();
            if (!_categoryRepository.CategoryExists(categoryId))
            {
                output.Success = false;
                output.Error = _const.NotFoundCode;
                output.Message = "Not found categoryId";
                return output;
            }

            var products = _productRepository.GetProductsByCategory(categoryId);
            if (!products.Any())
            {
                output.Success = false;
                output.Error = _const.NotFoundCode;
                output.Message = "Not found product";
                return output;
            }

            output.Success = true;
            output.Data = _mapper.Map<List<ProductDto>>(products);
            return output;
        }

        public Output GetByName(string name)
        {
            var output = new Output();
            var products = _productRepository.GetProductsByName(name);
            if (!products.Any())
            {
                output.Success = false;
                output.Error = _const.NotFoundCode;
                output.Message = "Not found product";
                return output;
            }

            output.Success = true;
            output.Data = _mapper.Map<List<ProductDto>>(products);
            return output;
        }

        public Output Post(ProductCreate productCreate)
        {

            var output = new Output();
            if (!_categoryRepository.CategoryExists(productCreate.CategoryId))
            {
                output.Success = false;
                output.Error = _const.NotFoundCode;
                output.Message = "Not found categoryId";
                return output;
            }

            var product = _productRepository.GetProducts().FirstOrDefault(c => c.Name.Trim().ToUpper() == productCreate.ProductDto.Name.Trim().ToUpper());
            if (product != null)
            {
                output.Success = false;
                output.Error = _const.UnprocessableEntityCode;
                output.Message = "Product already exists";
                return output;
            }

            var productMap = _mapper.Map<Product>(productCreate.ProductDto);
            if (!_productRepository.CreateProduct(productCreate.CategoryId, productMap))
            {
                output.Success = false;
                output.Error = _const.InternalServerError;
                output.Message = "Something went wrong while saving";
                return output;
            }

            output.Success = true;
            output.Data = _mapper.Map<ProductDto>(productMap);
            return output;
        }

        public Output Put(ProductDto product) 
        {
            var output = new Output();
            var productUpdate = _productRepository.GetProduct(product.Id);
            if (productUpdate == null)
            {
                output.Success = false;
                output.Error = _const.NotFoundCode;
                output.Message = "Not found productId";
                return output;
            }

            var productMap = _mapper.Map<Product>(product);
            if (!_productRepository.UpdateProduct(productUpdate, productMap))
            {
                output.Success = false;
                output.Error = _const.InternalServerError;
                output.Message = "Something went wrong while saving";
                return output;
            }

            output.Success = true;
            output.Data = _mapper.Map<ProductDto>(productMap);
            return output;
        }

        public Output Delete(int id)
        {
            var output = new Output();
            var product = _productRepository.GetProduct(id);
            if (product == null)
            {
                output.Success = false;
                output.Error = _const.NotFoundCode;
                output.Message = "Not found productId";
                return output;
            }

            if (!_productRepository.DeleteProduct(product))
            {
                output.Success = false;
                output.Error = _const.InternalServerError;
                output.Message = "Something went wrong while saving";
                return output;
            }

            output.Success = true;
            output.Data = _mapper.Map<ProductDto>(product);
            return output;
        }
    }
}
