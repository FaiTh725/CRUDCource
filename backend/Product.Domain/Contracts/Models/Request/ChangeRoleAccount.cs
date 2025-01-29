
namespace Product.Domain.Contracts.Models.Request
{
    public class ChangeRoleAccount
    {
        public string Email { get; set; }

        public string NewRole { get; set; }
    }
}
