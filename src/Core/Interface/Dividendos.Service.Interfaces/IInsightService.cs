using Dividendos.Entity.Entities;
using Dividendos.Entity.Enum;
using Dividendos.Entity.Model;
using Dividendos.Entity.Views;
using System;
using System.Collections.Generic;
using System.Text;

namespace Dividendos.Service.Interface
{
    public interface IInsightService : IBaseService
    {
        ResultServiceObject<IEnumerable<InsightView>> GetByTypeAndYear(int year, InsightEnum insightEnum);
    }
}
