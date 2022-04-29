using Dividendos.Entity.Entities;
using Dividendos.Entity.Enum;
using Dividendos.Entity.Model;
using Dividendos.Entity.Views;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Dividendos.Service.Interface
{
    public interface IStockService : IBaseService
    {
        ResultServiceObject<IEnumerable<Stock>> GetAllByCountry(int idCountry);
        ResultServiceObject<Stock> GetBySymbolOrLikeOldSymbol(string symbol, int idCountry);
        ResultServiceObject<Stock> Update(Stock stock);

        ResultServiceObject<IEnumerable<Stock>> GetByUser(string idUser);
        ResultServiceObject<Stock> GetById(long idStock);
        ResultServiceObject<IEnumerable<StockView>> GetLikeSymbol(string symbol, int idCountry);
        ResultServiceObject<StockStatementView> GetByIdStock(long idStock);
        ResultServiceObject<IEnumerable<Stock>> GetAllShowOnPortfolio(int idCountry);
        ResultServiceObject<Stock> Insert(Stock stock);

        ResultServiceObject<Stock> GetByGuid(Guid stockGui);

        ResultServiceObject<IEnumerable<string>> GetAllUsersWithStock(long idStock);

        ResultServiceObject<IEnumerable<Stock>> GetByCompanyID(long companyID);
        ResultServiceObject<Stock> GetByLastDividendUpdateSyncOrderingAsc(int idCountry);
        ResultServiceObject<Stock> UpdateLastDividendUpdateSync(Stock stock);
        ResultServiceObject<Stock> GetByLastDividendUpdateSyncOrderingAscAndStockType(int idCountry, StockTypeEnum stockType);
        ResultServiceObject<IEnumerable<StockView>> GetByNameOrSymbol(string name);
        ResultServiceObject<IEnumerable<Stock>> GetAll();
        ResultServiceObject<IEnumerable<StockView>> GetLikeCompanyName(string symbol, int idCountry);
        ResultServiceObject<IEnumerable<Stock>> GetAllByStockType(int idCountry, StockTypeEnum stockType);

        ResultServiceObject<IEnumerable<Stock>> GetAllByCountryOrderByLastDividendUpdateSync(int idCountry);

        ResultServiceObject<Stock> UpdateLastDailyVariationNotification(long idStock);
    }
}
