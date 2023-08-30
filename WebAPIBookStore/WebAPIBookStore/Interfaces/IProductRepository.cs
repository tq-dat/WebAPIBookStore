using WebAPIBookStore.Input;
using WebAPIBookStore.Models;
using WebAPIBookStore.Result;

namespace WebAPIBookStore.Interfaces
{
    public interface IProductRepository
    {
        public ICollection<ProductOutput> GetProducts();
        public ProductOutput? GetProductReturnProductOutput(int id);
        public Product? GetProductReturnProduct(int id);
        public ICollection<ProductOutput> GetProductsByCategory(int categoryId);
        public bool ProductExists(int prodId);
        public bool CreateProduct(AddProductInput addProductInput);
        public bool UpdateProduct(Product productUpdate, UpdateProductInput updateProductInput);
        public bool DeleteProduct(Product productDelete);
        public ICollection<ProductOutput>? SortBy(SortInput sortInput);
        public ICollection<ProductOutput> SearchByOption(string input, int? limit, int option);
        public bool Save();
    }
}
