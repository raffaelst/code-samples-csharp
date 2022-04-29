using Dividendos.Entity.Entities;
using Dividendos.Entity.Model;
using System;
using System.Collections.Generic;
using System.Text;

namespace Dividendos.Service.Interface
{
    public interface IRelevantFactService : IBaseService
    {
        ResultServiceObject<IEnumerable<RelevantFactView>> GetAll(bool onlyMyUser, string userID);
        ResultServiceObject<RelevantFact> Add(RelevantFact relevantFact);
        ResultServiceObject<IEnumerable<RelevantFact>> GetItensWaitToBeSend();
        ResultServiceObject<RelevantFact> Update(RelevantFact relevantFact);
        ResultServiceObject<RelevantFact> GetByUrl(string url);
    }
}
