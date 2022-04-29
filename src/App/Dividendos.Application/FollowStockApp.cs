using AutoMapper;
using K.Logger;
using Dividendos.API.Model.Response;
using Dividendos.API.Model.Response.Common;

using Dividendos.Application.Base;
using Dividendos.Application.Interface;
using Dividendos.Entity.Entities;
using Dividendos.Entity.Model;
using Dividendos.Finance.Interface;
using Dividendos.Finance.Interface.Model;
using Dividendos.Repository.Interface.UoW;
using Dividendos.Service.Interface;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;
using Dividendos.Application.Interface.Model;
using Dividendos.Entity.Views;
using Dividendos.TradeMap.Interface.Model;
using Dividendos.TradeMap.Interface;
using Dividendos.CrossCutting.Identity.Models;
using Newtonsoft.Json;
using Dividendos.Entity.Enum;
using Dividendos.API.Model.Request.Stock;
using System.Resources;
using Dividendos.API.Model.Request.FollowStock;

namespace Dividendos.Application
{
    public class FollowStockApp : BaseApp, IFollowStockApp
    {
        private readonly IMapper _mapper;
        private readonly IUnitOfWork _uow;
        private readonly IFollowStockService _followStockService;
        private readonly IGlobalAuthenticationService _globalAuthenticationService;
        private readonly IStockService _stockService;
        private readonly INotificationService _notificationService;
        private readonly IDeviceService _deviceService;
        private readonly ILogger _logger;
        private readonly IFollowStockAlertService _followStockAlertService;
        private readonly INotificationHistoricalService _notificationHistoricalService;
        private readonly ICacheService _cacheService;
        private readonly ICryptoCurrencyService _cryptoCurrencyService;

        public FollowStockApp(IMapper mapper,
            IUnitOfWork uow,
            IFollowStockService followStockService,
            IGlobalAuthenticationService globalAuthenticationService,
            IStockService stockService,
            INotificationService notificationService,
            ICryptoCurrencyService cryptoCurrencyService,
            IDeviceService deviceService,
            ILogger logger,
            IFollowStockAlertService followStockAlertService,
            INotificationHistoricalService notificationHistoricalService,
            ICacheService cacheService)
        {
            _mapper = mapper;
            _uow = uow;
            _followStockService = followStockService;
            _globalAuthenticationService = globalAuthenticationService;
            _stockService = stockService;
            _notificationService = notificationService;
            _deviceService = deviceService;
            _logger = logger;
            _followStockAlertService = followStockAlertService;
            _notificationHistoricalService = notificationHistoricalService;
            _cacheService = cacheService;
            _cryptoCurrencyService = cryptoCurrencyService;
        }


        public void CheckFollowStockAlerts()
        {
            using (_uow.Create())
            {
                //Obtem todos alertas ativos e as cotações das ações relacionadas a eles no momento atual
                ResultServiceObject<IEnumerable<FollowStockView>> resultService = _followStockAlertService.GetAllAlertsActiveAndNotReached();

                _followStockAlertService.ApplyAlertsRules(resultService.Value, _notificationService, _deviceService, _notificationHistoricalService, _cacheService, _logger);
            }
        }


        public ResultResponseObject<FollowStockVM> Add(FollowStockVM followStockVM)
        {
            ResultResponseObject<FollowStockVM> result;

            using (_uow.Create())
            {
                FollowStock followStock = _mapper.Map<FollowStock>(followStockVM);

                var stock = _stockService.GetByGuid(followStockVM.StockGuid);

                long stockIdOrCryptoId = 0;

                if (stock.Value != null)
                {
                    stockIdOrCryptoId = stock.Value.IdStock;
                    followStock.StockID = stockIdOrCryptoId;
                    followStock.CryptoCurrencyID = null;
                }
                else
                {
                    var crypto = _cryptoCurrencyService.GetByGuid(followStockVM.StockGuid);
                    stockIdOrCryptoId = crypto.Value.CryptoCurrencyID;
                    followStock.CryptoCurrencyID = stockIdOrCryptoId;
                    followStock.StockID = null;
                }

                followStock.UserID = _globalAuthenticationService.IdUser;

                ResultServiceObject<FollowStock> resultService = _followStockService.ExistForASpecificUser(stockIdOrCryptoId, _globalAuthenticationService.IdUser);

                if (resultService.Value == null)
                {
                    resultService = _followStockService.Add(followStock);
                }

                result = _mapper.Map<ResultResponseObject<FollowStockVM>>(resultService);
            }

            return result;
        }

