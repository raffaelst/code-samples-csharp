using FluentValidation.Results;
using Dividendos.Entity.Entities;
using Dividendos.Entity.Model;
using Dividendos.Repository.Interface;
using Dividendos.Repository.Interface.UoW;
using Dividendos.Service.Interface;
using Dividendos.Service.Validator.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;


namespace Dividendos.Service
{
    public class SubPortfolioOperationService : BaseService, ISubPortfolioOperationService
    {
        public SubPortfolioOperationService(IUnitOfWork uow)
        {
            _uow = uow;
        }

        public ResultServiceObject<IEnumerable<SubPortfolioOperation>> GetBySubPortfolio(long idSubPortfolio)
        {
            ResultServiceObject<IEnumerable<SubPortfolioOperation>> resultService = new ResultServiceObject<IEnumerable<SubPortfolioOperation>>();

            IEnumerable<SubPortfolioOperation> subPortfolioOperations = _uow.SubPortfolioOperationRepository.Select(p => p.IdSubPortfolio == idSubPortfolio);

            resultService.Value = subPortfolioOperations;

            return resultService;
        }

        public ResultServiceObject<SubPortfolioOperation> Insert(SubPortfolioOperation subPortfolioOperation)
        {
            ResultServiceObject<SubPortfolioOperation> resultService = new ResultServiceObject<SubPortfolioOperation>();

            subPortfolioOperation.IdSubPortfolio = _uow.SubPortfolioOperationRepository.Insert(subPortfolioOperation);

            resultService.Value = subPortfolioOperation;

            return resultService;
        }

        public ResultServiceObject<SubPortfolioOperation> Update(SubPortfolioOperation subPortfolioOperation)
        {
            ResultServiceObject<SubPortfolioOperation> resultService = new ResultServiceObject<SubPortfolioOperation>();

            subPortfolioOperation = _uow.SubPortfolioOperationRepository.Update(subPortfolioOperation);

            resultService.Value = subPortfolioOperation;

            return resultService;
        }

        public ResultServiceBase Delete(SubPortfolioOperation subPortfolioOperation)
        {
            ResultServiceBase resultService = new ResultServiceBase();

            _uow.SubPortfolioOperationRepository.Delete(subPortfolioOperation);


            return resultService;
        }

        public ResultServiceObject<SubPortfolioOperation> GetBySubPortfolioAndIdOperation(long idSubPortfolio, long idOperation)
        {
            ResultServiceObject<SubPortfolioOperation> resultService = new ResultServiceObject<SubPortfolioOperation>();

            IEnumerable<SubPortfolioOperation> subPortfolioOperations = _uow.SubPortfolioOperationRepository.Select(p => p.IdSubPortfolio == idSubPortfolio && p.IdOperation == idOperation);

            resultService.Value = subPortfolioOperations.FirstOrDefault();

            return resultService;
        }
    }
}
