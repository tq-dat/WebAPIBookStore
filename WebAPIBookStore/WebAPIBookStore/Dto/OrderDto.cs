using System.ComponentModel.DataAnnotations;

namespace WebAPIBookStore.Dto
{
    public class OrderDto
    {
        public int Id { get; set; }

        public DateTime DateOrder { get; set; }

        public int? UserAdminId { get; set; }

        [MaxLength(10)]
        public string Status { get; set; } = null!;
    }
}
