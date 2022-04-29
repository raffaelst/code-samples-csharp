using Dividendos.Entity.Entities;
using Dividendos.Entity.Model;
using Dividendos.Entity.Views;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dividendos.Service.Interface
{
    public interface IResearchProductService : IBaseService
    {
        ResultServiceObject<IEnumerable<ResearchProduct>> GetAllActive();
    }
}
