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
    public class CryptoCurrencyService : BaseService, ICryptoCurrencyService
    {
          public CryptoCurrencyService(IUnitOfWork uow)
        {
            _uow = uow;
        }

        public ResultServiceObject<IEnumerable<CryptoCurrency>> GetAll()
        {
            ResultServiceObject<IEnumerable<CryptoCurrency>> resultService = new ResultServiceObject<IEnumerable<CryptoCurrency>>();

            IEnumerable<CryptoCurrency> cryptoCurrencies = _uow.CryptoCurrencyRepository.GetAll();

            resultService.Value = cryptoCurrencies;

            return resultService;
        }

        public ResultServiceObject<CryptoCurrency> Insert(CryptoCurrency cryptoCurrency)
        {
            ResultServiceObject<CryptoCurrency> resultService = new ResultServiceObject<CryptoCurrency>();
            cryptoCurrency.CryptoCurrencyGuid = Guid.NewGuid();
            cryptoCurrency.UpdatedDate = DateTime.Now;
            _uow.CryptoCurrencyRepository.Insert(cryptoCurrency);

            return resultService;
        }

        public ResultServiceObject<CryptoCurrency> Update(CryptoCurrency cryptoCurrency)
        {
            ResultServiceObject<CryptoCurrency> resultService = new ResultServiceObject<CryptoCurrency>();

            resultService.Value = _uow.CryptoCurrencyRepository.Update(cryptoCurrency);

            return resultService;
        }


        public ResultServiceObject<IEnumerable<CryptoStatementView>> GetCryptosWithLogoByTrader(Guid? traderGuid, string idUser)
        {
            ResultServiceObject<IEnumerable<CryptoStatementView>> resultService = new ResultServiceObject<IEnumerable<CryptoStatementView>>();

            IEnumerable<CryptoStatementView> productUsers = _uow.CryptoCurrencyRepository.GetCryptosWithLogoByTrader(traderGuid, idUser);

            resultService.Value = productUsers;

            return resultService;
        }

        public ResultServiceObject<IEnumerable<CryptoStatementView>> GetCryptosByNameOrSymbol(string nameOrSymbol)
        {
            ResultServiceObject<IEnumerable<CryptoStatementView>> resultService = new ResultServiceObject<IEnumerable<CryptoStatementView>>();

            IEnumerable<CryptoStatementView> productUsers = _uow.CryptoCurrencyRepository.GetCryptosByNameOrSymbol(nameOrSymbol);

            resultService.Value = productUsers;

            return resultService;
        }

        public ResultServiceObject<CryptoCurrency> GetByGuid(Guid cryptoCurrencyGuid)
        {
            ResultServiceObject<CryptoCurrency> resultService = new ResultServiceObject<CryptoCurrency>();

            CryptoCurrency cryptoCurrency = _uow.CryptoCurrencyRepository.Select(item => item.CryptoCurrencyGuid == cryptoCurrencyGuid).FirstOrDefault();

            resultService.Value = cryptoCurrency;

            return resultService;
        }

        public ResultServiceObject<CryptoCurrency> GetById(long cryptoCurrencyId)
        {
            ResultServiceObject<CryptoCurrency> resultService = new ResultServiceObject<CryptoCurrency>();

            CryptoCurrency cryptoCurrency = _uow.CryptoCurrencyRepository.Select(item => item.CryptoCurrencyID == cryptoCurrencyId).FirstOrDefault();

            resultService.Value = cryptoCurrency;

            return resultService;
        }

        public ResultServiceObject<IEnumerable<CryptoStatementView>> GetMarketMoverByType(bool gainers)
        {
            ResultServiceObject<IEnumerable<CryptoStatementView>> resultService = new ResultServiceObject<IEnumerable<CryptoStatementView>>();

            IEnumerable<CryptoStatementView> productUsers = _uow.CryptoCurrencyRepository.GetMarketMoverByType(gainers);

            resultService.Value = productUsers;

            return resultService;
        }

        public ResultServiceObject<IEnumerable<CryptoStatementView>> GetTopCryptos()
        {
            ResultServiceObject<IEnumerable<CryptoStatementView>> resultService = new ResultServiceObject<IEnumerable<CryptoStatementView>>();

            IEnumerable<CryptoStatementView> productUsers = _uow.CryptoCurrencyRepository.GetTopCryptos();

            resultService.Value = productUsers;

            return resultService;
        }
    }
}
