using Dividendos.Entity.Entities;
using Dividendos.Repository.Interface.GenericRepository;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Dividendos.Repository.Interface
{
    public interface ISyncQueueRepository : IRepository<SyncQueue>
    {
        void ClearAllDoneAndInUse();

        SyncQueue GetLastAvailable();

        void CustomUpdate(long idSyncQueue, bool done, bool inUse);
    }
}
