using Dividendos.Entity.Entities;
using Dividendos.Entity.Views;
using Dividendos.Repository.Interface.GenericRepository;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Dividendos.Repository.Interface
{
    public interface IFollowStockViewRepository : IRepository<FollowStockView>
    {
        IEnumerable<FollowStockView> GetAllAlertsActiveAndNotReached();
        IEnumerable<FollowStockView> GetAlertsNotDeletedByUser(string userID);
        IEnumerable<FollowStockView> GetByIdStockOrIdCrypto(string idStockOrCrypto, string userID);
    }
}
