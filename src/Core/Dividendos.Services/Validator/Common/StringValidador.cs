using FluentValidation;
using System;

namespace Dividendos.Service.Validator.Common
{
    public class StringValidador : AbstractValidator<string>
    {
        public StringValidador()
        {
            RuleFor(x => x).NotEmpty();
        }
    }
}
