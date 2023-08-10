using WebAPIBookStore.Models;

namespace WebAPIBookStore.Interfaces
{
    public interface IOrderRepository
    {
        public Object GetOrders();
        public ICollection<Order> GetOrderByStatus(string status);
        public ICollection<Order> GetOrderByUserId(int userId);
        public Order? GetOrder(int id);
        public bool CreateOrder(List<CartItem> cartItems, Order order, int userId);
        public bool UpdateOrder(Order orderUpdate, string status, int manageId);
        public bool Save();
        public bool OrderExists(int id);
    }
}