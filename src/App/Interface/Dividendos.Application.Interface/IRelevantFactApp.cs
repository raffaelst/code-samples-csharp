using Dividendos.API.Model.Request.Settings;
using Dividendos.API.Model.Response;
using Dividendos.API.Model.Response.Common;
using Dividendos.API.Model.Response.v1;
using Dividendos.Entity.Model;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Dividendos.Application.Interface
{
    public interface IRelevantFactApp
    {
        ResultResponseObject<RelevantFactVM> Add(RelevantFactAdd relevantFactAdd);
        ResultResponseObject<IEnumerable<RelevantFactVM>> GetAll(bool onlyMyUser);
        void SendNotificationToInteressedUsers();
        void ImportRelevantFacts();
    }
}