        public ResultResponseObject<FollowStockAlertResponseVM> AddAlert(FollowStockAlertVM followStockVM)
        {
            ResultResponseObject<FollowStockAlertResponseVM> result;

            using (_uow.Create())
            {
                FollowStockAlert followStockAlert = _mapper.Map<FollowStockAlert>(followStockVM);
                decimal targetPrice = 0;
                decimal.TryParse(followStockVM.TargetPrice.Replace(".", string.Empty), NumberStyles.Currency, new CultureInfo("pt-br"), out targetPrice);
                followStockAlert.TargetPrice = targetPrice;
                followStockAlert.FollowStockTypeID = (int)followStockVM.FollowStockType;
                followStockAlert.FollowStockID = _followStockService.GetByGuid(followStockVM.FollowStockGuid).Value.FollowStockID;
                ResultServiceObject<FollowStockAlert> resultService = _followStockAlertService.Add(followStockAlert);

                result = _mapper.Map<ResultResponseObject<FollowStockAlertResponseVM>>(resultService);
            }

            return result;
        }

        public ResultResponseBase Delete(Guid followGuid)
        {
            ResultResponseBase result;

            using (_uow.Create())
            {
                //Get
                ResultServiceObject<FollowStock> resultService = _followStockService.GetByGuid(followGuid);

                resultService.Value.Active = false;

                //update delete related alerts
                ResultServiceObject<IEnumerable<FollowStockAlert>> resultServiceObject = _followStockAlertService.GetAllActiveAlertsByFollowStock(followGuid);

                foreach (FollowStockAlert itemFollowStockAlert in resultServiceObject.Value)
                {
                    itemFollowStockAlert.Active = false;
                    _followStockAlertService.Update(itemFollowStockAlert);
                }

                //update delete
                resultService  = _followStockService.Update(resultService.Value);

                result = _mapper.Map<ResultResponseBase>(resultService);
            }

            return result;
        }

        public ResultResponseBase DeleteAlert(Guid followAlertGuid)
        {
            ResultResponseBase result;

            using (_uow.Create())
            {
                //Get
                ResultServiceObject<FollowStockAlert> resultService = _followStockAlertService.GetByGuid(followAlertGuid);

                resultService.Value.Active = false;

                //update deleted
                resultService = _followStockAlertService.Update(resultService.Value);

                result = _mapper.Map<ResultResponseBase>(resultService);
            }

            return result;
        }

        public ResultResponseObject<IEnumerable<FollowStockAlertResponseVM>> GetAllActiveAlertsByFollowStock(Guid followStockGuid)
        {
            ResultResponseObject<IEnumerable<FollowStockAlertResponseVM>> resultResponseObject = null;

            using (_uow.Create())
            {
                ResultServiceObject<IEnumerable<FollowStockAlertView>> resultService = _followStockAlertService.GetAllActiveAlertsViewByFollowStock(followStockGuid);

                resultResponseObject = _mapper.Map<ResultResponseObject<IEnumerable<FollowStockAlertResponseVM>>>(resultService);
            }

            return resultResponseObject;
        }

        public ResultResponseObject<IEnumerable<FollowStockResponseVM>> GetAllActiveByUser()
        {
            ResultResponseObject<IEnumerable<FollowStockResponseVM>> resultResponseObject = null;

            using (_uow.Create())
            {
                ResultServiceObject<IEnumerable<FollowStockView>> resultService = _followStockService.GetAllActiveByUser(_globalAuthenticationService.IdUser);

                resultResponseObject = _mapper.Map<ResultResponseObject<IEnumerable<FollowStockResponseVM>>>(resultService);
            }

            return resultResponseObject;
        }

        public ResultResponseObject<IEnumerable<FollowStockResponseVM>> GetByIdStockOrIdCrypto(string idStockOrCrypto)
        {
            ResultResponseObject<IEnumerable<FollowStockResponseVM>> resultResponseObject = null;

            using (_uow.Create())
            {
                ResultServiceObject<IEnumerable<FollowStockView>> resultService = _followStockService.GetByIdStockOrIdCrypto(idStockOrCrypto, _globalAuthenticationService.IdUser);

                resultResponseObject = _mapper.Map<ResultResponseObject<IEnumerable<FollowStockResponseVM>>>(resultService);
            }

            return resultResponseObject;
        }
    }
}