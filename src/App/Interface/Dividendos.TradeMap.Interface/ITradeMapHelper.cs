using Dividendos.Entity.Entities;
using Dividendos.TradeMap.Interface.Model;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Dividendos.TradeMap.Interface
{
    public interface ITradeMapHelper
    {
        Task<TradeMapPrices> ImportStockPricesAsync(List<Stock> stocks, string cookie, int idCountry);
        Task<TradeMapUSAStocks> ImportUsStocks(string cookie);
        TradeMapUSAStocks ImportReistsAndEtfs(string cookie, int idCountry, int idStockType, List<Stock> stocksExclude);

        Task<List<StockMarketMover>> ImportMarketMoversAsync(string cookie, string country, string order, string index, string exchange);
        Task<string> GetLogo64(string logo);

        Task<List<StockMarketMover>> ImportMarketMoversDividendYieldAsync(string cookie, string market_type, string index, string exchange);

        Task<List<StockMarketMover>> ImportMarketMoversDividendPaidAsync(string cookie, string market_type, string index, string exchange);
        Task<List<StockMarketMover>> ImportMarketMoversDividendREITsAsync(string cookie, string market_type);
        Task<List<string>> GetHolidays(string cookie);
        CompanyIndicatorTd ImportCompanyIndicators(string cookie, string symbol);
        CompanyHistoricTd ImportCompanyHistoricCorporate(string cookie, string symbol);
        CompanyIndicatorUsTd ImportCompanyIndicatorsUs(string cookie, string symbol);
        CompanyIndicatorFiisTd ImportCompanyIndicatorsFiis(string cookie, string symbol);
    }
}
