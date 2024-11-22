namespace Authorize.Contracts.User
{
    public class CreateUser
    {
        public string Password { get; set; } = string.Empty;

        public string Email { get; set; } = string.Empty;
    }
}
