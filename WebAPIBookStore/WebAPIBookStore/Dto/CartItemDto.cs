using System.ComponentModel.DataAnnotations;
using WebAPIBookStore.Models;

namespace WebAPIBookStore.Dto
{
    public class CartItemDto
    {
        public int Id { get; set; }

        public int QuantityOfProduct { get; set; }

        [MaxLength(10)]
        public string Status { get; set; } = null!;

        public int? OrderId { get; set; }

        public int ProductId { get; set; }

        public int UserId { get; set; }
    }
}
