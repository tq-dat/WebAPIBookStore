﻿using WebAPIBookStore.Models;

namespace WebAPIBookStore.Interfaces
{
    public interface ICartItemRepository
    {
        public ICollection<CartItem> GetCartItems();
        public CartItem? GetCartItem(int id);
        public ICollection<CartItem> GetCartItemByUserId(int userId);
        public ICollection<CartItem> GetCartItemByOrderId(int orderId);
        public bool UpdateCartItem(CartItem cartItem, CartItem cartItemUpdate);
        public bool DeleteCartItem(CartItem deleteCartItem);
        public bool CreateCartItem(int productId, int userId, CartItem cartItem);
        public bool CartItemExists(int id);
        public bool Save();
    }
}
