using WebAPIBookStore.Data;
using WebAPIBookStore.Interfaces;
using WebAPIBookStore.Models;
using ZstdSharp.Unsafe;

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

        public bool DeleteCategory(int id)
        {
            var deleteCategory = _context.Categories.FirstOrDefault(p => p.Id == id);
            var productcategories = _context.ProductCategories.Where(p => p.CategoryId == id).ToList();
            foreach(var pc in productcategories) 
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

        public Category GetCategory(int id)
        {
            return _context.Categories.FirstOrDefault(c => c.Id == id);
        }

        public ICollection<Product> GetProductsByCategory(int categoryId)
        {
            return _context.ProductCategories.Where(pc => pc.CategoryId == categoryId).Select(p => p.Product).ToList();
        }

        public bool Save()
        {
            var saved = _context.SaveChanges();
            return saved > 0 ? true : false;
        }

        public bool UpdateCategory(int categoryId, string Name)
        {
            var updateCategory = _context.Categories.FirstOrDefault(p => p.Id == categoryId);
            updateCategory.Name = Name;
            _context.Update(updateCategory);
            return Save();
        }
    }
}
