using System.ComponentModel.DataAnnotations;
using WebAPIBookStore.Enum;

namespace WebAPIBookStore.Models;

public class Order
{
    public int Id { get; set; }

    public DateTime DateOrder { get; set; }

    public int UserId { get; set; }

    public int? UserAdminId { get; set; }

    public OrderStatus Status { get; set; }

    public User User { get; set; } = null!;

    public ICollection<CartItem>? CartItems { get; set; }
}
