using CSharpFunctionalExtensions;
using System.Text.RegularExpressions;

namespace Product.Domain.Entities
{
    public class ChangeAccountRoleRequest
    {
        private const int MAX_EMAIL_LENGTH = 30;

        public long Id { get; }

        public string Email { get; init; }

        public string NewRole { get; init; }

        public DateTime CreatedDate { get; init; }

        public bool? IsCommite { get; private set; }

        public bool IsReviewed { get; private set; }

        public ChangeAccountRoleRequest()
        {

        }

        private ChangeAccountRoleRequest (
            string email, 
            string newRole)
        {
            this.CreatedDate = DateTime.Now;
            this.Email = email;
            this.NewRole = newRole;
            IsCommite = null;
            IsReviewed = false;
        }

        public void CommitRequest(bool status)
        {
            if(!IsReviewed)
            {
                IsCommite = status;
                IsReviewed = true;
            }
        }

        public static Result<ChangeAccountRoleRequest> Initialize(string email, string newRole)
        {
            if(string.IsNullOrEmpty(newRole))
            {
                return Result.Failure<ChangeAccountRoleRequest>("New role should be not empty");
            }

            if(string.IsNullOrEmpty(email) ||
                email.Length > MAX_EMAIL_LENGTH)
            {
                return Result.Failure<ChangeAccountRoleRequest>("Email empty or length greate than 30 symbols");
            }

            var emailRegex = new Regex(@"^[^@\s]+@[^@\s]+\.[^@\s]+$");
            if (!emailRegex.IsMatch(email))
            {
                return Result.Failure<ChangeAccountRoleRequest>("Invalid email, should contains @ and . after");
            }

            return Result.Success(new ChangeAccountRoleRequest(email, newRole));
        }
    }
}
