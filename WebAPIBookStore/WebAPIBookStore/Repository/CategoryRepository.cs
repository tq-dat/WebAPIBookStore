using WebAPIBookStore.Data;
using WebAPIBookStore.Interfaces;
using WebAPIBookStore.Models;

namespace WebAPIBookStore.Repository
{
    public class CategoryRepository : ICategoryRepository
    {
        private readonly DataContext _context;

        public CategoryRepository (DataContext context) 
        {
            _context = context;
        }

        public bool CategoryExists(int id)
        {
            return _context.Categories.Any(c => c.Id == id); 
        }

        public bool CreateCategory(Category category)
        {
            _context.Add(category);
            return Save();
        }

        public bool DeleteCategory(Category deleteCategory)
        {
            var productCategories = _context.ProductCategories.Where(p => p.CategoryId == deleteCategory.Id).ToList();
            foreach(var pc in productCategories) 
            {
                _context.Remove(pc);
            }

            _context.Remove(deleteCategory);
            return Save();
        }

        public ICollection<Category> GetCategories()
        {
            return _context.Categories.OrderBy(c => c.Id).ToList();
        }

        public Category? GetCategory(int id)
        {
            return _context.Categories.FirstOrDefault(c => c.Id == id);
        }

        public bool Save()
        {
            var saved = _context.SaveChanges();
            if (saved > 0)
                return true;
            
            return false;
        }

        public bool UpdateCategory(Category category, string name)
        {
            category.Name = name;
            _context.Update(category);
            return Save();
        }
    }
}
