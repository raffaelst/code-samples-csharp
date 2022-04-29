using Dividendos.API.Model.Request.Crypto;
using Dividendos.API.Model.Response.Common;
using Dividendos.API.Model.Response.Crypto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dividendos.Application.Interface
{
    public interface ICryptoPortfolioApp
    {
        ResultResponseObject<CryptoPortfolioVM> CreateManualPortfolio(CryptoPortolioRequest cryptoPortolioRequest);
        ResultResponseObject<CryptoPortfolioViewWrapperVM> GetCryptoPortfolioViewWrapper();
        ResultResponseBase CalculatePerformance();
        ResultResponseObject<CryptoCurrencyStatementWrapperVM> GetCryptoCurrencyStatementWrapperVM(Guid guidCryptoportfolioSub);
        ResultResponseObject<CryptoCurrencyStatementVM> GetCryptoCurrencyStatementView(Guid guidCryptoPortfolio, Guid guidCryptoCurrency);
        ResultResponseBase Disable(Guid guidCryptoPortfolio);
        ResultResponseBase UpdateName(Guid guidCryptoPortfolio, CryptoPortfolioEditVM cryptoPortfolioEditVM);
        ResultResponseObject<CryptoSubportfolioWrapperVM> GetCryptoPortfolioContentSimple(Guid guidCryptoPortfolio);
        ResultResponseObject<CryptoSubportfolioWrapperVM> GetCryptoSubportfolioContentSimple(Guid guidCryptoPortfolio, Guid guidCryptoSubportfolio);
    }
}
