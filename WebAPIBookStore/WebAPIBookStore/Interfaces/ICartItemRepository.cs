using WebAPIBookStore.Dto;
using WebAPIBookStore.Models;

namespace WebAPIBookStore.Interfaces
{
    public interface ICartItemRepository
    {
        public ICollection<CartItem> GetCartItems();
        public CartItem? GetCartItem(int id);
        public List<CartDetail> GetCartItemByUserId(int userId);
        public List<CartDetail> GetCartItemByOrderId(int orderId);
        public bool UpdateCartItem(CartItem cartItem, CartItem cartItemUpdate);
        public bool DeleteCartItem(CartItem deleteCartItem);
        public bool CreateCartItem(CartItem cartItem);
        public bool CartItemExists(int id);
        public bool Save();
    }
}
