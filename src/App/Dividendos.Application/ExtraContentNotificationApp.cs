using AutoMapper;
using Dividendos.API.Model.Request.Notification;
using Dividendos.API.Model.Response.Common;
using Dividendos.API.Model.Response.Notification;
using Dividendos.Application.Base;
using Dividendos.Application.Interface;
using Dividendos.CrossCutting.Identity.Models;
using Dividendos.Entity.Entities;
using Dividendos.Entity.Enum;
using Dividendos.Entity.Model;
using Dividendos.Entity.Views;
using Dividendos.Repository.Interface.UoW;
using Dividendos.Service.Interface;
using K.Logger;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Dividendos.Application
{
    public class ExtraContentNotificationApp : BaseApp, IExtraContentNotificationApp
    {
        private readonly IMapper _mapper;
        private readonly IUnitOfWork _uow;
        private readonly ILogger _logger;
        private readonly IExtraContentNotificationService _extraContentNotificationService;
        private readonly INotificationHistoricalService _notificationHistoricalService;
        private readonly ISettingsService _settingsService;
        private readonly INotificationService _notificationService;
        private readonly IUserService _userService;
        private readonly IDeviceService _deviceService;
        private readonly ICacheService _cacheService;
        public ExtraContentNotificationApp(IMapper mapper,
            IUnitOfWork uow,
            ILogger logger,
            IDeviceService deviceService,
            IUserService userService,
            IExtraContentNotificationService extraContentNotificationService,
            INotificationService notificationService,
            INotificationHistoricalService notificationHistoricalService,
            ISettingsService settingsService,
            ICacheService cacheService)
        {
            _mapper = mapper;
            _uow = uow;
            _logger = logger;
            _extraContentNotificationService = extraContentNotificationService;
            _notificationService = notificationService;
            _notificationHistoricalService = notificationHistoricalService;
            _settingsService = settingsService;
            _deviceService = deviceService;
            _userService = userService;
            _cacheService = cacheService;
        }

        public ResultResponseObject<PushContentResponseVM> SendExtraContentNotification(PushContentVM pushContentVM)
        {
            ResultResponseObject<PushContentResponseVM> result;

            using (_uow.Create())
            {
                ExtraContentNotification extraContentNotification = _mapper.Map<ExtraContentNotification>(pushContentVM);

                ResultServiceObject<int> count = _userService.CountAllUserWithRecentActivitiesOnApp((PushTargetTypeEnum)extraContentNotification.PushTargetType);

                extraContentNotification.TotalAmountOfRegisters = count.Value;

                ResultServiceObject<ExtraContentNotification> resultServiceObject = _extraContentNotificationService.Add(extraContentNotification);

                result = _mapper.Map<ResultResponseObject<PushContentResponseVM>>(resultServiceObject);
            }

            return result;
        }

        public void CheckAndSendExtraContentPushNotification(int amountItensPerAgent)
        {
            ResultServiceObject<IEnumerable<ApplicationUser>> users;

            ResultServiceObject<ExtraContentNotification> resultServiceExtraContentNotification = new ResultServiceObject<ExtraContentNotification>();

            using (_uow.Create())
            {  
                //Get ExtraContentPushNotification in correct time
                resultServiceExtraContentNotification = _extraContentNotificationService.GetLastAvailable();

                if (resultServiceExtraContentNotification.Value != null && !resultServiceExtraContentNotification.Value.Complete)
                {
                    _ = _logger.SendDebugAsync(new { ExtraContentPushNotification = string.Format("Iniciando CheckAndSendExtraContentPushNotification with {0} items", amountItensPerAgent) });

                    resultServiceExtraContentNotification = _extraContentNotificationService.UpdateIterationSequence(resultServiceExtraContentNotification.Value);
                }
            }


            if (resultServiceExtraContentNotification.Value != null && !resultServiceExtraContentNotification.Value.Complete)
            {
                using (_uow.Create())
                {
                    //Obtemos as informações paginadas
                    users = _userService.GetAllUserWithRecentActivitiesOnAppPaged(resultServiceExtraContentNotification.Value.AgentIterationSequence, amountItensPerAgent, resultServiceExtraContentNotification.Value.PushTargetType);

                    //Verificamos se existem usuários disponíveis para enviar. caso não exista, atualizamos o envio como concluído
                    if (users.Value.Count() == 0)
                    {
                        _extraContentNotificationService.SubmissionComplete(resultServiceExtraContentNotification.Value);
                    }
                }

                List<PushNotificationView> pushNotificationViews = new List<PushNotificationView>();

                foreach (var userItem in users.Value)
                {
                    ResultServiceObject<IEnumerable<Device>> devices = new ResultServiceObject<IEnumerable<Device>>();

                    using (_uow.Create())
                    {
                        ResultServiceObject<Settings> settings = _settingsService.GetByUser(userItem.Id);

                        if ((resultServiceExtraContentNotification.Value.ExtraContentNotificationTypeID == (int)PushContentType.TopicDividendBR) ||
                            (resultServiceExtraContentNotification.Value.ExtraContentNotificationTypeID == (int)PushContentType.TopicRelevantFact) ||
                            (resultServiceExtraContentNotification.Value.ExtraContentNotificationTypeID == (int)PushContentType.TopicDividendEUA))
                        {
                            try
                            {
                                _notificationService.SendPushToTopic(resultServiceExtraContentNotification.Value.Title, resultServiceExtraContentNotification.Value.Message, ((PushContentType)resultServiceExtraContentNotification.Value.ExtraContentNotificationTypeID).ToString());
                            }
                            catch (Exception ex)
                            {
                                _ = _logger.SendErrorAsync(ex);
                            }
                        }
                        else if ((resultServiceExtraContentNotification.Value.ExtraContentNotificationTypeID == (int)PushContentType.General) ||
                            (resultServiceExtraContentNotification.Value.ExtraContentNotificationTypeID == (int)PushContentType.News &&
                            settings.Value == null ||
                            settings.Value.PushBreakingNews))
                        {
                            devices = _deviceService.GetByUser(userItem.Id);
                            _notificationHistoricalService.New(resultServiceExtraContentNotification.Value.Title,
                                resultServiceExtraContentNotification.Value.Message,
                                userItem.Id,
                                resultServiceExtraContentNotification.Value.AppScreenName,
                                resultServiceExtraContentNotification.Value.PushRedirectType,
                                resultServiceExtraContentNotification.Value.ExternalRedirectURL,
                                _cacheService);
                        }
                    }

                    if (devices.Value != null)
                    {
                        foreach (var itemDevice in devices.Value)
                        {
                            pushNotificationViews.Add(new PushNotificationView() { Device = itemDevice, Message = resultServiceExtraContentNotification.Value.Message, Title = resultServiceExtraContentNotification.Value.Title });
                        }
                    }
                }

                PushRedirect pushRedirect = new PushRedirect() { PushRedirectType = (Entity.Enum.PushRedirectTypeEnum)Enum.Parse(typeof(Entity.Enum.PushRedirectTypeEnum), resultServiceExtraContentNotification.Value.PushRedirectType), ExternalRedirectURL = resultServiceExtraContentNotification.Value.ExternalRedirectURL };

                if (!string.IsNullOrEmpty(resultServiceExtraContentNotification.Value.AppScreenName))
                {
                    pushRedirect.AppScreenName = (Entity.Enum.AppScreenNameEnum)Enum.Parse(typeof(Entity.Enum.AppScreenNameEnum), resultServiceExtraContentNotification.Value.AppScreenName);
                }

                _notificationService.SendPushParallel(pushNotificationViews, 500, pushRedirect);
                _ = _logger.SendDebugAsync(new { ExtraContentPushNotification = "finalizando CheckAndSendExtraContentPushNotification" });
            }
        }

    }
}