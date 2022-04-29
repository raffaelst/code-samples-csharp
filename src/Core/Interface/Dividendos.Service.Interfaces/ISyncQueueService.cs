using Dividendos.Entity.Entities;
using Dividendos.Entity.Model;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Dividendos.Service.Interface
{
    public interface ISyncQueueService : IBaseService
    {
        ResultServiceObject<SyncQueue> GetLastAvailable();

        ResultServiceObject<SyncQueue> Add(SyncQueue syncQueue);

        ResultServiceObject<SyncQueue> Update(SyncQueue syncQueue);

        void ClearAllDoneAndInUse();

        void CustomUpdate(long idSyncQueue, bool done, bool inUse);
    }
}
