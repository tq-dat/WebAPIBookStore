using AutoMapper;
using WebAPIBookStore.Consts;
using WebAPIBookStore.Dto;
using WebAPIBookStore.Interfaces;
using WebAPIBookStore.Models;

namespace WebAPIBookStore.UseCase
{
    public class CategoryUseCase
    {
        private readonly ICategoryRepository _categoryRepository;
        private readonly IMapper _mapper;

        public CategoryUseCase(
            ICategoryRepository categoryRepository,
            IMapper mapper)
        {
            _categoryRepository = categoryRepository;
            _mapper = mapper;
        }

        public Output Get()
        {
            var output = new Output();
            var categories = _categoryRepository.GetCategories();
            if (!categories.Any())
            {
                output.Success = false;
                output.Error = StatusCodeAPI.NotFound;
                output.Message = "Not found categories";
            }

            output.Success = true;
            output.Data = _mapper.Map<List<CategoryDto>>(categories);
            return output;
        }

        public Output GetById(int id)
        {
            var output = new Output();
            var category = _categoryRepository.GetCategory(id);
            if (category == null)
            {
                output.Success = false;
                output.Error = StatusCodeAPI.NotFound;
                output.Message = "Not found category";
            }

            output.Success = true;
            output.Data = _mapper.Map<CategoryDto>(category);
            return output;
        }

        public Output Post(CategoryDto categoryDto)
        {
            var output = new Output();
            var category = _categoryRepository.GetCategories().FirstOrDefault(c => c.Name.Trim().ToUpper() == categoryDto.Name.Trim().ToUpper());
            if (category != null)
            {
                output.Success = false;
                output.Error = StatusCodeAPI.UnprocessableEntity;
                output.Message = "Category already exists";
                return output;
            }

            var categoryMap = _mapper.Map<Category>(categoryDto);
            if (!_categoryRepository.CreateCategory(categoryMap))
            {
                output.Success = false;
                output.Error = StatusCodeAPI.InternalServer;
                output.Message = "Something went wrong while saving";
                return output;
            }

            output.Success = true;
            output.Data = _mapper.Map<CategoryDto>(categoryMap);
            return output;
        }

        public Output Put(CategoryDto categoryDto)
        {
            var output = new Output();
            var category = _categoryRepository.GetCategory(categoryDto.Id);
            if (category == null)
            {
                output.Success = false;
                output.Error = StatusCodeAPI.NotFound;
                output.Message = "Not found category";
                return output;
            }

            if (!_categoryRepository.UpdateCategory(category,categoryDto.Name))
            {
                output.Success = false;
                output.Error = StatusCodeAPI.InternalServer;
                output.Message = "Something went wrong while saving";
                return output;
            }

            output.Success = true;
            output.Data = category;
            return output;
        }

        public Output Delete(int id)
        {
            var output = new Output();
            var deleteCategory = _categoryRepository.GetCategory(id);
            if (deleteCategory == null)
            {
                output.Success = false;
                output.Error = StatusCodeAPI.NotFound;
                output.Message = "Not found category";
                return output;
            }


            if (!_categoryRepository.DeleteCategory(deleteCategory))
            {
                output.Success = false;
                output.Error = StatusCodeAPI.InternalServer;
                output.Message = "Something went wrong while saving";
                return output;
            }

            output.Success = true;
            output.Data = _mapper.Map<CategoryDto>(deleteCategory);
            return output;
        }
    }
}
