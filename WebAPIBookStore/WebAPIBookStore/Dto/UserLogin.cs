using System.ComponentModel.DataAnnotations;

namespace WebAPIBookStore.Dto
{
    public class UserLogin
    {
        [MaxLength(255)]
        public string UserName { get; set; }

        public string Password { get; set; }

        public string Role { get; set; }
    }
}
