using Microsoft.EntityFrameworkCore;
using WebAPIBookStore.Models;

namespace WebAPIBookStore.Interfaces
{
    public interface IProductRepository
    {
        public ICollection<Product> GetProducts();
        public Product? GetProduct(int id);
        public ICollection<Product> GetProductsByName(string name);
        public ICollection<Product?> GetProductsByCategory(int categoryId);
        public bool ProductExists(int prodId);
        public bool CreateProduct(int categoryId, Product product);
        public bool UpdateProduct(Product productUpdate, Product product);
        public bool DeleteProduct(Product productDelete);
        public bool Save();
    }
}
