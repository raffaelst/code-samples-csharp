using Dividendos.Entity.Entities;
using Dividendos.Entity.Model;
using Dividendos.Repository.Interface;
using Dividendos.Repository.Interface.UoW;
using Dividendos.Service.Interface;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Dividendos.Service
{
    public class StockTypeService : BaseService, IStockTypeService
    {
        public StockTypeService(IUnitOfWork uow)
        {
            _uow = uow;
        }

        public ResultServiceObject<IEnumerable<StockType>> GetAll()
        {
            ResultServiceObject<IEnumerable<StockType>> resultService = new ResultServiceObject<IEnumerable<StockType>>();

            resultService.Value = _uow.StockTypeRepository .GetAll();

            return resultService;
        }
    }
}
