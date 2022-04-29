using Dividendos.API.Model.Request;
using Dividendos.API.Model.Response.Common;
using System.Threading.Tasks;

namespace Dividendos.Application.Interface
{
    public interface IDashboardApp
    {
        ResultResponseObject<UserVM> GetDashboardDetails();

    }
}