using AutoMapper;
using Dividendos.API.Model.PurchaseAPI;
using Dividendos.API.Model.Request.Purchase;
using Dividendos.API.Model.Response;
using Dividendos.API.Model.Response.Common;
using Dividendos.API.Model.Response.Purchase;
using Dividendos.Application.Base;
using Dividendos.Application.Interface;
using Dividendos.CrossCutting.Identity.Models;
using Dividendos.Entity.Entities;
using Dividendos.Entity.Enum;
using Dividendos.Entity.Model;
using Dividendos.RDStation.Interface;
using Dividendos.Repository.Interface.UoW;
using Dividendos.Service.Interface;
using K.Logger;
using Microsoft.AspNetCore.Identity;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;

namespace Dividendos.Application
{
    public class PurchaseApp : BaseApp, IPurchaseApp
    {
        private readonly int _emailNewSubscriberTemplateId = 3;

        private readonly IMapper _mapper;
        private readonly IUnitOfWork _uow;
        private readonly IGlobalAuthenticationService _globalAuthenticationService;
        private readonly ILogger _logger;
        private readonly ISubscriptionService _subscriptionService;
        private readonly IEmailTemplateService _emailTemplateService;
        private readonly INotificationService _notificationService;
        private readonly IUserService _userService;
        private readonly IDeviceService _deviceService;
        private readonly ITraderService _traderService;
        private readonly INotificationHistoricalService _notificationHistoricalService;
        private readonly ICacheService _cacheService;
        private readonly ISystemSettingsService _systemSettingsService;
        private readonly IRDStationHelper _rDStationHelper;

        public PurchaseApp(IMapper mapper,
            IUnitOfWork uow,
            IGlobalAuthenticationService globalAuthenticationService,
            ILogger logger,
            ISubscriptionService subscriptionService,
            INotificationService notificationService,
            IUserService userService,
            IEmailTemplateService emailTemplateService,
            IDeviceService deviceService,
            ITraderService traderService,
            INotificationHistoricalService notificationHistoricalService,
            ICacheService cacheService,
            ISystemSettingsService systemSettingsService,
            IRDStationHelper rDStationHelper)
        {
            _mapper = mapper;
            _uow = uow;
            _globalAuthenticationService = globalAuthenticationService;
            _logger = logger;
            _subscriptionService = subscriptionService;
            _userService = userService;
            _emailTemplateService = emailTemplateService;
            _notificationService = notificationService;
            _deviceService = deviceService;
            _traderService = traderService;
            _notificationHistoricalService = notificationHistoricalService;
            _cacheService = cacheService;
            _systemSettingsService = systemSettingsService;
            _rDStationHelper = rDStationHelper;
        }

        public ResultResponseObject<Subscribe> SubscribeDetails(string userID)
        {
            ResultResponseObject<Subscribe> resultResponseObject = new ResultResponseObject<Subscribe>() { Success = true };

            Subscribe subscribe = new Subscribe() { Active = false, ValidUntil = DateTime.Now };

            using (_uow.Create())
            {
                if (userID == null)
                {
                    userID = _globalAuthenticationService.IdUser;
                }

                ResultServiceObject<Subscription> resultServiceObject = _subscriptionService.GetByUser(userID);

                if (resultServiceObject.Value != null)
                {
                    subscribe.Active = resultServiceObject.Value.Active;

                    switch (resultServiceObject.Value.SubscriptionTypeID)
                    {
                        case (int)SubscriptionTypeEnum.Gold:
                            {
                                subscribe.ProductIdentifier = "gold_plan";
                                break;
                            }
                        case (int)SubscriptionTypeEnum.Silver:
                            {
                                subscribe.ProductIdentifier = "silver_plan";
                                break;
                            }
                        case (int)SubscriptionTypeEnum.Annuity:
                            {
                                subscribe.ProductIdentifier = "gold_plan";
                                break;
                            }
                    }

                    subscribe.ValidUntil = resultServiceObject.Value.ValidUntil;
                    if(subscribe.ValidUntil < DateTime.Now)
                    {
                        subscribe.Active = false;
                    }
                }
            }

            resultResponseObject.Value = subscribe;

            return resultResponseObject;
        }

        public ResultResponseObject<List<Dividendos.API.Model.Response.Purchase.ProductVM>> GetProducts()
        {
            ResultResponseObject<List<Dividendos.API.Model.Response.Purchase.ProductVM>> resultResponseObject = new ResultResponseObject<List<API.Model.Response.Purchase.ProductVM>>() { Success = true };
            List<Dividendos.API.Model.Response.Purchase.ProductVM> products = new List<API.Model.Response.Purchase.ProductVM>();
            products.Add(new ProductVM() { Price = "9,50", Name = "Plano Ouro Mensal", Type = "Mensal", Discount = "0", Sku = "gold_new", OldPrice = "15,90" });
            products.Add(new ProductVM() { Price = "99,90", Name = "Plano Ouro Anual", Type = "Anual", Discount = "15", Sku = "annuity_new2", OldPrice = "150,90" });
            products.Add(new ProductVM() { Price = "0,00", Name = "Plano Ouro Anual (Toro Investimentos)", Type = "Anual", Discount = "100", Sku = "annuity_toro", OldPrice = "150,90", PartnerGuid = "a091b454-29e7-4907-9d7e-02cb73306e6e" });

            resultResponseObject.Value = products;

            return resultResponseObject;
        }

        public ResultResponseObject<Subscribe> GetSubscribeDetailsByEmail(string email)
        {
            ResultResponseObject<Subscribe> resultResponseObject = new ResultResponseObject<Subscribe>() { Success = true };

            ResultServiceObject<ApplicationUser> user = new ResultServiceObject<ApplicationUser>();

            using (_uow.Create())
            {
                user = _userService.GetByEmail(email);
            }

            if (user.Value != null)
            {
                resultResponseObject = this.SubscribeDetailsV2(user.Value.Id);
            }

            return resultResponseObject;
        }

