    using System.ComponentModel.DataAnnotations;

namespace WebAPIBookStore.Result
{
    public class ProductOutput
    {
        public int Id { get; set; }

        [MaxLength(255)]
        public string Name { get; set; } = null!;

        [MaxLength(500)]
        public string Description { get; set; } = null!;

        [MaxLength(255)]
        public string Author { get; set; } = null!;

        public double Price { get; set; }

        public int PublishYear { get; set; }

        public List<string> ImageURL { get; set; } = null!;

        public List<string> CategoryNames { get; set; } = null!;
    }
}
