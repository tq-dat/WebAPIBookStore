using WebAPIBookStore.Enum;

namespace WebAPIBookStore.Dto
{
    public class OrderOutput
    {
        public int Id { get; set; }

        public DateTime DateOrder { get; set; }

        public int UserId { get; set; }

        public string UserName { get; set; } = null!;

        public int? AdminId { get; set; }

        public string? AdminName { get; set; }

        public OrderStatus Status { get; set; }

        public List<OrderItem>? Items { get; set; }
    }
}
