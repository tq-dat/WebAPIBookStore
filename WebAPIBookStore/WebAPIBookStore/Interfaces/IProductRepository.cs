using Microsoft.EntityFrameworkCore;
using WebAPIBookStore.Models;

namespace WebAPIBookStore.Interfaces
{
    public interface IProductRepository
    {
        ICollection<Product> GetProducts();
        Product GetProduct(int id);
        ICollection<Product> GetProductsByName(string name);
        public bool ProductExists(int prodId);
        bool CreateProduct( int categoryId, Product product);
        bool UpdateProduct( int productId, Product product);
        bool DeleteProduct( int productId);
        bool Save();
    }
}
