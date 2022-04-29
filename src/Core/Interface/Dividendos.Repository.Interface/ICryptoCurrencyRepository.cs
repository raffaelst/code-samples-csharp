using Dividendos.Entity.Entities;
using Dividendos.Entity.Views;
using Dividendos.Repository.Interface.GenericRepository;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Dividendos.Repository.Interface
{
    public interface ICryptoCurrencyRepository : IRepository<CryptoCurrency>
    {
        IEnumerable<CryptoStatementView> GetCryptosWithLogoByTrader(Guid? traderGuid, string idUser);

        IEnumerable<CryptoStatementView> GetCryptosByNameOrSymbol(string nameOrSymbol);

        IEnumerable<CryptoStatementView> GetMarketMoverByType(bool gainers);
        IEnumerable<CryptoStatementView> GetTopCryptos();
    }
}
