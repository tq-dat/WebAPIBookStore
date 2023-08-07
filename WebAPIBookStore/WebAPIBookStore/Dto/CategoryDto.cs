using System.ComponentModel.DataAnnotations;

namespace WebAPIBookStore.Dto
{
    public class CategoryDto
    {
        public int Id { get; set; }

        [MaxLength(100)]
        public string Name { get; set; } = null!;
    }
}
