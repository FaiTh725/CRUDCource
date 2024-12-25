using FluentValidation;
using Product.Domain.Contracts.Models.Account;

namespace Product.Validators
{
    public class AccountValidator : AbstractValidator<CreateAccount>
    {
        public AccountValidator()
        {
            RuleFor(x => x.Name)
                .NotNull()
                .MaximumLength(20)
                .WithMessage("Name should be not empty and size between from 0 to 20");

            RuleFor(x => x.Email)
                .NotNull()
                .MaximumLength(30)
                .EmailAddress()
                .WithMessage("Enter correct email");
        }
    }
}
