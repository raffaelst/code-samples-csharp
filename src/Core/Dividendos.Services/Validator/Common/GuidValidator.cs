using FluentValidation;
using System;

namespace Dividendos.Service.Validator.Common
{
    public class GuidValidator : AbstractValidator<Guid>
    {
        public GuidValidator()
        {
            RuleFor(x => x).NotEmpty();
        }
    }
}
