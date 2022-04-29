using FluentValidation;
using Dividendos.CrossCutting.Identity.Models;
using Dividendos.Entity.Entities;

namespace Dividendos.Service.Validator.User
{
    public class UserValidator : AbstractValidator<ApplicationUser>
    {
        public UserValidator()
        {
            RuleFor(x => x.Email).NotEmpty().WithMessage("Informe o seu e-mail");
            RuleFor(x => x.Name).NotEmpty().WithMessage("Informe seu nome");
            //RuleFor(x => x.Username).Length(3, 20).WithMessage("Tamanho inválido");
            //RuleFor(x => x.Postcode).Must(BeAValidPostcode).WithMessage("Please specify a valid postcode");
        }

        //private bool BeAValidPostcode(string postcode)
        //{
        //    // custom postcode validating logic goes here
        //}
    }
}
