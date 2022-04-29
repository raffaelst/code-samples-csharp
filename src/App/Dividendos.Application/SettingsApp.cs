using AutoMapper;
using Dividendos.API.Model.Request.Settings;
using Dividendos.API.Model.Response.Common;

using Dividendos.Application.Base;
using Dividendos.Application.Interface;
using Dividendos.Entity.Entities;
using Dividendos.Entity.Model;
using Dividendos.Repository.Interface.UoW;
using Dividendos.Service.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Dividendos.Application
{
    public class SettingsApp : BaseApp, ISettingsApp
    {
        private readonly ISettingsService _settingsService;
        private readonly IMapper _mapper;
        private readonly IUnitOfWork _uow;
        private readonly IGlobalAuthenticationService _globalAuthenticationService;

        public SettingsApp(IMapper mapper,
            IUnitOfWork uow,
            ISettingsService settingsService,
            IGlobalAuthenticationService globalAuthenticationService)
        {
            _settingsService = settingsService;
            _mapper = mapper;
            _uow = uow;
            _globalAuthenticationService = globalAuthenticationService;
        }

        public ResultResponseBase ChangeSettings(SettingsEditVM settingsEditVM)
        {
            ResultResponseBase resultResponseBase;
            ResultServiceObject<Settings> result = null;

            using (_uow.Create())
            {
                ResultServiceObject<Settings> settings =  _settingsService.GetByUser(_globalAuthenticationService.IdUser);

                if (settings.Value != null)
                {
                    settings.Value.SendDailySummaryMail = settingsEditVM.SendDailySummaryMail;
                    settings.Value.PushChangeInPortfolio = settingsEditVM.PushChangeInPortfolio;
                    settings.Value.PushDividendDeposit = settingsEditVM.PushDividendDeposit;
                    settings.Value.PushNewDividend = settingsEditVM.PushNewDividend;
                    settings.Value.AutomaticRefreshPortfolio = settingsEditVM.AutomaticRefreshPortfolio;
                    settings.Value.PushMarketOpeningAndClosing = settingsEditVM.PushMarketOpeningAndClosing;
                    settings.Value.PushBreakingNews = settingsEditVM.PushBreakingNews;
                    settings.Value.PushDataComYourStocks = settingsEditVM.PushDataComYourStocks;
                    settings.Value.PushStocksWithAwesomeVariation = settingsEditVM.PushStocksWithAwesomeVariation;
                    settings.Value.PushRelevantFacts = settingsEditVM.PushRelevantFacts;
                    result = _settingsService.ChangeSettings(settings.Value);
                }
                else
                {
                    Settings settingsSave = new Settings();
                    settingsSave.IdUser = _globalAuthenticationService.IdUser;
                    settingsSave.AutoSyncPortfolio = true;
                    settingsSave.PushMarketOpeningAndClosing = settingsEditVM.PushMarketOpeningAndClosing;
                    settingsSave.SendDailySummaryMail = settingsEditVM.SendDailySummaryMail;
                    settingsSave.PushChangeInPortfolio = settingsEditVM.PushChangeInPortfolio;
                    settingsSave.PushDividendDeposit = settingsEditVM.PushDividendDeposit;
                    settingsSave.PushNewDividend = settingsEditVM.PushNewDividend;
                    settingsSave.AutomaticRefreshPortfolio = settingsEditVM.AutomaticRefreshPortfolio;
                    settingsSave.PushBreakingNews = settingsEditVM.PushBreakingNews;
                    settingsSave.PushDataComYourStocks = settingsEditVM.PushDataComYourStocks;
                    settingsSave.PushStocksWithAwesomeVariation = settingsEditVM.PushStocksWithAwesomeVariation;
                    settingsSave.PushRelevantFacts = true;
                    result = _settingsService.Insert(settingsSave);
                }
            }

            resultResponseBase = _mapper.Map<ResultResponseBase>(result);

            return resultResponseBase;
        }

        public ResultResponseBase ChangeSettingsV2(SettingsEditVM settingsEditVM)
        {
            ResultResponseBase resultResponseBase;
            ResultServiceObject<Settings> result = null;

            using (_uow.Create())
            {
                ResultServiceObject<Settings> settings = _settingsService.GetByUser(_globalAuthenticationService.IdUser);

                if (settings.Value != null)
                {
                    settings.Value.SendDailySummaryMail = settingsEditVM.SendDailySummaryMail;
                    settings.Value.PushChangeInPortfolio = settingsEditVM.PushChangeInPortfolio;
                    settings.Value.PushDividendDeposit = settingsEditVM.PushDividendDeposit;
                    settings.Value.PushNewDividend = settingsEditVM.PushNewDividend;
                    settings.Value.AutomaticRefreshPortfolio = settingsEditVM.AutomaticRefreshPortfolio;
                    settings.Value.PushMarketOpeningAndClosing = settingsEditVM.PushMarketOpeningAndClosing;
                    settings.Value.PushBreakingNews = settingsEditVM.PushBreakingNews;
                    settings.Value.PushDataComYourStocks = settingsEditVM.PushDataComYourStocks;
                    settings.Value.PushStocksWithAwesomeVariation = settingsEditVM.PushStocksWithAwesomeVariation;
                    settings.Value.PushRelevantFacts = settingsEditVM.PushRelevantFacts;
                    result = _settingsService.ChangeSettings(settings.Value);
                }
                else
                {
                    Settings settingsSave = new Settings();
                    settingsSave.IdUser = _globalAuthenticationService.IdUser;
                    settingsSave.AutoSyncPortfolio = true;
                    settingsSave.PushMarketOpeningAndClosing = settingsEditVM.PushMarketOpeningAndClosing;
                    settingsSave.SendDailySummaryMail = settingsEditVM.SendDailySummaryMail;
                    settingsSave.PushChangeInPortfolio = settingsEditVM.PushChangeInPortfolio;
                    settingsSave.PushDividendDeposit = settingsEditVM.PushDividendDeposit;
                    settingsSave.PushNewDividend = settingsEditVM.PushNewDividend;
                    settingsSave.AutomaticRefreshPortfolio = settingsEditVM.AutomaticRefreshPortfolio;
                    settingsSave.PushBreakingNews = settingsEditVM.PushBreakingNews;
                    settingsSave.PushDataComYourStocks = settingsEditVM.PushDataComYourStocks;
                    settingsSave.PushStocksWithAwesomeVariation = settingsEditVM.PushStocksWithAwesomeVariation;
                    settingsSave.PushRelevantFacts = settingsEditVM.PushRelevantFacts;
                    result = _settingsService.Insert(settingsSave);
                }
            }

            resultResponseBase = _mapper.Map<ResultResponseBase>(result);

            return resultResponseBase;
        }

        public ResultResponseObject<SettingsEditVM> Get()
        {
            ResultServiceObject<Settings> settings = null;

            using (_uow.Create())
            {
                settings = _settingsService.GetByUser(_globalAuthenticationService.IdUser);
            }

            ResultResponseObject<SettingsEditVM> result = _mapper.Map<ResultResponseObject<SettingsEditVM>>(settings);

            return result;
        }
    }
}