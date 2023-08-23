using WebAPIBookStore.Enum;

namespace WebAPIBookStore.Input
{
    public class UpdateUserInput
    {
        public int Id { get; set; }

        public string UserName { get; set; } = null!;

        public string Password { get; set; } = null!;

        public string Address { get; set; } = null!;

        public Role Role { get; set; }

        public bool EmailVerify { get; set; }
    }
}
