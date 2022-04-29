using Dividendos.Entity.Entities;
using Dividendos.Entity.Model;
using Dividendos.Entity.Views;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Dividendos.Service.Interface
{
    public interface ICryptoCurrencyPerformanceService : IBaseService
    {
        ResultServiceObject<CryptoCurrencyPerformance> Insert(CryptoCurrencyPerformance cryptoCurrencyPerformance);
        ResultServiceObject<CryptoCurrencyPerformance> Update(CryptoCurrencyPerformance cryptoCurrencyPerformance);
        ResultServiceObject<IEnumerable<CryptoCurrencyPerformance>> GetByIdPortfolioPerformance(long idCryptoPortfolioPerformance);
    }
}
