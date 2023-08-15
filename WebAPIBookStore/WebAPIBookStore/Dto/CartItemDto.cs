using WebAPIBookStore.Enum;

namespace WebAPIBookStore.Dto
{
    public class CartItemDto
    {
        public int Id { get; set; }

        public int QuantityOfProduct { get; set; }

        public CartItemStatus Status { get; set; }

        public int? OrderId { get; set; }

        public int ProductId { get; set; }

        public int UserId { get; set; }
    }
}
