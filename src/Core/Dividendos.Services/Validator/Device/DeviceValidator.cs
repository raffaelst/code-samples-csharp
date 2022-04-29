using FluentValidation;
using Dividendos.CrossCutting.Identity.Models;
using Dividendos.Entity.Entities;

namespace Dividendos.Service.Validator
{
    public class DeviceValidator : AbstractValidator<Device>
    {
        public DeviceValidator()
        {
            RuleFor(x => x.Name).NotEmpty().WithMessage("Informe um nome para o dispositivo");
            //RuleFor(x => x.Username).Length(3, 20).WithMessage("Tamanho inválido");
            //RuleFor(x => x.Postcode).Must(BeAValidPostcode).WithMessage("Please specify a valid postcode");
        }

        //private bool BeAValidPostcode(string postcode)
        //{
        //    // custom postcode validating logic goes here
        //}
    }
}
