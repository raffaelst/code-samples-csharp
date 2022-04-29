using Dividendos.Entity.Entities;
using Dividendos.Entity.Model;
using Dividendos.Repository.Interface.UoW;
using Dividendos.Service.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dividendos.Service
{
    public class CryptoCurrencyPerformanceService : BaseService, ICryptoCurrencyPerformanceService
    {
        public CryptoCurrencyPerformanceService(IUnitOfWork uow)
        {
            _uow = uow;
        }

        public ResultServiceObject<CryptoCurrencyPerformance> Insert(CryptoCurrencyPerformance cryptoCurrencyPerformance)
        {
            ResultServiceObject<CryptoCurrencyPerformance> resultService = new ResultServiceObject<CryptoCurrencyPerformance>();
            cryptoCurrencyPerformance.GuidCryptoCurrencyPerformance = Guid.NewGuid();
            cryptoCurrencyPerformance.LastUpdatedDate = DateTime.Now;
            cryptoCurrencyPerformance.IdCryptoCurrencyPerformance = _uow.CryptoCurrencyPerformanceRepository.Insert(cryptoCurrencyPerformance);
            resultService.Value = cryptoCurrencyPerformance;

            return resultService;
        }

        public ResultServiceObject<CryptoCurrencyPerformance> Update(CryptoCurrencyPerformance cryptoCurrencyPerformance)
        {
            ResultServiceObject<CryptoCurrencyPerformance> resultService = new ResultServiceObject<CryptoCurrencyPerformance>();
            cryptoCurrencyPerformance.LastUpdatedDate = DateTime.Now;
            resultService.Value = _uow.CryptoCurrencyPerformanceRepository.Update(cryptoCurrencyPerformance);

            return resultService;
        }

        public ResultServiceObject<IEnumerable<CryptoCurrencyPerformance>> GetByIdPortfolioPerformance(long idCryptoPortfolioPerformance)
        {
            ResultServiceObject<IEnumerable<CryptoCurrencyPerformance>> resultService = new ResultServiceObject<IEnumerable<CryptoCurrencyPerformance>>();

            IEnumerable<CryptoCurrencyPerformance> cryptoCurrencyPerformances = _uow.CryptoCurrencyPerformanceRepository.Select(p => p.IdCryptoPortfolioPerformance == idCryptoPortfolioPerformance);

            resultService.Value = cryptoCurrencyPerformances;

            return resultService;
        }
    }
}
