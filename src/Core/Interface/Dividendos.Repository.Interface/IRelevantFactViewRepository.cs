using Dividendos.Entity.Entities;
using Dividendos.Repository.Interface.GenericRepository;
using System;
using System.Collections.Generic;
using System.Text;

namespace Dividendos.Repository.Interface
{
    public interface IRelevantFactViewRepository : IRepository<RelevantFactView>
    {
        IEnumerable<RelevantFactView> GetAllByLoggedUser(string userID);

        IEnumerable<RelevantFactView> GetAll();
    }
}
