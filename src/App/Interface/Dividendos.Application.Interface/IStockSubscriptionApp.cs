using Dividendos.API.Model.Response;
using Dividendos.API.Model.Response.Common;
using Dividendos.Application.Interface.Model;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Dividendos.Application.Interface
{
    public interface IStockSubscriptionApp
    {
        ResultResponseObject<IEnumerable<PortfolioStatementViewModel>> GetPortfolioStatementSubscriptionView(Guid guidPortfolioSub);
    }
}
