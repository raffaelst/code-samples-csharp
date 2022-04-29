using Dividendos.Entity.Entities;
using Dividendos.Entity.Model;
using Dividendos.Entity.Views;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Dividendos.Service.Interface
{
    public interface ICryptoCurrencyService : IBaseService
    {
        ResultServiceObject<IEnumerable<CryptoCurrency>> GetAll();
        ResultServiceObject<CryptoCurrency> Update(CryptoCurrency cryptoCurrency);
        ResultServiceObject<IEnumerable<CryptoStatementView>> GetCryptosWithLogoByTrader(Guid? traderGuid, string idUser);
        ResultServiceObject<CryptoCurrency> Insert(CryptoCurrency cryptoCurrency);

        ResultServiceObject<IEnumerable<CryptoStatementView>> GetCryptosByNameOrSymbol(string nameOrSymbol);

        ResultServiceObject<CryptoCurrency> GetByGuid(Guid cryptoCurrencyGuid);

        ResultServiceObject<IEnumerable<CryptoStatementView>> GetMarketMoverByType(bool gainers);
        ResultServiceObject<CryptoCurrency> GetById(long cryptoCurrencyId);
        ResultServiceObject<IEnumerable<CryptoStatementView>> GetTopCryptos();
    }
}
