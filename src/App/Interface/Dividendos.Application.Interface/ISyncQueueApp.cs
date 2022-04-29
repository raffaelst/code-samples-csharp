using Dividendos.API.Model.Response.Common;
using System.Threading.Tasks;

namespace Dividendos.Application.Interface
{
    public interface ISyncQueueApp
    {
        void ClearAllDoneAndInUse();

        void ClearAllLogs();
    }
}
