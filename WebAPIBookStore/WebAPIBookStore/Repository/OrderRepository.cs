using Microsoft.AspNetCore.Mvc;
using WebAPIBookStore.Data;
using WebAPIBookStore.Enum;
using WebAPIBookStore.Interfaces;
using WebAPIBookStore.Models;

namespace WebAPIBookStore.Repository
{
    public class OrderRepository : IOrderRepository
    {
        private readonly DataContext _context;

        public OrderRepository(DataContext context)
        {
            _context = context;
        }

        public bool CreateOrder(List<CartItem> cartItems, Order order, int userId)
        {
            foreach (CartItem x in cartItems)
            {
                x.Order = order;
                x.Status = CartItemStatus.Paid;
                _context.Update(x);
            }

            order.UserId = userId;
            order.Status = OrderStatus.Wait;
            _context.Add(order);
            return Save();
        }

        public Order? GetOrder(int id)
        {
            return _context.Orders.FirstOrDefault(p => p.Id == id);
        }

        public ICollection<Order> GetOrderByStatus(OrderStatus status)
        {
            var orders = _context.Orders.Where(p => p.Status == status).ToList();
            return orders;
        }

        public ICollection<Order> GetOrderByUserId(int userId)
        {
            return _context.Orders.Where(p => p.UserId == userId).ToList();
        }

        public Object GetOrders()
        {
            var orders = _context.Orders.ToList();
            var kq = orders.Join(_context.Users, o => o.UserId, u => u.Id, (o, u) =>
                new {
                    Id = o.Id,
                    Status = o.Status,
                    DateOrder = o.DateOrder,
                    UserName = u.UserName,
                    UserEmail = u.Email,
                    UserAddress = u.Address,
                    CartItems = _context.CartItems.Where(c => c.OrderId == o.Id).Join(_context.Products, c => c.ProductId, p => p.Id, (c, p) =>
                        new {
                            ProductName = p.Name,
                            QuantityOfProduct = c.QuantityOfProduct,
                            CartItemTotal = p.Price * c.QuantityOfProduct
                        }).ToList(),
                    Total = _context.CartItems.Where(c => c.OrderId == o.Id).Join(_context.Products, c => c.ProductId, p => p.Id, (c, p) =>
                        new {
                            CartItemTotal = p.Price * c.QuantityOfProduct
                        }).Sum(t => t.CartItemTotal)
                }).ToList();

            return kq;
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

        public bool UpdateOrder(Order orderUpdate, OrderStatus status, int manageId)
        {
            orderUpdate.Status = status;
            orderUpdate.UserAdminId = manageId;
            _context.Update(orderUpdate);
            return Save();
        }
    }
}
