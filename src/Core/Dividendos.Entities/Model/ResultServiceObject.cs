using FluentValidation.Results;
using K.UnitOfWorkBase;

namespace Dividendos.Entity.Model
{
    public class ResultServiceObject<T> : ResultServiceBase
    {
        public ResultServiceObject()
        {
        }

        public ResultServiceObject(ValidationResult validationResult, IUnitOfWorkBase unitOfWorkBase, string successMessage = null) : base(validationResult, unitOfWorkBase, successMessage)
        {

        }

        public T Value { get; set; }
    }
}
