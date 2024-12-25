using Authorize.Contracts.User;
using FluentValidation;

namespace Authorize.Validators
{
    public class UserValidatorRegister : AbstractValidator<UserRegister>
    {
        public UserValidatorRegister()
        {
            RuleFor(user => user.UserName)
                .NotNull()
                .MaximumLength(20);

            RuleFor(user => user.Email)
                .NotNull()
                .Length(0, 30)
                .EmailAddress()
                .WithMessage("Enter correct email");

            RuleFor(user => user.Password)
                .NotNull()
                .Length(5, 20)
                .Matches(@"^(?=.*[A-Za-z])(?=.*\d).+$")
                .WithMessage("Enter password that containt one letter and number");
        }
    }
}
