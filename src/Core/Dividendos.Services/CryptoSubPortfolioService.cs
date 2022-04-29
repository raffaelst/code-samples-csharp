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
    public class CryptoSubPortfolioService : BaseService, ICryptoSubPortfolioService
    {
        public CryptoSubPortfolioService(IUnitOfWork uow)
        {
            _uow = uow;
        }

        public ResultServiceObject<CryptoSubPortfolio> GetByGuid(Guid guidCryptoSubPortfolio)
        {
            ResultServiceObject<CryptoSubPortfolio> resultService = new ResultServiceObject<CryptoSubPortfolio>();

            IEnumerable<CryptoSubPortfolio> portfolios = _uow.CryptoSubPortfolioRepository.Select(p => p.GuidCryptoSubPortfolio == guidCryptoSubPortfolio);
            
            resultService.Value = portfolios.FirstOrDefault();

            return resultService;
        }

        public ResultServiceObject<IEnumerable<CryptoSubPortfolio>> GetByIdCryptoPortfolio(long idCryptoPortfolio)
        {
            ResultServiceObject<IEnumerable<CryptoSubPortfolio>> resultService = new ResultServiceObject<IEnumerable<CryptoSubPortfolio>>();

            IEnumerable<CryptoSubPortfolio> portfolios = _uow.CryptoSubPortfolioRepository.Select(p => p.IdCryptoPortfolio == idCryptoPortfolio && p.Active == true);

            resultService.Value = portfolios;

            return resultService;
        }

        public void Disable(CryptoSubPortfolio cryptoSubPortfolio)
        {
            cryptoSubPortfolio.Active = false;
            _uow.CryptoSubPortfolioRepository.Update(cryptoSubPortfolio);
        }

        public ResultServiceObject<CryptoSubPortfolio> Add(CryptoSubPortfolio cryptoSubPortfolio)
        {
            cryptoSubPortfolio.Active = true;
            cryptoSubPortfolio.GuidCryptoSubPortfolio = Guid.NewGuid();
            cryptoSubPortfolio.CreatedDate = DateTime.Now;

            cryptoSubPortfolio.IdCryptoPortfolio = _uow.CryptoSubPortfolioRepository.Insert(cryptoSubPortfolio);

            ResultServiceObject<CryptoSubPortfolio> resultService = new ResultServiceObject<CryptoSubPortfolio>();
            resultService.Value = cryptoSubPortfolio;
            return resultService;
        }

        public ResultServiceObject<CryptoSubPortfolio> Update(CryptoSubPortfolio cryptoSubPortfolio)
        {
            ResultServiceObject<CryptoSubPortfolio> resultService = new ResultServiceObject<CryptoSubPortfolio>();
            resultService.Value = _uow.CryptoSubPortfolioRepository.Update(cryptoSubPortfolio);

            return resultService;
        }
    }
}
