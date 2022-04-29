using FluentValidation.Results;
using K.UnitOfWorkBase;

namespace Dividendos.Entity.Model
{
    public class ResultServiceString : ResultServiceBase
    {
        public ResultServiceString()
        {

        }

        public ResultServiceString(ValidationResult validationResult, IUnitOfWorkBase unitOfWorkBase, string successMessage = null) : base(validationResult, unitOfWorkBase, successMessage)
        {

        }

        public string Value { get; set; }
    }
}
