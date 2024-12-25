using FluentValidation;
using ProductEntity = Product.Domain.Models.Product;
using System.Text.RegularExpressions;
using Product.Domain.Contracts.Models.Product;

namespace Product.Validators
{
    public class CreateProductValidator : AbstractValidator<UploadProduct>
    {
        public CreateProductValidator()
        {
            RuleFor(x => x.Name)
                .NotNull()
                .NotEmpty()
                .MaximumLength(30)
                .WithMessage("Invalid Name. Name should be not empty and max length is 30");

            RuleFor(x => x.Description)
                .MaximumLength(300)
                .WithMessage("Too long description");

            var decimalRegex = new Regex(@"[+-]?\\d*\\.?\\d+");

            RuleFor(x => x.Price)
                .NotEmpty()
                .Must(m => !decimalRegex.IsMatch(m.ToString()))
                .WithMessage("Price should be in a correct format");

            RuleFor(x => x.SealerEmail)
                .NotNull()
                .EmailAddress()
                .WithMessage("Email sealer should be correct");

            RuleFor(x => x.Count)
                .Must(x => x >= 0 && x <= ProductEntity.MAX_COUNT_PRODUCT)
                .WithMessage("Product count should be positove and not greate than " + 
                    ProductEntity.MAX_COUNT_PRODUCT.ToString());
        }
    }
}
