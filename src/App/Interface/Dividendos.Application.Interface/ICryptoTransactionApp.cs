using Dividendos.API.Model.Request.Crypto;
using Dividendos.API.Model.Response;
using Dividendos.API.Model.Response.Common;
using Dividendos.API.Model.Response.Crypto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dividendos.Application.Interface
{
    public interface ICryptoTransactionApp
    {
        ResultResponseObject<CryptoBuyVM> BuyCrypto(Guid guidCryptoPortfolio, CryptoAddVM cryptoAddVM);
        ResultResponseBase SellCrypto(Guid guidCryptoPortfolio, CryptoAddVM cryptoAddVM);
        ResultResponseBase EditBuyTransaction(Guid guidCryptoPortfolio, CryptoEditVM cryptoEditVM);
        ResultResponseBase EditSellTransaction(Guid guidCryptoPortfolio, CryptoEditVM cryptoEditVM);
        ResultResponseBase InactiveCryptoTransactionItem(Guid guidCryptoPortfolio, Guid guidCryptoTransactionItem);
        ResultResponseObject<CryptoTransactionItemSummaryWrapperVM> GetTransactionItemSummary(Guid guidPortfolio, Guid guidCryptoCurrency, int transactionType);
        ResultResponseObject<CryptoTransactionSellViewWrapperVM> GetCryptoTransactionSellView(Guid guidPortfolioSub, string startDate, string endDate);
        ResultResponseObject<CryptoTransactionBuyViewWrapperVM> GetCryptoTransactionBuyView(Guid guidCryptoPortfolioSub, string startDate, string endDate);
    }
}
