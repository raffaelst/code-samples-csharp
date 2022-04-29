using AutoMapper;
using Dividendos.API.Model.Response;
using Dividendos.API.Model.Response.Common;
using Dividendos.API.Model.Response.v1;
using Dividendos.API.Model.Response.v1.ResearchProduct;
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
    public class ResearchProductApp : BaseApp, IResearchProductApp
    {
        private readonly IResearchProductService  _researchProductService;
        private readonly IMapper _mapper;
        private readonly IUnitOfWork _uow;
        private readonly ICacheService _cacheService;

        public ResearchProductApp(IMapper mapper,
            IUnitOfWork uow,
            IResearchProductService researchProductService,
            ICacheService cacheService)
        {
            _researchProductService = researchProductService;
            _mapper = mapper;
            _uow = uow;
            _cacheService = cacheService;
        }

        public ResultResponseObject<ResearchProductWrapperVM> GetAllActive()
        {
            ResultResponseObject<ResearchProductWrapperVM> result = new ResultResponseObject<ResearchProductWrapperVM>() {  Success = false };
            ResultResponseObject<IEnumerable<ResearchProductVM>> researchProduct = null;

            string resultFromCache = _cacheService.GetFromCache("ResearchProduct");

            if (resultFromCache == null)
            {
                using (_uow.Create())
                {
                    ResultServiceObject<IEnumerable<ResearchProduct>> resultService = _researchProductService.GetAllActive();

                    researchProduct = _mapper.Map<ResultResponseObject<IEnumerable<ResearchProductVM>>>(resultService);
                }

                result.Success = true;
                result.Value = new ResearchProductWrapperVM();
                result.Value.ResearchProducts = researchProduct.Value;
                result.Value.CallToActionURL = "https://lp1.empiricus.com.br/alerta-10em1-exc42-b/?xpromo=XE-MELH-ADM-EXC42-HOME-X-GIF-X-05&dtl=exc42-lpb";
                result.Value.HighlightHTML = "<html><div><p>Empiricus ADS</p></div></html>";

                _cacheService.SaveOnCache("ResearchProduct", TimeSpan.FromHours(12), JsonConvert.SerializeObject(result));
            }
            else
            {
                result = JsonConvert.DeserializeObject<ResultResponseObject<ResearchProductWrapperVM>>(resultFromCache);
                result.Success = true;
            }

            return result;
        }
    }
}