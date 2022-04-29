using Dividendos.API.Model.Request.Operation;
using Dividendos.API.Model.Request.Settings;
using Dividendos.API.Model.Response;
using Dividendos.API.Model.Response.Common;
using Dividendos.API.Model.Response.StockSplit;
using Dividendos.API.Model.Response.v1;
using Dividendos.Entity.Model;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Dividendos.Application.Interface
{
    public interface IStockSplitApp
    {
        ResultResponseObject<IEnumerable<StockSplitVM>> Get(bool onlyMyStocks, DateTime startDate, DateTime endDate);
        void CheckAndSendNotificationAboutSplit();
        void ImportStockSplit(int idCountry);
        ResultResponseObject<StockSplitWrapperVM> GetStockSplits(string idUser);
        ResultResponseObject<OperationEditAvgPriceVM> ApplyStockSplit(Guid guidOperation, OperationEditAvgPriceVM operationEditVM);
        ResultResponseObject<StockSplitWrapperVM> GetStockSplits();
        ResultResponseObject<IEnumerable<StockSplitVM>> GetByGuidAndDate(Guid stockGuid, DateTime startDate, DateTime endDate);
        ResultResponseObject<Guid> DiscardStockSplit(Guid guidOperation);
    }
}
