using Dividendos.Entity.Entities;
using Dividendos.Entity.Model;
using Dividendos.Entity.Views;
using K.Logger;
using System;
using System.Collections.Generic;
using System.Text;

namespace Dividendos.Service.Interface
{
    public interface IFollowStockAlertService : IBaseService
    {
        void ApplyAlertsRules(IEnumerable<FollowStockView> followStockViews, INotificationService notificationService, IDeviceService deviceService, INotificationHistoricalService notificationHistoricalService, ICacheService cacheService, ILogger logger);

        ResultServiceObject<IEnumerable<FollowStockView>> GetAllAlertsActiveAndNotReached();

        ResultServiceObject<FollowStockAlert> Add(FollowStockAlert followStockAlert);
        ResultServiceObject<FollowStockAlert> Update(FollowStockAlert followStockAlert);

        ResultServiceObject<FollowStockAlert> GetByGuid(Guid followStockAlertGuid);

        ResultServiceObject<IEnumerable<FollowStockAlert>> GetAllActiveAlertsByFollowStock(Guid followStockGuid);
        ResultServiceObject<IEnumerable<FollowStockAlertView>> GetAllActiveAlertsViewByFollowStock(Guid followStockGuid);
    }
}
