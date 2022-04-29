using Dividendos.Entity.Entities;
using Dividendos.Entity.Enum;
using Dividendos.Repository.Interface.GenericRepository;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Dividendos.Repository.Interface
{
    public interface IStockRepository : IRepository<Stock>
    {
        IEnumerable<Stock> GetByUser(string idUser);
        IEnumerable<Stock> GetLikeSymbol(string symbol, int idCountry);

        IEnumerable<string> GetAllUsersWithStock(long idStock);
        public Stock GetByLastDividendUpdateSyncOrderingAsc(int idCountry);
        Stock UpdateLastDividendUpdateSync(Stock stock, DateTime date);
        Stock GetByLastDividendUpdateSyncOrderingAsc(int idCountry, StockTypeEnum stockType);
        IEnumerable<Stock> GetByNameOrSymbol(string name);
        IEnumerable<Stock> GetByCompanyID(long idCompany);
        IEnumerable<Stock> GetAllByCountry(int idCountry);
        IEnumerable<Stock> GetAllShowOnPortfolio(int idCountry);
        IEnumerable<Stock> GetBySymbolOrLikeOldSymbol(string symbol, int idCountry);
        IEnumerable<Stock> GetAllByStockType(int idCountry, StockTypeEnum stockType);
        IEnumerable<Stock> GetAllByCountryOrderByLastDividendUpdateSync(int idCountry);
        public void UpdateLastDailyVariationNotification(long idStock, DateTime date);
    }
}
