using Dividendos.Entity.Entities;
using Dividendos.Entity.Enum;
using Dividendos.Repository.Interface.GenericRepository;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Dividendos.Repository.Interface
{
    public interface ITraderRepository : IRepository<Trader>
    {
        Trader GetByOlderSyncDate();
        void UpdateSyncData(long idTrader, DateTime syncDate);
        void Disable(long idTrader);
        Trader GetByLatestSyncDate();
        IEnumerable<Trader> GetTodayDelayedSync();
        IEnumerable<Trader> GetYesterdayDelayedSync();
        IEnumerable<Trader> GetTradersBlocked(string userID, TraderTypeEnum traderTypeEnum);
        IEnumerable<Trader> GetLikeIdentifier(string identifier);
        IEnumerable<Trader> GetWeekDelayedSync();
        IEnumerable<Trader> GetAllBlockedAutomatic();
        void UpdateTraderCei(long idTrader, DateTime syncDate, bool blockedCei, string password, bool active);
        IEnumerable<Trader> GetTradersInactiveB3();
        IEnumerable<Trader> GetLatestInactiveCei(string idUser);
    }
}
