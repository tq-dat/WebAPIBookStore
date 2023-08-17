using AutoMapper;
using WebAPIBookStore.Dto;
using WebAPIBookStore.Interfaces;
using WebAPIBookStore.Models;
using WebAPIBookStore.Usecase;

namespace WebAPIBookStore.UseCase
{
    public class CategoryUseCase
    {
        private readonly ICategoryRepository _categoryRepository;
        private readonly IMapper _mapper;
        private readonly IUseCaseOutput _useCaseOutput;

        public CategoryUseCase(
            ICategoryRepository categoryRepository,
            IMapper mapper,
            IUseCaseOutput useCaseOutput)
        {
            _categoryRepository = categoryRepository;
            _mapper = mapper;
            _useCaseOutput = useCaseOutput;
        }

        public Output Get()
        {
            var categories = _categoryRepository.GetCategories();
            if (!categories.Any())
            {
                return _useCaseOutput.NotFound("Not found categories");
            }

            return _useCaseOutput.Success(_mapper.Map<List<CategoryDto>>(categories));
        }

        public Output GetById(int id)
        {
            var category = _categoryRepository.GetCategory(id);
            if (category == null)
            {
                return _useCaseOutput.NotFound("Not found categoryId");
            }

            return _useCaseOutput.Success(_mapper.Map<CategoryDto>(category));
        }

        public Output Post(CategoryDto categoryDto)
        {
            var category = _categoryRepository.GetCategories().FirstOrDefault(c => c.Name.Trim().ToUpper() == categoryDto.Name.Trim().ToUpper());
            if (category != null)
            {
                return _useCaseOutput.NotFound("Category already exists");
            }

            var categoryMap = _mapper.Map<Category>(categoryDto);
            if (!_categoryRepository.CreateCategory(categoryMap))
            {
                return _useCaseOutput.InternalServer("Something went wrong while saving");
            }

            return _useCaseOutput.Success(_mapper.Map<CategoryDto>(categoryMap));
        }

        public Output Put(CategoryDto categoryDto)
        {
            var category = _categoryRepository.GetCategory(categoryDto.Id);
            if (category == null)
            {
                return _useCaseOutput.NotFound("Not found category");
            }

            if (!_categoryRepository.UpdateCategory(category,categoryDto.Name))
            {
                return _useCaseOutput.InternalServer("Something went wrong while saving");
            }

            return _useCaseOutput.Success(category);
        }

        public Output Delete(int id)
        {
            var deleteCategory = _categoryRepository.GetCategory(id);
            if (deleteCategory == null)
            {
                return _useCaseOutput.NotFound("Not found category");
            }

            if (!_categoryRepository.DeleteCategory(deleteCategory))
            {
                return _useCaseOutput.InternalServer("Something went wrong while saving");
            }

            return _useCaseOutput.Success(_mapper.Map<CategoryDto>(deleteCategory));
        }
    }
}
