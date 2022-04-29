using Dividendos.Entity.Views;
using Dividendos.Repository.Interface.GenericRepository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dividendos.Repository.Interface
{
    public interface ICryptoTransactionViewRepository : IRepository<CryptoTransactionDetailsView>
    {
        IEnumerable<CryptoTransactionDetailsView> GetDetailsByIdCryptoPortfolio(long idCryptoPortfolio, int transactionType, DateTime? startDate, DateTime? endDate);
        IEnumerable<CryptoTransactionDetailsView> GetDetailsByIdCryptoSubportfolio(long idCryptoPortfolio, long idCryptoSubPortfolio, int transactionType, DateTime? startDate, DateTime? endDate);
    }
}
