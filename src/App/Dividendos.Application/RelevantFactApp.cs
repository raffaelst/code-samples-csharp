using AutoMapper;
using Dividendos.API.Model.Response;
using Dividendos.API.Model.Response.Common;
using Dividendos.API.Model.Response.v1;
using Dividendos.Application.Base;
using Dividendos.Application.Interface;
using Dividendos.AWS;
using Dividendos.Entity.Entities;
using Dividendos.Entity.Enum;
using Dividendos.Entity.Model;
using Dividendos.InfoMoney;
using Dividendos.InfoMoney.Interface.Model;
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
    public class RelevantFactApp : BaseApp, IRelevantFactApp
    {
        private readonly IRelevantFactService _relevantFactService;
        private readonly IMapper _mapper;
        private readonly IUnitOfWork _uow;
        private readonly ICacheService _cacheService;
        private readonly IStockService _stockService;
        private readonly INotificationHistoricalService _notificationHistoricalService;
        private readonly IDeviceService _deviceService;
        private readonly ISettingsService _settingsService;
        private readonly INotificationService _notificationService;
        private readonly ILogger _logger;
        private readonly IGlobalAuthenticationService _globalAuthenticationService;
        private readonly IS3Service _s3Service;
        private readonly ICompanyService _companySerivce;
        private readonly ISystemSettingsService _systemSettingsService;

        public RelevantFactApp(IMapper mapper,
            IUnitOfWork uow,
            IRelevantFactService relevantFactService,
            ICacheService cacheService,
            IStockService stockService,
            INotificationHistoricalService notificationHistoricalService,
            IDeviceService deviceService,
            ISettingsService settingsService,
            INotificationService notificationService,
            ILogger logger,
            IGlobalAuthenticationService globalAuthenticationService,
            IS3Service s3Service,
            ICompanyService companyService,
            ISystemSettingsService systemSettingsService)
        {
            _relevantFactService = relevantFactService;
            _mapper = mapper;
            _uow = uow;
            _cacheService = cacheService;
            _stockService = stockService;
            _notificationHistoricalService = notificationHistoricalService;
            _deviceService = deviceService;
            _settingsService = settingsService;
            _notificationService = notificationService;
            _logger = logger;
            _globalAuthenticationService = globalAuthenticationService;
            _s3Service = s3Service;
            _companySerivce = companyService;
            _systemSettingsService = systemSettingsService;
        }

        public ResultResponseObject<IEnumerable<RelevantFactVM>> GetAll(bool onlyMyUser)
        {
            ResultResponseObject<IEnumerable<RelevantFactVM>> result = null;

            string nameOnCache = "RelevantFact";

            if (onlyMyUser)
            {
                nameOnCache = string.Concat("RelevantFact:", _globalAuthenticationService.IdUser);
            }

            string resultFromCache = _cacheService.GetFromCache(nameOnCache);

            if (resultFromCache == null)
            {
                using (_uow.Create())
                {
                    ResultServiceObject<IEnumerable<RelevantFactView>> resultService = _relevantFactService.GetAll(onlyMyUser, _globalAuthenticationService.IdUser);

                    result = _mapper.Map<ResultResponseObject<IEnumerable<RelevantFactVM>>>(resultService);

                    _cacheService.SaveOnCache(nameOnCache, TimeSpan.FromMinutes(1), JsonConvert.SerializeObject(result));
                }
            }
            else
            {
                result = JsonConvert.DeserializeObject<ResultResponseObject<IEnumerable<RelevantFactVM>>>(resultFromCache);
                result.Success = true;
            }

            return result;
        }

        public ResultResponseObject<RelevantFactVM> Add(RelevantFactAdd relevantFactAdd)
        {
            ResultResponseObject<RelevantFactVM> result = null;

            RelevantFact relevantFact = _mapper.Map<RelevantFact>(relevantFactAdd);

            using (_uow.Create())
            {
                relevantFact.CompanyID = _companySerivce.GetByGuid(relevantFactAdd.GuidCompany).Value.IdCompany;
                ResultServiceObject<RelevantFact> resultService = _relevantFactService.Add(relevantFact);
                result = _mapper.Map<ResultResponseObject<RelevantFactVM>>(resultService);
            }

            return result;
        }

        public void SendNotificationToInteressedUsers()
        {
            //Get list of user that have this stock in portfolio
            using (_uow.Create())
            {
                var relevantFacts = _relevantFactService.GetItensWaitToBeSend();

                foreach (var itemRelevantFact in relevantFacts.Value)
                {
                    itemRelevantFact.NotificationSent = true;
                    _relevantFactService.Update(itemRelevantFact);
                }

                foreach (var itemRelevantFact in relevantFacts.Value)
                {
                    var stock = _stockService.GetByCompanyID(itemRelevantFact.CompanyID);

                    foreach (var itemStock in stock.Value)
                    {
                        ResultServiceObject<IEnumerable<string>> resultUsersServiceObject = _stockService.GetAllUsersWithStock(itemStock.IdStock);

                        string pushMessage = string.Format("App Dividendos.me alerta: Novo fato relevante referente a {0} disponível. Entre no App para saber mais.", itemStock.Symbol);

                        string pushMessageTitle = "Novo fato relevante relacionado as suas ações";

                        foreach (var itemUser in resultUsersServiceObject.Value)
                        {
                            ResultServiceObject<Settings> settings = _settingsService.GetByUser(itemUser);

                            if (settings.Value == null || settings.Value.PushRelevantFacts)
                            {
                                ResultServiceObject<IEnumerable<Device>> devices = _deviceService.GetByUserAndOffSetVersion(itemUser, "4.2.43");

                                if (devices.Value.Count() > 0)
                                {
                                    _notificationHistoricalService.New(pushMessageTitle, pushMessage, itemUser, AppScreenNameEnum.RelevantFact.ToString(), PushRedirectTypeEnum.Internal.ToString(), null, _cacheService);
                                }

                                foreach (Device itemDevice in devices.Value)
                                {
                                    try
                                    {
                                        _notificationService.SendPush(pushMessageTitle, pushMessage, itemDevice, new PushRedirect() { PushRedirectType = PushRedirectTypeEnum.Internal, AppScreenName = AppScreenNameEnum.RelevantFact });
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

        public void ImportRelevantFacts()
        {
            string nonce = string.Empty;

            using (_uow.Create())
            {
                nonce = _systemSettingsService.GetByKey(Constants.SYSTEM_SETTINGS_FATOS_RELEVANTES_NONCE).Value.SettingValue;
            }

            InfoMoneyHelper infoMoneyHelper = new InfoMoneyHelper();
            RelevantFactInfoMoney relevantFactInfoMoney =  infoMoneyHelper.ImportRelevantFacts(nonce);

            if (relevantFactInfoMoney != null && relevantFactInfoMoney.Data != null && relevantFactInfoMoney.Data.Count > 0)
            {
                Entity.Entities.SystemSettings systemSettings = null;

                using (_uow.Create())
                {
                    systemSettings = _systemSettingsService.GetByKey(Constants.SYSTEM_SETTINGS_DISCARD_RELEVANT_FACTS).Value;
                }

                foreach (DataInfoMoney dataInfoMoney in relevantFactInfoMoney.Data)
                {
                    bool discard = false;

                    if (systemSettings != null && !string.IsNullOrWhiteSpace(systemSettings.SettingValue))
                    {
                        discard = systemSettings.SettingValue.Contains(dataInfoMoney.CodigoCvm);
                    }

                    if (!discard)
                    {
                        RelevantFact relevantFact = null;

                        using (_uow.Create())
                        {
                            relevantFact = _relevantFactService.GetByUrl(dataInfoMoney.UrlDocumento).Value;
                        }

                        if (relevantFact == null)
                        {
                            IEnumerable<Company> companies = null;

                            using (_uow.Create())
                            {
                                companies = _companySerivce.GetByName(dataInfoMoney.Nome).Value;
                            }

                            if (companies != null && companies.Count() > 0)
                            {
                                companies = companies.Where(cp => cp.IdCountry == 1);
                            }

                            if (companies != null && companies.Count() == 1)
                            {
                                RelevantFactAdd relevantFactAdd = new RelevantFactAdd();
                                relevantFactAdd.GuidCompany = companies.First().GuidCompany.ToString();
                                relevantFactAdd.ReferenceDate = Convert.ToDateTime(dataInfoMoney.Referencia, new CultureInfo("pt-br"));
                                relevantFactAdd.URL = dataInfoMoney.UrlDocumento;

                                Add(relevantFactAdd);
                            }
                            else
                            {
                                SendAdminNotification(string.Format("Fato Irrelevante: Empresa {0} não encontrada", dataInfoMoney.Nome));
                            }
                        }
                    }
                }
            }
        }

        public void SendAdminNotification(string message)
        {
            using (_uow.Create())
            {
                ResultServiceObject<IEnumerable<Device>> devices = _deviceService.GetAdminDevices();

                foreach (Device itemDevice in devices.Value)
                {
                    try
                    {
                        _notificationService.SendPush("Falha na integração", message, itemDevice, null);
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