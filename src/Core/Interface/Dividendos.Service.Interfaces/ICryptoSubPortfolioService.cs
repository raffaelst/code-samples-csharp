using Dividendos.Entity.Entities;
using Dividendos.Entity.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dividendos.Service.Interface
{
    public interface ICryptoSubPortfolioService : IBaseService
    {
        ResultServiceObject<CryptoSubPortfolio> GetByGuid(Guid guidCryptoSubPortfolio);
        ResultServiceObject<IEnumerable<CryptoSubPortfolio>> GetByIdCryptoPortfolio(long idCryptoPortfolio);
        void Disable(CryptoSubPortfolio cryptoSubPortfolio);
        ResultServiceObject<CryptoSubPortfolio> Add(CryptoSubPortfolio cryptoSubPortfolio);
        ResultServiceObject<CryptoSubPortfolio> Update(CryptoSubPortfolio cryptoSubPortfolio);
    }
}
