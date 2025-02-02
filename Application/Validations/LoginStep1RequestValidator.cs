using Application.DTOs;
using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Validations
{
    public class LoginStep1RequestValidator : AbstractValidator<LoginStep1Request>
    {
        public LoginStep1RequestValidator()
        {
            RuleFor(x => x.ICNumber).NotEmpty().Length(5, 20);
        }
    }
}
