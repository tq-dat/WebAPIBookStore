using System.ComponentModel.DataAnnotations;
using WebAPIBookStore.Enum;

namespace WebAPIBookStore.Dto
{
    public class UserLogin
    {
        [MaxLength(255)]
        public string UserName { get; set; } = null!;

        public string Password { get; set; } = null!;

        public Role Role { get; set; }
    }
}
