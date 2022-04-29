using Dividendos.API.Model.Request.Operation;
using Dividendos.API.Model.Response;
using Dividendos.API.Model.Response.Common;
using System;
using System.Threading.Tasks;

namespace Dividendos.Application.Interface
{
    public interface IOperationApp
    {
        ResultResponseObject<OperationEditAvgPriceVM> Update(Guid guidIdOperation, OperationEditAvgPriceVM operationEditVM);
        ResultResponseObject<OperationSummaryWrapperVM> GetOperationSummary();
        ResultResponseBase BuyStock(Guid guidPortfolio, OperationAddVM operationAddVM);
        ResultResponseBase SellStock(Guid guidPortfolio, OperationAddVM operationAddVM);
        ResultResponseBase EditBuyOperation(Guid guidPortfolio, OperationEditVM operationEditVM);
        ResultResponseBase EditSellOperation(Guid guidPortfolio, OperationEditVM operationEditVM);
        ResultResponseBase InactiveOperationItem(Guid guidPortfolio, long idOperationItem);
        ResultResponseObject<OperationItemSummaryWrapperVM> GetOperationItemSummary(Guid guidPortfolio, long idStock, int idOperationType);
        ResultResponseObject<OperationSummaryWrapperVM> GetOperationSummaryV3(string portfolioOrSubPortfolio);
        ResultResponseObject<OperationSummaryWrapperVM> GetOperationSummaryV4(string portfolioOrSubPortfolio);
        ResultResponseObject<OperationSellViewWrapperVM> GetOperationSellView(Guid guidPortfolioSub, string startDate, string endDate);
        ResultResponseObject<OperationBuyViewWrapperVM> GetOperationBuyView(Guid guidPortfolioSub, string startDate, string endDate);
        ResultResponseBase EditSellOperationIem(Guid guidOperationItem, OperationItemEditVM operationItemEditVM);
        ResultResponseBase InactiveOperationItem(Guid guidOperationItem);
        ResultResponseBase AddSellOperationIem(Guid guidPortfolio, OperationItemAddVM operationItemAddVM);
        ResultResponseObject<OperationEditAvgPriceVM> Adjust(Guid guidOperation, OperationEditAvgPriceVM operationEditVM);
        ResultResponseObject<OperationSummaryWrapperVM> GetOperationSummaryV5(string portfolioOrSubPortfolio);
    }
}
