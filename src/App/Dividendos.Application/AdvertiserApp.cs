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
    public class AdvertiserApp : BaseApp, IAdvertiserApp
    {
        private readonly IUnitOfWork _uow;
        private readonly IAdvertiserService _advertiserService;
        private readonly IMapper _mapper;
        private readonly IGlobalAuthenticationService _globalAuthenticationService;
        private readonly ICacheService _cacheService;
        public AdvertiserApp(IUnitOfWork uow,
            IAdvertiserService advertiserService,
            IMapper mapper,
            ICacheService cacheService,
            IGlobalAuthenticationService globalAuthenticationService)
        {
            _uow = uow;
            _advertiserService = advertiserService;
            _mapper = mapper;
            _globalAuthenticationService = globalAuthenticationService;
            _cacheService = cacheService;
        }

        public ResultResponseObject<AdvertiserVM> Get()
        {
            ResultResponseObject<AdvertiserVM> result = null;

            string resultFromCache = _cacheService.GetFromCache("AdvertiserV1");

            if (resultFromCache == null)
            {
                using (_uow.Create())
                {
                    ResultServiceObject<Advertiser> resultService = _advertiserService.Get();

                    result = _mapper.Map<ResultResponseObject<AdvertiserVM>>(resultService);

                    _cacheService.SaveOnCache("AdvertiserV1", TimeSpan.FromHours(12), JsonConvert.SerializeObject(result));
                }
            }
            else
            {
                result = JsonConvert.DeserializeObject<ResultResponseObject<AdvertiserVM>>(resultFromCache);
                result.Success = true;
            }

            return result;
        }

        public ResultResponseObject<IEnumerable<AdvertiserVM>> GetV2()
        {
            ResultResponseObject<IEnumerable<AdvertiserVM>> result = null;

            using (_uow.Create())
            {
                ResultServiceObject<IEnumerable<Advertiser>> resultService = _advertiserService.GetGeneralAndByUser(_globalAuthenticationService.IdUser);

                result = _mapper.Map<ResultResponseObject<IEnumerable<AdvertiserVM>>>(resultService);
            }

            return result;
        }

        public ResultResponseObject<AdvertiserDetailsVM> GetDetails(string advertiserGuid)
        {
            ResultResponseObject<AdvertiserDetailsVM> result = null;
            
            string resultFromCache = _cacheService.GetFromCache(string.Concat("AdvertiserDetails:", advertiserGuid));

            if (resultFromCache == null)
            {
                using (_uow.Create())
                {
                    ResultServiceObject<AdvertiserDetails> resultAdvertiserDetailsService = _advertiserService.GetDetails(advertiserGuid);

                    result = _mapper.Map<ResultResponseObject<AdvertiserDetailsVM>>(resultAdvertiserDetailsService);

                    _cacheService.SaveOnCache(string.Concat("AdvertiserDetails:", advertiserGuid), TimeSpan.FromHours(12), JsonConvert.SerializeObject(result));
                }
            }
            else
            {
                result = JsonConvert.DeserializeObject<ResultResponseObject<AdvertiserDetailsVM>>(resultFromCache);
                result.Success = true;
            }

            return result;
        }
    }
}