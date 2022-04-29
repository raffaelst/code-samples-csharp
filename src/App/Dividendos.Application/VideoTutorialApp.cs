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
    public class VideoTutorialApp : BaseApp, IVideoTutorialApp
    {
        private readonly IVideoTutorialService  _videoTutorialService;
        private readonly IMapper _mapper;
        private readonly IUnitOfWork _uow;
        private readonly ICacheService _cacheService;

        public VideoTutorialApp(IMapper mapper,
            IUnitOfWork uow,
            IVideoTutorialService videoTutorialService,
            ICacheService cacheService)
        {
            _videoTutorialService = videoTutorialService;
            _mapper = mapper;
            _uow = uow;
            _cacheService = cacheService;
        }

        public ResultResponseObject<IEnumerable<VideoTutorialVM>> GetAll()
        {
            ResultResponseObject<IEnumerable<VideoTutorialVM>> result = null;

            string resultFromCache = _cacheService.GetFromCache("VideoTutorial");

            if (resultFromCache == null)
            {
                using (_uow.Create())
                {
                    ResultServiceObject<IEnumerable<VideoTutorial>> resultService = _videoTutorialService.GetAll();

                    result = _mapper.Map<ResultResponseObject<IEnumerable<VideoTutorialVM>>>(resultService);

                    _cacheService.SaveOnCache("VideoTutorial", TimeSpan.FromHours(12), JsonConvert.SerializeObject(result));
                }
            }
            else
            {
                result = JsonConvert.DeserializeObject<ResultResponseObject<IEnumerable<VideoTutorialVM>>>(resultFromCache);
                result.Success = true;
            }

            return result;
        }
    }
}