using System.ComponentModel.DataAnnotations;

namespace WebAPIBookStore.Dto
{
    public class UserLogin
    {
        [MaxLength(255)]
        public string UserName { get; set; } = null!;

        public string Password { get; set; } = null!;

        public string Role { get; set; } = null!;
    }
}
