using WebAPIBookStore.Enum;
using WebAPIBookStore.Models;

namespace WebAPIBookStore.Interfaces
{
    public interface IOrderRepository
    {
        public ICollection<Order> GetOrders();
        public ICollection<Order> GetOrderByStatus(OrderStatus status);
        public ICollection<Order> GetOrderByUserId(int userId);
        public Order? GetOrder(int id);
        public bool CreateOrder(List<CartItem> cartItems, Order order, int userId);
        public bool UpdateOrder(Order orderUpdate, OrderStatus status, int manageId);
        public bool Save();
        public bool OrderExists(int id);
    }
}