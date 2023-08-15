using System.ComponentModel.DataAnnotations;
using WebAPIBookStore.Enum;

namespace WebAPIBookStore.Dto
{
    public class OrderDto
    {
        public int Id { get; set; }

        public DateTime DateOrder { get; set; }

        public int? UserAdminId { get; set; }

        public OrderStatus Status { get; set; }
    }
}
