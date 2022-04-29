using AutoMapper;
using Dividendos.API.Model.Response;
using Dividendos.API.Model.Response.Common;
using Dividendos.API.Model.Response.v1;
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
    public class TutorialApp : BaseApp, ITutorialApp
    {
        private readonly ITutorialService  _tutorialService;
        private readonly IMapper _mapper;
        private readonly IUnitOfWork _uow;
        private readonly ICacheService _cacheService;

        public TutorialApp(IMapper mapper,
            IUnitOfWork uow,
            ITutorialService tutorialService,
            ICacheService cacheService)
        {
            _tutorialService = tutorialService;
            _mapper = mapper;
            _uow = uow;
            _cacheService = cacheService;
        }

        public ResultResponseObject<IEnumerable<TutorialVM>> GetAll()
        {
            ResultResponseObject<IEnumerable<TutorialVM>> result = null;

            string resultFromCache = _cacheService.GetFromCache("Tutorial");

            if (resultFromCache == null)
            {
                using (_uow.Create())
                {
                    ResultServiceObject<IEnumerable<Tutorial>> resultService = _tutorialService.GetAll();

                    result = _mapper.Map<ResultResponseObject<IEnumerable<TutorialVM>>>(resultService);

                    _cacheService.SaveOnCache("Tutorial", TimeSpan.FromHours(6), JsonConvert.SerializeObject(result));
                }
            }
            else
            {
                result = JsonConvert.DeserializeObject<ResultResponseObject<IEnumerable<TutorialVM>>>(resultFromCache);
                result.Success = true;
            }

            return result;
        }
    }
}