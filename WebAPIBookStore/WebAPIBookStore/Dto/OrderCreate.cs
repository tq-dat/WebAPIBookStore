namespace WebAPIBookStore.Dto
{
    public class OrderCreate
    {
        public OrderDto orderDto { get; set; } = null!;

        public List<int> cartItemIds { get; set; } = null!;
    }
}
