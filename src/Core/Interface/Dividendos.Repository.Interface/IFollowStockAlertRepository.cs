using Dividendos.Entity.Entities;
using Dividendos.Repository.Interface.GenericRepository;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Dividendos.Repository.Interface
{
    public interface IFollowStockAlertRepository : IRepository<FollowStockAlert>
    {
        IEnumerable<FollowStockAlert> GetAllActiveAlertsByFollowStock(string followStockGuid);
    }
}
