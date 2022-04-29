using FluentValidation;

namespace Dividendos.Service.Validator.Common
{
    public class IdValidator : AbstractValidator<long>
    {
        public IdValidator()
        {
            RuleFor(x => x).NotEqual(0).WithMessage("Entidade não informada");
        }
    }
}
