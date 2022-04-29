using Dividendos.Entity.Entities;
using Dividendos.Entity.Model;
using Dividendos.Entity.Views;
using System;
using System.Collections.Generic;
using System.Text;

namespace Dividendos.Service.Interface
{
    public interface IGoalService : IBaseService
    {
        ResultServiceObject<IEnumerable<GoalView>> GetAllByUser(string userID);
        ResultServiceObject<Goal> GetByGuid(Guid goalGuid);
        ResultServiceObject<Goal> Add(Goal goal);
        ResultServiceObject<Goal> Delete(Goal goal);
    }
}
