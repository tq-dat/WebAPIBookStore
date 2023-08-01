using WebAPIBookStore.Models;

namespace WebAPIBookStore.Interfaces
{
    public interface ICategoryRepository
    {
        public ICollection<Category> GetCategories();
        public Category? GetCategory(int id);
        public bool UpdateCategory(Category category, string name);
        public bool DeleteCategory(Category deleteCategory);
        public bool CategoryExists(int id);
        public bool CreateCategory(Category category);
        public bool Save();
    }
}
