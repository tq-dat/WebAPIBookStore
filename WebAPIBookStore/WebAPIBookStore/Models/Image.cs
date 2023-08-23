namespace WebAPIBookStore.Models
{
    public class Image
    {
        public int Id { get; set; }

        public string URL { get; set; } = null!;

        public int ProductId { get; set; }

        public Product? Product { get; set; }
    }
}
