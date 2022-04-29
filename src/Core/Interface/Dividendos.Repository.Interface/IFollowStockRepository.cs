using Dividendos.Entity.Entities;
using Dividendos.Repository.Interface.GenericRepository;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Dividendos.Repository.Interface
{
    public interface IFollowStockRepository : IRepository<FollowStock>
    {
        IEnumerable<FollowStock> ExistForASpecificUser(long stockIdOrCryptoId, string userID);
    }
}
