using Dividendos.API.Model.Request.Crypto;
using Dividendos.API.Model.Response.Common;
using Dividendos.API.Model.Response.Crypto;
using Dividendos.Entity.Entities;
using Dividendos.Entity.Model;
using Dividendos.Entity.Views;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Dividendos.Service.Interface
{
    public interface ICryptoTransactionService : IBaseService
    {
        ResultServiceObject<CryptoTransaction> Update(CryptoTransaction cryptoTransaction);
        bool CalculateAveragePrice(ref List<CryptoTransactionItem> transactionItems, out decimal numberOfShares, out decimal avgPrice, bool removeNewItems = false, DateTime? lastSync = null, bool breakInvalid = true);
        ResultServiceObject<IEnumerable<CryptoTransaction>> GetByPortfolioTransactionType(long idCryptoPortfolio, int idTransactionType);
        ResultServiceObject<CryptoTransaction> GetByCryptoPortfolioAndIdCryptoCurrency(long idCryptoPortfolio, long idCryptoCurrency, int transactionType);
        ResultResponseObject<CryptoBuyVM> BuyCrypto(Guid guidCryptoPortfolio, CryptoAddVM cryptoAddVM, ICryptoPortfolioService _cryptoPortfolioService, ICryptoTransactionService _cryptoTransactionService,
        ICryptoTransactionItemService _cryptoTransactionItem, ICryptoPortfolioPerformanceService _cryptoPortfolioPerformanceService, ICryptoCurrencyService _cryptoCurrencyService, ICryptoCurrencyPerformanceService _cryptoCurrencyPerformanceService, ISubscriptionService _subscriptionService, string idUser, bool priceAdjust = false,
        bool calculatePeformance = true, bool removeNewItems = false, DateTime? lastSync = null, bool breakInvalid = true, bool editedByUser = false);

        void SellCrypto(Guid guidCryptoPortfolio, CryptoAddVM cryptoAddVM, ResultResponseBase resultResponseBase, ICryptoPortfolioService _cryptoPortfolioService, ICryptoTransactionService _cryptoTransactionService,
        ICryptoTransactionItemService _cryptoTransactionItem, ICryptoPortfolioPerformanceService _cryptoPortfolioPerformanceService, ICryptoCurrencyService _cryptoCurrencyService, ICryptoCurrencyPerformanceService _cryptoCurrencyPerformanceService, bool priceAdjust = false,
        bool calculatePeformance = true, bool removeNewItems = false, DateTime? lastSync = null, bool breakInvalid = true, bool editedByUser = false);

        ResultResponseBase EditBuyTransaction(Guid guidCryptoPortfolio, CryptoEditVM cryptoEditVM, ICryptoPortfolioService _cryptoPortfolioService, ICryptoTransactionItemService _cryptoTransactionItemService, ICryptoPortfolioPerformanceService _cryptoPortfolioPerformanceService, ICryptoCurrencyService _cryptoCurrencyService, ICryptoCurrencyPerformanceService _cryptoCurrencyPerformanceService, ICryptoTransactionService _cryptoTransactionService);

        void EditSellTransaction(Guid guidCryptoPortfolio, CryptoEditVM cryptoEditVM, ResultResponseBase resultResponseBase, ICryptoPortfolioService _cryptoPortfolioService, ICryptoTransactionItemService _cryptoTransactionItemService,
            ICryptoPortfolioPerformanceService _cryptoPortfolioPerformanceService, ICryptoCurrencyService _cryptoCurrencyService, ICryptoCurrencyPerformanceService _cryptoCurrencyPerformanceService, ICryptoTransactionService _cryptoTransactionService);

        ResultServiceObject<IEnumerable<CryptoTransactionDetailsView>> GetDetailsByIdCryptoPortfolio(long idCryptoPortfolio, int transactionType, DateTime? startDate = null, DateTime? endDate = null);
        ResultServiceObject<IEnumerable<CryptoTransactionDetailsView>> GetDetailsByIdCryptoSubPortfolio(long idCryptoPortfolio, long idCryptoSubPortfolio, int transactionType, DateTime? startDate = null, DateTime? endDate = null);
        ResultServiceObject<CryptoTransaction> GetByGuid(Guid guidCryptoTransaction);
        ResultServiceObject<int> CountCryptoCurrencyByUser(string idUser);
    }
}
