using System.Collections.Generic;
using System.Linq;
using FluentValidation.Results;
using K.UnitOfWorkBase;

namespace Dividendos.Entity.Model
{ 
    public class ResultServiceBase
    {
        public ResultServiceBase()
        {
        }

        public ResultServiceBase(ValidationResult validationResult, IUnitOfWorkBase unitOfWorkBase, string successMessage = null)
        {
            VerifyErrors(validationResult, unitOfWorkBase);
            this.SuccessMessage = successMessage;
        }

        public bool Success
        {
            get
            {
                return this.ErrorMessages == null || !this.ErrorMessages.Any();
            }
        }

        public string SuccessMessage { get; set; }

        public IList<string> ErrorMessages { get; set; }


        public void VerifyErrors(ValidationResult validationResult, IUnitOfWorkBase unitOfWorkBase)
        {
            if (validationResult != null && !validationResult.IsValid)
            {
                if (!unitOfWorkBase.RollbackTransaction)
                {
                    unitOfWorkBase.RollbackTransaction = true;
                }

                if (validationResult.Errors != null)
                {

                    if (this.ErrorMessages == null)
                    {
                        this.ErrorMessages = new List<string>();
                    }

                    this.ErrorMessages = validationResult.Errors.Select(x => x.ErrorMessage).ToList();
                }
            }
        }
    }
}
