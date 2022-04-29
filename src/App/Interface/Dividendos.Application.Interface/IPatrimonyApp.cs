using Dividendos.API.Model.Request;
using Dividendos.API.Model.Response;
using Dividendos.API.Model.Response.Common;
using Dividendos.API.Model.Response.FinancialProducts;
using Dividendos.Application.Interface.Model;
using System.Collections.Generic;


namespace Dividendos.Application.Interface
{
    public interface IPatrimonyApp
    {
        ResultResponseObject<PatrimonyVM> GetPatrimony();
        ResultResponseObject<List<PatrimonyComposeResponse>> GetPatrimonyCompose();
        void ChangePatrimonyCompose(IEnumerable<PatrimonyComposeRequest> patrimonyComposeRequests);
    }
}
