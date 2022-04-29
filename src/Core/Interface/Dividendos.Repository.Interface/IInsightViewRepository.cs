using Dividendos.Entity.Entities;
using Dividendos.Entity.Enum;
using Dividendos.Entity.Views;
using Dividendos.Repository.Interface.GenericRepository;
using System;
using System.Collections.Generic;
using System.Text;

namespace Dividendos.Repository.Interface
{
    public interface IInsightViewRepository : IRepository<InsightView>
    {
        IEnumerable<InsightView> GetByTypeAndYear(int year, InsightEnum insightEnum);
    }
}
