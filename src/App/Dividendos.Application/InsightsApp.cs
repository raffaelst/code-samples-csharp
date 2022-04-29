using AutoMapper;
using Dividendos.API.Model.Request.Insight;
using Dividendos.API.Model.Response;
using Dividendos.API.Model.Response.Common;
using Dividendos.API.Model.Response.Insight;
using Dividendos.Application.Base;
using Dividendos.Application.Interface;
using Dividendos.Entity.Entities;
using Dividendos.Entity.Enum;
using Dividendos.Entity.Model;
using Dividendos.Entity.Views;
using Dividendos.Repository.Interface.UoW;
using Dividendos.Service.Interface;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;

namespace Dividendos.Application
{
    public class InsightsApp : BaseApp, IInsightsApp
    {
        private readonly IInsightService _insightService;
        private readonly IMapper _mapper;
        private readonly IUnitOfWork _uow;
        private readonly ICacheService _cacheService;

        public InsightsApp(IMapper mapper,
            IUnitOfWork uow,
            IInsightService insightService,
            ICacheService cacheService)
        {
            _insightService = insightService;
            _mapper = mapper;
            _uow = uow;
            _cacheService = cacheService;
        }

        public ResultResponseObject<InsightsWrapper> GetByFilter(InsightsVM insightsVM)
        {
            ResultResponseObject<InsightsWrapper> result = null;

            string resultFromCache = _cacheService.GetFromCache(string.Format("Insights:{0}:{1}", insightsVM.Year, insightsVM.InsightType.ToString()));

            if (resultFromCache == null)
            {
                using (_uow.Create())
                {
                    ResultServiceObject<IEnumerable<InsightView>> resultService = _insightService.GetByTypeAndYear(insightsVM.Year, (InsightEnum)insightsVM.InsightType);

                    List<InsightsResponse> insightsResponses = new List<InsightsResponse>();

                    foreach (var item in resultService.Value)
                    {
                        insightsResponses.Add(new InsightsResponse() { Description = item.Description, Position = item.Position, LogoURL = item.LogoURL });
                    }

                    result = new ResultResponseObject<InsightsWrapper>();
                    result.Value = new InsightsWrapper() { Insights = insightsResponses };
                    result.Success = true;
                    _cacheService.SaveOnCache(string.Format("Insights:{0}:{1}", insightsVM.Year, insightsVM.InsightType.ToString()), TimeSpan.FromHours(24), JsonConvert.SerializeObject(result));
                }
            }
            else
            {
                result = JsonConvert.DeserializeObject<ResultResponseObject<InsightsWrapper>>(resultFromCache);
                result.Success = true;
            }

            return result;
        }
    }
}