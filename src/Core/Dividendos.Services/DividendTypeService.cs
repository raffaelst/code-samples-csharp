using Dividendos.Entity.Entities;
using Dividendos.Entity.Model;
using Dividendos.Repository.Interface.UoW;
using Dividendos.Service.Interface;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Dividendos.Service
{
    public class DividendTypeService : BaseService, IDividendTypeService
    {
          public DividendTypeService(IUnitOfWork uow)
        {
            _uow = uow;
        }

        public ResultServiceObject<IEnumerable<DividendType>> GetAll()
        {
            ResultServiceObject<IEnumerable<DividendType>> resultService = new ResultServiceObject<IEnumerable<DividendType>>();

            IEnumerable<DividendType> stocks = _uow.DividendTypeRepository.GetAll();

            resultService.Value = stocks;

            return resultService;
        }
    }
}
