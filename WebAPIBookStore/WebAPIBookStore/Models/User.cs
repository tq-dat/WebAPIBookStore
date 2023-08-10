using System.ComponentModel.DataAnnotations;
using WebAPIBookStore.Enum;

namespace WebAPIBookStore.Models;

public class User
{
    public int Id { get; set; }

    [MaxLength(255)]
    public string FullName { get; set; } = null!;

    [MaxLength(255)]
    public string UserName { get; set; } = null!;

    public string Password { get; set; } = null!;

    public string Email { get; set; } = null!;

    [MaxLength(255)]
    public string Address { get; set; } = null!;

    public Role Role { get; set; }

    public ICollection<CartItem>? CartItems { get; set; }

    public ICollection<Order>? Orders { get; set; }
}
