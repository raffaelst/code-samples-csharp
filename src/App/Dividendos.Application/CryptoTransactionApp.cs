using AutoMapper;
using Dividendos.API.Model.Request.Crypto;
using Dividendos.API.Model.Response;
using Dividendos.API.Model.Response.Common;
using Dividendos.API.Model.Response.Crypto;
using Dividendos.Application.Base;
using Dividendos.Application.Interface;
using Dividendos.Entity.Entities;
using Dividendos.Entity.Enum;
using Dividendos.Entity.Model;
using Dividendos.Entity.Views;
using Dividendos.Repository.Interface.UoW;
using Dividendos.Service.Interface;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dividendos.Application
{
    public class CryptoTransactionApp : BaseApp, ICryptoTransactionApp
    {
        private readonly IGlobalAuthenticationService _globalAuthenticationService;
        private readonly IMapper _mapper;
        private readonly IUnitOfWork _uow;
        private readonly ICryptoPortfolioService _cryptoPortfolioService;
        private readonly ITraderService _traderService;
        private readonly ICryptoTransactionService _cryptoTransactionService;
        private readonly ICryptoTransactionItemService _cryptoTransactionItemService;
        private readonly ICryptoPortfolioPerformanceService _cryptoPortfolioPerformanceService;
        private readonly ICryptoCurrencyService _cryptoCurrencyService;
        private readonly ICryptoCurrencyPerformanceService _cryptoCurrencyPerformanceService;
        private readonly ILogoService _logoService;
        private readonly ICryptoSubPortfolioService _cryptoSubPortfolioService;
        private readonly IFiatCurrencyService _fiatCurrencyService;
        private readonly ISubscriptionService _subscriptionService;

        public CryptoTransactionApp(
            IMapper mapper,
            IUnitOfWork uow,
            IGlobalAuthenticationService globalAuthenticationService,
            ICryptoPortfolioService cryptoPortfolioService,
            ITraderService traderService,
            ICryptoTransactionService cryptoTransactionService,
            ICryptoTransactionItemService cryptoTransactionItemService,
            ICryptoPortfolioPerformanceService cryptoPortfolioPerformanceService,
            ICryptoCurrencyService cryptoCurrencyService,
            ICryptoCurrencyPerformanceService cryptoCurrencyPerformanceService,
            ILogoService logoService,
            ICryptoSubPortfolioService cryptoSubPortfolioService,
            IFiatCurrencyService fiatCurrencyService,
            ISubscriptionService subscriptionService)
        {
            _mapper = mapper;
            _globalAuthenticationService = globalAuthenticationService;
            _uow = uow;
            _cryptoPortfolioService = cryptoPortfolioService;
            _traderService = traderService;
            _cryptoTransactionService = cryptoTransactionService;
            _cryptoTransactionItemService = cryptoTransactionItemService;
            _cryptoPortfolioPerformanceService = cryptoPortfolioPerformanceService;
            _cryptoCurrencyService = cryptoCurrencyService;
            _cryptoCurrencyPerformanceService = cryptoCurrencyPerformanceService;
            _logoService = logoService;
            _cryptoSubPortfolioService = cryptoSubPortfolioService;
            _fiatCurrencyService = fiatCurrencyService;
            _subscriptionService = subscriptionService;
        }

        public ResultResponseObject<CryptoBuyVM> BuyCrypto(Guid guidCryptoPortfolio, CryptoAddVM cryptoAddVM)
        {
            ResultResponseObject<CryptoBuyVM> resultResponseBase = new ResultResponseObject<CryptoBuyVM>();
            resultResponseBase.Success = false;

            using (_uow.Create())
            {
                resultResponseBase = BuyCrypto(guidCryptoPortfolio, cryptoAddVM, false);
            }

            return resultResponseBase;
        }

        private ResultResponseObject<CryptoBuyVM> BuyCrypto(Guid guidCryptoPortfolio, CryptoAddVM cryptoAddVM, bool priceAdjust = false)
        {
            return _cryptoTransactionService.BuyCrypto(guidCryptoPortfolio, cryptoAddVM, _cryptoPortfolioService, _cryptoTransactionService, _cryptoTransactionItemService,
                                       _cryptoPortfolioPerformanceService, _cryptoCurrencyService, _cryptoCurrencyPerformanceService, _subscriptionService, _globalAuthenticationService.IdUser, priceAdjust, true, false, null, true, true);
        }

        public ResultResponseBase SellCrypto(Guid guidCryptoPortfolio, CryptoAddVM cryptoAddVM)
        {
            ResultResponseBase resultResponseBase = new ResultResponseBase();
            resultResponseBase.Success = false;

            using (_uow.Create())
            {
                SellCrypto(guidCryptoPortfolio, cryptoAddVM, resultResponseBase);
            }

            return resultResponseBase;
        }

        private void SellCrypto(Guid guidCryptoPortfolio, CryptoAddVM cryptoAddVM, ResultResponseBase resultResponseBase, bool priceAdjust = false)
        {
            _cryptoTransactionService.SellCrypto(guidCryptoPortfolio, cryptoAddVM, resultResponseBase, _cryptoPortfolioService, _cryptoTransactionService, _cryptoTransactionItemService,
                                       _cryptoPortfolioPerformanceService, _cryptoCurrencyService, _cryptoCurrencyPerformanceService, priceAdjust, true, false, null, true, true);
        }

        public ResultResponseBase EditBuyTransaction(Guid guidCryptoPortfolio, CryptoEditVM cryptoEditVM)
        {
            ResultResponseBase resultResponseBase = new ResultResponseBase();
            resultResponseBase.Success = false;

            using (_uow.Create())
            {
                resultResponseBase = _cryptoTransactionService.EditBuyTransaction(guidCryptoPortfolio, cryptoEditVM, _cryptoPortfolioService, _cryptoTransactionItemService, _cryptoPortfolioPerformanceService, _cryptoCurrencyService, _cryptoCurrencyPerformanceService, _cryptoTransactionService);
            }

            return resultResponseBase;
        }

        public ResultResponseBase EditSellTransaction(Guid guidCryptoPortfolio, CryptoEditVM cryptoEditVM)
        {
            ResultResponseBase resultResponseBase = new ResultResponseBase();
            resultResponseBase.Success = false;

            using (_uow.Create())
            {
                _cryptoTransactionService.EditSellTransaction(guidCryptoPortfolio, cryptoEditVM, resultResponseBase, _cryptoPortfolioService, _cryptoTransactionItemService, _cryptoPortfolioPerformanceService, _cryptoCurrencyService, _cryptoCurrencyPerformanceService, _cryptoTransactionService);
            }

            return resultResponseBase;
        }

        public ResultResponseBase InactiveCryptoTransactionItem(Guid guidCryptoPortfolio, Guid guidCryptoTransactionItem)
        {
            ResultResponseBase resultResponseBase = new ResultResponseBase();
            resultResponseBase.Success = false;

            using (_uow.Create())
            {
                InactiveCryptoTransactionItem(guidCryptoPortfolio, guidCryptoTransactionItem, resultResponseBase);
            }

            return resultResponseBase;
        }

        private void InactiveCryptoTransactionItem(Guid guidCryptoPortfolio, Guid guidCryptoTransactionItem, ResultResponseBase resultResponseBase)
        {
            if (guidCryptoTransactionItem != Guid.Empty)
            {
                ResultServiceObject<CryptoTransactionItem> resultCryptoTransactionItem = _cryptoTransactionItemService.GetByGuid(guidCryptoTransactionItem);

                if (resultCryptoTransactionItem.Success && resultCryptoTransactionItem.Value != null)
                {
                    CryptoCurrency cryptoCurrency = _cryptoCurrencyService.GetById(resultCryptoTransactionItem.Value.IdCryptoCurrency).Value;

                    if (cryptoCurrency != null)
                    {
                        CryptoTransactionItem cryptoTransactionItemDb = resultCryptoTransactionItem.Value;
                        cryptoTransactionItemDb.GuidCryptoTransactionItem = guidCryptoTransactionItem;

                        ResultServiceObject<CryptoPortfolio> resultCryptoPortfolio = _cryptoPortfolioService.GetByGuid(guidCryptoPortfolio);

                        if (resultCryptoPortfolio.Success && resultCryptoPortfolio.Value != null)
                        {
                            CryptoPortfolio cryptoPortfolio = resultCryptoPortfolio.Value;
                            ResultServiceObject<CryptoTransaction> resultCryptoTransactionSell = _cryptoTransactionService.GetByCryptoPortfolioAndIdCryptoCurrency(cryptoPortfolio.IdCryptoPortfolio, cryptoCurrency.CryptoCurrencyID, 2);
                            ResultServiceObject<CryptoTransaction> resultCryptoTransactionBuy = _cryptoTransactionService.GetByCryptoPortfolioAndIdCryptoCurrency(cryptoPortfolio.IdCryptoPortfolio, cryptoCurrency.CryptoCurrencyID, 1);

                            if (resultCryptoTransactionSell.Success && resultCryptoTransactionBuy.Success)
                            {
                                CryptoTransaction cryptoTransactionSell = resultCryptoTransactionSell.Value;
                                CryptoTransaction cryptoTransactionBuy = resultCryptoTransactionBuy.Value;

                                if (cryptoTransactionBuy != null)
                                {
                                    decimal quantitySell = 0;

                                    if (cryptoTransactionSell != null)
                                    {
                                        quantitySell = cryptoTransactionSell.Quantity;
                                    }

                                    ResultServiceObject<IEnumerable<CryptoTransactionItem>> resultCryptoTransactionBuyItems = _cryptoTransactionItemService.GetByIdCryptoTransaction(cryptoTransactionBuy.IdCryptoTransaction, 1);
                                    decimal quantityBuy = 0;

                                    if (resultCryptoTransactionBuyItems.Success && resultCryptoTransactionBuyItems.Value != null && resultCryptoTransactionBuyItems.Value.Count() > 0)
                                    {
                                        quantityBuy = resultCryptoTransactionBuyItems.Value.Sum(opItemTmp => opItemTmp.Quantity);
                                    }

                                    if (cryptoTransactionItemDb.TransactionType == 1)
                                    {
                                        quantityBuy -= cryptoTransactionItemDb.Quantity;
                                    }
                                    else if (cryptoTransactionItemDb.TransactionType == 2)
                                    {
                                        quantitySell -= cryptoTransactionItemDb.Quantity;
                                    }

                                    if (quantitySell <= quantityBuy)
                                    {
                                        List<CryptoTransactionItem> cryptoTransactionItems = new List<CryptoTransactionItem>();

                                        _cryptoTransactionItemService.Delete(cryptoTransactionItemDb);

                                        quantitySell = 0;
                                        decimal totalPriceSell = 0;
                                        quantityBuy = 0;
                                        decimal totalPriceBuy = 0;

                                        if (cryptoTransactionSell != null)
                                        {
                                            ResultServiceObject<IEnumerable<CryptoTransactionItem>> resultCryptoTransactionSellItems = _cryptoTransactionItemService.GetByIdCryptoTransaction(cryptoTransactionSell.IdCryptoTransaction, 2);

                                            if (resultCryptoTransactionSellItems.Success)
                                            {
                                                cryptoTransactionItems.AddRange(resultCryptoTransactionSellItems.Value.ToList());
                                            }

                                            if (cryptoTransactionItems != null && cryptoTransactionItems.Count > 0)
                                            {
                                                quantitySell = cryptoTransactionItems.Where(op => op.TransactionType == 2).Sum(opItemTmp => opItemTmp.Quantity);
                                                totalPriceSell = cryptoTransactionItems.Where(op => op.TransactionType == 2).Sum(opItemTmp => (opItemTmp.AveragePrice * opItemTmp.Quantity));
                                            }
                                            else
                                            {
                                                cryptoTransactionSell.Quantity = 0;
                                            }

                                            cryptoTransactionSell.AveragePrice = quantitySell == 0 ? 0 : totalPriceSell / quantitySell;
                                            cryptoTransactionSell.Quantity = quantitySell;

                                            if (cryptoTransactionSell.Quantity <= 0)
                                            {
                                                cryptoTransactionSell.Active = false;
                                                cryptoTransactionSell.Quantity = 0;
                                                cryptoTransactionSell.AveragePrice = 0;
                                            }
                                            else
                                            {
                                                cryptoTransactionSell.Active = true;
                                            }

                                            _cryptoTransactionService.Update(cryptoTransactionSell);
                                        }


                                        if (cryptoTransactionBuy != null)
                                        {
                                            resultCryptoTransactionBuyItems = _cryptoTransactionItemService.GetByIdCryptoTransaction(cryptoTransactionBuy.IdCryptoTransaction, 1);

                                            if (resultCryptoTransactionBuyItems.Success)
                                            {
                                                cryptoTransactionItems.AddRange(resultCryptoTransactionBuyItems.Value.ToList());
                                            }

                                            if (cryptoTransactionItems != null && cryptoTransactionItems.Count > 0)
                                            {
                                                quantityBuy = cryptoTransactionItems.Where(op => op.TransactionType == 1).Sum(opItemTmp => opItemTmp.Quantity);
                                                totalPriceBuy = cryptoTransactionItems.Where(op => op.TransactionType == 1).Sum(opItemTmp => (opItemTmp.AveragePrice * opItemTmp.Quantity));
                                            }
                                            else
                                            {
                                                cryptoTransactionBuy.Quantity = 0;
                                            }

                                            decimal quantityCalc = 0;
                                            decimal avgPriceCalc = 0;
                                            bool valid = _cryptoTransactionService.CalculateAveragePrice(ref cryptoTransactionItems, out quantityCalc, out avgPriceCalc);

                                            if (valid)
                                            {
                                                cryptoTransactionBuy.AveragePrice = avgPriceCalc;
                                                cryptoTransactionBuy.Quantity = quantityCalc;

                                                if (cryptoTransactionBuy.Quantity <= 0)
                                                {
                                                    cryptoTransactionBuy.Active = false;
                                                    cryptoTransactionBuy.Quantity = 0;
                                                    cryptoTransactionBuy.AveragePrice = 0;
                                                }
                                                else
                                                {
                                                    cryptoTransactionBuy.Active = true;
                                                }

                                                _cryptoTransactionService.Update(cryptoTransactionBuy);
                                            }
                                            else
                                            {
                                                throw new Exception("Quantidade vendida informada é maior do que a quantidade que você possui para este ativo");
                                            }
                                        }

                                        _cryptoPortfolioPerformanceService.CalculatePerformance(cryptoPortfolio.IdCryptoPortfolio, _cryptoPortfolioService, _cryptoCurrencyService, _cryptoTransactionService, _cryptoCurrencyPerformanceService);

                                        resultResponseBase.Success = true;
                                    }
                                    else
                                    {
                                        resultResponseBase.ErrorMessages.Add("Quantidade vendida informada é maior do que a quantidade que você possui para este ativo");
                                    }
                                }
                            }
                        }
                    }
                }
            }
        }

        public ResultResponseObject<CryptoTransactionItemSummaryWrapperVM> GetTransactionItemSummary(Guid guidPortfolio, Guid guidCryptoCurrency, int transactionType)
        {
            ResultResponseObject<CryptoTransactionItemSummaryWrapperVM> resultServiceObject = new ResultResponseObject<CryptoTransactionItemSummaryWrapperVM>();
            CryptoTransactionItemSummaryWrapperVM cryptoTransactionItemSummaryWrapperVM = new CryptoTransactionItemSummaryWrapperVM();

            using (_uow.Create())
            {
                ResultServiceObject<CryptoCurrency> resultCryptoCurrency = _cryptoCurrencyService.GetByGuid(guidCryptoCurrency);

                if (guidPortfolio != Guid.Empty && resultCryptoCurrency.Value != null && transactionType != 0)
                {
                    ResultServiceObject<Logo> resultLogo = _logoService.GetById(resultCryptoCurrency.Value.LogoID);

                    ResultServiceObject<CryptoPortfolio> resultCryptoPortfolio = _cryptoPortfolioService.GetByGuid(guidPortfolio);

                    if (resultCryptoPortfolio.Success && resultCryptoPortfolio.Value != null && resultLogo.Value != null)
                    {
                        CryptoPortfolio cryptoPortfolio = resultCryptoPortfolio.Value;

                        ResultServiceObject<CryptoTransaction> resultCryptoTransaction = _cryptoTransactionService.GetByCryptoPortfolioAndIdCryptoCurrency(cryptoPortfolio.IdCryptoPortfolio, resultCryptoCurrency.Value.CryptoCurrencyID, transactionType);

                        if (resultCryptoTransaction.Success && resultCryptoTransaction.Value != null)
                        {
                            ResultServiceObject<IEnumerable<CryptoTransactionItem>> resultCryptoTransactionItems = _cryptoTransactionItemService.GetByIdCryptoTransaction(resultCryptoTransaction.Value.IdCryptoTransaction, transactionType);

                            if (resultCryptoTransactionItems.Success && resultCryptoTransactionItems.Value != null && resultCryptoTransactionItems.Value.Count() > 0)
                            {
                                List<CryptoTransactionItem> cryptoTransactionItems = resultCryptoTransactionItems.Value.OrderByDescending(opItemTmp => opItemTmp.EventDate).ToList();
                                cryptoTransactionItemSummaryWrapperVM = new CryptoTransactionItemSummaryWrapperVM();
                                cryptoTransactionItemSummaryWrapperVM.CryptoTransactionItemsSummary = new List<CryptoTransactionItemSummaryVM>();

                                cryptoTransactionItemSummaryWrapperVM.Name = resultCryptoCurrency.Value.Name;
                                cryptoTransactionItemSummaryWrapperVM.Logo = resultLogo.Value.URL;
                                cryptoTransactionItemSummaryWrapperVM.GuidCryptoPortfolio = cryptoPortfolio.GuidCryptoPortfolio;
                                cryptoTransactionItemSummaryWrapperVM.GuidCryptoTransaction = resultCryptoTransaction.Value.GuidCryptoTransaction;
                                cryptoTransactionItemSummaryWrapperVM.GuidCryptoCurrency = resultCryptoCurrency.Value.CryptoCurrencyGuid;
                                cryptoTransactionItemSummaryWrapperVM.IdFiatCurrency = cryptoPortfolio.IdFiatCurrency;

                                foreach (CryptoTransactionItem cryptoTransactionItem in cryptoTransactionItems)
                                {
                                    CryptoTransactionItemSummaryVM cryptoTransactionItemSummaryVM = new CryptoTransactionItemSummaryVM();
                                    cryptoTransactionItemSummaryVM.AveragePrice = string.Format("{0} {1}", _fiatCurrencyService.GetCurrencySymbol((FiatCurrencyEnum)cryptoPortfolio.IdFiatCurrency), cryptoTransactionItem.AveragePrice.ToString("n2", new CultureInfo("pt-br")));
                                    cryptoTransactionItemSummaryVM.GuidCryptoCurrency = resultCryptoCurrency.Value.CryptoCurrencyGuid;
                                    cryptoTransactionItemSummaryVM.GuidCryptoTransactionItem = cryptoTransactionItem.GuidCryptoTransactionItem;
                                    cryptoTransactionItemSummaryVM.GuidCryptoTransaction = resultCryptoTransaction.Value.GuidCryptoTransaction;
                                    cryptoTransactionItemSummaryVM.TransactionType = cryptoTransactionItem.TransactionType;
                                    cryptoTransactionItemSummaryVM.Quantity = cryptoTransactionItem.Quantity.ToString("0.#############################", new CultureInfo("pt-br"));
                                    cryptoTransactionItemSummaryVM.Exchange = cryptoTransactionItem.Exchange;
                                    cryptoTransactionItemSummaryVM.EventDate = cryptoTransactionItem.EventDate.ToString("dd/MM/yyyy");

                                    cryptoTransactionItemSummaryWrapperVM.CryptoTransactionItemsSummary.Add(cryptoTransactionItemSummaryVM);
                                }
                            }
                        }
                    }
                }
            }

            if (cryptoTransactionItemSummaryWrapperVM != null && cryptoTransactionItemSummaryWrapperVM.CryptoTransactionItemsSummary == null)
            {
                cryptoTransactionItemSummaryWrapperVM.CryptoTransactionItemsSummary = new List<CryptoTransactionItemSummaryVM>();
            }

            resultServiceObject.Value = cryptoTransactionItemSummaryWrapperVM;
            resultServiceObject.Success = true;

            return resultServiceObject;
        }

        public ResultResponseObject<CryptoTransactionSellViewWrapperVM> GetCryptoTransactionSellView(Guid guidCryptoPortfolioSub, string startDate, string endDate)
        {
            ResultResponseObject<CryptoTransactionSellViewWrapperVM> resultServiceObject = new ResultResponseObject<CryptoTransactionSellViewWrapperVM>();
            resultServiceObject.Success = true;
            CryptoTransactionSellViewWrapperVM cryptoTransactionSellViewWrapperVM = new CryptoTransactionSellViewWrapperVM();


            DateTime? startDateParam = null;
            DateTime? endDateParam = null;

            if (!string.IsNullOrWhiteSpace(startDate) && !string.IsNullOrWhiteSpace(endDate))
            {
                DateTime operationStartDate;

                if (DateTime.TryParseExact(startDate, "dd/MM/yyyy", new CultureInfo("pt-br"), DateTimeStyles.None, out operationStartDate))
                {
                    startDateParam = operationStartDate;
                }
                else
                {
                    resultServiceObject.ErrorMessages.Add("Data de início inválida");
                }

                DateTime operationEndDate;

                if (DateTime.TryParseExact(endDate, "dd/MM/yyyy", new CultureInfo("pt-br"), DateTimeStyles.None, out operationEndDate))
                {
                    endDateParam = operationEndDate;
                }
                else
                {
                    resultServiceObject.ErrorMessages.Add("Data fim inválida");
                }
            }

            if (resultServiceObject.ErrorMessages == null || resultServiceObject.ErrorMessages.Count() == 0)
            {
                using (_uow.Create())
                {
                    ResultServiceObject<CryptoPortfolio> resultCryptoPortfolio = _cryptoPortfolioService.GetByGuid(guidCryptoPortfolioSub);
                    ResultServiceObject<CryptoSubPortfolio> resultCryptoSubPortfolio = _cryptoSubPortfolioService.GetByGuid(guidCryptoPortfolioSub);

                    if (resultCryptoSubPortfolio.Success && resultCryptoPortfolio.Success)
                    {
                        ResultServiceObject<IEnumerable<CryptoTransactionDetailsView>> result = new ResultServiceObject<IEnumerable<CryptoTransactionDetailsView>>();
                        CryptoPortfolio cryptoPortfolio = resultCryptoPortfolio.Value;
                        CryptoSubPortfolio cryptoSubportfolio = resultCryptoSubPortfolio.Value;

                        if (cryptoPortfolio != null)
                        {
                            cryptoTransactionSellViewWrapperVM.GuidCryptoPortfolioSubPortfolio = cryptoPortfolio.GuidCryptoPortfolio;
                            cryptoTransactionSellViewWrapperVM.IdCryptoPortfolio = cryptoPortfolio.IdCryptoPortfolio;
                            result = _cryptoTransactionService.GetDetailsByIdCryptoPortfolio(cryptoPortfolio.IdCryptoPortfolio, 2, startDateParam, endDateParam);
                            cryptoTransactionSellViewWrapperVM.IdFiatCurrency = cryptoPortfolio.IdFiatCurrency;
                        }
                        else if (cryptoSubportfolio != null)
                        {
                            cryptoTransactionSellViewWrapperVM.GuidCryptoPortfolioSubPortfolio = cryptoSubportfolio.GuidCryptoSubPortfolio;
                            cryptoTransactionSellViewWrapperVM.IdCryptoPortfolio = cryptoSubportfolio.IdCryptoPortfolio;
                            result = _cryptoTransactionService.GetDetailsByIdCryptoSubPortfolio(cryptoSubportfolio.IdCryptoPortfolio, cryptoSubportfolio.IdCryptoSubPortfolio, 2, startDateParam, endDateParam);

                            resultCryptoPortfolio = _cryptoPortfolioService.GetById(cryptoSubportfolio.IdCryptoSubPortfolio);

                            if (resultCryptoPortfolio.Value != null)
                            {
                                cryptoTransactionSellViewWrapperVM.IdFiatCurrency = resultCryptoPortfolio.Value.IdFiatCurrency;
                            }
                        }

                        if (result.Success && result.Value != null && result.Value.Count() > 0)
                        {
                            cryptoTransactionSellViewWrapperVM.CryptoTransactionSellCurrency = new List<CryptoTransactionSellCurrencyVM>();
                            decimal totalSoldGeneral = 0;
                            decimal totalLossProfitGeneral = 0;
                            decimal totalLossGeneral = 0;
                            decimal totalProfitGeneral = 0;

                            List<CryptoTransactionDetailsView> cryptoTransactionsSellDetailsView = result.Value.OrderBy(op => op.Name).ThenByDescending(op => op.EventDate).ToList();

                            foreach (CryptoTransactionDetailsView cryptoTransactionSellDetailsView in cryptoTransactionsSellDetailsView)
                            {
                                #region Item

                                CryptoTransactionSellDetailsVM cryptoTransactionSellDetailsVM = new CryptoTransactionSellDetailsVM();
                                cryptoTransactionSellDetailsVM.AveragePrice = string.Format("{0} {1}", _fiatCurrencyService.GetCurrencySymbol((FiatCurrencyEnum)cryptoPortfolio.IdFiatCurrency), cryptoTransactionSellDetailsView.AveragePrice.ToString("n2", new CultureInfo("pt-br")));

                                if (cryptoTransactionSellDetailsView.AcquisitionPrice != 0)
                                {
                                    cryptoTransactionSellDetailsVM.AcquisitionPrice = string.Format("{0} {1}", _fiatCurrencyService.GetCurrencySymbol((FiatCurrencyEnum)cryptoPortfolio.IdFiatCurrency), cryptoTransactionSellDetailsView.AcquisitionPrice.ToString("n2", new CultureInfo("pt-br")));
                                }
                                else
                                {
                                    cryptoTransactionSellDetailsVM.AcquisitionPrice = "--";
                                }

                                cryptoTransactionSellDetailsVM.GuidCryptoTransactionItem = cryptoTransactionSellDetailsView.GuidCryptoTransactionItem;
                                cryptoTransactionSellDetailsVM.Quantity = cryptoTransactionSellDetailsView.Quantity.ToString("0.#############################", new CultureInfo("pt-br"));
                                cryptoTransactionSellDetailsVM.EventDate = cryptoTransactionSellDetailsView.EventDate.ToString("dd/MM/yyyy");
                                cryptoTransactionSellDetailsVM.LossProfit = "--";
                                cryptoTransactionSellDetailsVM.GuidCryptoCurrency = cryptoTransactionSellDetailsView.GuidCryptoCurrency;
                                cryptoTransactionSellDetailsVM.Exchange = cryptoTransactionSellDetailsView.Exchange;


                                decimal totalLossProfitItem = 0;

                                if (cryptoTransactionSellDetailsView.AcquisitionPrice != 0)
                                {
                                    totalLossProfitItem = (cryptoTransactionSellDetailsView.AveragePrice - cryptoTransactionSellDetailsView.AcquisitionPrice) * cryptoTransactionSellDetailsView.Quantity;
                                    cryptoTransactionSellDetailsVM.LossProfit = string.Format("{0} {1}{2}", _fiatCurrencyService.GetCurrencySymbol((FiatCurrencyEnum)cryptoPortfolio.IdFiatCurrency), GetSignal(totalLossProfitItem), totalLossProfitItem.ToString("n2", new CultureInfo("pt-br")));
                                }

                                decimal totalItem = cryptoTransactionSellDetailsView.AveragePrice * cryptoTransactionSellDetailsView.Quantity;
                                cryptoTransactionSellDetailsVM.Total = string.Format("{0} {1}", _fiatCurrencyService.GetCurrencySymbol((FiatCurrencyEnum)cryptoPortfolio.IdFiatCurrency), totalItem.ToString("n2", new CultureInfo("pt-br")));

                                if (totalItem != 0)
                                {
                                    decimal perc = totalLossProfitItem / totalItem * 100;
                                    cryptoTransactionSellDetailsVM.Perc = GetSignal(perc) + perc.ToString("n2", new CultureInfo("pt-br"));
                                }

                                #endregion

                                #region Company

                                CryptoTransactionSellCurrencyVM cryptoTransactionSellCurrencyVM = cryptoTransactionSellViewWrapperVM.CryptoTransactionSellCurrency.FirstOrDefault(op => op.Name == cryptoTransactionSellDetailsView.Name);

                                if (cryptoTransactionSellCurrencyVM == null)
                                {
                                    decimal lossCp = 0;
                                    decimal profitCp = 0;

                                    decimal totalSoldCp = cryptoTransactionsSellDetailsView.Where(opTmp => opTmp.Name == cryptoTransactionSellDetailsView.Name)
                                        .Sum(opTmp =>
                                        {
                                            decimal total = opTmp.AveragePrice * opTmp.Quantity;

                                            return total;
                                        });

                                    totalSoldGeneral += totalSoldCp;

                                    decimal totalProfitCp = cryptoTransactionsSellDetailsView.Where(opTmp => opTmp.Name == cryptoTransactionSellDetailsView.Name)
                                        .Sum(opTmp =>
                                        {
                                            decimal totalLossProfit = 0;

                                            if (opTmp.AcquisitionPrice != 0)
                                            {
                                                totalLossProfit = (opTmp.AveragePrice - opTmp.AcquisitionPrice) * opTmp.Quantity;
                                            }

                                            if (totalLossProfit > 0)
                                            {
                                                profitCp += totalLossProfit;
                                            }
                                            else
                                            {
                                                lossCp += totalLossProfit;
                                            }

                                            return totalLossProfit;
                                        });

                                    totalLossGeneral += lossCp;
                                    totalProfitGeneral += profitCp;
                                    totalLossProfitGeneral += totalProfitCp;


                                    cryptoTransactionSellCurrencyVM = new CryptoTransactionSellCurrencyVM();
                                    cryptoTransactionSellCurrencyVM.GuidCryptoCurrency = cryptoTransactionSellDetailsView.GuidCryptoCurrency;
                                    cryptoTransactionSellCurrencyVM.Logo = cryptoTransactionSellDetailsView.Logo;
                                    cryptoTransactionSellCurrencyVM.Name = cryptoTransactionSellDetailsView.Name;
                                    cryptoTransactionSellCurrencyVM.TotalSold = string.Format("{0} {1}", _fiatCurrencyService.GetCurrencySymbol((FiatCurrencyEnum)cryptoPortfolio.IdFiatCurrency), totalSoldCp.ToString("n2", new CultureInfo("pt-br")));
                                    cryptoTransactionSellCurrencyVM.Profit = string.Format("{0} {1}", _fiatCurrencyService.GetCurrencySymbol((FiatCurrencyEnum)cryptoPortfolio.IdFiatCurrency), GetSignal(profitCp) + profitCp.ToString("n2", new CultureInfo("pt-br")));
                                    cryptoTransactionSellCurrencyVM.Loss = string.Format("{0} {1}{2}", _fiatCurrencyService.GetCurrencySymbol((FiatCurrencyEnum)cryptoPortfolio.IdFiatCurrency), GetSignal(lossCp) , lossCp.ToString("n2", new CultureInfo("pt-br")));
                                    cryptoTransactionSellCurrencyVM.LossProfit = string.Format("{0} {1}{2}", _fiatCurrencyService.GetCurrencySymbol((FiatCurrencyEnum)cryptoPortfolio.IdFiatCurrency), GetSignal(totalProfitCp), totalProfitCp.ToString("n2", new CultureInfo("pt-br")));


                                    if (totalSoldCp != 0)
                                    {
                                        decimal perc = (totalProfitCp / totalSoldCp * 100);
                                        cryptoTransactionSellCurrencyVM.Perc = GetSignal(perc) + perc.ToString("n2", new CultureInfo("pt-br"));
                                    }

                                    cryptoTransactionSellCurrencyVM.Transactions = new List<CryptoTransactionSellDetailsVM>();

                                    cryptoTransactionSellViewWrapperVM.CryptoTransactionSellCurrency.Add(cryptoTransactionSellCurrencyVM);

                                    totalSoldCp = 0;
                                    totalProfitCp = 0;
                                }

                                #endregion

                                cryptoTransactionSellCurrencyVM.Transactions.Add(cryptoTransactionSellDetailsVM);
                            }

                            if (totalSoldGeneral != 0)
                            {
                                decimal perc = (totalLossProfitGeneral / totalSoldGeneral * 100);
                                cryptoTransactionSellViewWrapperVM.Perc = GetSignal(perc) + perc.ToString("n2", new CultureInfo("pt-br"));
                            }

                            cryptoTransactionSellViewWrapperVM.TotalLoss = string.Format("{0} {1}{2}", _fiatCurrencyService.GetCurrencySymbol((FiatCurrencyEnum)cryptoPortfolio.IdFiatCurrency), GetSignal(totalLossGeneral), totalLossGeneral.ToString("n2", new CultureInfo("pt-br")));
                            cryptoTransactionSellViewWrapperVM.TotalProfit = string.Format("{0} {1}{2}", _fiatCurrencyService.GetCurrencySymbol((FiatCurrencyEnum)cryptoPortfolio.IdFiatCurrency), GetSignal(totalProfitGeneral), totalProfitGeneral.ToString("n2", new CultureInfo("pt-br")));
                            cryptoTransactionSellViewWrapperVM.TotalLossProfit = string.Format("{0} {1}{2}", _fiatCurrencyService.GetCurrencySymbol((FiatCurrencyEnum)cryptoPortfolio.IdFiatCurrency), GetSignal(totalLossProfitGeneral), totalLossProfitGeneral.ToString("n2", new CultureInfo("pt-br")));
                            cryptoTransactionSellViewWrapperVM.TotalSold = string.Format("{0} {1}", _fiatCurrencyService.GetCurrencySymbol((FiatCurrencyEnum)cryptoPortfolio.IdFiatCurrency), totalSoldGeneral.ToString("n2", new CultureInfo("pt-br")));
                        }
                    }
                }
            }

            if (resultServiceObject.ErrorMessages != null && resultServiceObject.ErrorMessages.Count() > 0)
            {
                resultServiceObject.Success = false;
            }

            resultServiceObject.Value = cryptoTransactionSellViewWrapperVM;
            resultServiceObject.Success = true;

            return resultServiceObject;
        }

        public ResultResponseObject<CryptoTransactionBuyViewWrapperVM> GetCryptoTransactionBuyView(Guid guidCryptoPortfolioSub, string startDate, string endDate)
        {
            ResultResponseObject<CryptoTransactionBuyViewWrapperVM> resultServiceObject = new ResultResponseObject<CryptoTransactionBuyViewWrapperVM>();
            CryptoTransactionBuyViewWrapperVM cryptoTransactionBuyViewWrapperVM = new CryptoTransactionBuyViewWrapperVM();

            DateTime? startDateParam = null;
            DateTime? endDateParam = null;

            if (!string.IsNullOrWhiteSpace(startDate) && !string.IsNullOrWhiteSpace(endDate))
            {
                DateTime operationStartDate;

                if (DateTime.TryParseExact(startDate, "dd/MM/yyyy", new CultureInfo("pt-br"), DateTimeStyles.None, out operationStartDate))
                {
                    startDateParam = operationStartDate;
                }
                else
                {
                    resultServiceObject.ErrorMessages.Add("Data de início inválida");
                }

                DateTime operationEndDate;

                if (DateTime.TryParseExact(endDate, "dd/MM/yyyy", new CultureInfo("pt-br"), DateTimeStyles.None, out operationEndDate))
                {
                    endDateParam = operationEndDate;
                }
                else
                {
                    resultServiceObject.ErrorMessages.Add("Data fim inválida");
                }
            }

            using (_uow.Create())
            {
                ResultServiceObject<CryptoPortfolio> resultCryptoPortfolio = _cryptoPortfolioService.GetByGuid(guidCryptoPortfolioSub);
                ResultServiceObject<CryptoSubPortfolio> resultCryptoSubPortfolio = _cryptoSubPortfolioService.GetByGuid(guidCryptoPortfolioSub);

                if (resultCryptoSubPortfolio.Success && resultCryptoPortfolio.Success)
                {
                    ResultServiceObject<IEnumerable<CryptoTransactionDetailsView>> result = new ResultServiceObject<IEnumerable<CryptoTransactionDetailsView>>();
                    CryptoPortfolio cryptoPortfolio = resultCryptoPortfolio.Value;
                    CryptoSubPortfolio cryptoSubportfolio = resultCryptoSubPortfolio.Value;

                    //ResultServiceObject<IEnumerable<CryptoCurrencyStatementView>> result = new ResultServiceObject<IEnumerable<CryptoCurrencyStatementView>>();
                    ResultServiceObject<IEnumerable<CryptoCurrencyStatementView>> resultCryptoCurrencyStatementView = new ResultServiceObject<IEnumerable<CryptoCurrencyStatementView>>();

                    if (cryptoPortfolio != null)
                    {
                        cryptoTransactionBuyViewWrapperVM.GuidCryptoPortfolioSubPortfolio = cryptoPortfolio.GuidCryptoPortfolio;
                        cryptoTransactionBuyViewWrapperVM.IdCryptoPortfolio = cryptoPortfolio.IdCryptoPortfolio;
                        result = _cryptoTransactionService.GetDetailsByIdCryptoPortfolio(cryptoPortfolio.IdCryptoPortfolio, 1, startDateParam, endDateParam);
                        cryptoTransactionBuyViewWrapperVM.IdFiatCurrency = cryptoPortfolio.IdFiatCurrency;
                        resultCryptoCurrencyStatementView = _cryptoPortfolioService.GetByCryptoPortfolio(cryptoPortfolio.GuidCryptoPortfolio);
                    }
                    else if (cryptoSubportfolio != null)
                    {
                        cryptoTransactionBuyViewWrapperVM.GuidCryptoPortfolioSubPortfolio = cryptoSubportfolio.GuidCryptoSubPortfolio;
                        cryptoTransactionBuyViewWrapperVM.IdCryptoPortfolio = cryptoSubportfolio.IdCryptoPortfolio;
                        result = _cryptoTransactionService.GetDetailsByIdCryptoSubPortfolio(cryptoSubportfolio.IdCryptoPortfolio, cryptoSubportfolio.IdCryptoSubPortfolio, 1, startDateParam, endDateParam);

                        resultCryptoPortfolio = _cryptoPortfolioService.GetById(cryptoSubportfolio.IdCryptoPortfolio);
                        resultCryptoCurrencyStatementView = _cryptoPortfolioService.GetByCryptoSubportfolio(cryptoSubportfolio.GuidCryptoSubPortfolio);

                        if (resultCryptoPortfolio.Value != null)
                        {
                            cryptoTransactionBuyViewWrapperVM.IdFiatCurrency = resultCryptoPortfolio.Value.IdFiatCurrency;
                        }
                    }

                    if (result.Success && result.Value != null && result.Value.Count() > 0 && resultCryptoCurrencyStatementView.Success)
                    {
                        cryptoTransactionBuyViewWrapperVM.CryptoTransactionBuyCurrency = new List<CryptoTransactionBuyCurrencyVM>();
                        decimal totalBuyGeneral = 0;
                        string[] stocksThousand = null;
                        
                        List<CryptoTransactionDetailsView> cryptoTransactionsBuyDetailsView = result.Value.OrderBy(op => op.Name).ThenByDescending(op => op.EventDate).ToList();

                        foreach (CryptoTransactionDetailsView cryptoTransactiosBuyDetailsView in cryptoTransactionsBuyDetailsView)
                        {
                            #region Item

                            CryptoTransactionBuyDetailsVM cryptoTransactionBuyDetailsVM = new CryptoTransactionBuyDetailsVM();
                            cryptoTransactionBuyDetailsVM.AveragePrice = string.Format("{0} {1}", _fiatCurrencyService.GetCurrencySymbol((FiatCurrencyEnum)cryptoPortfolio.IdFiatCurrency), cryptoTransactiosBuyDetailsView.AveragePrice.ToString("n2", new CultureInfo("pt-br")));
                            cryptoTransactionBuyDetailsVM.GuidCryptoTransactionItem = cryptoTransactiosBuyDetailsView.GuidCryptoTransactionItem;
                            cryptoTransactionBuyDetailsVM.GuidCryptoCurrency = cryptoTransactiosBuyDetailsView.GuidCryptoCurrency;
                            cryptoTransactionBuyDetailsVM.Quantity = cryptoTransactiosBuyDetailsView.Quantity.ToString("0.#############################", new CultureInfo("pt-br"));
                            cryptoTransactionBuyDetailsVM.EventDate = cryptoTransactiosBuyDetailsView.EventDate.ToString("dd/MM/yyyy");
                            cryptoTransactionBuyDetailsVM.Exchange = cryptoTransactiosBuyDetailsView.Exchange;

                            decimal totalItem = cryptoTransactiosBuyDetailsView.AveragePrice * cryptoTransactiosBuyDetailsView.Quantity;

                            cryptoTransactionBuyDetailsVM.Total = string.Format("{0} {1}", _fiatCurrencyService.GetCurrencySymbol((FiatCurrencyEnum)cryptoPortfolio.IdFiatCurrency), totalItem.ToString("n2", new CultureInfo("pt-br")));

                            #endregion

                            #region Company

                            CryptoTransactionBuyCurrencyVM cryptoTransactionBuyCurrencyVM = cryptoTransactionBuyViewWrapperVM.CryptoTransactionBuyCurrency.FirstOrDefault(op => op.Name == cryptoTransactiosBuyDetailsView.Name);

                            if (cryptoTransactionBuyCurrencyVM == null)
                            {
                                decimal totalBuyCp = cryptoTransactionsBuyDetailsView.Where(opTmp => opTmp.Name == cryptoTransactiosBuyDetailsView.Name)
                                    .Sum(opTmp =>
                                    {
                                        decimal total = opTmp.AveragePrice * opTmp.Quantity;

                                        if (stocksThousand != null && stocksThousand.Length > 0 && stocksThousand.Contains(opTmp.Name))
                                        {
                                            total = total / 1000;
                                        }

                                        return total;
                                    });

                                totalBuyGeneral += totalBuyCp;


                                cryptoTransactionBuyCurrencyVM = new CryptoTransactionBuyCurrencyVM();
                                cryptoTransactionBuyCurrencyVM.GuidCryptoCurrency = cryptoTransactiosBuyDetailsView.GuidCryptoCurrency;
                                cryptoTransactionBuyCurrencyVM.Logo = cryptoTransactiosBuyDetailsView.Logo;
                                cryptoTransactionBuyCurrencyVM.Name = cryptoTransactiosBuyDetailsView.Name;
                                cryptoTransactionBuyCurrencyVM.TotalBuy = string.Format("{0} {1}", _fiatCurrencyService.GetCurrencySymbol((FiatCurrencyEnum)cryptoPortfolio.IdFiatCurrency), totalBuyCp.ToString("n2", new CultureInfo("pt-br")));

                                if (resultCryptoCurrencyStatementView.Value != null && resultCryptoCurrencyStatementView.Value.Count() > 0)
                                {
                                    CryptoCurrencyStatementView cryptoCurrencyStatementView = resultCryptoCurrencyStatementView.Value.FirstOrDefault(portStView => portStView.GuidCryptoCurrency == cryptoTransactiosBuyDetailsView.GuidCryptoCurrency);

                                    if (cryptoCurrencyStatementView != null)
                                    {
                                        decimal perc = cryptoCurrencyStatementView.PerformancePerc * 100;
                                        cryptoTransactionBuyCurrencyVM.Quantity = cryptoCurrencyStatementView.Quantity.ToString("0.#############################", new CultureInfo("pt-br"));
                                        cryptoTransactionBuyCurrencyVM.NetValue = string.Format("{0} {1}{2}", _fiatCurrencyService.GetCurrencySymbol((FiatCurrencyEnum)cryptoPortfolio.IdFiatCurrency), GetSignal(cryptoCurrencyStatementView.NetValue), cryptoCurrencyStatementView.NetValue.ToString("n2", new CultureInfo("pt-br")));
                                        cryptoTransactionBuyCurrencyVM.PerformancePerc = GetSignal(perc) + perc.ToString("n2", new CultureInfo("pt-br"));
                                        cryptoTransactionBuyCurrencyVM.TotalMarket = string.Format("{0} {1}", _fiatCurrencyService.GetCurrencySymbol((FiatCurrencyEnum)cryptoPortfolio.IdFiatCurrency), cryptoCurrencyStatementView.TotalMarket.ToString("n2", new CultureInfo("pt-br")));
                                    }
                                }

                                cryptoTransactionBuyCurrencyVM.Transactions = new List<CryptoTransactionBuyDetailsVM>();

                                cryptoTransactionBuyViewWrapperVM.CryptoTransactionBuyCurrency.Add(cryptoTransactionBuyCurrencyVM);

                                totalBuyCp = 0;
                            }

                            #endregion

                            cryptoTransactionBuyCurrencyVM.Transactions.Add(cryptoTransactionBuyDetailsVM);
                        }

                        cryptoTransactionBuyViewWrapperVM.TotalBuy = string.Format("{0} {1}", _fiatCurrencyService.GetCurrencySymbol((FiatCurrencyEnum)cryptoPortfolio.IdFiatCurrency), totalBuyGeneral.ToString("n2", new CultureInfo("pt-br")));
                    }
                }
            }

            if (resultServiceObject.ErrorMessages != null && resultServiceObject.ErrorMessages.Count() > 0)
            {
                resultServiceObject.Success = false;
            }

            resultServiceObject.Value = cryptoTransactionBuyViewWrapperVM;
            resultServiceObject.Success = true;

            return resultServiceObject;
        }

        public string GetSignal(decimal value)
        {
            string signal = string.Empty;

            if (value > 0)
            {
                signal = "+";
            }

            return signal;
        }



    }
}
