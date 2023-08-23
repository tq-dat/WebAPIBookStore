namespace WebAPIBookStore.Input
{
    public class SignUpInput
    {
        public string Email { get; set; } = null!;

        public string UserName { get; set; } = null!;

        public string Password { get; set; } = null!;

        public string Address { get; set; } = null!;
    }
}
