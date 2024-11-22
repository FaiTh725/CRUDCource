using FluentValidation;
using Notification.Domain.Contacrs.Email;

namespace Notification.Validators
{
    public class EmailValidator : AbstractValidator<SendEmail>
    {
        public EmailValidator() 
        {
            RuleFor(x => x.Subject)
                .NotNull()
                .NotEmpty()
                .WithMessage("Subject email should not be null");

            RuleFor(x => x.Recipients)
                .NotEmpty()
                .WithMessage("Need minumum one recipient");
        }
    }
}
