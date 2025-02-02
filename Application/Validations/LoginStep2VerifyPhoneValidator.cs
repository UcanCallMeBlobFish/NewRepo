using Application.DTOs;
using FluentValidation;

namespace Application.Validations
{
    public class LoginStep2VerifyPhoneValidator : AbstractValidator<LoginStep2VerifyPhoneRequest>
    {
        public LoginStep2VerifyPhoneValidator()
        {
            RuleFor(x => x.ICNumber).NotEmpty();
            RuleFor(x => x.PhoneCode).Length(4);
        }
    }
}
