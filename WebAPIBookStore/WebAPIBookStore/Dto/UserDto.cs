using System.ComponentModel.DataAnnotations;
using WebAPIBookStore.Enum;

namespace WebAPIBookStore.Dto
{
    public class UserDto
    {
        public int Id { get; set; }

        [MaxLength(255)]
        public string FullName { get; set; } = null!;

        [MaxLength(255)]
        public string UserName { get; set; } = null!;

        public string Password { get; set; } = null!;

        public string Email { get; set; } = null!;

        [MaxLength(255)]
        public string Address { get; set; } = null!;

        public Role Role { get; set; }
    }
}
