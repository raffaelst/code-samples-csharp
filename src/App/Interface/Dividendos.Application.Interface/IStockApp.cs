using Dividendos.API.Model.Response;
using Dividendos.API.Model.Response.Common;
using Dividendos.Application.Interface.Model;
using System.Collections.Generic;
using System.Threading.Tasks;
using Dividendos.API.Model.Request.Stock;
using Dividendos.API.Model.Response.MilkingCows;
using Dividendos.Entity.Model;

namespace Dividendos.Application.Interface
{
    public interface IStockApp
    {
        Task SyncStockPriceUsingGoogleFinanceAsync(int idCountry);
        ResultResponseObject<IEnumerable<StockVM>> GetAllByLoggedUser();
        ResultResponseObject<IEnumerable<StockSymbolVM>> GetLikeSymbol(string symbol, int? idCountry);
        ResultResponseObject<StockStatementViewModel> GetPortfolioStatementView(long idStock);
        void ImportUsStocks(int stockType);
        Task SyncStockPriceUsingTradeMapAsync(int idCountry);
        void ImportMarketMover(MakertMoversType makertMoversType);
        ResultResponseObject<IEnumerable<MarketMoverVM>> GetMarketMoverByType(MakertMoversType makertMoversType);
        ResultResponseObject<IEnumerable<Dividendos.API.Model.Response.v3.StockSymbolVM>> GetLikeSymbolV3(string symbol, int? idCountry);
        void ImportStatusInvestCompanies(int type);
        ResultResponseObject<IEnumerable<MilkingCowsVM>> GetMilkingCows(CountryType countryType);
        void SendAlertAwesomeDailyVariations();
        ResultServiceObject<Entity.Entities.Stock> GetBySymbolOrLikeOldSymbol(string symbol, int idCountry);
        public void SendAlertAwesomeDailyVariationsCheckingAllDayLong();
    }
}