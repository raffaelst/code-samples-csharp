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
using Microsoft.Extensions.Caching.Distributed;
using Newtonsoft.Json;
using Dividendos.API.Model.Request.Goals;
using Dividendos.API.Model.Request;
using Dividendos.RDStation.Interface;
using Dividendos.RDStation.Interface.Model.Response;
using Dividendos.API.Model.Request.RDStation;

namespace Dividendos.Application
{
    public class TaxesApp : ITaxesApp
    {
        private readonly IUnitOfWork _uow;
        private readonly IMapper _mapper;
        private readonly INotificationHistoricalService _notificationHistoricalService;
        private readonly ILogger _logger;
        private readonly INotificationService _notificationService;
        private readonly ICacheService _cacheService;
        private readonly IDeviceService _deviceService;
        private readonly IUserService _userService;

        public TaxesApp(IUnitOfWork uow,
                            IMapper mapper,
                            INotificationHistoricalService notificationHistoricalService,
                            ILogger logger,
                            INotificationService notificationService,
                            ICacheService cacheService,
                            IDeviceService deviceService,
                            IUserService userService)
        {
            _uow = uow;
            _mapper = mapper;
            _notificationHistoricalService = notificationHistoricalService;
            _logger = logger;
            _notificationService = notificationService;
            _cacheService = cacheService;
            _deviceService = deviceService;
            _userService = userService;
        }

        public void SendEvent(EventData eventData)
        {
            //_rDStationHelper.SendEvent(eventData.Name, eventData.Email, eventData.UserId, eventData.Tags, eventData.EventType);
        }

        public void ReceiveInformationFromRDStationAndSendPushNotification(dynamic rootWebHook, RDStationPushNotificationType rDStationPushNotificationType)
        {
            _logger.SendDebugAsync(new { ReceiveInformationFromRDStationAndSendPushNotification = rootWebHook }, "ReceiveInformationFromRDStationAndSendPushNotification");

            string pushMessage = null;
            string pushMessageTitle = null;
            string link = null;

            switch (rDStationPushNotificationType)
            {
                case RDStationPushNotificationType.ToroInvestimentos:
                    {
                        pushMessage = "Clique neste comunicado, preencha o formulário, faça o depósito inicial (de qualquer valor) para ativar sua conta Toro e pronto! Sua assinatura estará disponível em até 3 dias úteis! E você também vai ter acesso a melhor corretora do Brasil, com taxas 0 para operações em renda variável.";
                        pushMessageTitle = "Não perca tempo! Assinatura grátis do App Dividendos.me pra você!";
                        link = "https://conteudos.toroinvestimentos.com.br/plataformas/dividendosme";
                        break;
                    }
                case RDStationPushNotificationType.RequestSubscribe:
                    {
                        pushMessage = "Experimente a assinatura do app Dividendos.me por 3 dias grátis! Caso prefira, solicite a assinatura anual através da Toro Investimentos ou do Banco Safra de forma 100% gratuita. Entre no seu app e saiba como solicitar.";
                        pushMessageTitle = "Já experimentou a assinatura Dividendos.me?";
                        break;
                    }
                case RDStationPushNotificationType.Safra:
                    {
                        pushMessage = "Clique neste comunicado, preencha o formulário e pronto! Sua assinatura estará disponível em até 3 dias úteis! E você também receberá uma conta de investimentos Safra totalmente sem taxas.";
                        pushMessageTitle = "Quer receber um ano de app Dividendos.me de graça?";
                        link = "https://conteudo.dividendos.me/parceria-direct-capital";
                        break;
                    }
            }

            string email = null;

            foreach (var item in rootWebHook.leads)
            {
                email = item.email;
                break;
            }

            if (!string.IsNullOrEmpty(pushMessage) && !string.IsNullOrEmpty(email))
            {
                using (_uow.Create())
                {
                    var user = _userService.GetByEmail(email);

                    if (user.Value != null)
                    {
                        ResultServiceObject<IEnumerable<Device>> devices = _deviceService.GetByUser(user.Value.Id);

                        if (string.IsNullOrEmpty(link))
                        {
                            _notificationHistoricalService.New(pushMessageTitle, pushMessage, user.Value.Id, AppScreenNameEnum.HomeToday.ToString(), PushRedirectTypeEnum.Internal.ToString(), null, _cacheService);
                        }
                        else
                        {
                            _notificationHistoricalService.New(pushMessageTitle, pushMessage, user.Value.Id, null, PushRedirectTypeEnum.External.ToString(), link, _cacheService);
                        }

                        foreach (Device itemDevice in devices.Value)
                        {
                            try
                            {
                                if (string.IsNullOrEmpty(link))
                                {
                                    _notificationService.SendPush(pushMessageTitle, pushMessage, itemDevice, new PushRedirect() { PushRedirectType = PushRedirectTypeEnum.Internal, AppScreenName = AppScreenNameEnum.HomeToday });
                                }
                                else
                                {
                                    _notificationService.SendPush(pushMessageTitle, pushMessage, itemDevice, new PushRedirect() { PushRedirectType = PushRedirectTypeEnum.External, ExternalRedirectURL = link });
                                }
                            }
                            catch (Exception exception)
                            {
                                _logger.SendErrorAsync(exception);
                            }
                        }
                    }
                }
            }
        }

    }
}
