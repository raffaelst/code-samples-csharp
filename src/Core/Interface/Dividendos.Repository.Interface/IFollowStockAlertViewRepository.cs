using Dividendos.Entity.Entities;
using Dividendos.Entity.Views;
using Dividendos.Repository.Interface.GenericRepository;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Dividendos.Repository.Interface
{
    public interface IFollowStockAlertViewRepository : IRepository<FollowStockAlertView>
    {
        IEnumerable<FollowStockAlertView> GetAllActiveAlertsByFollowStock(string followStockGuid);
    }
}