        public ResultResponseObject<Subscribe> SubscribeDetailsV2(string userID)
        {
            ResultResponseObject<Subscribe> resultResponseObject = new ResultResponseObject<Subscribe>() { Success = true };

            Subscribe subscribe = new Subscribe() { Active = false, ValidUntil = DateTime.Now };

            using (_uow.Create())
            {
                if (userID == null)
                {
                    userID = _globalAuthenticationService.IdUser;
                }

                ResultServiceObject<Subscription> resultServiceObject = _subscriptionService.GetByUser(userID);

                if (resultServiceObject.Value != null)
                {
                    subscribe.Active = resultServiceObject.Value.Active;

                    switch (resultServiceObject.Value.SubscriptionTypeID)
                    {
                        case (int)SubscriptionTypeEnum.Gold:
                            {
                                subscribe.ProductIdentifier = "gold_plan";
                                break;
                            }
                        case (int)SubscriptionTypeEnum.Silver:
                            {
                                subscribe.ProductIdentifier = "silver_plan";
                                break;
                            }
                        case (int)SubscriptionTypeEnum.Annuity:
                        case (int)SubscriptionTypeEnum.AnnuityToro:
                            {
                                subscribe.ProductIdentifier = "annuity";
                                break;
                            }
                    }

                    subscribe.ValidUntil = resultServiceObject.Value.ValidUntil;
                    subscribe.PartnerID = resultServiceObject.Value.PartnerID;
                    subscribe.CreatedDate = resultServiceObject.Value.CreatedDate;
                    subscribe.SubscriptionID = resultServiceObject.Value.SubscriptionID;
                    if (subscribe.ValidUntil < DateTime.Now)
                    {
                        subscribe.Active = false;
                    }
                }
            }

            resultResponseObject.Value = subscribe;

            return resultResponseObject;
        }

