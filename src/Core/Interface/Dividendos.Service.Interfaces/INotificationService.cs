using Dividendos.Entity.Entities;
using Dividendos.Entity.Model;
using Dividendos.Entity.Views;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Dividendos.Service.Interface
{
    public interface INotificationService : IBaseService
    {
        void SendPush(string title, string message, Device device, PushRedirect pushRedirect);
        void SendMail(string to, string subject, string body);
        void SendPushToTopic(string title, string message, string topicName);

        void SendPushParallel(List<PushNotificationView> pushNotificationViews, int maxDegreeOfParallelism, PushRedirect pushRedirect);
    }
}
