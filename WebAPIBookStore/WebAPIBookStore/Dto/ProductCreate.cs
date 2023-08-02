namespace WebAPIBookStore.Dto
{
    public class ProductCreate
    {
        public int categoryId { get; set; }
        public ProductDto ProductDto { get; set; } = null!;
    }
}
