using Dividendos.Entity.Entities;
using Dividendos.Entity.Model;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Dividendos.Service.Interface
{
    public interface IExtraContentNotificationService : IBaseService
    {
        ResultServiceObject<ExtraContentNotification> GetLastAvailable();

        ResultServiceObject<ExtraContentNotification> Add(ExtraContentNotification extraContentNotification);

        ResultServiceObject<ExtraContentNotification> SubmissionComplete(ExtraContentNotification extraContentNotification);
        ResultServiceObject<ExtraContentNotification> UpdateIterationSequence(ExtraContentNotification extraContentNotification);
    }
}
