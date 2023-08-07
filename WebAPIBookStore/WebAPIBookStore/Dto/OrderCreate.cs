namespace WebAPIBookStore.Dto
{
    public class OrderCreate
    {
        public int UserId { get; set; }
        public OrderDto OrderDto { get; set; } = null!;

        public List<int> CartItemIds { get; set; } = null!;
    }
}
