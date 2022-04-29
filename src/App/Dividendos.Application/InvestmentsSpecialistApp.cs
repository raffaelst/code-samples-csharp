using AutoMapper;
using Dividendos.API.Model.Request.Settings;
using Dividendos.API.Model.Response;
using Dividendos.API.Model.Response.Common;

using Dividendos.Application.Base;
using Dividendos.Application.Interface;
using Dividendos.Entity.Entities;
using Dividendos.Entity.Model;
using Dividendos.Entity.Views;
using Dividendos.Repository.Interface.UoW;
using Dividendos.Service.Interface;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Dividendos.Application
{
    public class InvestmentsSpecialistApp : BaseApp, IInvestmentsSpecialistApp
    {
        private readonly IInvestmentsSpecialistService _investmentsSpecialistService;
        private readonly IMapper _mapper;
        private readonly IUnitOfWork _uow;
        private readonly ICacheService _cacheService;

        public InvestmentsSpecialistApp(IMapper mapper,
            IUnitOfWork uow,
            IInvestmentsSpecialistService investmentsSpecialistService,
            ICacheService cacheService)
        {
            _investmentsSpecialistService = investmentsSpecialistService;
            _mapper = mapper;
            _uow = uow;
            _cacheService = cacheService;
        }

        public ResultResponseObject<IEnumerable<InvestmentsSpecialistVM>> GetAllInvestmentsSpecialist()
        {
            ResultResponseObject<IEnumerable<InvestmentsSpecialistVM>> result = null;

            string resultFromCache = _cacheService.GetFromCache("InvestmentsSpecialist");

            if (resultFromCache == null)
            {
                using (_uow.Create())
                {
                    ResultServiceObject<IEnumerable<InvestmentsSpecialist>> resultService = _investmentsSpecialistService.GetAllInvestmentsSpecialist();

                    result = _mapper.Map<ResultResponseObject<IEnumerable<InvestmentsSpecialistVM>>>(resultService);

                    _cacheService.SaveOnCache("InvestmentsSpecialist", TimeSpan.FromHours(6), JsonConvert.SerializeObject(result));
                }
            }
            else
            {
                result = JsonConvert.DeserializeObject<ResultResponseObject<IEnumerable<InvestmentsSpecialistVM>>>(resultFromCache);
                result.Success = true;
            }

            return result;
        }

        public ResultResponseObject<IEnumerable<SuggestedPortfolioVM>> GetSuggestedPortfolioBySpecialist(string specialistGuid)
        {
            ResultResponseObject<IEnumerable<SuggestedPortfolioVM>> result = null;

            string resultFromCache = _cacheService.GetFromCache(string.Concat("InvestmentsSpecialist:SuggestedPortfolioBySpecialist:", specialistGuid));

            if (resultFromCache == null)
            {
                using (_uow.Create())
                {
                    ResultServiceObject<IEnumerable<SuggestedPortfolio>> resultService = _investmentsSpecialistService.GetSuggestedPortfolio(specialistGuid);

                    result = _mapper.Map<ResultResponseObject<IEnumerable<SuggestedPortfolioVM>>>(resultService);

                    _cacheService.SaveOnCache(string.Concat("InvestmentsSpecialist:SuggestedPortfolioBySpecialist:", specialistGuid), TimeSpan.FromHours(6), JsonConvert.SerializeObject(result));
                }
            }
            else
            {
                result = JsonConvert.DeserializeObject<ResultResponseObject<IEnumerable<SuggestedPortfolioVM>>>(resultFromCache);
                result.Success = true;
            }



            return result;
        }

        public ResultResponseObject<IEnumerable<SuggestedPortfolioItemVM>> GetSuggestedPortfolioItems(string suggestedPortfolioGuid)
        {
            ResultResponseObject<IEnumerable<SuggestedPortfolioItemVM>> result = null;

            string resultFromCache = _cacheService.GetFromCache(string.Concat("InvestmentsSpecialist:SuggestedPortfolioBySpecialist:SuggestedPortfolioItems:",suggestedPortfolioGuid));

            if (resultFromCache == null)
            {
                using (_uow.Create())
                {
                    ResultServiceObject<IEnumerable<SuggestedPortfolioItemView>> resultService = _investmentsSpecialistService.GetSuggestedPortfolioItems(suggestedPortfolioGuid);

                    result = _mapper.Map<ResultResponseObject<IEnumerable<SuggestedPortfolioItemVM>>>(resultService);

                    _cacheService.SaveOnCache(string.Concat("InvestmentsSpecialist:SuggestedPortfolioBySpecialist:SuggestedPortfolioItems:",suggestedPortfolioGuid), TimeSpan.FromHours(1), JsonConvert.SerializeObject(result));
                }
            }
            else
            {
                result = JsonConvert.DeserializeObject<ResultResponseObject<IEnumerable<SuggestedPortfolioItemVM>>>(resultFromCache);
                result.Success = true;
            }

            return result;
        }


        public ResultResponseBase DeleteInvestmentsSpecialist(Guid investmentsSpecialistGuid)
        {
            ResultResponseBase result = null;

            using (_uow.Create())
            {
                var suggestedPortfolios = _investmentsSpecialistService.GetSuggestedPortfolio(investmentsSpecialistGuid.ToString());

                foreach (var item in suggestedPortfolios.Value)
                {
                    _investmentsSpecialistService.DeleteSuggestedPortfolioWithItens(item.SuggestedPortfolioGuid, _cacheService);
                }

                ResultServiceBase resultServiceBase = _investmentsSpecialistService.DeleteInvestmentsSpecialist(investmentsSpecialistGuid, _cacheService);

                result = _mapper.Map<ResultResponseBase>(resultServiceBase);
            }

            return result;
        }

        public ResultResponseBase DeleteSuggestedPortfolio(Guid suggestedPortfolioGuid)
        {
            ResultResponseBase result = null;

            using (_uow.Create())
            {
                ResultServiceBase resultServiceBase = _investmentsSpecialistService.DeleteSuggestedPortfolioWithItens(suggestedPortfolioGuid, _cacheService);

                result = _mapper.Map<ResultResponseBase>(resultServiceBase);
            }

            return result;
        }
    }
}