using System.ComponentModel.DataAnnotations;
using WebAPIBookStore.Enum;

namespace WebAPIBookStore.Models;

public class CartItem
{
    public int Id { get; set; }

    public int QuantityOfProduct { get; set; }

    public CartItemStatus Status { get; set; }

    public int? OrderId { get; set; }

    public int ProductId { get; set; }

    public int UserId { get; set; }

    public Order? Order { get; set; }

    public Product? Product { get; set; }

    public User? User { get; set; }
}
