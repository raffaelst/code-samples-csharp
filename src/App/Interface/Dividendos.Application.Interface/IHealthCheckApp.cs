using Dividendos.API.Model.Request.Purchase;
using Dividendos.API.Model.Response.Common;
using Dividendos.API.Model.Response.Purchase;
using System.Security.Cryptography.X509Certificates;
using System.Threading.Tasks;

namespace Dividendos.Application.Interface
{
    public interface IHealthCheckApp
    {
        ResultResponseObject<bool> GetStatus(string token);

    }
}
