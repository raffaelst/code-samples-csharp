using AutoMapper;
using Dividendos.API.Model.Request;
using Dividendos.API.Model.Response;
using Dividendos.API.Model.Response.Common;

using Dividendos.Application.Base;
using Dividendos.Application.Interface;
using Dividendos.B3.Interface;
using Dividendos.B3.Interface.Model;
using Dividendos.Entity.Entities;
using Dividendos.Entity.Model;
using Dividendos.Repository.Interface.UoW;
using Dividendos.Service.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Dividendos.Application
{
    public class SubPortfolioApp : BaseApp, ISubPortfolioApp
    {
        private readonly IGlobalAuthenticationService _globalAuthenticationService;
        private readonly ISubPortfolioService _subPortfolioService;
        private readonly IMapper _mapper;
        private readonly IUnitOfWork _uow;
        private readonly IPortfolioService _portfolioService;
        private readonly ISubPortfolioOperationService _subPortfolioOperationService;

        public SubPortfolioApp(
            IMapper mapper,
            IUnitOfWork uow,
            IGlobalAuthenticationService globalAuthenticationService,
            ISubPortfolioService subPortfolioService,
            IPortfolioService portfolioService,
            ISubPortfolioOperationService subPortfolioOperationService)
        {
            _subPortfolioService = subPortfolioService;
            _mapper = mapper;
            _globalAuthenticationService = globalAuthenticationService;
            _uow = uow;
            _portfolioService = portfolioService;
            _subPortfolioOperationService = subPortfolioOperationService;
        }

        public ResultResponseBase Disable(Guid idPortfolio, Guid idSubPortfolio)
        {

            ResultResponseBase resultResponse = new ResultResponseBase();

            ResultServiceBase resultServiceBase = new ResultServiceBase();

            using (_uow.Create())
            {
                ResultServiceObject<SubPortfolio> resultServiceObject = _subPortfolioService.GetByGuid(idSubPortfolio);
                resultServiceBase = _subPortfolioService.Disable(resultServiceObject.Value);
            }

            resultResponse = _mapper.Map<ResultResponseBase>(resultServiceBase);

            return resultResponse;
        }

        public ResultResponseObject<SubPortfolioVM> Add(Guid idPortfolio, SubPortfolioVM subPortfolioVM)
        {
            ResultServiceObject<SubPortfolio> resultServiceBase = new ResultServiceObject<SubPortfolio>();

            using (_uow.Create())
            {
                ResultServiceObject<Portfolio> resultServiceObject = _portfolioService.GetByGuid(idPortfolio);
                SubPortfolio subPortfolio = _mapper.Map<SubPortfolio>(subPortfolioVM);
                subPortfolio.IdPortfolio = resultServiceObject.Value.IdPortfolio;
                resultServiceBase = _subPortfolioService.Add(subPortfolio);

                foreach (var itemOperation in subPortfolioVM.Operations)
                {
                    SubPortfolioOperation subPortfolioOperation = new SubPortfolioOperation();
                    subPortfolioOperation.IdOperation = itemOperation;
                    subPortfolioOperation.IdSubPortfolio = resultServiceBase.Value.IdSubPortfolio;
                    
                    _subPortfolioOperationService.Insert(subPortfolioOperation);
                }
            }

            ResultResponseObject<SubPortfolioVM> resultResponse = _mapper.Map<ResultResponseObject<SubPortfolioVM>>(resultServiceBase);

            return resultResponse;
        }

        public ResultResponseObject<SubPortfolioVM> Update(Guid idSubPortfolio, SubPortfolioVM subPortfolioVM)
        {
            ResultServiceObject<SubPortfolio> resultServiceBase = new ResultServiceObject<SubPortfolio>();

            using (_uow.Create())
            {
                ResultServiceObject<SubPortfolio> resultServiceObject = _subPortfolioService.GetByGuid(idSubPortfolio);

                ResultServiceObject<IEnumerable<SubPortfolioOperation>> resultSubPortfolioOperationServiceObject = _subPortfolioOperationService.GetBySubPortfolio(resultServiceObject.Value.IdSubPortfolio);


                resultServiceObject.Value.Name = subPortfolioVM.Name;
                _subPortfolioService.Update(resultServiceObject.Value);

                foreach (var item in resultSubPortfolioOperationServiceObject.Value)
                {
                    _subPortfolioOperationService.Delete(item);
                }

                foreach (var itemOperation in subPortfolioVM.Operations)
                {
                    SubPortfolioOperation subPortfolioOperation = new SubPortfolioOperation();
                    subPortfolioOperation.IdOperation = itemOperation;
                    subPortfolioOperation.IdSubPortfolio = resultServiceObject.Value.IdSubPortfolio;

                    _subPortfolioOperationService.Insert(subPortfolioOperation);
                }
            }

            ResultResponseObject<SubPortfolioVM> resultResponse = _mapper.Map<ResultResponseObject<SubPortfolioVM>>(resultServiceBase);

            return resultResponse;
        }

        public ResultResponseObject<IEnumerable<SubPortfolioVM>> GetByPortfolio(Guid idPortifolio)
        {
            ResultResponseObject<IEnumerable<SubPortfolioVM>> resultResponse = new ResultResponseObject<IEnumerable<SubPortfolioVM>>();

            ResultServiceObject<IEnumerable<SubPortfolio>> resultService = new ResultServiceObject<IEnumerable<SubPortfolio>>();


            using (_uow.Create())
            {
                ResultServiceObject<Portfolio> resultPortfolio = _portfolioService.GetByGuid(idPortifolio);

                resultService = _subPortfolioService.GetByPortfolio(resultPortfolio.Value.IdPortfolio);

                if (resultService.Success)
                {
                    resultResponse = _mapper.Map<ResultResponseObject<IEnumerable<SubPortfolioVM>>>(resultService);
                }
            }

            return resultResponse;
        }
    }
}