using System.ComponentModel.DataAnnotations;
using WebAPIBookStore.Enum;

namespace WebAPIBookStore.Result
{
    public class UserOutput
    {
        public int Id { get; set; }

        [MaxLength(255)]
        public string Username { get; set; } = null!;

        public string Password { get; set; } = null!;

        public string Email { get; set; } = null!;

        [MaxLength(255)]
        public string Address { get; set; } = null!;

        public bool EmailVerify { get; set; }

        public Role Role { get; set; }
    }
}
