using AutoMapper;
using Dividendos.API.Model.Request.Crypto;
using Dividendos.API.Model.Response.Common;
using Dividendos.Application.Base;
using Dividendos.Application.Interface;
using Dividendos.Entity.Entities;
using Dividendos.Entity.Model;
using Dividendos.Repository.Interface.UoW;
using Dividendos.Service.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dividendos.Application
{
    public class CryptoSubPortfolioApp : BaseApp, ICryptoSubPortfolioApp
    {
        private readonly IGlobalAuthenticationService _globalAuthenticationService;
        private readonly IMapper _mapper;
        private readonly IUnitOfWork _uow;
        private readonly ICryptoPortfolioService _cryptoPortfolioService;
        private readonly ITraderService _traderService;
        private readonly ICryptoPortfolioPerformanceService _cryptoPortfolioPerformanceService;
        private readonly ICryptoCurrencyService _cryptoCurrencyService;
        private readonly ICryptoTransactionService _cryptoTransactionService;
        private readonly ICryptoTransactionItemService _cryptoTransactionItemService;
        private readonly ICryptoCurrencyPerformanceService _cryptoCurrencyPerformanceService;
        private readonly ICryptoSubPortfolioService _cryptoSubPortfolioService;
        private readonly ICryptoSubPortfolioTransactionService _cryptoSubPortfolioTransactionService;

        public CryptoSubPortfolioApp(
            IMapper mapper,
            IUnitOfWork uow,
            IGlobalAuthenticationService globalAuthenticationService,
            ICryptoPortfolioService cryptoPortfolioService,
            ITraderService traderService,
            ICryptoPortfolioPerformanceService cryptoPortfolioPerformanceService,
            ICryptoCurrencyService cryptoCurrencyService,
            ICryptoTransactionService cryptoTransactionService,
            ICryptoTransactionItemService cryptoTransactionItemService,
            ICryptoCurrencyPerformanceService cryptoCurrencyPerformanceService,
            ICryptoSubPortfolioService cryptoSubPortfolioService,
            ICryptoSubPortfolioTransactionService cryptoSubPortfolioTransactionService)
        {
            _mapper = mapper;
            _globalAuthenticationService = globalAuthenticationService;
            _uow = uow;
            _cryptoPortfolioService = cryptoPortfolioService;
            _traderService = traderService;
            _cryptoPortfolioPerformanceService = cryptoPortfolioPerformanceService;
            _cryptoCurrencyService = cryptoCurrencyService;
            _cryptoTransactionService = cryptoTransactionService;
            _cryptoTransactionItemService = cryptoTransactionItemService;
            _cryptoCurrencyPerformanceService = cryptoCurrencyPerformanceService;
            _cryptoSubPortfolioService = cryptoSubPortfolioService;
            _cryptoSubPortfolioTransactionService = cryptoSubPortfolioTransactionService;
        }

        public ResultResponseBase Disable(Guid guidCryptoSubPortfolio)
        {
            ResultResponseBase resultResponse = new ResultResponseBase();

            ResultServiceBase resultServiceBase = new ResultServiceBase();

            using (_uow.Create())
            {
                CryptoSubPortfolio cryptoSubPortfolio = _cryptoSubPortfolioService.GetByGuid(guidCryptoSubPortfolio).Value;

                if (cryptoSubPortfolio != null)
                {
                    _cryptoSubPortfolioService.Disable(cryptoSubPortfolio);
                }
            }

            resultResponse = _mapper.Map<ResultResponseBase>(resultServiceBase);

            return resultResponse;
        }

        public ResultResponseObject<CryptoSubportfolioVM> Add(Guid guidCryptoPortfolio, CryptoSubportfolioVM cryptoSubportfolioAddVM)
        {
            ResultResponseObject<CryptoSubportfolioVM> resultServiceBase = new ResultResponseObject<CryptoSubportfolioVM>();

            using (_uow.Create())
            {
                ResultServiceObject<CryptoPortfolio> resultServiceObject = _cryptoPortfolioService.GetByGuid(guidCryptoPortfolio);

                CryptoSubPortfolio cryptoSubPortfolio = new CryptoSubPortfolio();
                cryptoSubPortfolio.IdCryptoPortfolio = resultServiceObject.Value.IdCryptoPortfolio;
                cryptoSubPortfolio.Name = cryptoSubportfolioAddVM.Name;

                cryptoSubPortfolio = _cryptoSubPortfolioService.Add(cryptoSubPortfolio).Value;

                if (cryptoSubPortfolio != null)
                {
                    CryptoSubportfolioVM cryptoSubportfolioVM = new CryptoSubportfolioVM();
                    cryptoSubportfolioVM.GuidCryptoPortfolio = resultServiceObject.Value.GuidCryptoPortfolio;
                    cryptoSubportfolioVM.GuidCryptoSubportfolio = cryptoSubPortfolio.GuidCryptoSubPortfolio;
                    cryptoSubportfolioVM.Name = cryptoSubPortfolio.Name;

                    resultServiceBase.Value = cryptoSubportfolioVM;
                    resultServiceBase.Success = true;

                    foreach (var transactionGuid in cryptoSubportfolioAddVM.TransactionsGuid)
                    {
                        CryptoTransaction cryptoTransaction = _cryptoTransactionService.GetByGuid(transactionGuid).Value;

                        if (cryptoTransaction != null)
                        {
                            CryptoCurrency cryptoCurrency = _cryptoCurrencyService.GetById(cryptoTransaction.IdCryptoCurrency).Value;

                            if (cryptoCurrency != null)
                            {
                                CryptoSubPortfolioTransaction cryptoSubPortfolioTransaction = new CryptoSubPortfolioTransaction();
                                cryptoSubPortfolioTransaction.IdCryptoPortfolio = resultServiceObject.Value.IdCryptoPortfolio;
                                cryptoSubPortfolioTransaction.IdCryptoSubPortfolio = cryptoSubPortfolio.IdCryptoSubPortfolio;
                                cryptoSubPortfolioTransaction.IdCryptoTransaction = cryptoTransaction.IdCryptoTransaction;
                                cryptoSubPortfolioTransaction.IdCryptoCurrency = cryptoCurrency.CryptoCurrencyID;
                                cryptoSubPortfolioTransaction = _cryptoSubPortfolioTransactionService.Insert(cryptoSubPortfolioTransaction).Value;
                            }
                        }
                    }
                }
            }

            return resultServiceBase;
        }

        public ResultResponseObject<CryptoSubportfolioVM> Update(Guid guidCryptoSubportfolio, CryptoSubportfolioVM subPortfolioVM)
        {
            ResultResponseObject<CryptoSubportfolioVM> resultServiceBase = new ResultResponseObject<CryptoSubportfolioVM>();

            using (_uow.Create())
            {
                CryptoSubPortfolio cryptoSubPortfolio = _cryptoSubPortfolioService.GetByGuid(guidCryptoSubportfolio).Value;

                ResultServiceObject<IEnumerable<CryptoSubPortfolioTransaction>> resultSubPortfolioOperationServiceObject = _cryptoSubPortfolioTransactionService.GetByCryptoSubportfolio(cryptoSubPortfolio.IdCryptoSubPortfolio);
                cryptoSubPortfolio.Name = subPortfolioVM.Name;
                _cryptoSubPortfolioService.Update(cryptoSubPortfolio);

                if (resultSubPortfolioOperationServiceObject.Value != null && resultSubPortfolioOperationServiceObject.Value.Count() > 0)
                {
                    foreach (var item in resultSubPortfolioOperationServiceObject.Value)
                    {
                        _cryptoSubPortfolioTransactionService.Delete(item);
                    }
                }

                if (subPortfolioVM.TransactionsGuid != null && subPortfolioVM.TransactionsGuid.Count > 0)
                {
                    foreach (Guid transactionGuid in subPortfolioVM.TransactionsGuid)
                    {
                        CryptoTransaction cryptoTransaction = _cryptoTransactionService.GetByGuid(transactionGuid).Value;

                        if (cryptoTransaction != null)
                        {
                            CryptoSubPortfolioTransaction cryptoSubPortfolioTransaction = new CryptoSubPortfolioTransaction();
                            cryptoSubPortfolioTransaction.IdCryptoCurrency = cryptoTransaction.IdCryptoCurrency;
                            cryptoSubPortfolioTransaction.IdCryptoPortfolio = cryptoTransaction.IdCryptoPortfolio;
                            cryptoSubPortfolioTransaction.IdCryptoSubPortfolio = cryptoSubPortfolio.IdCryptoSubPortfolio;
                            cryptoSubPortfolioTransaction.IdCryptoTransaction = cryptoTransaction.IdCryptoTransaction;

                            _cryptoSubPortfolioTransactionService.Insert(cryptoSubPortfolioTransaction);
                        }
                    }
                }

                CryptoSubportfolioVM cryptoSubportfolioVM = new CryptoSubportfolioVM();
                cryptoSubportfolioVM.GuidCryptoSubportfolio = cryptoSubPortfolio.GuidCryptoSubPortfolio;
                cryptoSubportfolioVM.Name = cryptoSubPortfolio.Name;

                resultServiceBase.Value = cryptoSubportfolioVM;
                resultServiceBase.Success = true;
            }

            return resultServiceBase;
        }
    }
}
