using Dividendos.Entity.Views;
using Dividendos.Repository.Interface.GenericRepository;
using System;
using System.Collections.Generic;
using System.Text;

namespace Dividendos.Repository.Interface
{
    public interface ICryptoCurrencyStatementViewRepository : IRepository<CryptoCurrencyStatementView>
    {
        IEnumerable<CryptoCurrencyStatementView> GetByCryptoPortfolio(Guid guidCryptoPortfolio);
        IEnumerable<CryptoCurrencyStatementView> GetByCryptoSubportfolio(Guid guidCryptoSubPortfolio);
        CryptoCurrencyStatementView GetByGuidCurrency(Guid guidCryptoPortfolio, Guid guidCryptoCurrency);
        IEnumerable<CryptoCurrencyStatementView> GetCryptoSummaryByPortfolioOrSubPortfolio(string idUser, string guidCryptoPortfolioSub);
    }
}
