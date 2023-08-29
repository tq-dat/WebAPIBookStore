using NuGet.Protocol.Core.Types;
using WebAPIBookStore.Data;
using WebAPIBookStore.Enum;
using WebAPIBookStore.Input;
using WebAPIBookStore.Interfaces;
using WebAPIBookStore.Models;
using WebAPIBookStore.Result;
using ZstdSharp.Unsafe;

namespace WebAPIBookStore.Repository
{
    public class ProductRepository : IProductRepository
    {
        private readonly DataContext _context;

        public ProductRepository(DataContext context)
        {
            _context = context;
        }
        public ProductOutput? GetProductReturnProductOutput(int id)
        {
            return toProductOutput(_context.Products.FirstOrDefault(p => p.Id == id));
        }

        public ICollection<ProductOutput> GetProducts()
        {
            return toProductOutputs(_context.Products.ToList());
        }

        public bool ProductExists(int prodId)
        {
            return _context.Products.Any(p => p.Id == prodId);
        }

        public bool CreateProduct(AddProductInput addProductInput)
        {
            var product = new Product
            {
                Name = addProductInput.Name,
                Description = addProductInput.Description,
                Author = addProductInput.Author,
                Price = addProductInput.Price,
                PublishYear = addProductInput.PublishYear
            };

            _context.Add(product);
            var images = addProductInput.ImageURL.Select(p => new Image { Product = product, URL = p });
            _context.Add(images);
            var productCategorys = addProductInput.CategoryIds.Select(p => new ProductCategory { Product = product, Category = _context.Categories.FirstOrDefault(c => c.Id == p) });
            _context.Add(productCategorys);
            return Save();
        }

        public bool Save()
        {
            var saved = _context.SaveChanges();
            if (saved > 0)
                return true;

            return false;
        }

        public bool UpdateProduct(Product productUpdate, UpdateProductInput updateProductInput)
        {
            productUpdate.Name = updateProductInput.Name;
            productUpdate.Description = updateProductInput.Description;
            productUpdate.Author = updateProductInput.Author;
            productUpdate.Price = updateProductInput.Price;
            var images = _context.Images.Where(p => p.ProductId == updateProductInput.Id).ToList();
            _context.Remove(images);
            var newImages = updateProductInput.ImageURL.Select(_ => new Image { URL = _, ProductId = updateProductInput.Id });
            _context.Add(newImages);
            _context.Update(productUpdate);
            return Save();
        }

        public bool DeleteProduct(Product productDelete)
        {
            var productCategories = _context.ProductCategories.Where(p => p.ProductId == productDelete.Id).ToList();
            _context.Remove(productCategories);
            var cartItems = _context.CartItems.Where(p => p.ProductId == productDelete.Id && p.Status == CartItemStatus.UnPaid).ToList();
            _context.Remove(cartItems);
            var images = _context.Images.Where(p => p.ProductId == productDelete.Id).ToList();
            _context.Remove(images);
            productDelete.Name = "Deleted";
            _context.Update(productDelete);
            return Save();
        }

        public ICollection<ProductOutput> GetProductsByCategory(int categoryId)
        {
            var products = _context.ProductCategories.Where(pc => pc.CategoryId == categoryId).Join(_context.Products, pc => pc.ProductId, p => p.Id, (pc ,p) => p).ToList();
            return toProductOutputs(products);
        }

        public ICollection<ProductOutput>? SortBy(SortInput sortInput)
        {
            if (sortInput.Descending)
            {
                switch (sortInput.ByValue)
                {
                    case 1:
                        var products1 = _context.Products.OrderByDescending(p => p.Name).ToList();
                        return toProductOutputs(products1);

                    case 2:
                        var products2 = _context.Products.OrderByDescending(p => p.Price).ToList();
                        return toProductOutputs(products2);

                    case 3:
                        var products3 = _context.Products.OrderByDescending(p => p.PublishYear).ToList();
                        return toProductOutputs(products3);

                    default:
                        return null;
                }
            }
            else
            {
                switch (sortInput.ByValue)
                {
                    case 1:
                        var products1 = _context.Products.OrderBy(p => p.Name).ToList();
                        return toProductOutputs(products1);

                    case 2:
                        var products2 = _context.Products.OrderBy(p => p.Price).ToList();
                        return toProductOutputs(products2);

                    case 3:
                        var products3 = _context.Products.OrderBy(p => p.PublishYear).ToList();
                        return toProductOutputs(products3);

                    default:
                        return null;
                }
            }
        }
        
        public ICollection<ProductOutput> SearchByOption(string input, int? limit, int option)
        {
            if (limit.HasValue)
            {
                switch (option)
                {
                    case 1:
                        var productByAuthor = _context.Products.Take((int)limit).Where(p => p.Author.Contains(input)).ToList();
                        return toProductOutputs(productByAuthor);

                    case 2:
                        var productByYear = _context.Products.Take((int)limit).Where(p => p.PublishYear.ToString().Contains(input)).ToList();
                        return toProductOutputs(productByYear);

                    default:
                        var productByName = _context.Products.Take((int)limit).Where(p => p.Name.Contains(input)).ToList();
                        return toProductOutputs(productByName);
                }
            }
            else
            {
                switch (option)
                {
                    case 1:
                        var productByAuthor = _context.Products.Where(p => p.Author.Contains(input)).ToList();
                        return toProductOutputs(productByAuthor);

                    case 2:
                        var productByYear = _context.Products.Where(p => p.PublishYear.ToString().Contains(input)).ToList();
                        return toProductOutputs(productByYear);

                    default:
                        var productByName = _context.Products.Where(p => p.Name.Contains(input)).ToList();
                        return toProductOutputs(productByName);
                }
            }
        }

        public Product? GetProductReturnProduct(int id)
        {
            return _context.Products.FirstOrDefault(p => p.Id == id);
        }

        public ICollection<ProductOutput> toProductOutputs(ICollection<Product> products)
        {
            return products.Select(p => toProductOutput(p)).ToList();
        }

        public ProductOutput? toProductOutput(Product? product)
        {
            if (product == null) 
                return null;

            return new ProductOutput
            {
                Id = product.Id,
                Name = product.Name,
                Description = product.Description,
                Author = product.Author,
                Price = product.Price,
                PublishYear = product.PublishYear,
                ImageURL = _context.Images.Where(i => i.ProductId == product.Id).Select(i => i.URL).ToList(),
                CategoryNames = _context.ProductCategories.Where(pc => pc.ProductId == product.Id).Join(_context.Categories, pc => pc.CategoryId, c => c.Id, (pc, c) => c.Name).ToList()
            };
        }
    }
}
