using Dividendos.Entity.Entities;
using Dividendos.Entity.Model;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Dividendos.Service.Interface
{
    public interface INotificationHistoricalService : IBaseService
    {
        ResultServiceObject<IEnumerable<NotificationHistorical>> GetAllByUser(string userId);

        ResultServiceObject<NotificationHistorical> GetByGuid(Guid notificationHistoricalGuid);

        ResultServiceObject<NotificationHistorical> New(string notificationTitle, string notificationText, string userId, string appScreenName, string pushRedirectType, string externalRedirectURL, ICacheService cacheService);

        void Disable(long notificationHistoricalId);
    }
}
