using Dividendos.Entity.Entities;
using Dividendos.Entity.Enum;
using Dividendos.Entity.Views;
using Dividendos.Repository.Interface.GenericRepository;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Dividendos.Repository.Interface
{
    public interface ITraderViewRepository : IRepository<TraderView>
    {
        IEnumerable<TraderView> GetItemsComposePatrimony(string userID);
        void UpdateShowOnPatrimony(Guid guidTrader, bool showOnPatrimony);
    }
}
