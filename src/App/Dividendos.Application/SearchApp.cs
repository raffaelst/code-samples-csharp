using AutoMapper;
using Dividendos.API.Model.Response;
using Dividendos.API.Model.Response.Common;
using Dividendos.API.Model.Response.Crypto;
using Dividendos.API.Model.Response.StockSplit;
using Dividendos.API.Model.Response.v1;
using Dividendos.Application.Base;
using Dividendos.Application.Interface;
using Dividendos.Entity.Entities;
using Dividendos.Entity.Enum;
using Dividendos.Entity.Model;
using Dividendos.Entity.Views;
using Dividendos.InvestingCom.Interface;
using Dividendos.InvestingCom.Interface.Model;
using Dividendos.Repository.Interface.UoW;
using Dividendos.Service.Interface;
using K.Logger;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;

namespace Dividendos.Application
{
    public class SearchApp : BaseApp, ISearchApp
    {
        private readonly IMapper _mapper;
        private readonly IUnitOfWork _uow;
        private readonly ICacheService _cacheService;
        private readonly IStockService _stockService;
        private readonly ILogger _logger;
        private readonly IGlobalAuthenticationService _globalAuthenticationService;
        private readonly ICryptoCurrencyService _cryptoCurrencyService;

        public SearchApp(IMapper mapper,
            IUnitOfWork uow,
            ICacheService cacheService,
            IStockService stockService,
            ILogger logger,
            IGlobalAuthenticationService globalAuthenticationService,
            ICryptoCurrencyService cryptoCurrencyService)
        {
            _mapper = mapper;
            _uow = uow;
            _cacheService = cacheService;
            _stockService = stockService;
            _logger = logger;
            _globalAuthenticationService = globalAuthenticationService;
            _cryptoCurrencyService = cryptoCurrencyService;
        } 

        public ResultResponseObject<IEnumerable<StockVM>> GetByNameOrSymbol(string name)
        {
            ResultResponseObject<IEnumerable<StockVM>> result = null;

            string resultFromCache = _cacheService.GetFromCache(string.Concat("Search:", name));

            if (resultFromCache == null)
            {
                using (_uow.Create())
                {
                    ResultServiceObject<IEnumerable<StockView>> resultService = _stockService.GetByNameOrSymbol(name);
                    result = _mapper.Map<ResultResponseObject<IEnumerable<StockVM>>>(resultService);

                    //ResultServiceObject<IEnumerable<CryptoStatementView>> CryptoResultService = _cryptoCurrencyService.GetCryptosByNameOrSymbol(name);
                    //result.Value = result.Value.Concat(_mapper.Map<ResultResponseObject<IEnumerable<StockVM>>>(CryptoResultService).Value);

                    _cacheService.SaveOnCache(string.Concat("Search:", name), TimeSpan.FromHours(10), JsonConvert.SerializeObject(result));
                }
            }
            else
            {
                result = JsonConvert.DeserializeObject<ResultResponseObject<IEnumerable<StockVM>>>(resultFromCache);
                result.Success = true;
            }

            return result;
        }
    }
}