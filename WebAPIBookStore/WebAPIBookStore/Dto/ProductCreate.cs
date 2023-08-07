namespace WebAPIBookStore.Dto
{
    public class ProductCreate
    {
        public int CategoryId { get; set; }
        public ProductDto ProductDto { get; set; } = null!;
    }
}
