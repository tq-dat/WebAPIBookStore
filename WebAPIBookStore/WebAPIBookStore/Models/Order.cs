using System.ComponentModel.DataAnnotations;

namespace WebAPIBookStore.Models;

public class Order
{
    public int Id { get; set; }

    public DateTime DateOrder { get; set; }

    public int UserId { get; set; }

    public int? UserAdminId { get; set; }

    [MaxLength(10)]
    public string Status { get; set; } = null!;

    public User User { get; set; } = null!;

    public ICollection<CartItem>? CartItems { get; set; }
}
