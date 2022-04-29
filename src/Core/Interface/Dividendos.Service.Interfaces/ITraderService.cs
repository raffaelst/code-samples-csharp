using Dividendos.Entity.Entities;
using Dividendos.Entity.Enum;
using Dividendos.Entity.Model;
using Dividendos.Entity.Views;
using K.UnitOfWorkBase;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Dividendos.Service.Interface
{
    public interface ITraderService : IBaseService
    {
        ResultServiceObject<IEnumerable<Trader>> GetAll();
        ResultServiceObject<Trader> GetById(long idTrader);
        ResultServiceObject<Trader> GetByIdentifierAndUserActive(string identifier, string idUser, TraderTypeEnum traderTypeEnum);
        ResultServiceObject<Trader> Insert(Trader trader);
        ResultServiceObject<IEnumerable<Trader>> GetByUserActiveAutomatic(string idUser);
        ResultServiceObject<IEnumerable<Trader>> GetByUserActive(string idUser);

        ResultServiceObject<Trader> SaveTrader(string identifier, string password, string idUser, bool block, bool manual, TraderTypeEnum traderTypeEnun);
        ResultServiceObject<Trader> SaveTrader(string identifier, string password, string idUser, bool block, bool manual, TraderTypeEnum traderTypeEnun, out DateTime lastSync);

        ResultServiceObject<IEnumerable<Trader>> GetAllAutomaticActive();
        ResultServiceObject<Trader> GetByOlderSyncDate();
        ResultServiceObject<Trader> GetBlockedByUser(string idUser);

        ResultServiceObject<IEnumerable<Trader>> GetAllBlockedAutomatic();
        void UpdateSyncData(long idTrader, DateTime dateTime);
        void Disable(long idTrader);

        ResultServiceObject<Trader> GetByUserAndGuidTrader(string idUser, Guid traderGuid);
        ResultServiceObject<Trader> GetByLatestSyncDate();
        ResultServiceObject<Trader> GetByIdentifier(string identifier, TraderTypeEnum traderTypeEnum);
        ResultServiceObject<IEnumerable<Trader>> GetTodayDelayedSync();
        ResultServiceObject<IEnumerable<Trader>> GetYesterdayDelayedSync();
        ResultServiceObject<Trader> GetLikeIdentifier(string identifier);
        ResultServiceObject<Trader> GetByUser(string idUser);
        ResultServiceObject<IEnumerable<Trader>> GetWeekDelayedSync();
        ResultServiceObject<IEnumerable<Trader>> GetByUserManual(string idUser);
        ResultServiceObject<IEnumerable<Trader>> GetAllByIdentifierActive(string identifier, TraderTypeEnum traderTypeEnum);
        ResultServiceObject<IEnumerable<TraderView>> GetItemsComposePatrimony(string idUser);

        ResultServiceObject<TraderView> ChangeComposePatrimony(Guid guidTrader, bool showOnPatrimony);
        ResultServiceObject<Trader> Update(Trader trader);
        ResultServiceObject<Trader> GetByIdentifierAndUser(string identifier, string idUser, TraderTypeEnum traderTypeEnum);
        ResultServiceObject<IEnumerable<Trader>> GetAllByIdentifier(string identifier, TraderTypeEnum traderTypeEnum);
        void DisableCascade(long idTrader, IPortfolioService portfolioService, ISubPortfolioService subPortfolioService, IFinancialProductService financialProductService, ICryptoPortfolioService cryptoPortfolioService, ICryptoSubPortfolioService cryptoSubPortfolioService);
        ResultServiceObject<IEnumerable<Trader>> GetTradersInactiveB3();
        ResultServiceObject<Trader> GetLatestInactiveCei(string idUser);
    }
}
