using Dividendos.Entity.Entities;
using Dividendos.Repository.Interface.GenericRepository;
using System;
using System.Collections.Generic;
using System.Text;

namespace Dividendos.Repository.Interface
{
    public interface ICryptoTransactionRepository : IRepository<CryptoTransaction>
    {
        int CountCryptoCurrencyByUser(string idUser);
    }
}
