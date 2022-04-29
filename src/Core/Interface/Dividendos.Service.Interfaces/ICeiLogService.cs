using Dividendos.Entity.Entities;
using Dividendos.Entity.Model;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Dividendos.Service.Interface
{
    public interface ICeiLogService : IBaseService
    {
        ResultServiceObject<CeiLog> Add(CeiLog ceiLog);

        ResultServiceObject<CeiLog> Update(CeiLog ceiLog);

        void DeleteAll();
    }
}
