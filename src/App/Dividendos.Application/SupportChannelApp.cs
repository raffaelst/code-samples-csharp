using AutoMapper;
using Dividendos.API.Model.Response;
using Dividendos.API.Model.Response.Common;
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
    public class SupportChannelApp : BaseApp, ISupportChannelApp
    {
        private readonly ISupportChannelService  _supportChannelService;
        private readonly IMapper _mapper;
        private readonly IUnitOfWork _uow;
        private readonly ICacheService _cacheService;

        public SupportChannelApp(IMapper mapper,
            IUnitOfWork uow,
            ISupportChannelService supportChannelService,
            ICacheService cacheService)
        {
            _supportChannelService = supportChannelService;
            _mapper = mapper;
            _uow = uow;
            _cacheService = cacheService;
        }

        public ResultResponseObject<IEnumerable<SupportChannelVM>> GetAll()
        {
            ResultResponseObject<IEnumerable<SupportChannelVM>> result = null;

            string resultFromCache = _cacheService.GetFromCache("SupportChannels");

            if (resultFromCache == null)
            {
                using (_uow.Create())
                {
                    ResultServiceObject<IEnumerable<SupportChannel>> resultService = _supportChannelService.GetAll();

                    result = _mapper.Map<ResultResponseObject<IEnumerable<SupportChannelVM>>>(resultService);

                    _cacheService.SaveOnCache("SupportChannels", TimeSpan.FromHours(6), JsonConvert.SerializeObject(result));
                }
            }
            else
            {
                result = JsonConvert.DeserializeObject<ResultResponseObject<IEnumerable<SupportChannelVM>>>(resultFromCache);
                result.Success = true;
            }

            return result;
        }
    }
}