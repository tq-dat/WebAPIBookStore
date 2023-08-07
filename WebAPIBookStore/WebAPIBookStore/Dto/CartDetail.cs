namespace WebAPIBookStore.Dto
{
    public class CartDetail
    {
        public string Name { get; set; } = null!;
        public int QuantityOfProduct { get; set; }
        public double Price { get; set; }
    }
}
