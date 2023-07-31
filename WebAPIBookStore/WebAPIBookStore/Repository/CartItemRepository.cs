using Microsoft.EntityFrameworkCore;
using WebAPIBookStore.Data;
using WebAPIBookStore.Interfaces;
using WebAPIBookStore.Models;
using ZstdSharp.Unsafe;

namespace WebAPIBookStore.Repository
{
    public class CartItemRepository : ICartItemRepository
    {
        private readonly DataContext _context;

        public CartItemRepository(DataContext context) 
        {
            _context = context;
        }

        public bool CartItemExists(int id)
        {
            return _context.CartItems.Any(p => p.Id == id);
        }

        public bool CreateCartItem(int productId, int userId, CartItem cartItem)
        {
            var product = _context.Products.FirstOrDefault(p => p.Id == productId);
            var user = _context.Users.FirstOrDefault(p => p.Id == userId);
            
            cartItem.Product = product;
            cartItem.User = user;
     
            _context.Add(cartItem);
            return Save();
        }

        public bool DeleteCartItem(CartItem deleteCartItem)
        {
            _context.Remove(deleteCartItem);
            return Save();
        }

        public CartItem? GetCartItem(int id)
        {
            return _context.CartItems.FirstOrDefault(p => p.Id == id);
        }

        public Object GetCartItemByUserId(int userId)
        {
            var cartItems = _context.CartItems.Where(c => c.UserId == userId && c.Status == "UnPaid").ToList();
            var kq = cartItems.Join(_context.Products, c => c.ProductId, p => p.Id, (c, p) => {
                return new {
                    p.Name,
                    c.QuantityOfProduct,
                    p.Price
                };
            });

            return kq;
        }

        public Object GetCartItemByOrderId(int orderId)
        {
            var cartItems = _context.CartItems.Where(c => c.OrderId == orderId && c.Status == "Paid").ToList();
            var kq = cartItems.Join(_context.Products, c => c.ProductId, p => p.Id, (c, p) => {
                return new {
                    p.Name,
                    c.QuantityOfProduct,
                    p.Price
                };
            });

            return kq;
        }

        public ICollection<CartItem> GetCartItems()
        {
            return _context.CartItems.ToList();
        }

        public bool Save()
        {
            var saved = _context.SaveChanges();
            return saved > 0 ? true : false;
        }

        public bool UpdateCartItem(CartItem cartItem, CartItem cartItemUpdate)
        {
            cartItemUpdate.QuantityOfProduct = cartItem.QuantityOfProduct;
            cartItemUpdate.Status = cartItem.Status;
            _context.Update(cartItemUpdate);
            return Save();
        }
    }
}
