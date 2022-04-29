using Dividendos.Entity.Entities;
using Dividendos.Entity.Model;
using Dividendos.Entity.Views;
using Dividendos.Repository.Interface.UoW;
using Dividendos.Service.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dividendos.Service
{
    public class CryptoPortfolioService : BaseService, ICryptoPortfolioService
    {
        public CryptoPortfolioService(IUnitOfWork uow)
        {
            _uow = uow;
        }

        public ResultServiceObject<CryptoPortfolio> Insert(CryptoPortfolio cryptoPortfolio)
        {
            ResultServiceObject<CryptoPortfolio> resultService = new ResultServiceObject<CryptoPortfolio>();
            cryptoPortfolio.Active = true;
            cryptoPortfolio.CalculatePerformanceDate = DateTime.Now;
            cryptoPortfolio.CreatedDate = DateTime.Now;
            cryptoPortfolio.GuidCryptoPortfolio = Guid.NewGuid();

            cryptoPortfolio.IdCryptoPortfolio = _uow.CryptoPortfolioRepository.Insert(cryptoPortfolio);


            return resultService;
        }

        public void UpdateCalculatePerformanceDate(long idCryptoPortfolio, DateTime dateTime)
        {
            _uow.CryptoPortfolioRepository.UpdateCalculatePerformanceDate(idCryptoPortfolio, dateTime);
        }

        public ResultServiceObject<CryptoPortfolio> GetById(long idCryptoPortfolio)
        {
            ResultServiceObject<CryptoPortfolio> resultService = new ResultServiceObject<CryptoPortfolio>();

            IEnumerable<CryptoPortfolio> portfolios = _uow.CryptoPortfolioRepository.Select(p => p.IdCryptoPortfolio == idCryptoPortfolio);

            resultService.Value = portfolios.FirstOrDefault();

            return resultService;
        }

        public ResultServiceObject<CryptoPortfolio> GetByGuid(Guid guidCryptoPortfolio)
        {
            ResultServiceObject<CryptoPortfolio> resultService = new ResultServiceObject<CryptoPortfolio>();

            IEnumerable<CryptoPortfolio> portfolios = _uow.CryptoPortfolioRepository.Select(p => p.GuidCryptoPortfolio == guidCryptoPortfolio);

            resultService.Value = portfolios.FirstOrDefault();

            return resultService;
        }

        public ResultServiceObject<IEnumerable<CryptoPortfolioView>> GetByUser(string idUser)
        {
            ResultServiceObject<IEnumerable<CryptoPortfolioView>> resultService = new ResultServiceObject<IEnumerable<CryptoPortfolioView>>();

            IEnumerable<CryptoPortfolioView> portfolios = _uow.CryptoPortfolioViewRepository.GetByUser(idUser);

            resultService.Value = portfolios;

            return resultService;
        }

        public ResultServiceObject<IEnumerable<CryptoPortfolio>> GetByUser(string idUser, bool status, bool? manual = null)
        {
            ResultServiceObject<IEnumerable<CryptoPortfolio>> resultService = new ResultServiceObject<IEnumerable<CryptoPortfolio>>();

            IEnumerable<CryptoPortfolio> portfolios = _uow.CryptoPortfolioRepository.GetByUserAndStatus(idUser, status, manual);

            resultService.Value = portfolios;

            return resultService;
        }

        public ResultServiceObject<IEnumerable<CryptoCurrencyStatementView>> GetByCryptoPortfolio(Guid guidPortfolio)
        {
            ResultServiceObject<IEnumerable<CryptoCurrencyStatementView>> resultService = new ResultServiceObject<IEnumerable<CryptoCurrencyStatementView>>();

            IEnumerable<CryptoCurrencyStatementView> portfolios = _uow.CryptoCurrencyStatementViewRepository.GetByCryptoPortfolio(guidPortfolio);

            resultService.Value = portfolios;

            return resultService;
        }

        public ResultServiceObject<IEnumerable<CryptoCurrencyStatementView>> GetByCryptoSubportfolio(Guid guidCryptoSubPortfolio)
        {
            ResultServiceObject<IEnumerable<CryptoCurrencyStatementView>> resultService = new ResultServiceObject<IEnumerable<CryptoCurrencyStatementView>>();

            IEnumerable<CryptoCurrencyStatementView> portfolios = _uow.CryptoCurrencyStatementViewRepository.GetByCryptoSubportfolio(guidCryptoSubPortfolio);

            resultService.Value = portfolios;

            return resultService;
        }

        public ResultServiceObject<CryptoCurrencyStatementView> GetByGuidCurrency(Guid guidCryptoPortfolio, Guid guidCryptoCurrency)
        {
            ResultServiceObject<CryptoCurrencyStatementView> resultService = new ResultServiceObject<CryptoCurrencyStatementView>();

            CryptoCurrencyStatementView cryptoCurrencyStatementView = _uow.CryptoCurrencyStatementViewRepository.GetByGuidCurrency(guidCryptoPortfolio, guidCryptoCurrency);

            resultService.Value = cryptoCurrencyStatementView;

            return resultService;
        }

        public ResultServiceObject<IEnumerable<CryptoCurrencyStatementView>> GetCryptoSummaryByPortfolioOrSubPortfolio(string idUser, string guidCryptoPortfolioSub)
        {
            ResultServiceObject<IEnumerable<CryptoCurrencyStatementView>> resultService = new ResultServiceObject<IEnumerable<CryptoCurrencyStatementView>>();

            IEnumerable<CryptoCurrencyStatementView> portfolios = _uow.CryptoCurrencyStatementViewRepository.GetCryptoSummaryByPortfolioOrSubPortfolio(idUser, guidCryptoPortfolioSub);

            resultService.Value = portfolios;

            return resultService;
        }

        public ResultServiceObject<CryptoPortfolio> GetByIdTrader(long idTrader)
        {
            ResultServiceObject<CryptoPortfolio> resultService = new ResultServiceObject<CryptoPortfolio>();

            IEnumerable<CryptoPortfolio> portfolios = _uow.CryptoPortfolioRepository.Select(p => p.IdTrader == idTrader);

            resultService.Value = portfolios.FirstOrDefault();

            return resultService;
        }

        public void Disable(CryptoPortfolio cryptoPortfolio)
        {
            cryptoPortfolio.Active = false;
            _uow.CryptoPortfolioRepository.Update(cryptoPortfolio);
        }

        public void UpdateName(long idCryptoPortfolio, string name)
        {
            _uow.CryptoPortfolioRepository.UpdateName(idCryptoPortfolio, name);
        }

        public ResultServiceObject<IEnumerable<CryptoSubportfolioItemView>> GetSubViewByCryptoPortfolio(Guid guidCryptoPortfolio)
        {
            ResultServiceObject<IEnumerable<CryptoSubportfolioItemView>> resultService = new ResultServiceObject<IEnumerable<CryptoSubportfolioItemView>>();

            IEnumerable<CryptoSubportfolioItemView> portfolios = _uow.CryptoPortfolioViewRepository.GetByCryptoPortfolio(guidCryptoPortfolio);

            resultService.Value = portfolios;

            return resultService;
        }

        public ResultServiceObject<IEnumerable<CryptoSubportfolioItemView>> GetBySubCryptoPortfolio(long idCryptoPortfolio, long idCryptoSubPortfolio)
        {
            ResultServiceObject<IEnumerable<CryptoSubportfolioItemView>> resultService = new ResultServiceObject<IEnumerable<CryptoSubportfolioItemView>>();

            IEnumerable<CryptoSubportfolioItemView> portfolios = _uow.CryptoPortfolioViewRepository.GetBySubCryptoPortfolio(idCryptoPortfolio, idCryptoSubPortfolio);

            resultService.Value = portfolios;

            return resultService;
        }
    }
}
