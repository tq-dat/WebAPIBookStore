namespace WebAPIBookStore.Dto
{
    public class OrderItem
    {
        public int ProductId { get; set; }

        public string ProductName { get; set; } = null!;

        public int QuantityOfProduct { get; set; }

        public double PriceOfProduct { get; set; }
    }
}
