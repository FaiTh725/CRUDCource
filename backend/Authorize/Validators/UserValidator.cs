using Authorize.Contracts.User;
using Authorize.Domain.Entities;
using FluentValidation;

namespace Authorize.Validators
{
    public class UserValidator : AbstractValidator<UserLogin>
    {
        public UserValidator() 
        {
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
