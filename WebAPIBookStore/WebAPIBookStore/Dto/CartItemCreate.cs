using System.ComponentModel.DataAnnotations;
using WebAPIBookStore.Enum;

namespace WebAPIBookStore.Dto
{
    public class CartItemCreate
    {
        public int QuantityOfProduct { get; set; }

        public CartItemStatus Status { get; set; }
    }
}
