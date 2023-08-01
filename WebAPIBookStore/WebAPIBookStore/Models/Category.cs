using System.ComponentModel.DataAnnotations;

namespace WebAPIBookStore.Models;

public class Category
{
    public int Id { get; set; }

    [MaxLength(100)]
    public string Name { get; set; } = null!;

    public ICollection<ProductCategory>? ProductCategories;

}
