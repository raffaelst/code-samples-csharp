using Dividendos.API.Model.Request.Operation;
using Dividendos.API.Model.Response.Common;
using Dividendos.Entity.Entities;
using Dividendos.Entity.Model;
using Dividendos.Entity.Views;
using K.Logger;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;


namespace Dividendos.Service.Interface
{
    public interface IOperationService : IBaseService
    {
        ResultServiceObject<bool> DeleteAllByPortfolio(long idPortfolio);
        ResultServiceObject<IEnumerable<Operation>> GetByPortfolio(long idPortfolio);

        ResultServiceObject<Operation> Update(Operation operation);

        ResultServiceObject<Operation> Insert(Operation operation);

        ResultServiceObject<bool> Delete(Operation operation);

        ResultServiceObject<IEnumerable<Operation>> GetByPortfolioOperationType(long idPortfolio, int idOperationType);

        ResultServiceObject<IEnumerable<OperationView>> GetByIdPortfolio(long idPortfolio);

        ResultServiceObject<IEnumerable<OperationView>> GetByIdSubPortfolio(long idSubPortfolio);

        ResultServiceObject<Operation> GetByGuid(Guid guidOperation);

        ResultServiceObject<IEnumerable<OperationSellDetailsView>> GetSellDetailsByIdPortfolio(long idPortfolio, DateTime? startDate = null, DateTime? endDate = null);

        ResultServiceObject<IEnumerable<OperationSellDetailsView>> GetSellDetailsByIdSubportfolio(long idPortfolio, long idSubPortfolio, DateTime? startDate = null, DateTime? endDate = null);

        ResultServiceObject<IEnumerable<OperationSummaryView>> GetOperationSummary(string idUser);

        ResultServiceObject<Operation> GetByPortfolioAndIdStock(long idPortfolio, long idStock, int idOperationType);

        ResultServiceObject<IEnumerable<OperationSummaryView>> GetOperationSummaryByPortfolioOrSubPortfolio(string idUser, string portfolioOrSubPortfolio);

        void MoveOperation(long idPortfolio, long idStock, IOperationHistService _operationHistService, IOperationItemService _operationItemService, IOperationItemHistService _operationItemHistService);

        ResultServiceObject<bool> RecoveryAveragePrice(long idPortfolio, IOperationHistService _operationHistService, IOperationItemHistService _operationItemHistService);

        ResultServiceObject<IEnumerable<OperationBuyDetailsView>> GetBuyDetailsByIdPortfolio(long idPortfolio, DateTime? startDate, DateTime? endDate);

        ResultServiceObject<IEnumerable<OperationBuyDetailsView>> GetBuyDetailsByIdSubportfolio(long idPortfolio, long idSubPortfolio, DateTime? startDate, DateTime? endDate);

        ResultServiceObject<Operation> GetById(long idOperation);

        bool CalculateAveragePrice(ref List<OperationItem> operationItems, out decimal numberOfShares, out decimal avgPrice, bool removeNewItems = false, DateTime? lastSync = null, bool breakInvalid = true);

        void Adjust(Guid guidOperation, OperationEditAvgPriceVM operationEditVM, IPortfolioService _portfolioService, IOperationService _operationService,
            IOperationItemService _operationItemService, IOperationHistService _operationHistService, IOperationItemHistService _operationItemHistService, ISystemSettingsService _systemSettingsService,
            IPortfolioPerformanceService _portfolioPerformanceService, IStockService _stockService, IPerformanceStockService _performanceStockService, IHolidayService _holidayService, DateTime? dateMax = null,
            bool calculatePeformance = true, bool removeNewItems = false, DateTime? lastSync = null, bool breakInvalid = true, bool editedByUser = false, bool onlyDate = true, DateTime? splitDate = null);

        void BuyStock(Guid guidPortfolio, OperationAddVM operationAddVM, ResultResponseBase resultResponseBase, IPortfolioService _portfolioService, IOperationService _operationService,
            IOperationItemService _operationItemService, IOperationHistService _operationHistService, IOperationItemHistService _operationItemHistService, ISystemSettingsService _systemSettingsService,
            IPortfolioPerformanceService _portfolioPerformanceService, IStockService _stockService, IPerformanceStockService _performanceStockService, IHolidayService _holidayService, bool priceAdjust = false, bool calculatePeformance = true,
            bool removeNewItems = false, DateTime? lastSync = null, bool breakInvalid = true, bool editedByUser = false, DateTime? splitDate = null);

        void SellStock(Guid guidPortfolio, OperationAddVM operationAddVM, ResultResponseBase resultResponseBase, IPortfolioService _portfolioService, IOperationService _operationService,
            IOperationItemService _operationItemService, IOperationHistService _operationHistService, IOperationItemHistService _operationItemHistService, ISystemSettingsService _systemSettingsService,
            IPortfolioPerformanceService _portfolioPerformanceService, IStockService _stockService, IPerformanceStockService _performanceStockService, IHolidayService _holidayService, bool priceAdjust = false,
            bool calculatePeformance = true, bool removeNewItems = false, DateTime? lastSync = null, bool breakInvalid = true, bool editedByUser = false, DateTime? splitDate = null);

        ResultServiceObject<Operation> GetByIdOperationItem(long idOperationItem);

        bool IsAdjusted(long idOperation);
        bool CalculateAveragePriceMatch(ref List<OperationItem> operationItems, decimal currentNumberOfShares, out decimal numberOfSharesMatch, out decimal avgPriceMatch, out DateTime? lastUpdateDate, out DateTime? eventDateMatch);
        bool HasRecentSubscription(long idCompany, long idPortfolio);
        DateTime? GetLatestEventDate(long idPortfolio, long idStock);
        decimal GetTotalAmount(long idPortfolio, DateTime limitDate, long idStock);
        ResultServiceObject<IEnumerable<Operation>> GetOperationSplits(string idUser, DateTime limitDate);
        ResultResponseObject<OperationEditAvgPriceVM> UpdateOperation(Guid guidOperation, OperationEditAvgPriceVM operationEditVM, IPortfolioService _portfolioService, IOperationService _operationService, IOperationItemService _operationItemService,
            IOperationItemHistService _operationItemHistService, IOperationHistService _operationHistService, ISystemSettingsService _systemSettingsService, IPortfolioPerformanceService _portfolioPerformanceService, IStockService _stockService,
            IPerformanceStockService _performanceStockService, IHolidayService _holidayService, ILogger _logger, DateTime? splitDate = null);

        void EditSellOperation(Guid guidPortfolio, OperationEditVM operationEditVM, ResultResponseBase resultResponseBase, IPortfolioService _portfolioService, IOperationItemService _operationItemService, ISystemSettingsService _systemSettingsService, IPortfolioPerformanceService _portfolioPerformanceService, IStockService _stockService, IPerformanceStockService _performanceStockService, IHolidayService _holidayService, IOperationService _operationService);

        ResultResponseBase EditBuyOperation(Guid guidPortfolio, OperationEditVM operationEditVM, IPortfolioService _portfolioService, IOperationItemService _operationItemService, ISystemSettingsService _systemSettingsService, IPortfolioPerformanceService _portfolioPerformanceService, IStockService _stockService, IPerformanceStockService _performanceStockService, IHolidayService _holidayService, IOperationService _operationService);

        void Update(long idOperation, bool active);
    }
}
