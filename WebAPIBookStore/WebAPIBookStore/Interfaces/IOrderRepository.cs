using Microsoft.EntityFrameworkCore.Storage.ValueConversion.Internal;
using WebAPIBookStore.Models;

namespace WebAPIBookStore.Interfaces
{
    public interface IOrderRepository
    {
        public ICollection<Order> GetOrders();
        public ICollection<Order> GetOrderByStatus(string status);
        public ICollection<Order> GetOrderByUserId(int userId);
        public Order? GetOrder(int id);
        public bool CreateOrder(List<CartItem> cartItems, Order order, int userId);
        public bool UpdateOrder(Order orderUpdate, string status, int manageId);
        public bool Save();
        public bool OrderExists(int id);
    }
}