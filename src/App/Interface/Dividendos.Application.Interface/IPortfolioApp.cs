using Dividendos.API.Model.Request.BrokerIntegration;
using Dividendos.API.Model.Request.Portfolio;
using Dividendos.API.Model.Response;
using Dividendos.API.Model.Response.Common;
using Dividendos.API.Model.Response.v2;
using Dividendos.API.Model.Response.v3;
using Dividendos.Application.Interface.Model;
using Dividendos.Entity.Entities;
using Dividendos.Entity.Model;
using Dividendos.Entity.Views;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Dividendos.Application.Interface
{
    public interface IPortfolioApp
    {
        ResultResponseObject<TraderVM> ImportStocksAndTesouroDiretoCei(string identifier, string password, string idUser);
        void CalculatePerformanceOneByOne();
        ResultResponseObject<StockTypeChart> GetStockTypeChart(Guid guidPortfolioSub);

        ResultResponseObject<List<PortfolioModel>> GetPortfoliosAndSubPortfolios();
        ResultResponseObject<StockAllocationChart> GetPortfolioStockAllocation(Guid guidPortfolioSub);
        ResultResponseObject<StockAllocationChart> GetSectorAllocationChart(Guid guidPortfolioSub);
        ResultResponseObject<StockAllocationChart> GetSubsectorAllocationChart(Guid guidPortfolioSub);
        ResultResponseObject<StockAllocationChart> GetSegmentAllocationChart(Guid guidPortfolioSub);
        ResultResponseObject<PerformanceStockChart> GetPerformanceStockChart(Guid guidPortfolioSub);
        ResultResponseObject<ComparativeChart> GetComparativeChart(Guid guidPortfolioSub, int calculateType, int periodType, int amountSeries);
        ResultResponseObject<ComparativeContainerChart> GetComparativeContainerChart(Guid guidPortfolioSub);
        ResultResponseObject<DividendChartScrollVM> GetDividendScrollChart(Guid guidPortfolioSub, int year);

        void AutoSyncPortfolios();

        ResultResponseBase Disable(Guid idportfolio);

        ResultResponseObject<IEnumerable<OperationBasicVM>> GetPortfolioContentSimple(Guid idportfolio);

        ResultResponseObject<IEnumerable<OperationBasicVM>> GetSubPortfolioContentSimple(Guid idportfolio, Guid idsubportfolio);

        ResultResponseObject<IEnumerable<PortfolioViewModel>> GetPortfolioView();
        ResultResponseObject<IEnumerable<PortfolioStatementViewModel>> GetPortfolioStatementView(Guid guidPortfolio, API.Model.Request.Stock.StockType stockType);
        ResultResponseObject<IEnumerable<PortfolioStatementViewModel>> GetZeroPriceByUser();
        ResultResponseObject<API.Model.Response.v2.PortfolioViewWrapperVM> GetPortfolioViewWrapperV2();

        ResultResponseObject<DividendViewWrapperVM> GetDividendView(Guid guidPortfolioSub);

        ResultResponseObject<OperationSellViewWrapperVM> GetOperationSellView(Guid guidPortfolioSub);

        ResultResponseBase DoANewSyncIfNecessary();

        ResultResponseBase CalculatePerformance();

        ResultResponseBase UpdateName(Guid idportfolio, PortfolioEditVM portfolioEditVM);
        ResultResponseObject<StockStatementViewModel> GetStockStatementView(Guid guidPortfolio, long idStock);
        ResultResponseObject<PortfolioVM> CreateManualPortfolio(string portfolioName, int idCountry);
        ResultResponseObject<API.Model.Response.v2.PortfolioStatementWrapperVM> GetPortfolioStatementViewWrapper(Guid guidPortfolioSub);
        ResultResponseObject<API.Model.Response.v3.PortfolioStatementWrapperVM> GetPortfolioStatementViewWrapperV3(Guid guidPortfolioSub);
        ResultResponseObject<TraderVM> ImportMercadoBitcoin(string identifier, string password);

        ResultResponseObject<TraderVM> CreateAutomaticPortfolio(string identifier, string password);

        ResultResponseObject<SyncQueue> RunSyncFromCEI(string serverName);

        ResultResponseObject<API.Model.Response.v3.PortfolioViewGroupedVM> GetPortfolioViewWrapperV3();

        ResultResponseObject<API.Model.Response.v4.PortfolioViewWrapperVM> GetPortfolioViewWrapperV4();

        void GeneratePerfomanceHistorical(Guid guidPortfolio);

        ResultResponseObject<TraderVM> Validate2FAAndImportFromPassfolio(Passfolio2FARequest passfolioRequest);

        ResultResponseObject<StockTypeChart> GetMainChartByLoggedUser();

        ResultResponseObject<TraderVM> ForceSync(API.Model.Response.v7.TraderType traderType, Guid traderGuid);

        ResultResponseObject<TraderVM> ImportBinance(string identifier, string password);
        ResultResponseObject<TraderVM> ImportBiscoint(string identifier, string password);
        ResultResponseObject<List<PortfolioModel>> GetPortfoliosAndSubPortfoliosV3(bool includeCryptoWallets);
        ResultResponseObject<List<PortfolioModel>> GetPortfoliosAndSubPortfoliosV2();

        ResultResponseObject<TraderVM> ImportBitcoinTrade(string apiKey);


        void GetCryptoDetails(string symbol);

        ResultResponseObject<TraderVM> ImportCoinbase(string identifier, string password);
        ResultResponseObject<TraderVM> ImportBitcoinToYou(string identifier, string password);
        void RunDelayedCeiImport();
        void RunScrapyAgent(string agentName, int amountItems);
        Task<ResultResponseObject<TraderVM>> ImportFromAvenue(string email, string password, string token, string idUser, string challenge, string sessiondId);
        Task<ResultResponseObject<TraderVM>> ImportFromAvenue(Avenue2FARequest avenue2FARequest);

        ResultResponseObject<StockAllocationChart> GetSegmentAllocationChartByType(Guid guidPortfolioSub, API.Model.Request.Stock.StockType stockType);
        void ImportFromPasfolio(string email, string auth, string idUser, bool automaticProcess);

        void ImportAllLogos();
        ResultResponseObject<List<PortfolioModel>> GetPortfoliosAndSubPortfoliosV4(bool includeCryptoWallets);
        Task<ResultResponseObject<TraderVM>> ImportFromAvenueInternal(Avenue2FARequest avenue2FARequest);
        ResultResponseObject<TraderVM> ActivateEnqueue(string documentNumber);
        public ResultResponseObject<IEnumerable<Dividendos.API.Model.Response.v4.PortfolioStatementViewModel>> GetPortfolioStatementViewV4(Guid guidPortfolioSub, API.Model.Request.Stock.StockType stockType);
        ResultResponseObject<TraderVM> ImportBitPreco(string identifier, string password);
    }
}