        public void GetPurchaseDetailsFromIapHub(string idPurchase)
        {
            Subscribe subscribe = new Subscribe() { Active = false, ValidUntil = DateTime.Now };

            var handler = new HttpClientHandler();
            handler.UseCookies = false;

            using (var httpClient = new HttpClient(handler))
            {
                using (var request = new HttpRequestMessage(new HttpMethod("GET"), string.Format("https://api.iaphub.com/v1/app/5ee61d4c73d61a0e99de0b44/purchase/{0}", idPurchase)))
                {
                    request.Headers.TryAddWithoutValidation("Authorization", "ApiKey VdxXQAJTQ4IWecizlxhmHHcGUDgoumV");

                    var response = httpClient.SendAsync(request).Result;

                    if (response != null)
                    {
                        dynamic resultAPI = JsonConvert.DeserializeObject<ExpandoObject>(response.Content.ReadAsStringAsync().Result);

                        SubscriptionTypeEnum subscriptionTypeEnum = SubscriptionTypeEnum.Gold;

                        if (resultAPI.isSubscription)
                        {
                            string userId = resultAPI.userId;
                            subscribe.ProductIdentifier = resultAPI.productSku;
                            subscribe.ValidUntil = resultAPI.expirationDate;
                            subscribe.Active = resultAPI.isSubscriptionActive;

                            switch (subscribe.ProductIdentifier)
                            {
                                case "gold_plan":
                                    {
                                        subscriptionTypeEnum = SubscriptionTypeEnum.Gold;
                                        break;
                                    }
                                case "gold_new":
                                    {
                                        subscriptionTypeEnum = SubscriptionTypeEnum.Gold;
                                        break;
                                    }
                                case "silver_plan":
                                    {
                                        subscriptionTypeEnum = SubscriptionTypeEnum.Silver;
                                        break;
                                    }
                                case "annuity":
                                    {
                                        subscriptionTypeEnum = SubscriptionTypeEnum.Annuity;
                                        break;
                                    }
                                case "annuity_new2":
                                    {
                                        subscriptionTypeEnum = SubscriptionTypeEnum.Annuity;
                                        break;
                                    }
                            }


                            using (_uow.Create())
                            {
                                ResultServiceObject<Subscription> resultServiceObject = _subscriptionService.GetByUser(userId);

                                if (resultServiceObject.Value != null)
                                {
                                    if ((resultServiceObject.Value.SubscriptionTypeID == (int)SubscriptionTypeEnum.Annuity && 
                                    resultServiceObject.Value.Active && 
                                    resultServiceObject.Value.ValidUntil >= DateTime.Now)
                                     && subscriptionTypeEnum == SubscriptionTypeEnum.Gold)
                                    {
                                        _logger.SendInformationAsync(new { IAPHubWebhooks = "Cancelamento de produto gold depois de ter assinado o anual" });
                                    }
                                    else if ((resultServiceObject.Value.PartnerID != null &&
                                    resultServiceObject.Value.Active && 
                                    resultServiceObject.Value.ValidUntil >= DateTime.Now)
                                     && subscriptionTypeEnum == SubscriptionTypeEnum.Gold)
                                    {
                                        _logger.SendInformationAsync(new { IAPHubWebhooks = "Cancelamento de produto gold depois de ter assinado pelo parceiro" });
                                    }
                                    else if ((resultServiceObject.Value.PartnerID != null &&
                                    resultServiceObject.Value.Active && 
                                    resultServiceObject.Value.ValidUntil >= DateTime.Now
                                    )
                                     && subscriptionTypeEnum == SubscriptionTypeEnum.Annuity)
                                    {
                                        _logger.SendInformationAsync(new { IAPHubWebhooks = "Cancelamento de produto gold ANUAL depois de ter assinado pelo parceiro" });
                                    }
                                    else
                                    {
                                        resultServiceObject.Value.Active = subscribe.Active;
                                        resultServiceObject.Value.ValidUntil = subscribe.ValidUntil;
                                        resultServiceObject.Value.SubscriptionTypeID = (int)subscriptionTypeEnum;

                                        if (resultAPI.isSubscriptionRenewable == false)
                                        {
                                            var user = _userService.GetByID(userId);
                                            _rDStationHelper.SendEvent(null, user.Value.Email, userId, null, user.Value.PhoneNumber, RDStation.Interface.Model.Request.EventType.GoldSubscriptionEnded);
                                        }

                                        _subscriptionService.Update(resultServiceObject.Value);
                                    }
                                }
                                else
                                {
                                    Subscription subscription = new Subscription();
                                    subscription.IdUser = userId;
                                    subscription.Active = subscribe.Active;
                                    subscription.ValidUntil = subscribe.ValidUntil;
                                    subscription.SubscriptionTypeID = (int)subscriptionTypeEnum;
                                    subscription.CreatedDate = DateTime.Now;
                                    _subscriptionService.Insert(subscription);

                                    var user = _userService.GetByID(userId);

                                    try
                                    {
                                        SendAdminNotification(string.Format("E-mail: {0} - Assinatura: {1}", user.Value.Email, subscribe.ProductIdentifier));

                                        _rDStationHelper.SendEvent(null, user.Value.Email, userId, null, user.Value.PhoneNumber, RDStation.Interface.Model.Request.EventType.TrialSubscriptionStarted);
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

        private void SendAdminNotification(string message)
        {
            ResultServiceObject<IEnumerable<Device>> devices = _deviceService.GetAdminDevices();

            foreach (Device itemDevice in devices.Value)
            {
                try
                {
                    _notificationService.SendPush("Nova assinatura", message, itemDevice, null);
                }
                catch (Exception exception)
                {
                    _logger.SendErrorAsync(exception);
                }

            }
        }

        public void ReceiveInformation(string token, dynamic value)
        {
            if (token.Equals("60JVf8vak1Si87tYfKwNSPgTwZ0nFZU"))
            {
                _logger.SendInformationAsync(new { IAPHubWebhooks = value });

                string idPurchase = value.data.id;

                GetPurchaseDetailsFromIapHub(idPurchase);
            }
            else
            {
                _logger.SendInformationAsync(new { IAPHubWebhooks = "Tentativa de acesso ao IAPHubWebhooks sem token de acesso" });
            }
        }

        public ResultResponseObject<Subscribe> AddNewLicence(AddSubscription addSubscription)
        {
            Subscription subscription = new Subscription();
            subscription.Active = true;
            subscription.IdUser = addSubscription.UserId;
            subscription.ValidUntil = Convert.ToDateTime(addSubscription.ValidUntil, new CultureInfo("pt-br"));
            subscription.PartnerID = addSubscription.PartnerID;
            subscription.SubscriptionTypeID = (int)addSubscription.SubscriptionType;

            ResultServiceObject<Subscription> resultServiceObject = null;

            using (_uow.Create())
            {
                resultServiceObject = _subscriptionService.Insert(subscription);
            }

            ResultResponseObject<Subscribe> result = _mapper.Map<ResultResponseObject<Subscribe>>(resultServiceObject);
            return result;
        }

        public ResultResponseObject<Subscribe> UpdateLicence(EditSubscription editSubscription)
        {
            ResultServiceObject<Subscription> resultServiceObject = null;

            using (_uow.Create())
            {
                ResultServiceObject<Subscription> subscriptionFound = _subscriptionService.GetByUser(editSubscription.UserId);
                subscriptionFound.Value.ValidUntil = Convert.ToDateTime(editSubscription.ValidUntil, new CultureInfo("pt-br"));
                subscriptionFound.Value.SubscriptionTypeID = (int)editSubscription.SubscriptionType;
                subscriptionFound.Value.SubscriptionID = editSubscription.SubscriptionID;
                subscriptionFound.Value.Active = editSubscription.Active;
                subscriptionFound.Value.PartnerID = editSubscription.PartnerID;
                resultServiceObject = _subscriptionService.Update(subscriptionFound.Value);
            }

            ResultResponseObject<Subscribe> result = _mapper.Map<ResultResponseObject<Subscribe>>(resultServiceObject);
            return result;
        }

        public void SubscribePartnerFile(string path, string fileName)
        {
            using (StreamReader sr = File.OpenText(Path.Combine(path, fileName)))
            {
                string s = String.Empty;
                List<Device> devices = new List<Device>();
                StringBuilder sbHasSubscription = new StringBuilder();
                using (_uow.Create())
                {
                    while ((s = sr.ReadLine()) != null)
                    {
                        string[] useInfo = s.Split(",");
                        string email = string.Empty;
                        string phone = string.Empty;

                        if (useInfo.Length > 1)
                        {
                            email = useInfo[1];
                        }

                        if (useInfo.Length > 2)
                        {
                            phone = useInfo[2];
                        }


                        ResultServiceObject<ApplicationUser> resultUser = _userService.GetByEmail(email);

                        if (resultUser.Value != null)
                        {
                            ResultServiceObject<Subscription> resultSubscription = _subscriptionService.GetByUser(resultUser.Value.Id);

                            if (resultSubscription.Value == null)
                            {
                                Subscription subscription = new Subscription();
                                subscription.Active = true;
                                subscription.SubscriptionTypeID = 3;
                                subscription.PartnerID = 2;
                                subscription.ValidUntil = DateTime.Now.AddYears(1);
                                subscription.IdUser = resultUser.Value.Id;

                                subscription = _subscriptionService.Insert(subscription).Value;

                                devices.AddRange(_deviceService.GetByUser(resultUser.Value.Id).Value);

                                sbHasSubscription.AppendLine(resultUser.Value.Email + " - Assinatura Toro");
                            }
                            else
                            {
                                if (resultSubscription.Value.Active)
                                {
                                    if (resultSubscription.Value.SubscriptionTypeID != 3 && resultSubscription.Value.PartnerID != 2 && resultSubscription.Value.ValidUntil.Month != 7)
                                    {
                                        resultSubscription.Value.Active = true;
                                        resultSubscription.Value.SubscriptionTypeID = 3;
                                        resultSubscription.Value.PartnerID = 2;
                                        if (resultSubscription.Value.ValidUntil > DateTime.Now)
                                        {
                                            resultSubscription.Value.ValidUntil = resultSubscription.Value.ValidUntil.AddYears(1);
                                        }
                                        else
                                        {
                                            resultSubscription.Value.ValidUntil = DateTime.Now.AddYears(1);
                                        }

                                        resultSubscription.Value = _subscriptionService.Update(resultSubscription.Value).Value;

                                        devices.AddRange(_deviceService.GetByUser(resultUser.Value.Id).Value);

                                        sbHasSubscription.AppendLine(resultUser.Value.Email + " - Assinatura Toro");
                                    }
                                    else
                                    {
                                        sbHasSubscription.AppendLine(resultUser.Value.Email + " - Assinatura Toro");

                                        if (resultSubscription.Value.ValidUntil.Date == DateTime.Now.Date)
                                        {
                                            resultSubscription.Value.ValidUntil = DateTime.Now.AddYears(1);
                                            resultSubscription.Value = _subscriptionService.Update(resultSubscription.Value).Value;
                                            devices.AddRange(_deviceService.GetByUser(resultUser.Value.Id).Value);
                                        }
                                    }
                                }
                                else
                                {
                                    resultSubscription.Value.Active = true;
                                    resultSubscription.Value.SubscriptionTypeID = 3;
                                    resultSubscription.Value.PartnerID = 2;
                                    resultSubscription.Value.ValidUntil = DateTime.Now.AddYears(1);

                                    resultSubscription.Value = _subscriptionService.Update(resultSubscription.Value).Value;

                                    devices.AddRange(_deviceService.GetByUser(resultUser.Value.Id).Value);

                                    sbHasSubscription.AppendLine(resultUser.Value.Email + " - Assinatura Toro");
                                }
                            }

                            if (!string.IsNullOrWhiteSpace(phone))
                            {
                                _userService.UpdatePhone(resultUser.Value.Id, phone);
                            }
                        }
                        else
                        {
                            sbHasSubscription.AppendLine(email + " - Usuário não encontrado");
                        }                        
                    }

                    if (devices != null && devices.Count > 0)
                    {
                        File.WriteAllText(Path.Combine(path, "result.txt"), sbHasSubscription.ToString());

                        foreach (Device device in devices)
                        {
                            _notificationService.SendPush("Assinatura Anual Toro Investimentos", "Parabéns! Sua assinatura anual está ativa! Aproveite o melhor do App Dividendos.me e invista cada vez melhor. Caso sua assinatura não esteja aparecendo, vá até o menu conta, sair e depois faça login novamente. Bons investimentos.", device, null);
                        }
                    }
                }
            }
        }

        public void SubscribePartnerFileNotDefined(string path, string fileName, bool checkAll = false)
        {
            string notDef = "#N/A";
            List<Device> devices = new List<Device>();
            List<ApplicationUser> users = new List<ApplicationUser>();
            StringBuilder sbHasSubscription = new StringBuilder();

            using (StreamReader sr = File.OpenText(Path.Combine(path, fileName)))
            {
                string s = String.Empty;


                while ((s = sr.ReadLine()) != null)
                {
                    using (_uow.Create())
                    {
                        string[] useInfo = s.Split(",");
                        string identifier = string.Empty;

                        if (useInfo.Length > 3)
                        {
                            identifier = useInfo[3];
                        }

                        string email = string.Empty;

                        if (useInfo.Length > 5)
                        {
                            email = useInfo[5];
                        }

                        ResultServiceObject<ApplicationUser> resultUser = new ResultServiceObject<ApplicationUser>();

                        if (email != notDef || checkAll)
                        {
                            ResultServiceObject<Trader> resultTrader = _traderService.GetLikeIdentifier(identifier);

                            if (resultTrader.Value == null)
                            {
                                resultUser = _userService.GetByEmail(email);
                            }
                            else
                            {
                                resultUser = _userService.GetByID(resultTrader.Value.IdUser);
                            }
                        }

                        if (resultUser.Value != null)
                        {
                            if (resultUser.Value != null)
                            {
                                ResultServiceObject<Subscription> resultSubscription = _subscriptionService.GetByUser(resultUser.Value.Id);

                                if (resultSubscription.Value == null)
                                {
                                    Subscription subscription = new Subscription();
                                    subscription.Active = true;
                                    subscription.SubscriptionTypeID = 3;
                                    subscription.PartnerID = 2;
                                    subscription.ValidUntil = DateTime.Now.AddYears(1);
                                    subscription.IdUser = resultUser.Value.Id;

                                    subscription = _subscriptionService.Insert(subscription).Value;

                                    
                                    devices.AddRange(_deviceService.GetByUser(resultUser.Value.Id).Value);
                                    users.Add(resultUser.Value);

                                    sbHasSubscription.AppendLine(resultUser.Value.Email + " - Assinatura Toro");
                                }
                                else
                                {
                                    if (resultSubscription.Value.Active)
                                    {
                                        if (!resultSubscription.Value.PartnerID.HasValue || resultSubscription.Value.PartnerID.Value != 2)
                                        {
                                            resultSubscription.Value.Active = true;
                                            resultSubscription.Value.SubscriptionTypeID = 3;
                                            resultSubscription.Value.PartnerID = 2;
                                            if (resultSubscription.Value.ValidUntil > DateTime.Now)
                                            {
                                                resultSubscription.Value.ValidUntil = resultSubscription.Value.ValidUntil.AddYears(1);
                                            }
                                            else
                                            {
                                                resultSubscription.Value.ValidUntil = DateTime.Now.AddYears(1);
                                            }

                                            resultSubscription.Value = _subscriptionService.Update(resultSubscription.Value).Value;

                                            devices.AddRange(_deviceService.GetByUser(resultUser.Value.Id).Value);
                                            users.Add(resultUser.Value);

                                            sbHasSubscription.AppendLine(resultUser.Value.Email + " - Assinatura Toro");
                                        }
                                        else
                                        {
                                            sbHasSubscription.AppendLine(resultUser.Value.Email + " - Assinatura Toro");

                                            if (resultSubscription.Value.ValidUntil.Date == DateTime.Now.Date)
                                            {
                                                resultSubscription.Value.ValidUntil = DateTime.Now.AddYears(1);
                                                resultSubscription.Value = _subscriptionService.Update(resultSubscription.Value).Value;
                                                devices.AddRange(_deviceService.GetByUser(resultUser.Value.Id).Value);
                                                users.Add(resultUser.Value);
                                            }
                                        }
                                    }
                                    else
                                    {
                                        resultSubscription.Value.Active = true;
                                        resultSubscription.Value.SubscriptionTypeID = 3;
                                        resultSubscription.Value.PartnerID = 2;
                                        resultSubscription.Value.ValidUntil = DateTime.Now.AddYears(1);

                                        resultSubscription.Value = _subscriptionService.Update(resultSubscription.Value).Value;

                                        devices.AddRange(_deviceService.GetByUser(resultUser.Value.Id).Value);
                                        users.Add(resultUser.Value);
                                        sbHasSubscription.AppendLine(resultUser.Value.Email + " - Assinatura Toro");
                                    }
                                }
                            }
                            else
                            {
                                sbHasSubscription.AppendLine(identifier + " - Usuário não encontrado");
                            }
                        }
                        else
                        {
                            if (email == notDef)
                            {
                                sbHasSubscription.AppendLine(identifier + " - #N/A");
                            }
                            else
                            {
                                sbHasSubscription.AppendLine(identifier + " - Usuário não encontrado");
                            }
                        }
                    }
                }
            }

            if (devices != null && devices.Count > 0)
            {
                File.WriteAllText(Path.Combine(path, "result.txt"), sbHasSubscription.ToString());

                foreach (Device device in devices)
                {
                    _notificationService.SendPush("Assinatura Anual Toro", "Parabéns! Sua assinatura anual está ativa! Aproveite o melhor do App Dividendos.me e invista cada vez melhor. Caso sua assinatura não esteja aparecendo, vá até o menu conta, sair e depois faça login novamente. Bons investimentos.", device, null);
                }
            }

            if (users != null && users.Count > 0)
            {
                foreach (ApplicationUser user in users)
                {
                    using (_uow.Create())
                    {
                        SendAdminNotification(string.Format("E-mail: {0} - Assinatura: {1}", user.Email, "Toro Invest"));

                        _rDStationHelper.SendEvent(null, user.Email, user.Id, null, user.PhoneNumber, RDStation.Interface.Model.Request.EventType.PartnerToroSubscription);
                    }
                }
            }
        }

        public void SubscribePartnerActivated(string path, string fileName)
        {
            using (StreamReader sr = File.OpenText(Path.Combine(path, fileName)))
            {
                string s = String.Empty;
                List<Device> devices = new List<Device>();
                StringBuilder sbHasSubscription = new StringBuilder();
                using (_uow.Create())
                {
                    while ((s = sr.ReadLine()) != null)
                    {
                        string[] useInfo = s.Split(",");
                        string identifier = string.Empty;

                        if (useInfo.Length > 0)
                        {
                            identifier = useInfo[0];
                        }

                        ResultServiceObject<Trader> resultTrader = _traderService.GetLikeIdentifier(identifier);

                        if (resultTrader.Value != null)
                        {
                            ResultServiceObject<ApplicationUser> resultUser = _userService.GetByID(resultTrader.Value.IdUser);

                            if (resultUser.Value != null)
                            {
                                ResultServiceObject<Subscription> resultSubscription = _subscriptionService.GetByUser(resultUser.Value.Id);

                                if (resultSubscription.Value == null)
                                {
                                    Subscription subscription = new Subscription();
                                    subscription.Active = true;
                                    subscription.SubscriptionTypeID = 3;
                                    subscription.PartnerID = 2;
                                    subscription.ValidUntil = DateTime.Now.AddYears(1);
                                    subscription.IdUser = resultUser.Value.Id;

                                    subscription = _subscriptionService.Insert(subscription).Value;

                                    devices.AddRange(_deviceService.GetByUser(resultUser.Value.Id).Value);

                                    sbHasSubscription.AppendLine(resultUser.Value.Email + " - Assinatura Toro");
                                }
                                else
                                {
                                    if (resultSubscription.Value.Active)
                                    {
                                        if (resultSubscription.Value.SubscriptionTypeID != 3 && (!resultSubscription.Value.PartnerID.HasValue || resultSubscription.Value.PartnerID.Value != 2))
                                        {
                                            resultSubscription.Value.Active = true;
                                            resultSubscription.Value.SubscriptionTypeID = 3;
                                            resultSubscription.Value.PartnerID = 2;
                                            if (resultSubscription.Value.ValidUntil > DateTime.Now)
                                            {
                                                resultSubscription.Value.ValidUntil = resultSubscription.Value.ValidUntil.AddYears(1);
                                            }
                                            else
                                            {
                                                resultSubscription.Value.ValidUntil = DateTime.Now.AddYears(1);
                                            }

                                            resultSubscription.Value = _subscriptionService.Update(resultSubscription.Value).Value;

                                            devices.AddRange(_deviceService.GetByUser(resultUser.Value.Id).Value);

                                            sbHasSubscription.AppendLine(resultUser.Value.Email + " - Assinatura Toro");
                                        }
                                        else
                                        {
                                            sbHasSubscription.AppendLine(resultUser.Value.Email + " - Assinatura Toro");

                                            if (resultSubscription.Value.ValidUntil.Date == DateTime.Now.Date)
                                            {
                                                resultSubscription.Value.ValidUntil = DateTime.Now.AddYears(1);
                                                resultSubscription.Value = _subscriptionService.Update(resultSubscription.Value).Value;
                                                devices.AddRange(_deviceService.GetByUser(resultUser.Value.Id).Value);
                                            }
                                        }
                                    }
                                    else
                                    {
                                        resultSubscription.Value.Active = true;
                                        resultSubscription.Value.SubscriptionTypeID = 3;
                                        resultSubscription.Value.PartnerID = 2;
                                        resultSubscription.Value.ValidUntil = DateTime.Now.AddYears(1);

                                        resultSubscription.Value = _subscriptionService.Update(resultSubscription.Value).Value;

                                        devices.AddRange(_deviceService.GetByUser(resultUser.Value.Id).Value);

                                        sbHasSubscription.AppendLine(resultUser.Value.Email + " - Assinatura Toro");
                                    }
                                }
                            }
                            else
                            {
                                sbHasSubscription.AppendLine(identifier + " - Usuário não encontrado");
                            }
                        }
                        else
                        {
                            sbHasSubscription.AppendLine(identifier + " - Usuário não encontrado");
                        }
                    }

                    if (devices != null && devices.Count > 0)
                    {
                        File.WriteAllText(Path.Combine(path, "result.txt"), sbHasSubscription.ToString());

                        foreach (Device device in devices)
                        {
                            _notificationService.SendPush("Assinatura Anual Toro Investimentos", "Parab�ns! Sua assinatura anual est� ativa! Aproveite o melhor do App Dividendos.me e invista cada vez melhor. Caso sua assinatura não esteja aparecendo, v� at� o menu conta, sair e depois fa�a login novamente. Bons investimentos.", device, null);
                        }
                    }
                }
            }
        }

        public void GetEmailByIdentifier(string path, string fileName)
        {
            using (StreamReader sr = File.OpenText(Path.Combine(path, fileName)))
            {
                string s = String.Empty;
                List<Device> devices = new List<Device>();
                StringBuilder sbHasSubscription = new StringBuilder();
                using (_uow.Create())
                {
                    while ((s = sr.ReadLine()) != null)
                    {
                        //string[] useInfo = s.Split(",");
                        //string identifier = string.Empty;

                        //if (useInfo.Length > 0)
                        //{
                        //    identifier = useInfo[0];
                        //}

                        ResultServiceObject<Trader> resultTrader = _traderService.GetLikeIdentifier(s);

                        if (resultTrader.Value != null)
                        {
                            ResultServiceObject<ApplicationUser> resultUser = _userService.GetByID(resultTrader.Value.IdUser);

                            if (resultUser.Value != null)
                            {
                                sbHasSubscription.AppendLine(resultUser.Value.Email);
                            }

                            devices.AddRange(_deviceService.GetByUser(resultTrader.Value.IdUser).Value);

                        }
                        else
                        {
                            sbHasSubscription.AppendLine(s + " - Usuário não encontrado");
                        }
                    }

                    if (devices != null && devices.Count > 0)
                    {
                        File.WriteAllText(Path.Combine(path, "result.txt"), sbHasSubscription.ToString());

                        foreach (Device device in devices)
                        {
                            _notificationService.SendPush("Aviso Importante", "Sua assinatura anual do App Dividendos.me ainda não foi liberada! Para ter acesso ao benef�cio, voc� deve fazer a transfer�ncia de qualquer valor para a sua conta da corretora Toro. não perca tempo! COM QUALQUER VALOR voc� tem acesso a um ano de assinatura do App.", device, null);
                        }
                    }

                }
            }
        }

        public void SendPushPartner()
        {
            List<Device> devices = new List<Device>();
            using (_uow.Create())
            {
                List<Device> devicesAll = _deviceService.GetAllNewVersion().Value.ToList();

                foreach (Device device in devicesAll)
                {
                    ResultServiceObject<Subscription> resultSubscription = _subscriptionService.GetByUser(device.IdUser);

                    if (resultSubscription.Value == null || resultSubscription.Value.PartnerID != 2)
                    {
                        devices.Add(device);
                    }
                }

                if (devices != null && devices.Count > 0)
                {
                    foreach (Device device in devices)
                    {
                        _notificationService.SendPush("1 ano de assinatura gr�tis!", "Temos uma super novidade: Anuidade gr�tis! Voc� economiza R$ 99,90 e ainda conhece uma corretora muito melhor que todas as outras gr�tis do Brasil. Veja o que preparamos para te ajudar ainda mais. não perca tempo, � por tempo limitado.", device, null);
                    }
                }
            }


        }


        public void SubscribePartnerFileV2(string path, string fileName, bool checkAll = false)
        {
            string notDef = "#N/A";

            using (StreamReader sr = File.OpenText(Path.Combine(path, fileName)))
            {
                string s = String.Empty;
                List<ApplicationUser> users = new List<ApplicationUser>();
                List<Device> devices = new List<Device>();
                StringBuilder sbHasSubscription = new StringBuilder();

                while ((s = sr.ReadLine()) != null)
                {
                    using (_uow.Create())
                    {
                        string[] useInfo = s.Split(",");
                        string identifier = string.Empty;
                        string phone = string.Empty;

                        if (useInfo.Length >= 1)
                        {
                            identifier = useInfo[0];
                        }

                        string email = string.Empty;

                        if (useInfo.Length >= 2)
                        {
                            email = useInfo[1];
                        }

                        if (useInfo.Length >= 3)
                        {
                            phone = useInfo[2];
                        }

                        ResultServiceObject<ApplicationUser> resultUser = new ResultServiceObject<ApplicationUser>();

                        if (email != notDef || checkAll)
                        {
                            ResultServiceObject<Trader> resultTrader = _traderService.GetLikeIdentifier(identifier);

                            if (resultTrader.Value == null)
                            {
                                resultUser = _userService.GetByEmail(email);
                            }
                            else
                            {
                                resultUser = _userService.GetByID(resultTrader.Value.IdUser);
                            }
                        }


                        if (resultUser.Value != null)
                        {
                            ResultServiceObject<Subscription> resultSubscription = _subscriptionService.GetByUser(resultUser.Value.Id);

                            if (resultSubscription.Value == null)
                            {
                                Subscription subscription = new Subscription();
                                subscription.Active = true;
                                subscription.SubscriptionTypeID = 3;
                                subscription.PartnerID = 2;
                                subscription.ValidUntil = DateTime.Now.AddYears(1);
                                subscription.IdUser = resultUser.Value.Id;

                                subscription = _subscriptionService.Insert(subscription).Value;

                                devices.AddRange(_deviceService.GetByUser(resultUser.Value.Id).Value);
                                users.Add(resultUser.Value);

                                sbHasSubscription.AppendLine(resultUser.Value.Email + " - Assinatura Toro");
                            }
                            else
                            {
                                if (resultSubscription.Value.Active)
                                {
                                    if (!resultSubscription.Value.PartnerID.HasValue || resultSubscription.Value.PartnerID.Value != 2)
                                    {
                                        resultSubscription.Value.Active = true;
                                        resultSubscription.Value.SubscriptionTypeID = 3;
                                        resultSubscription.Value.PartnerID = 2;
                                        if (resultSubscription.Value.ValidUntil > DateTime.Now)
                                        {
                                            resultSubscription.Value.ValidUntil = resultSubscription.Value.ValidUntil.AddYears(1);
                                        }
                                        else
                                        {
                                            resultSubscription.Value.ValidUntil = DateTime.Now.AddYears(1);
                                        }

                                        resultSubscription.Value = _subscriptionService.Update(resultSubscription.Value).Value;

                                        devices.AddRange(_deviceService.GetByUser(resultUser.Value.Id).Value);
                                        users.Add(resultUser.Value);

                                        sbHasSubscription.AppendLine(resultUser.Value.Email + " - Assinatura Toro");
                                    }
                                    else
                                    {
                                        sbHasSubscription.AppendLine(resultUser.Value.Email + " - Assinatura Toro");

                                        if (resultSubscription.Value.ValidUntil.Date == DateTime.Now.Date)
                                        {
                                            resultSubscription.Value.ValidUntil = DateTime.Now.AddYears(1);
                                            resultSubscription.Value = _subscriptionService.Update(resultSubscription.Value).Value;
                                            devices.AddRange(_deviceService.GetByUser(resultUser.Value.Id).Value);
                                            users.Add(resultUser.Value);
                                        }
                                    }
                                }
                                else
                                {
                                    resultSubscription.Value.Active = true;
                                    resultSubscription.Value.SubscriptionTypeID = 3;
                                    resultSubscription.Value.PartnerID = 2;
                                    resultSubscription.Value.ValidUntil = DateTime.Now.AddYears(1);

                                    resultSubscription.Value = _subscriptionService.Update(resultSubscription.Value).Value;

                                    devices.AddRange(_deviceService.GetByUser(resultUser.Value.Id).Value);
                                    users.Add(resultUser.Value);

                                    sbHasSubscription.AppendLine(resultUser.Value.Email + " - Assinatura Toro");
                                }
                            }

                            if (!string.IsNullOrWhiteSpace(phone))
                            {
                                _userService.UpdatePhone(resultUser.Value.Id, phone);
                            }
                        }
                        else
                        {
                            sbHasSubscription.AppendLine(identifier + " - Usuário não encontrado");
                        }
                    }
                }

                if (devices != null && devices.Count > 0)
                {
                    File.WriteAllText(Path.Combine(path, "result.txt"), sbHasSubscription.ToString());

                    foreach (Device device in devices)
                    {
                        _notificationService.SendPush("Assinatura Anual Toro", "Parabéns! Sua assinatura anual está ativa! Aproveite o melhor do App Dividendos.me e invista cada vez melhor. Caso sua assinatura não esteja aparecendo, vá até o menu conta, sair e depois faça login novamente. Bons investimentos.", device, null);
                    }
                }

                if (users != null && users.Count > 0)
                {
                    foreach (ApplicationUser user in users)
                    {
                        using (_uow.Create())
                        {
                            SendAdminNotification(string.Format("E-mail: {0} - Assinatura: {1}", user.Email, "Toro Invest"));

                            _rDStationHelper.SendEvent(null, user.Email, user.Id, null, user.PhoneNumber, RDStation.Interface.Model.Request.EventType.PartnerToroSubscription);
                        }
                    }
                }
            }
        }


        public void CheckPilantra(string path, string fileName, bool checkAll = false)
        {
            string notDef = "#N/A";

            using (StreamReader sr = File.OpenText(Path.Combine(path, fileName)))
            {
                string s = String.Empty;
                List<Device> devices = new List<Device>();
                StringBuilder sbHasSubscription = new StringBuilder();

                List<string> usersActive = new List<string>();
                List<string> usersPartners = new List<string>();
                List<ApplicationUser> pilantras = new List<ApplicationUser>();
                ResultServiceObject<ApplicationUser> resultUser = new ResultServiceObject<ApplicationUser>();

                using (_uow.Create())
                {
                    while ((s = sr.ReadLine()) != null)
                    {
                        string[] useInfo = s.Split(",");
                        string identifier = string.Empty;

                        if (useInfo.Length > 0)
                        {
                            identifier = useInfo[0];
                        }

                        string email = string.Empty;

                        if (useInfo.Length > 2)
                        {
                            email = useInfo[2];
                        }


                        if (email != notDef || checkAll)
                        {
                            ResultServiceObject<Trader> resultTrader = _traderService.GetLikeIdentifier(identifier);

                            if (resultTrader.Value == null)
                            {
                                resultUser = _userService.GetByEmail(email);
                            }
                            else
                            {
                                resultUser = _userService.GetByID(resultTrader.Value.IdUser);
                            }
                        }

                        if (resultUser.Value != null)
                        {
                            usersActive.Add(resultUser.Value.Id);
                        }
                    }

                    ResultServiceObject<IEnumerable<Subscription>> resultSubscription = _subscriptionService.GetByPartner(2);

                    if (resultSubscription.Value != null && resultSubscription.Value.Count() > 0)
                    {
                        foreach (Subscription subs in resultSubscription.Value)
                        {
                            usersPartners.Add(subs.IdUser);
                        }
                    }

                    if (usersActive != null && usersActive.Count > 0)
                    {
                        foreach (string user in usersPartners)
                        {
                            if (!usersActive.Exists(userAc => userAc == user))
                            {
                                resultUser = _userService.GetByID(user);
                                pilantras.Add(resultUser.Value);

                                ResultServiceObject<Trader> resultTrader = _traderService.GetByUser(resultUser.Value.Id);

                                if (resultTrader.Value != null)
                                {
                                    sbHasSubscription.AppendLine(resultUser.Value.Email + " - " + resultTrader.Value.Identifier);
                                }
                                else
                                {
                                    sbHasSubscription.AppendLine(resultUser.Value.Email);
                                }
                            }
                        }
                    }

                    if (sbHasSubscription != null && sbHasSubscription.Length > 0)
                    {
                        File.WriteAllText(Path.Combine(path, "result.txt"), sbHasSubscription.ToString());

                        //foreach (Device device in devices)
                        //{
                        //    _notificationService.SendPush("Assinatura Anual Toro", "Parab�ns! Sua assinatura anual est� ativa! Aproveite o melhor do App Dividendos.me e invista cada vez melhor. Caso sua assinatura não esteja aparecendo, v� at� o menu conta, sair e depois fa�a login novamente. Bons investimentos.", device, null);
                        //}
                    }
                }
            }
        }

        public void SendPushPilantra(string path, string fileName)
        {
            List<Device> devicesAll = new List<Device>();

            using (StreamReader sr = File.OpenText(Path.Combine(path, fileName)))
            {
                string s = String.Empty;
                StringBuilder sbHasSubscription = new StringBuilder();

                List<string> usersActive = new List<string>();
                List<string> usersPartners = new List<string>();
                List<ApplicationUser> pilantras = new List<ApplicationUser>();
                ResultServiceObject<ApplicationUser> resultUser = new ResultServiceObject<ApplicationUser>();

                using (_uow.Create())
                {
                    while ((s = sr.ReadLine()) != null)
                    {
                        resultUser = _userService.GetByEmail(s);

                        if (resultUser.Value != null)
                        {
                            devicesAll.AddRange(_deviceService.GetByUser(resultUser.Value.Id).Value);
                        }
                        else
                        {
                            string teste = "po";
                        }
                    }
                }
            }

            using (_uow.Create())
            {
                if (devicesAll != null && devicesAll.Count > 0)
                {
                    foreach (Device device in devicesAll)
                    {
                        string title = "Ação urgente!";
                        string message = "Identificamos um problema com a sua assinatura do app Dividendos.me / Toro Investimentos. Por favor, entre em contato conosco para verificar a situação. O acesso premium será removido no dia 16/08 caso não haja contato.";
                        //_notificationService.SendPush(title, message, device, null);
                        _notificationService.SendPush(title, message, device, new PushRedirect() { PushRedirectType = PushRedirectTypeEnum.External, ExternalRedirectURL = "https://forms.gle/ViPnGbHeSkSQyHot9" });
                        _notificationHistoricalService.New(title, message, device.IdUser, AppScreenNameEnum.HomeToday.ToString(), PushRedirectTypeEnum.Internal.ToString(), null, _cacheService);
                    }
                }
            }
        }

        public ResultResponseObject<List<Dividendos.API.Model.Response.Purchase.ProductVM>> GetProducts(DeviceType deviceType, string appVersion)
        {
            ResultResponseObject<List<Dividendos.API.Model.Response.Purchase.ProductVM>> resultResponseObject = new ResultResponseObject<List<API.Model.Response.Purchase.ProductVM>>() { Success = true };

            string appleVersionApproval = string.Empty;
            ResultServiceObject<Entity.Entities.SystemSettings> resultAppleVersionApproval = new ResultServiceObject<SystemSettings>();

            using (_uow.Create())
            {
                resultAppleVersionApproval = _systemSettingsService.GetByKey(Constants.SYSTEM_SETTINGS_APPLE_VERSION_FOR_APPROVAL);

                if (resultAppleVersionApproval.Value != null)
                {
                    appleVersionApproval = resultAppleVersionApproval.Value.SettingValue;
                }
            }

            List<Dividendos.API.Model.Response.Purchase.ProductVM> products = new List<API.Model.Response.Purchase.ProductVM>();
            products.Add(new ProductVM() { Price = "9,50", Name = "Plano Ouro Mensal", Type = "Mensal", Discount = "0", Sku = "gold_new", OldPrice = "15,90" });
            products.Add(new ProductVM() { Price = "99,90", Name = "Plano Ouro Anual", Type = "Anual", Discount = "15", Sku = "annuity_new2", OldPrice = "150,90" });

            if (deviceType == DeviceType.Android || (deviceType == DeviceType.Iphone && appVersion != appleVersionApproval))
            {
                products.Add(new ProductVM() { Price = "0,00", Name = "Plano Ouro Anual (Toro Investimentos)", Type = "Anual", Discount = "100", Sku = "annuity_toro", OldPrice = "150,90", PartnerGuid = "a091b454-29e7-4907-9d7e-02cb73306e6e" });
            }

            resultResponseObject.Value = products;

            return resultResponseObject;
        }

        public ResultResponseObject<List<Dividendos.API.Model.Response.Purchase.ProductVM>> GetProductsV4(DeviceType deviceType, string appVersion)
        {
            ResultResponseObject<List<Dividendos.API.Model.Response.Purchase.ProductVM>> resultResponseObject = new ResultResponseObject<List<API.Model.Response.Purchase.ProductVM>>() { Success = true };

            string appleVersionApproval = string.Empty;
            ResultServiceObject<Entity.Entities.SystemSettings> resultAppleVersionApproval = new ResultServiceObject<SystemSettings>();

            using (_uow.Create())
            {
                resultAppleVersionApproval = _systemSettingsService.GetByKey(Constants.SYSTEM_SETTINGS_APPLE_VERSION_FOR_APPROVAL);

                if (resultAppleVersionApproval.Value != null)
                {
                    appleVersionApproval = resultAppleVersionApproval.Value.SettingValue;
                }
            }

            List<Dividendos.API.Model.Response.Purchase.ProductVM> products = new List<API.Model.Response.Purchase.ProductVM>();
            products.Add(new ProductVM() { Price = "9,50", Name = "Plano Ouro Mensal", Type = "Mensal", Discount = "0", Sku = "gold_new", OldPrice = "15,90" });
            products.Add(new ProductVM() { Price = "99,90", Name = "Plano Ouro Anual", Type = "Anual", Discount = "15", Sku = "annuity_new2", OldPrice = "150,90" });

            if (deviceType == DeviceType.Android || (deviceType == DeviceType.Iphone && appVersion != appleVersionApproval))
            {
                products.Add(new ProductVM() { Price = "99,90", Name = "Plano Ouro Anual (PIX)", Type = "Anual", Discount = "0", Sku = "annuity_pix", OldPrice = "150,90" });
                products.Add(new ProductVM() { Price = "0,00", Name = "Plano Ouro Anual (Toro Investimentos)", Type = "Anual", Discount = "100", Sku = "annuity_toro", OldPrice = "150,90", PartnerGuid = "a091b454-29e7-4907-9d7e-02cb73306e6e" });
            }

            resultResponseObject.Value = products;

            return resultResponseObject;
        }

        public ResultResponseObject<List<ProductVM>> GetProductsV5(DeviceType deviceType, string appVersion)
        {
            ResultResponseObject<List<ProductVM>> resultResponseObject = new ResultResponseObject<List<ProductVM>>() { Success = true };

            string appleVersionApproval = string.Empty;
            ResultServiceObject<Entity.Entities.SystemSettings> resultAppleVersionApproval = new ResultServiceObject<SystemSettings>();

            using (_uow.Create())
            {
                resultAppleVersionApproval = _systemSettingsService.GetByKey(Constants.SYSTEM_SETTINGS_APPLE_VERSION_FOR_APPROVAL);

                if (resultAppleVersionApproval.Value != null)
                {
                    appleVersionApproval = resultAppleVersionApproval.Value.SettingValue;
                }
            }

            List<Dividendos.API.Model.Response.Purchase.ProductVM> products = new List<API.Model.Response.Purchase.ProductVM>();
            products.Add(new ProductVM() { Price = "9,50", Name = "Plano Ouro Mensal", Type = "Mensal", Discount = "0", Sku = "gold_new", OldPrice = "15,90" });
            products.Add(new ProductVM() { Price = "99,90", Name = "Plano Ouro Anual", Type = "Anual", Discount = "15", Sku = "annuity_new2", OldPrice = "150,90" });

            if (deviceType == DeviceType.Android || (deviceType == DeviceType.Iphone && appVersion != appleVersionApproval))
            {
                products.Add(new ProductVM() { Price = "99,90", Name = "Plano Ouro Anual (PIX)", Type = "Anual", Discount = "0", Sku = "annuity_pix", OldPrice = "150,90" });
                products.Add(new ProductVM() { Price = "0,00", Name = "Plano Ouro Anual (Toro Investimentos)", Type = "Anual", Discount = "100", Sku = "annuity_toro", OldPrice = "150,90", PartnerGuid = "a091b454-29e7-4907-9d7e-02cb73306e6e" });
                products.Add(new ProductVM() { Price = "0,00", Name = "Plano Ouro Anual (Vitreo)", Type = "Anual", Discount = "100", Sku = "annuity_vitreo", OldPrice = "150,90", PartnerGuid = "d0909ca5-44d5-4257-9155-e1401163cb70" });
            }

            resultResponseObject.Value = products;

            return resultResponseObject;
        }
    }
}