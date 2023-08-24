using WebAPIBookStore.Data;
using WebAPIBookStore.Enum;
using WebAPIBookStore.Input;
using WebAPIBookStore.Interfaces;
using WebAPIBookStore.Models;
using WebAPIBookStore.Result;

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
            return _context.Products.Select(p => new ProductOutput
            {
                Id = p.Id,
                Name = p.Name,
                Description = p.Description,
                Author = p.Author,
                Price = p.Price,
                PublishYear = p.PublishYear,
                ImageURL = _context.Images.Where(i => i.ProductId == p.Id).Select(i => i.URL).ToList(),
                CategoryNames = _context.ProductCategories.Where(pc => pc.ProductId == p.Id).Join(_context.Categories, pc => pc.CategoryId, c => c.Id, (pc, c) => c.Name).ToList()
            }).FirstOrDefault(p => p.Id == id);
        }

        public ICollection<ProductOutput> GetProducts()
        {
            var products = _context.Products.Select(p => new ProductOutput
            {
                Id = p.Id,
                Name = p.Name,
                Description = p.Description,
                Author = p.Author,
                Price = p.Price,
                PublishYear = p.PublishYear,
                ImageURL = _context.Images.Where(i => i.ProductId == p.Id).Select(i => i.URL).ToList(),
                CategoryNames = _context.ProductCategories.Where(pc => pc.ProductId == p.Id).Join(_context.Categories, pc => pc.CategoryId, c => c.Id, (pc, c) => c.Name).ToList()
            }).ToList();

            return products;
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
            foreach (int id in addProductInput.CategoryIds)
            {
                var category = _context.Categories.FirstOrDefault(p => p.Id == id);
                var productCategory = new ProductCategory()
                {
                    Product = product,
                    Category = category
                };
                _context.Add(productCategory);
            }

            foreach (string Url in addProductInput.ImageURL)
            {
                var image = new Image
                {
                    Product = product,
                    URL = Url
                };
                _context.Add(image);
            }

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
            foreach (var image in images)
            {
                _context.Remove(image);
            }
            foreach (var url in updateProductInput.ImageURL)
            {
                var image = new Image
                {
                    URL = url,
                    ProductId = updateProductInput.Id
                };
                _context.Add(image);
            }
            _context.Update(productUpdate);
            return Save();
        }

        public bool DeleteProduct(Product productDelete)
        {
            var productCategories = _context.ProductCategories.Where(p => p.ProductId == productDelete.Id).ToList();
            foreach (var pc in productCategories)
            {
                _context.Remove(pc);
            }

            var cartItems = _context.CartItems.Where(p => p.ProductId == productDelete.Id && p.Status == CartItemStatus.UnPaid).ToList();
            foreach (var cartItem in cartItems)
            {
                _context.Remove(cartItem);
            }

            var images = _context.Images.Where(p => p.ProductId == productDelete.Id).ToList();
            foreach(var image in images)
            {
                _context.Remove(image);
            }

            productDelete.Name = "Deleted";
            _context.Update(productDelete);
            return Save();
        }

        public ICollection<ProductOutput> GetProductsByCategory(int categoryId)
        {
            var products = _context.ProductCategories.Where(pc => pc.CategoryId == categoryId).Join(_context.Products, pc => pc.ProductId, p => p.Id, (pc ,p) => p).ToList();
            var output = products.Select(p => new ProductOutput
            {
                Id = p.Id,
                Name = p.Name,
                Description = p.Description,
                Author = p.Author,
                Price = p.Price,
                PublishYear = p.PublishYear,
                ImageURL = _context.Images.Where(i => i.ProductId == p.Id).Select(i => i.URL).ToList(),
                CategoryNames = _context.ProductCategories.Where(pc => pc.ProductId == p.Id).Join(_context.Categories, pc => pc.CategoryId, c => c.Id, (pc, c) => c.Name).ToList()
            }).ToList();

            return output;
        }

        public ICollection<ProductOutput> SortUp(string value)
        {
            var products = _context.Products.OrderBy(p => value).Select(p => new ProductOutput
            {
                Id = p.Id,
                Name = p.Name,
                Description = p.Description,
                Author = p.Author,
                Price = p.Price,
                PublishYear = p.PublishYear,
                ImageURL = _context.Images.Where(i => i.ProductId == p.Id).Select(i => i.URL).ToList(),
                CategoryNames = _context.ProductCategories.Where(pc => pc.ProductId == p.Id).Join(_context.Categories, pc => pc.CategoryId, c => c.Id, (pc, c) => c.Name).ToList()
            }).ToList();

            return products;
        }

        public ICollection<ProductOutput> SortDown(string value)
        {
            var products = _context.Products.OrderByDescending(p => value).Select(p => new ProductOutput
            {
                Id = p.Id,
                Name = p.Name,
                Description = p.Description,
                Author = p.Author,
                Price = p.Price,
                PublishYear = p.PublishYear,
                ImageURL = _context.Images.Where(i => i.ProductId == p.Id).Select(i => i.URL).ToList(),
                CategoryNames = _context.ProductCategories.Where(pc => pc.ProductId == p.Id).Join(_context.Categories, pc => pc.CategoryId, c => c.Id, (pc, c) => c.Name).ToList()
            }).ToList();

            return products;
        }

        public ICollection<ProductOutput> Search(string value, int limit)
        {
            if (limit == 0)
            {
                var products = _context.Products.Where(
                    p => p.Name.Contains(value) ||
                    p.Author.Contains(value) ||
                    p.PublishYear.ToString().Contains(value)
                ).Select(p => new ProductOutput
                {
                    Id = p.Id,
                    Name = p.Name,
                    Description = p.Description,
                    Author = p.Author,
                    Price = p.Price,
                    PublishYear = p.PublishYear,
                    ImageURL = _context.Images.Where(i => i.ProductId == p.Id).Select(i => i.URL).ToList(),
                    CategoryNames = _context.ProductCategories.Where(pc => pc.ProductId == p.Id).Join(_context.Categories, pc => pc.CategoryId, c => c.Id, (pc, c) => c.Name).ToList()
                }).ToList();

                return products;
            }

            var productLimited = _context.Products.Where(
                p => p.Name.Contains(value) ||
                p.Author.Contains(value) ||
                p.PublishYear.ToString().Contains(value)
            ).Select(p => new ProductOutput
            {
                Id = p.Id,
                Name = p.Name,
                Description = p.Description,
                Author = p.Author,
                Price = p.Price,
                PublishYear = p.PublishYear,
                ImageURL = _context.Images.Where(i => i.ProductId == p.Id).Select(i => i.URL).ToList(),
                CategoryNames = _context.ProductCategories.Where(pc => pc.ProductId == p.Id).Join(_context.Categories, pc => pc.CategoryId, c => c.Id, (pc, c) => c.Name).ToList()
            }).Take(limit).ToList();

            return productLimited;
        }

        public Product? GetProductReturnProduct(int id)
        {
            return _context.Products.FirstOrDefault(p => p.Id == id);
        }
    }
}
