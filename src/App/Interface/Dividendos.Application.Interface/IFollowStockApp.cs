using Dividendos.API.Model.Response;
using Dividendos.API.Model.Response.Common;
using Dividendos.Application.Interface.Model;
using System.Collections.Generic;
using System.Threading.Tasks;
using Dividendos.API.Model.Request.Stock;
using Dividendos.API.Model.Request.FollowStock;
using System;
using Dividendos.Entity.Views;

namespace Dividendos.Application.Interface
{
    public interface IFollowStockApp
    {
        void CheckFollowStockAlerts();
        ResultResponseObject<FollowStockVM> Add(FollowStockVM followStockVM);

        ResultResponseBase Delete(Guid followGuid);

        ResultResponseObject<IEnumerable<FollowStockResponseVM>> GetAllActiveByUser();

        ResultResponseBase DeleteAlert(Guid followAlertGuid);

        ResultResponseObject<IEnumerable<FollowStockAlertResponseVM>> GetAllActiveAlertsByFollowStock(Guid followStockGuid);

        ResultResponseObject<FollowStockAlertResponseVM> AddAlert(FollowStockAlertVM followStockVM);

        ResultResponseObject<IEnumerable<FollowStockResponseVM>> GetByIdStockOrIdCrypto(string idstockorcrypto);
    }
}