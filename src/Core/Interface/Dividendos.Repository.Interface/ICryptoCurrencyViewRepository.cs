using Dividendos.Entity.Entities;
using Dividendos.Entity.Views;
using Dividendos.Repository.Interface.GenericRepository;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Dividendos.Repository.Interface
{
    public interface ICryptoCurrencyViewRepository : IRepository<CryptoBrokerView>
    {
       IEnumerable<CryptoBrokerView> GetCryptosBroker(string idUser);
    }
}
