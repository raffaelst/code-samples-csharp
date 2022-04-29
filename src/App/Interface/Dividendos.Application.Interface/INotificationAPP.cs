using Dividendos.API.Model.Request.Notification;
using Dividendos.API.Model.Response.Common;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Dividendos.Application.Interface
{
    public interface INotificationAPP
    {
        Task<ResultResponseBase> SendTestPushNotification();
        void SendNotificationList(PushContentListVM pushContentListVM);
    }
}
