namespace WebAPIBookStore.Input
{
    public class UpdateProductInput
    {
        public int Id { get; set; }

        public string Name { get; set; } = null!;

        public string Description { get; set; } = null!;

        public string Author { get; set; } = null!;

        public double Price { get; set; }

        public int PublishYear { get; set; }

        public List<string> ImageURL { get; set; } = null!;
    }
}
