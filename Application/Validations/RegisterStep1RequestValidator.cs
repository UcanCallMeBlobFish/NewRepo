using Application.DTOs;
using FluentValidation;

namespace Application.Validations
{
    public class RegisterStep1RequestValidator : AbstractValidator<RegisterStep1Request>
    {
        public RegisterStep1RequestValidator()
        {
            RuleFor(x => x.Name).NotEmpty().MaximumLength(50);
            RuleFor(x => x.ICNumber).NotEmpty().Length(5, 20);
            RuleFor(x => x.PhoneNumber).NotEmpty().MaximumLength(15);
            RuleFor(x => x.Email).NotEmpty().EmailAddress();
        }
    }
}
