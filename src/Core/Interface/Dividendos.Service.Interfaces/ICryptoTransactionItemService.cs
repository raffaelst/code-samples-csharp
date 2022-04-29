using Dividendos.Entity.Entities;
using Dividendos.Entity.Model;
using Dividendos.Entity.Views;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Dividendos.Service.Interface
{
    public interface ICryptoTransactionItemService : IBaseService
    {
        ResultServiceObject<CryptoTransactionItem> Insert(CryptoTransactionItem cryptoTransactionItem, DateTime? lastUpdatedDate = null);
        ResultServiceObject<CryptoTransactionItem> Update(CryptoTransactionItem cryptoTransactionItem);
        ResultServiceObject<IEnumerable<CryptoTransactionItem>> GetByIdCryptoTransaction(long idCryptoTransaction, int transactionType);
        ResultServiceObject<CryptoTransactionItem> GetByGuid(Guid guidCryptoTransactionItem);
        ResultServiceObject<bool> Delete(CryptoTransactionItem cryptoTransactionItem);
    }
}
