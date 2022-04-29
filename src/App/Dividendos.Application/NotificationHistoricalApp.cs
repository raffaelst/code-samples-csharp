using AutoMapper;
using Dividendos.API.Model.Request.Settings;
using Dividendos.API.Model.Response;
using Dividendos.API.Model.Response.Common;
using Dividendos.API.Model.Response.NotificationHistorical;
using Dividendos.Application.Base;
using Dividendos.Application.Interface;
using Dividendos.Entity.Entities;
using Dividendos.Entity.Model;
using Dividendos.Repository.Interface.UoW;
using Dividendos.Service.Interface;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Dividendos.Application
{
    public class NotificationHistoricalApp : BaseApp, INotificationHistoricalApp
    {
        private readonly INotificationHistoricalService _notificationHistoricalService;
        private readonly IGlobalAuthenticationService _globalAuthenticationService;
        private readonly IMapper _mapper;
        private readonly IUnitOfWork _uow;
        private readonly ICacheService _cacheService;

        public NotificationHistoricalApp(IMapper mapper,
            IUnitOfWork uow,
            INotificationHistoricalService notificationHistoricalService,
            IGlobalAuthenticationService globalAuthenticationService,
            ICacheService cacheService)
        {
            _notificationHistoricalService = notificationHistoricalService;
            _mapper = mapper;
            _uow = uow;
            _globalAuthenticationService = globalAuthenticationService;
            _cacheService = cacheService;
        }

        public ResultResponseObject<IEnumerable<NotificationHistoricalVM>> GetAllByUser()
        {
            ResultResponseObject<IEnumerable<NotificationHistoricalVM>> result = null;

            //string resultFromCache = _cacheService.GetFromCache(string.Concat("NotificationHistorical:", _globalAuthenticationService.IdUser));

            //if (resultFromCache == null)
            //{
                using (_uow.Create())
                {
                    ResultServiceObject<IEnumerable<NotificationHistorical>> resultService = _notificationHistoricalService.GetAllByUser(_globalAuthenticationService.IdUser);

                    result = _mapper.Map<ResultResponseObject<IEnumerable<NotificationHistoricalVM>>>(resultService);

                    //_cacheService.SaveOnCache(string.Concat("NotificationHistorical:", _globalAuthenticationService.IdUser), TimeSpan.FromHours(6), JsonConvert.SerializeObject(result));
                }
            //}
            //else
            //{
            //    result = JsonConvert.DeserializeObject<ResultResponseObject<IEnumerable<NotificationHistoricalVM>>>(resultFromCache);
            //    result.Success = true;
            //}

            return result;
        }

        public ResultResponseObject<NotificationHistoricalVM> InactivateByGuid(string notificationHistoricalGuid)
        {
            ResultServiceObject<NotificationHistorical> resultServiceObject = null;

            using (_uow.Create())
            {
                resultServiceObject = _notificationHistoricalService.GetByGuid(Guid.Parse(notificationHistoricalGuid));

                if (resultServiceObject.Value != null)
                {
                    _notificationHistoricalService.Disable(resultServiceObject.Value.NotificationHistoricalId);
                }
            }

            //_cacheService.DeleteOnCache(string.Concat("NotificationHistorical:", _globalAuthenticationService.IdUser));
            //_cacheService.DeleteOnCache(string.Concat("AccountDetailsWithNotification:", _globalAuthenticationService.IdUser));

            ResultResponseObject<NotificationHistoricalVM> result = _mapper.Map<ResultResponseObject<NotificationHistoricalVM>>(resultServiceObject);

            return result;
        }

        public ResultResponseBase InactivateAllByUser()
        {
            ResultResponseBase resultService = new ResultResponseBase() { Success = false };

            using (_uow.Create())
            {
                ResultServiceObject<IEnumerable<NotificationHistorical>> notifications = _notificationHistoricalService.GetAllByUser(_globalAuthenticationService.IdUser);

                foreach (var itemNotification in notifications.Value)
                {
                    _notificationHistoricalService.Disable(itemNotification.NotificationHistoricalId);
                }

                resultService.Success = true;
            }

            //_cacheService.DeleteOnCache(string.Concat("NotificationHistorical:", _globalAuthenticationService.IdUser));
            //_cacheService.DeleteOnCache(string.Concat("AccountDetailsWithNotification:", _globalAuthenticationService.IdUser));

            return resultService;
        }
    }
}