using Dividendos.Entity.Entities;
using Dividendos.Entity.Views;
using Dividendos.Repository.Interface.GenericRepository;
using System;
using System.Collections.Generic;
using System.Text;

namespace Dividendos.Repository.Interface
{
    public interface IGoalViewRepository : IRepository<GoalView>
    {
        IEnumerable<GoalView> GetAllActiveByUserID(string userID);
    }
}
