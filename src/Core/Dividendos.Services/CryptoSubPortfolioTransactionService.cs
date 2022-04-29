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
    public class CryptoSubPortfolioTransactionService : BaseService, ICryptoSubPortfolioTransactionService
    {
        public CryptoSubPortfolioTransactionService(IUnitOfWork uow)
        {
            _uow = uow;
        }

        public ResultServiceObject<CryptoSubPortfolioTransaction> Insert(CryptoSubPortfolioTransaction cryptoSubPortfolioTransaction)
        {
            ResultServiceObject<CryptoSubPortfolioTransaction> resultService = new ResultServiceObject<CryptoSubPortfolioTransaction>();
            cryptoSubPortfolioTransaction.IdCryptoSubPortfolioTransaction = _uow.CryptoSubPortfolioTransactionRepository.Insert(cryptoSubPortfolioTransaction);
            resultService.Value = cryptoSubPortfolioTransaction;

            return resultService;
        }

        public ResultServiceObject<IEnumerable<CryptoSubPortfolioTransaction>> GetByCryptoSubportfolio(long idCryptoSubportfolio)
        {
            ResultServiceObject<IEnumerable<CryptoSubPortfolioTransaction>> resultService = new ResultServiceObject<IEnumerable<CryptoSubPortfolioTransaction>>();

            IEnumerable<CryptoSubPortfolioTransaction> cryptoSubPortfolioTransaction = _uow.CryptoSubPortfolioTransactionRepository.Select(p => p.IdCryptoSubPortfolio == idCryptoSubportfolio);

            resultService.Value = cryptoSubPortfolioTransaction;

            return resultService;
        }

        public ResultServiceBase Delete(CryptoSubPortfolioTransaction cryptoSubPortfolioTransaction)
        {
            ResultServiceBase resultService = new ResultServiceBase();

            _uow.CryptoSubPortfolioTransactionRepository.Delete(cryptoSubPortfolioTransaction);


            return resultService;
        }
    }
}
