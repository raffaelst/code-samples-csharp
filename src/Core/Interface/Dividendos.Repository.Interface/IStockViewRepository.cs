using Dividendos.Entity.Entities;
using Dividendos.Entity.Enum;
using Dividendos.Entity.Views;
using Dividendos.Repository.Interface.GenericRepository;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Dividendos.Repository.Interface
{
    public interface IStockViewRepository : IRepository<StockView>
    {
        public IEnumerable<StockView> GetByNameOrSymbol(string name);
        IEnumerable<StockView> GetLikeSymbol(string symbol, int idCountry);
        IEnumerable<StockView> GetLikeCompanyName(string symbol, int idCountry);
    }
}
