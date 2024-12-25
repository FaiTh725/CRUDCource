namespace Authorize.Contracts.User
{
    public class UserRegister : UserLogin
    {
        public string UserName { get; set; } = string.Empty;
    }
}
