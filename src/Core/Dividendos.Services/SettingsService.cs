using FluentValidation.Results;
using Dividendos.Entity.Entities;
using Dividendos.Entity.Model;
using Dividendos.Repository.Interface;
using Dividendos.Repository.Interface.UoW;
using Dividendos.Service.Interface;
using Dividendos.Service.Validator.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Dividendos.Service
{
    public class SettingsService : BaseService, ISettingsService
    {
        public SettingsService(IUnitOfWork uow)
        {
            _uow = uow;
        }

        public ResultServiceObject<Settings> ChangeSettings(Settings settings)
        {
            ResultServiceObject<Settings> resultService = new ResultServiceObject<Settings>();

            resultService.Value =  _uow.SettingsRepository.Update(settings);

            return resultService;
        }

        public ResultServiceObject<Settings> Insert(Settings settings)
        {
            ResultServiceObject<Settings> resultService = new ResultServiceObject<Settings>();
            settings.GuidSettings = Guid.NewGuid();
            long idSettings = _uow.SettingsRepository.Insert(settings);

            settings.IdSettings = idSettings;

            return resultService;
        }


        public ResultServiceObject<Settings> GetByUser(string idUser)
        {
            ResultServiceObject<Settings> resultService = new ResultServiceObject<Settings>();

            Settings settings = _uow.SettingsRepository.GetByIdUser(idUser);

            resultService.Value = settings;

            return resultService;
        }

        public ResultServiceObject<Settings> InitAndCreate(string idUser)
        {
            ResultServiceObject<Settings> resultService = new ResultServiceObject<Settings>();


            Settings settings = new Settings();

            settings.GuidSettings = Guid.NewGuid();
            settings.AutoSyncPortfolio = true;
            settings.IdUser = idUser;
            settings.PushChangeInPortfolio = true;
            settings.PushDividendDeposit = true;
            settings.PushNewDividend = true;
            settings.SendDailySummaryMail = false;
            settings.AutomaticRefreshPortfolio = true;
            settings.PushMarketOpeningAndClosing = true;
            settings.PushBreakingNews = true;
            settings.PushDataComYourStocks = true;
            settings.PushStocksWithAwesomeVariation = true;
            settings.PushRelevantFacts = true;
            long idSettings = _uow.SettingsRepository.Insert(settings);

            settings.IdSettings = idSettings;

            return resultService;
        }
    }
}
