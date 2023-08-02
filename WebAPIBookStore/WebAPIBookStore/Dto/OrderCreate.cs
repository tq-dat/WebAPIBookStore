namespace WebAPIBookStore.Dto
{
    public class OrderCreate
    {
        public int userId { get; set; }
        public OrderDto OrderDto { get; set; } = null!;

        public List<int> CartItemIds { get; set; } = null!;
    }
}
