using Dividendos.API.Model.Response;
using Dividendos.API.Model.Response.Common;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Dividendos.Application.Interface
{
    public interface IInitialOfferApp
    {
        ResultResponseObject<IEnumerable<InitialOfferVM>> GetAllCryptos();
        ResultResponseObject<IEnumerable<InitialOfferVM>> GetAllStocks();
    }
}
