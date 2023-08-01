using WebAPIBookStore.Data;
using WebAPIBookStore.Interfaces;
using WebAPIBookStore.Models;

namespace WebAPIBookStore.Repository
{
    public class OrderRepository : IOrderRepository
    {
        private readonly DataContext _context;

        public OrderRepository(DataContext context) {
            _context = context;
        }

        public bool CreateOrder(List<CartItem> cartItems, Order order, int userId)
        {
            foreach (CartItem x in cartItems)
            {
                x.Order = order;
                x.Status = "Paid";
                x.Status = "Wait";
                _context.Update(x);
            }

            order.UserId = userId;
            _context.Add(order);
            return Save();
        }

        public Order? GetOrder(int id)
        {
           return _context.Orders.FirstOrDefault(p => p.Id == id);
        }

        public ICollection<Order> GetOrderByStatus(string status)
        {
            var orders = _context.Orders.Where(p => p.Status.Contains(status)).ToList();
            return orders;
        }

        public ICollection<Order> GetOrderByUserId(int userId)
        {
            return _context.Orders.Where(p => p.UserId == userId).ToList();
        }

        public ICollection<Order> GetOrders()
        {
            return _context.Orders.ToList();
        }

        public bool OrderExists(int id)
        {
            return _context.Orders.Any(p => p.Id == id);
        }

        public bool Save()
        {
            var saved = _context.SaveChanges();
            if (saved > 0)
                return true;
            return false;
        }

        public bool UpdateOrder(Order orderUpdate, string status, int manageId)
        {
            orderUpdate.Status = status;
            orderUpdate.UserAdminId = manageId;
            _context.Update(orderUpdate);
            return Save();
        }
    }
}
