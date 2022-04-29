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
using Newtonsoft.Json;

namespace Dividendos.Application
{
    public class InitialOfferApp : IInitialOfferApp
    {
        private readonly IUnitOfWork _uow;
        private readonly IInitialOfferService _initialOfferService;
        private readonly ILogger _logger;
        private readonly IMapper _mapper;
        private readonly ICacheService _cacheService;

        public InitialOfferApp(IUnitOfWork uow,
                            IInitialOfferService initialOfferService,
                            ILogger logger,
                            IMapper mapper,
                            ICacheService cacheService)
        {
            _uow = uow;
            _initialOfferService = initialOfferService;
            _logger = logger;
            _mapper = mapper;
            _cacheService = cacheService;
        }

        public ResultResponseObject<IEnumerable<InitialOfferVM>> GetAllStocks()
        {
            return GetAllBase(true);
        }

        public ResultResponseObject<IEnumerable<InitialOfferVM>> GetAllCryptos()
        {
            return GetAllBase(false);
        }

        private ResultResponseObject<IEnumerable<InitialOfferVM>> GetAllBase(bool returnStocks)
        {
            ResultResponseObject<IEnumerable<InitialOfferVM>> result = null;

            string sufixNameCache = "Cryptos";

            if(returnStocks)
            {
                sufixNameCache = "Stocks";
            }

            string resultFromCache = _cacheService.GetFromCache(string.Concat("InitialOffer", sufixNameCache));

            if (resultFromCache == null)
            {
                using (_uow.Create())
                {
                    ResultServiceObject<IEnumerable<InitialOffer>> resultService = _initialOfferService.GetAll(returnStocks);

                    result = _mapper.Map<ResultResponseObject<IEnumerable<InitialOfferVM>>>(resultService);

                    _cacheService.SaveOnCache(string.Concat("InitialOffer", sufixNameCache), TimeSpan.FromHours(2), JsonConvert.SerializeObject(result));
                }
            }
            else
            {
                result = JsonConvert.DeserializeObject<ResultResponseObject<IEnumerable<InitialOfferVM>>>(resultFromCache);
                result.Success = true;
            }

            return result;
        }
    }
}
