using AutoMapper;
using Dividendos.API.Model.Response;
using Dividendos.API.Model.Response.Common;
using Dividendos.Application.Base;
using Dividendos.Application.Interface;
using Dividendos.Entity.Entities;
using Dividendos.Entity.Model;
using Dividendos.Repository.Interface.UoW;
using Dividendos.Service.Interface;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;

namespace Dividendos.Application
{
    public class AdvertiserExternalApp : BaseApp, IAdvertiserExternalApp
    {
        private readonly IUnitOfWork _uow;
        private readonly IAdvertiserExternalService _advertiserExternalService;
        private readonly IMapper _mapper;
        private readonly IGlobalAuthenticationService _globalAuthenticationService;
        private readonly ICacheService _cacheService;

        public AdvertiserExternalApp(IUnitOfWork uow,
            IAdvertiserExternalService advertiserExternalService,
            IMapper mapper,
            ICacheService cacheService,
            IGlobalAuthenticationService globalAuthenticationService)
        {
            _uow = uow;
            _advertiserExternalService = advertiserExternalService;
            _mapper = mapper;
            _globalAuthenticationService = globalAuthenticationService;
            _cacheService = cacheService;
        }

        public ResultResponseObject<IEnumerable<AdvertiserExternalVM>> Get()
        {
            ResultResponseObject<IEnumerable<AdvertiserExternalVM>> result = null;

            string resultFromCache = _cacheService.GetFromCache("AdvertiserExternal");

            if (resultFromCache == null)
            {
                using (_uow.Create())
                {
                    ResultServiceObject<IEnumerable<AdvertiserExternal>> resultService = new ResultServiceObject<IEnumerable<AdvertiserExternal>>();

                    result = _mapper.Map<ResultResponseObject<IEnumerable<AdvertiserExternalVM>>>(resultService);

                    _cacheService.SaveOnCache("AdvertiserExternal", TimeSpan.FromHours(4), JsonConvert.SerializeObject(result));
                }
            }
            else
            {
                result = JsonConvert.DeserializeObject<ResultResponseObject<IEnumerable<AdvertiserExternalVM>>>(resultFromCache);
                result.Success = true;
            }

            return result;
        }

        public ResultResponseObject<AdvertiserExternalDetailVM> GetDetails(string advertiserGuid)
        {
            ResultResponseObject<AdvertiserExternalDetailVM> result = new ResultResponseObject<AdvertiserExternalDetailVM>();

            string resultFromCache = _cacheService.GetFromCache(string.Concat("AdvertiserExternalDetail", advertiserGuid));

            if (resultFromCache == null)
            {
                using (_uow.Create())
                {
                    ResultServiceObject<AdvertiserExternalDetail> resultService = null;

                    result = _mapper.Map<ResultResponseObject<AdvertiserExternalDetailVM>>(resultService);

                    _cacheService.SaveOnCache(string.Concat("AdvertiserExternalDetail", advertiserGuid), TimeSpan.FromHours(4), JsonConvert.SerializeObject(result));
                }
            }
            else
            {
                result = JsonConvert.DeserializeObject<ResultResponseObject<AdvertiserExternalDetailVM>>(resultFromCache);
                result.Success = true;
            }

            return result;
        }

        public ResultResponseObject<IEnumerable<AdvertiserExternalVM>> GetV2()
        {
            ResultResponseObject<IEnumerable<AdvertiserExternalVM>> result = null;

            string resultFromCache = _cacheService.GetFromCache("AdvertiserExternalV2");

            if (resultFromCache == null)
            {
                using (_uow.Create())
                {
                    ResultServiceObject<IEnumerable<AdvertiserExternal>> resultService = _advertiserExternalService.Get();

                    result = _mapper.Map<ResultResponseObject<IEnumerable<AdvertiserExternalVM>>>(resultService);

                    _cacheService.SaveOnCache("AdvertiserExternalV2", TimeSpan.FromHours(4), JsonConvert.SerializeObject(result));
                }
            }
            else
            {
                result = JsonConvert.DeserializeObject<ResultResponseObject<IEnumerable<AdvertiserExternalVM>>>(resultFromCache);
                result.Success = true;
            }

            return result;
        }

        public ResultResponseObject<AdvertiserExternalDetailVM> GetDetailsV2(string advertiserGuid)
        {
            ResultResponseObject<AdvertiserExternalDetailVM> result = null;

            string resultFromCache = _cacheService.GetFromCache(string.Concat("AdvertiserExternalDetailV2", advertiserGuid));

            if (resultFromCache == null)
            {
                using (_uow.Create())
                {
                    ResultServiceObject<AdvertiserExternalDetail> resultService = _advertiserExternalService.GetDetails(advertiserGuid);

                    result = _mapper.Map<ResultResponseObject<AdvertiserExternalDetailVM>>(resultService);

                    _cacheService.SaveOnCache(string.Concat("AdvertiserExternalDetailV2", advertiserGuid), TimeSpan.FromHours(4), JsonConvert.SerializeObject(result));
                }
            }
            else
            {
                result = JsonConvert.DeserializeObject<ResultResponseObject<AdvertiserExternalDetailVM>>(resultFromCache);
                result.Success = true;
            }

            return result;
        }
    }
}