using Dividendos.API.Model.Request.Notification;
using Dividendos.API.Model.Response.Common;
using Dividendos.API.Model.Response.Notification;
using System.Threading.Tasks;

namespace Dividendos.Application.Interface
{
    public interface IExtraContentNotificationApp
    {
        ResultResponseObject<PushContentResponseVM> SendExtraContentNotification(PushContentVM pushContentVM);

        void CheckAndSendExtraContentPushNotification(int amountItensPerAgent);
    }
}
