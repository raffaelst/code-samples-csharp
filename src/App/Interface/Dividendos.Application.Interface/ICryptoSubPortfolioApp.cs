using Dividendos.API.Model.Request.Crypto;
using Dividendos.API.Model.Response.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dividendos.Application.Interface
{
    public interface ICryptoSubPortfolioApp
    {
        ResultResponseBase Disable(Guid guidCryptoSubPortfolio);
        ResultResponseObject<CryptoSubportfolioVM> Add(Guid guidCryptoPortfolio, CryptoSubportfolioVM cryptoSubportfolioVM);
        ResultResponseObject<CryptoSubportfolioVM> Update(Guid guidCryptoSubportfolio, CryptoSubportfolioVM subPortfolioVM);
    }
}
