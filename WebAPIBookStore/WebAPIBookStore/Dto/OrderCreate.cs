namespace WebAPIBookStore.Dto
{
    public class OrderCreate
    {
        public OrderDto OrderDto { get; set; } = null!;

        public List<int> CartItemIds { get; set; } = null!;
    }
}
