using Dividendos.RDStation.Interface.Model;
using Dividendos.RDStation.Interface.Model.Request;
using Dividendos.RDStation.Interface.Model.Response;
using K.Logger;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace Dividendos.RDStation.Interface
{
    public interface IRDStationHelper
    {
        void SendEvent(string nome, string email, string userId, List<string> tags, string phoneNumber, EventType eventType);
        void SendEvent(string name, string email, string userId, List<string> tags, string customEventType);
    }
}
