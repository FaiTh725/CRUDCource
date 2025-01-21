using FluentValidation;
using FluentValidation.Results;
using Product.Domain.Contracts.Models.FeedBack;
using Product.Domain.Entities;

namespace Product.Validators
{
    public class AddFeedBackValidator : AbstractValidator<FeedBackAddRequest>
    {
        public AddFeedBackValidator() 
        {
            RuleFor(x => x.EmailAccount)
                .EmailAddress()
                .WithMessage("Enter Account that have sended FeedBack");

            RuleFor(x => x.Rate)
                .Must(x => x >= FeedBack.MIN_RATE && x <= FeedBack.MAX_RATE)
                .WithMessage($"Rate should in range " +
                $"{FeedBack.MIN_RATE} to {FeedBack.MAX_RATE}");

            RuleFor(x => x.Text)
                .NotNull()
                .WithMessage("Feed back shoud not null");
        }
    }
}
