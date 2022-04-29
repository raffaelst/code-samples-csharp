using Dividendos.Bacen.Interface;
using Dividendos.Bacen.Interface.Model;
using Dividendos.Repository.Interface.UoW;
using Dividendos.Service.Interface;
using System.Linq;
using System.Collections.Generic;
using System.Text;
using Dividendos.Entity.Model;
using Dividendos.Entity.Entities;
using System;
using System.Globalization;
using K.Logger;
using Dividendos.Application.Interface;
using System.Threading.Tasks;
using Dividendos.API.Model.Response.Common;
using Dividendos.API.Model.Response;
using AutoMapper;
using Dividendos.Entity.Enum;
using Dividendos.Entity.Views;
using Microsoft.Extensions.Caching.Distributed;
using Newtonsoft.Json;
using Dividendos.API.Model.Request.Goals;
using Dividendos.API.Model.Response.Influencer;

namespace Dividendos.Application
{
    public class InfluencerApp : IInfluencerApp
    {
        private readonly IUnitOfWork _uow;
        private readonly IGoalService _goalService;
        private readonly ILogger _logger;
        private readonly IMapper _mapper;
        private readonly IGlobalAuthenticationService _globalAuthenticationService;
        private readonly ICacheService _cacheService;
        private readonly IInfluencerService _influencerService;

        public InfluencerApp(IUnitOfWork uow,
                            IGoalService goalService,
                            ILogger logger,
                            IMapper mapper,
                            IGlobalAuthenticationService globalAuthenticationService,
                            IInfluencerService influencerService,
                            ICacheService cacheService)
        {
            _uow = uow;
            _goalService = goalService;
            _logger = logger;
            _mapper = mapper;
            _globalAuthenticationService = globalAuthenticationService;
            _influencerService = influencerService;
            _cacheService = cacheService;
        }

        public ResultResponseObject<IEnumerable<InfluencerVM>> GetUsersBehindInfluencer(string influencerAffiliatorGuid)
        {
            ResultResponseObject<IEnumerable<InfluencerVM>> result = null;

            using (_uow.Create())
            {
                ResultServiceObject<IEnumerable<InfluencerView>> resultService = _influencerService.GetUsersBehindInfluencer(influencerAffiliatorGuid);
                result = _mapper.Map<ResultResponseObject<IEnumerable<InfluencerVM>>>(resultService);
            }

            return result;
        }
    }
}
