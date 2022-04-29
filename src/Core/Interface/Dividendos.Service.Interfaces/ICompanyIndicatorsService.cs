using Dividendos.Entity.Entities;
using Dividendos.Entity.Model;
using Dividendos.Entity.Views;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dividendos.Service.Interface
{
    public interface ICompanyIndicatorsService : IBaseService
    {
        ResultServiceObject<CompanyIndicators> Update(CompanyIndicators companyIndicators);
        ResultServiceObject<CompanyIndicators> Insert(CompanyIndicators companyIndicators);
        ResultServiceObject<CompanyIndicatorsView> GetCompanyIndicators(Guid guidStock);
        ResultServiceObject<IEnumerable<CompanyIndicatorsView>> GetList(IGlobalAuthenticationService globalAuthenticationService, bool onlyMyStocks);
        ResultServiceObject<IEnumerable<CompanyIndicatorsView>> GetListGetListFilterByType(IGlobalAuthenticationService globalAuthenticationService, StockType stockType, bool onlyMyStocks);
    }
}
