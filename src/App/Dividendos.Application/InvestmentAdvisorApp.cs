using AutoMapper;
using Dividendos.API.Model.Request.FreeRecommendations;
using Dividendos.API.Model.Request.InvestmentAdvisor;
using Dividendos.API.Model.Response;
using Dividendos.API.Model.Response.Common;
using Dividendos.API.Model.Response.v1.InvestmentAdvisor;
using Dividendos.Application.Base;
using Dividendos.Application.Interface;
using Dividendos.Entity.Entities;
using Dividendos.Entity.Model;
using Dividendos.Repository.Interface.UoW;
using Dividendos.Service.Interface;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;

namespace Dividendos.Application
{
    public class InvestmentAdvisorApp : BaseApp, IInvestmentAdvisorApp
    {
        private readonly IMapper _mapper;
        private readonly IUnitOfWork _uow;
        private readonly ICacheService _cacheService;
        private readonly IStockService _stockService;

        public InvestmentAdvisorApp(IMapper mapper,
            IUnitOfWork uow,
            ICacheService cacheService,
            IStockService stockService)
        {
            _mapper = mapper;
            _uow = uow;
            _cacheService = cacheService;
            _stockService = stockService;
        }

        public ResultResponseObject<IEnumerable<InvestmentAdvisorVideoResponse>> GetVideosAvailable(bool onlyLastVideo)
        {
            string nameOnCache = "InvestmentAdvisorVideo";

            if (onlyLastVideo)
            {
                nameOnCache = "LastPublishedInvestmentAdvisorVideo";
            }

            ResultResponseObject<IEnumerable<InvestmentAdvisorVideoResponse>> result = null;

            string resultFromCache = _cacheService.GetFromCache(nameOnCache);

            if (resultFromCache == null)
            {
                using (_uow.Create())
                {
                    ResultServiceObject<IEnumerable<InvestmentAdvisorVideo>> resultService = null;

                    result = _mapper.Map<ResultResponseObject<IEnumerable<InvestmentAdvisorVideoResponse>>>(resultService);

                    _cacheService.SaveOnCache(nameOnCache, TimeSpan.FromMinutes(15), JsonConvert.SerializeObject(result));
                }
            }
            else
            {
                result = JsonConvert.DeserializeObject<ResultResponseObject<IEnumerable<InvestmentAdvisorVideoResponse>>>(resultFromCache);
                result.Success = true;
            }

            return result;
        }


        public ResultResponseObject<IEnumerable<FreeRecommendationResponse>> GetFreeRecommendation()
        {
            string nameOnCache = "InvestmentAdvisorFreeRecommendationResponse";

            ResultResponseObject<IEnumerable<FreeRecommendationResponse>> result = null;

            string resultFromCache = _cacheService.GetFromCache(nameOnCache);

            if (resultFromCache == null)
            {
                using (_uow.Create())
                {
                    ResultServiceObject<IEnumerable<InvestmentAdvisorFreeRecommendationView>> resultService = null;

                    result = _mapper.Map<ResultResponseObject<IEnumerable<FreeRecommendationResponse>>>(resultService);

                    _cacheService.SaveOnCache(nameOnCache, TimeSpan.FromMinutes(15), JsonConvert.SerializeObject(result));
                }
            }
            else
            {
                result = JsonConvert.DeserializeObject<ResultResponseObject<IEnumerable<FreeRecommendationResponse>>>(resultFromCache);
                result.Success = true;
            }

            return result;
        }
    }
}