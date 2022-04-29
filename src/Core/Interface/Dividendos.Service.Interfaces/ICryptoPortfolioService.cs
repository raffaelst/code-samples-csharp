using Dividendos.Entity.Entities;
using Dividendos.Entity.Model;
using Dividendos.Entity.Views;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Dividendos.Service.Interface
{
    public interface ICryptoPortfolioService : IBaseService
    {
        ResultServiceObject<CryptoPortfolio> Insert(CryptoPortfolio cryptoPortfolio);
        void UpdateCalculatePerformanceDate(long idCryptoPortfolio, DateTime dateTime);
        ResultServiceObject<CryptoPortfolio> GetById(long idCryptoPortfolio);
        ResultServiceObject<CryptoPortfolio> GetByGuid(Guid guidCryptoPortfolio);
        ResultServiceObject<IEnumerable<CryptoPortfolioView>> GetByUser(string idUser);
        ResultServiceObject<IEnumerable<CryptoPortfolio>> GetByUser(string idUser, bool status, bool? manual = null);
        ResultServiceObject<IEnumerable<CryptoCurrencyStatementView>> GetByCryptoPortfolio(Guid guidPortfolio);
        ResultServiceObject<IEnumerable<CryptoCurrencyStatementView>> GetByCryptoSubportfolio(Guid guidCryptoSubPortfolio);
        ResultServiceObject<CryptoCurrencyStatementView> GetByGuidCurrency(Guid guidCryptoPortfolio, Guid guidCryptoCurrency);
        ResultServiceObject<IEnumerable<CryptoCurrencyStatementView>> GetCryptoSummaryByPortfolioOrSubPortfolio(string idUser, string guidCryptoPortfolioSub);
        ResultServiceObject<CryptoPortfolio> GetByIdTrader(long idTrader);
        void Disable(CryptoPortfolio cryptoPortfolio);
        void UpdateName(long idCryptoPortfolio, string name);
        ResultServiceObject<IEnumerable<CryptoSubportfolioItemView>> GetSubViewByCryptoPortfolio(Guid guidCryptoPortfolio);
        ResultServiceObject<IEnumerable<CryptoSubportfolioItemView>> GetBySubCryptoPortfolio(long idCryptoPortfolio, long idCryptoSubPortfolio);
    }
}
