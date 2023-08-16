using WebAPIBookStore.Data;
using WebAPIBookStore.Dto;
using WebAPIBookStore.Enum;
using WebAPIBookStore.Interfaces;
using WebAPIBookStore.Models;

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

        public bool CreateCartItem(CartItem cartItem)
        {
            _context.Add(cartItem);
            return Save();
        }

        public bool DeleteCartItem(CartItem deleteCartItem)
        {
            if (deleteCartItem.Status == CartItemStatus.Paid)
                return false;

            _context.Remove(deleteCartItem);
            return Save();
        }

        public CartItem? GetCartItem(int id)
        {
            return _context.CartItems.FirstOrDefault(p => p.Id == id);
        }

        public List<CartDetail> GetCartItemByUserId(int userId)
        {
            var cartItems = _context.CartItems.Where(c => c.UserId == userId && c.Status == CartItemStatus.UnPaid).ToList();
            var kq = cartItems.Join(_context.Products, c => c.ProductId, p => p.Id, (c, p) => 
                new CartDetail{
                Name = p.Name,
                QuantityOfProduct = c.QuantityOfProduct,
                Price = p.Price
            }).ToList();

            return kq;
        }

        public List<CartDetail> GetCartItemByOrderId(int orderId)
        {
            var cartItems = _context.CartItems.Where(c => c.OrderId == orderId && c.Status == CartItemStatus.Paid).ToList();
            var kq = cartItems.Join(_context.Products, c => c.ProductId, p => p.Id, (c, p) =>
                new CartDetail{
                Name = p.Name,
                QuantityOfProduct = c.QuantityOfProduct,
                Price = p.Price
            }).ToList();

            return kq;
        }

        public ICollection<CartItem> GetCartItems()
        {
            return _context.CartItems.ToList();
        }

        public bool Save()
        {
            var saved = _context.SaveChanges();
            if (saved > 0)
                return true;
            
            return false;
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
