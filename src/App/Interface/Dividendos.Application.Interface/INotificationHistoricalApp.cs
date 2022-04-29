using Dividendos.API.Model.Request.Settings;
using Dividendos.API.Model.Response;
using Dividendos.API.Model.Response.Common;
using Dividendos.API.Model.Response.NotificationHistorical;
using Dividendos.Entity.Entities;
using Dividendos.Entity.Model;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Dividendos.Application.Interface
{
    public interface INotificationHistoricalApp
    {
        ResultResponseObject<NotificationHistoricalVM> InactivateByGuid(string notificationHistoricalGuid);
        ResultResponseObject<IEnumerable<NotificationHistoricalVM>> GetAllByUser();

        ResultResponseBase InactivateAllByUser();
    }
}
