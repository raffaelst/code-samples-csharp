using AutoMapper;
using Dividendos.API.Model.Request.Notification;
using Dividendos.API.Model.Response.Common;
using Dividendos.Application.Interface;
using Dividendos.CrossCutting.Identity.Models;
using Dividendos.Entity.Entities;
using Dividendos.Entity.Enum;
using Dividendos.Entity.Model;
using Dividendos.Repository.Interface.UoW;
using Dividendos.Service.Interface;
using K.Logger;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dividendos.Application
{
    public class NotificationApp : INotificationAPP
    {
        private readonly IUnitOfWork _uow;
        private readonly IUserService _userService;
        private readonly IDeviceService _deviceService;
        private readonly ILogger _logger;
        private readonly IEmailTemplateService _emailTemplateService;
        private readonly IGlobalAuthenticationService _globalAuthenticationService;
        private readonly INotificationService _notificationService;
        private readonly INotificationHistoricalService _notificationHistoricalService;
        private readonly ICacheService _cacheService;
        public NotificationApp(IUnitOfWork uow,
            IDeviceService deviceService,
            ILogger logger,
            IEmailTemplateService emailTemplateService,
            IUserService userService,
            IGlobalAuthenticationService globalAuthenticationService,
            INotificationService notificationService,
            INotificationHistoricalService notificationHistoricalService,
            ICacheService cacheService)
        {
            _uow = uow;
            _deviceService = deviceService;
            _logger = logger;
            _emailTemplateService = emailTemplateService;
            _userService = userService;
            _globalAuthenticationService = globalAuthenticationService;
            _notificationService = notificationService;
            _notificationHistoricalService = notificationHistoricalService;
            _cacheService = cacheService;
        }
        
        public async Task<ResultResponseBase> SendTestPushNotification()
        {
            ResultResponseBase result = new ResultResponseBase() { Success = false };

            ResultServiceObject<IEnumerable<Device>> devices = new ResultServiceObject<IEnumerable<Device>>();

            string title = "Tudo certo!";
            string message = "Testamos e está tudo certo com suas notificações no App Dividendos.me. Bons investimentos!";

            using (_uow.Create())
            {
                devices = _deviceService.GetByUser(_globalAuthenticationService.IdUser);
                _notificationHistoricalService.New(title, message, _globalAuthenticationService.IdUser, AppScreenNameEnum.HomeToday.ToString(), PushRedirectTypeEnum.Internal.ToString(), null, _cacheService);
            }

            foreach (var itemDevice in devices.Value)
            {
                try
                {
                    _notificationService.SendPush(title, message, itemDevice, null);
                }
                catch (Exception ex)
                {
                    _ = _logger.SendErrorAsync(ex);
                }
            }

            return result;
        }

        public void SendNotificationList(PushContentListVM pushContentListVM)
        {
            foreach (var itemUser in pushContentListVM.UserIds)
            {
                ResultServiceObject<IEnumerable<Device>> devices = new ResultServiceObject<IEnumerable<Device>>();

                PushRedirect pushRedirect = new PushRedirect()
                {
                    PushRedirectType = (Entity.Enum.PushRedirectTypeEnum)Enum.Parse(typeof(Entity.Enum.PushRedirectTypeEnum), pushContentListVM.PushBasicContent.PushRedirectType.ToString()),
                    ExternalRedirectURL = pushContentListVM.PushBasicContent.ExternalRedirectURL
                };

                if (pushContentListVM.PushBasicContent.AppScreenNameType != null)
                {
                    pushRedirect.AppScreenName = (Entity.Enum.AppScreenNameEnum)Enum.Parse(typeof(Entity.Enum.AppScreenNameEnum), pushContentListVM.PushBasicContent.AppScreenNameType.ToString());
                }

                using (_uow.Create())
                {
                    devices = _deviceService.GetByUser(itemUser);
                    _notificationHistoricalService.New(pushContentListVM.PushBasicContent.Title, pushContentListVM.PushBasicContent.Message, itemUser, pushRedirect.AppScreenName.ToString(), pushContentListVM.PushBasicContent.PushRedirectType.ToString(), pushRedirect.ExternalRedirectURL, _cacheService);
                }

                foreach (var itemDevice in devices.Value)
                {
                    try
                    {
                        _notificationService.SendPush(pushContentListVM.PushBasicContent.Title, pushContentListVM.PushBasicContent.Message, itemDevice, pushRedirect);
                    }
                    catch (Exception ex)
                    {
                        _ = _logger.SendErrorAsync(ex);
                    }
                }
            }
        }
    }
}
