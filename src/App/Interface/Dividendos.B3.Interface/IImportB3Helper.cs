using Dividendos.B3.Interface.Model;
using K.Logger;
using System.Threading;
using System.Threading.Tasks;

namespace Dividendos.B3.Interface
{
    public interface IImportB3Helper
    {
       ImportCeiResult ImportCei(string username, string password, string idUser, bool automaticProcess, string startDateFilter, CancellationTokenSource ct = null);
    }
}
