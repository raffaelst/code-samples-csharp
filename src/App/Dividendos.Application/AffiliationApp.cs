using AutoMapper;
using K.Logger;
using Dividendos.API.Model.Request.Device;
using Dividendos.API.Model.Response.Common;

using Dividendos.Application.Base;
using Dividendos.Application.Interface;
using Dividendos.Entity.Entities;
using Dividendos.Entity.Model;
using Dividendos.Repository.Interface.UoW;
using Dividendos.Service.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Dividendos.API.Model.Response;
using Dividendos.API.Model.Request.Affiliation;
using Microsoft.Extensions.Caching.Distributed;
using Newtonsoft.Json;

namespace Dividendos.Application
{
    public class AffiliationApp : BaseApp, IAffiliationApp
    {

        private readonly IMapper _mapper;
        private readonly IUnitOfWork _uow;
        private readonly IAffiliationService _affiliationService;
        private readonly ICacheService _cacheService;
        public AffiliationApp(IMapper mapper,
            IUnitOfWork uow,
            IAffiliationService affiliationService,
            ICacheService cacheService)
        {
            _mapper = mapper;
            _uow = uow;
            _affiliationService = affiliationService;
            _cacheService = cacheService;
        }


        public ResultResponseObject<IEnumerable<AffiliateProductDetailVM>> GetByType(AffiliationType affiliationType)
        {
            ResultResponseObject<IEnumerable<AffiliateProductDetailVM>> result = null;

            string resultFromCache = _cacheService.GetFromCache(string.Concat("Affiliation:", affiliationType.ToString()));

            if (resultFromCache == null)
            {
                using (_uow.Create())
                {
                    ResultServiceObject<IEnumerable<AffiliateProduct>> resultService = _affiliationService.GetByType((int)affiliationType);

                    result = _mapper.Map<ResultResponseObject<IEnumerable<AffiliateProductDetailVM>>>(resultService);

                    _cacheService.SaveOnCache(string.Concat("Affiliation:", affiliationType.ToString()), TimeSpan.FromHours(6), JsonConvert.SerializeObject(result));
                }
            }
            else
            {
                result = JsonConvert.DeserializeObject<ResultResponseObject<IEnumerable<AffiliateProductDetailVM>>>(resultFromCache);
                result.Success = true;
            }

            return result;
        }
    }
}