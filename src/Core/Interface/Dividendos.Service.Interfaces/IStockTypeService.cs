using Dividendos.Entity.Entities;
using Dividendos.Entity.Model;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Dividendos.Service.Interface
{
    public interface IStockTypeService : IBaseService
    {
        ResultServiceObject<IEnumerable<StockType>> GetAll();
    }
}
