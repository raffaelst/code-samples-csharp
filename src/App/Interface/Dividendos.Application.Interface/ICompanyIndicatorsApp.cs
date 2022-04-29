using Dividendos.API.Model.Response;
using Dividendos.API.Model.Response.Common;
using Dividendos.API.Model.Response.v1.CompanyIndicator;
using Dividendos.Entity.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dividendos.Application.Interface
{
    public interface ICompanyIndicatorsApp
    {
        void ImportCompanyIndicators(Stock stock);
        void ImportCompanyIndicators();
        ResultResponseObject<CompanyIndicatorWrapperVM> GetCompanyIndicators(Guid guidStock);
        ResultResponseObject<IEnumerable<CompanyIndicatorVM>> GetList(API.Model.Request.Stock.StockType stockType, bool onlyMyStocks);
    }
}
