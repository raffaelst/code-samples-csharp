using Dividendos.Entity.Entities;
using Dividendos.Entity.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dividendos.Service.Interface
{
    public interface ICryptoSubPortfolioTransactionService : IBaseService
    {
        ResultServiceObject<CryptoSubPortfolioTransaction> Insert(CryptoSubPortfolioTransaction cryptoSubPortfolioTransaction);
        ResultServiceObject<IEnumerable<CryptoSubPortfolioTransaction>> GetByCryptoSubportfolio(long idCryptoSubportfolio);
        ResultServiceBase Delete(CryptoSubPortfolioTransaction cryptoSubPortfolioTransaction);
    }
}
