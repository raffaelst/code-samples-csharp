using Dividendos.API.Model.Request.Crypto;
using Dividendos.API.Model.Response.Common;
using Dividendos.API.Model.Response.Crypto;
using Dividendos.Entity.Entities;
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

namespace Dividendos.Service
{
    public class CryptoTransactionService : BaseService, ICryptoTransactionService
    {
        public CryptoTransactionService(IUnitOfWork uow)
        {
            _uow = uow;
        }

        public ResultServiceObject<CryptoTransaction> Insert(CryptoTransaction cryptoTransaction)
        {
            ResultServiceObject<CryptoTransaction> resultService = new ResultServiceObject<CryptoTransaction>();
            cryptoTransaction.GuidCryptoTransaction = Guid.NewGuid();
            cryptoTransaction.LastUpdatedDate = DateTime.Now;
            cryptoTransaction.Exchange = string.IsNullOrWhiteSpace(cryptoTransaction.Exchange) ? "ND" : cryptoTransaction.Exchange;

            if (cryptoTransaction.Quantity > 0)
            {
                cryptoTransaction.Active = true;
            }
            else
            {
                cryptoTransaction.Active = false;
            }

            cryptoTransaction.IdCryptoTransaction = _uow.CryptoTransactionRepository.Insert(cryptoTransaction);
            resultService.Value = cryptoTransaction;

            return resultService;
        }

        public ResultServiceObject<CryptoTransaction> Update(CryptoTransaction cryptoTransaction)
        {
            ResultServiceObject<CryptoTransaction> resultService = new ResultServiceObject<CryptoTransaction>();
            cryptoTransaction.LastUpdatedDate = DateTime.Now;

            resultService.Value = _uow.CryptoTransactionRepository.Update(cryptoTransaction);

            return resultService;
        }

        public ResultServiceObject<IEnumerable<CryptoTransaction>> GetByPortfolioTransactionType(long idCryptoPortfolio, int idTransactionType)
        {
            ResultServiceObject<IEnumerable<CryptoTransaction>> resultService = new ResultServiceObject<IEnumerable<CryptoTransaction>>();

            IEnumerable<CryptoTransaction> operations = _uow.CryptoTransactionRepository.Select(p => p.IdCryptoPortfolio == idCryptoPortfolio && p.TransactionType == idTransactionType && p.Active == true);

            resultService.Value = operations;

            return resultService;
        }

        public ResultServiceObject<CryptoTransaction> GetByCryptoPortfolioAndIdCryptoCurrency(long idCryptoPortfolio, long idCryptoCurrency, int transactionType)
        {
            ResultServiceObject<CryptoTransaction> resultService = new ResultServiceObject<CryptoTransaction>();

            IEnumerable<CryptoTransaction> transactions = _uow.CryptoTransactionRepository.Select(p => p.IdCryptoPortfolio == idCryptoPortfolio && p.IdCryptoCurrency == idCryptoCurrency && p.TransactionType == transactionType);

            resultService.Value = transactions.FirstOrDefault();

            return resultService;
        }

        public bool CalculateAveragePrice(ref List<CryptoTransactionItem> transactionItems, out decimal numberOfShares, out decimal avgPrice, bool removeNewItems = false, DateTime? lastSync = null, bool breakInvalid = true)
        {
            bool valid = true;
            numberOfShares = 0;
            decimal totalPrice = 0;
            avgPrice = 0;

            if (transactionItems != null && transactionItems.Count > 0)
            {
                if (transactionItems != null && transactionItems.Count > 0)
                {
                    if (transactionItems != null && transactionItems.Count > 0)
                    {
                        transactionItems = transactionItems.OrderBy(op => op.EventDate).ToList();

                        bool firstBuyOp = false;

                        for (int i = 0; i < transactionItems.Count; i++)
                        {
                            try
                            {
                                if (transactionItems[i].TransactionType == 1)
                                {
                                    firstBuyOp = true;
                                    numberOfShares += transactionItems[i].Quantity;
                                    totalPrice += transactionItems[i].AveragePrice * transactionItems[i].Quantity;
                                }
                                else if (transactionItems[i].TransactionType == 2)
                                {
                                    //If sell op. comes before buy op. (in case CEI truncates the operations (18 months))
                                    if (!firstBuyOp)
                                    {
                                        continue;
                                    }

                                    avgPrice = numberOfShares == 0 ? 0 : totalPrice / numberOfShares;
                                    numberOfShares -= transactionItems[i].Quantity;
                                    totalPrice -= transactionItems[i].Quantity * avgPrice;
                                    transactionItems[i].AcquisitionPrice = avgPrice;
                                }

                                if ((i == transactionItems.Count - 1) && (transactionItems[i].TransactionType == 1))
                                {
                                    avgPrice = numberOfShares == 0 ? 0 : totalPrice / numberOfShares;
                                }

                                if (numberOfShares < 0)
                                {
                                    if (breakInvalid)
                                    {
                                        valid = false;
                                        break;
                                    }
                                }
                                else if (numberOfShares == 0)
                                {
                                    avgPrice = 0;
                                    totalPrice = 0;
                                }
                            }
                            catch (OverflowException ex)
                            {
                                valid = false;
                                break;
                            }

                        }

                    }
                }

                if (numberOfShares < 0)
                {
                    valid = false;
                }

                if (avgPrice < 0)
                {
                    avgPrice = 0;
                }
            }

            return valid;
        }

        private static void OrderItems(List<CryptoTransactionItem> cryptoTransactionItems)
        {
            if (cryptoTransactionItems != null && cryptoTransactionItems.Count > 0)
            {
                int index = 0;
                for (int i = 0; i < cryptoTransactionItems.Count; i++)
                {
                    if (cryptoTransactionItems[i].EventDate.TimeOfDay == TimeSpan.Zero)
                    {
                        index++;
                        cryptoTransactionItems[i].EventDate = cryptoTransactionItems[i].EventDate.AddSeconds(index);
                    }
                }

                cryptoTransactionItems = cryptoTransactionItems.OrderBy(op => op.EventDate).ThenBy(op => op.TransactionType).ToList();
            }
        }

        public ResultResponseObject<CryptoBuyVM> BuyCrypto(Guid guidCryptoPortfolio, CryptoAddVM cryptoAddVM, ICryptoPortfolioService _cryptoPortfolioService, ICryptoTransactionService _cryptoTransactionService,
        ICryptoTransactionItemService _cryptoTransactionItem, ICryptoPortfolioPerformanceService _cryptoPortfolioPerformanceService, ICryptoCurrencyService _cryptoCurrencyService, ICryptoCurrencyPerformanceService _cryptoCurrencyPerformanceService, ISubscriptionService _subscriptionService,
        string idUser, bool priceAdjust = false, bool calculatePeformance = true, bool removeNewItems = false, DateTime? lastSync = null, bool breakInvalid = true, bool editedByUser = false)
        {
            ResultResponseObject<CryptoBuyVM> resultServiceCryptoBuyVM = new ResultResponseObject<CryptoBuyVM>();
            resultServiceCryptoBuyVM.Value = new CryptoBuyVM();
            int resultCount = 0;
            Subscription subscription = _subscriptionService.GetByUser(idUser).Value;

            if (subscription == null || !subscription.Active || subscription.ValidUntil < DateTime.Now)
            {
                resultCount = CountCryptoCurrencyByUser(idUser).Value;
            }

            if (resultCount >= 3)
            {
                resultServiceCryptoBuyVM.Success = true;
                resultServiceCryptoBuyVM.Value.CryptoAmountExceeded = true;
            }
            else
            {
                ResultServiceObject<CryptoPortfolio> resultCryptoPortfolio = _cryptoPortfolioService.GetByGuid(guidCryptoPortfolio);

                if (resultCryptoPortfolio.Success && resultCryptoPortfolio.Value != null && cryptoAddVM != null)
                {
                    ResultServiceObject<CryptoCurrency> resultCryptoCurrency = _cryptoCurrencyService.GetByGuid(cryptoAddVM.GuidCryptoCurrency);
                    decimal quantityInput = 0;
                    decimal.TryParse(cryptoAddVM.Quantity.Replace(".", string.Empty), NumberStyles.Currency, new CultureInfo("pt-br"), out quantityInput);

                    if (quantityInput > 0 && !string.IsNullOrWhiteSpace(cryptoAddVM.Price) && !string.IsNullOrWhiteSpace(cryptoAddVM.EventDate) && resultCryptoCurrency.Value != null)
                    {
                        CryptoPortfolio cryptoPortfolio = resultCryptoPortfolio.Value;
                        ResultServiceObject<CryptoTransaction> resultCryptoTransactionBuy = GetByCryptoPortfolioAndIdCryptoCurrency(cryptoPortfolio.IdCryptoPortfolio, resultCryptoCurrency.Value.CryptoCurrencyID, 1);
                        ResultServiceObject<CryptoTransaction> resultCryptoTransactionSell = GetByCryptoPortfolioAndIdCryptoCurrency(cryptoPortfolio.IdCryptoPortfolio, resultCryptoCurrency.Value.CryptoCurrencyID, 2);

                        List<CryptoTransactionItem> cryptoTransactionItems = new List<CryptoTransactionItem>();

                        if (resultCryptoTransactionBuy.Success && resultCryptoTransactionSell.Success)
                        {
                            CryptoTransaction cryptoTransactionBuy = resultCryptoTransactionBuy.Value;
                            decimal price = 0;
                            decimal.TryParse(cryptoAddVM.Price.Replace(".", string.Empty), NumberStyles.Currency, new CultureInfo("pt-br"), out price);

                            DateTime eventDate;
                            if (!DateTime.TryParseExact(cryptoAddVM.EventDate, "dd/MM/yyyy HH:mm", new CultureInfo("pt-br"), DateTimeStyles.None, out eventDate))
                            {
                                if (!DateTime.TryParseExact(cryptoAddVM.EventDate, "dd/MM/yyyy", new CultureInfo("pt-br"), DateTimeStyles.None, out eventDate))
                                {
                                    if (!DateTime.TryParseExact(cryptoAddVM.EventDate, "dd/MM/yy", new CultureInfo("pt-br"), DateTimeStyles.None, out eventDate))
                                    {
                                        resultServiceCryptoBuyVM.ErrorMessages.Add("A data da operação é inválida");
                                    }
                                }
                            }


                            CryptoTransactionItem cryptoTransactionItem = new CryptoTransactionItem();

                            if (resultServiceCryptoBuyVM.ErrorMessages.Count.Equals(0))
                            {
                                cryptoTransactionItem.AveragePrice = price;
                                if (eventDate.TimeOfDay == TimeSpan.Zero)
                                {
                                    cryptoTransactionItem.EventDate = eventDate.Add(DateTime.Now.TimeOfDay);
                                }
                                else
                                {
                                    cryptoTransactionItem.EventDate = eventDate;
                                }

                                cryptoTransactionItem.IdCryptoPortfolio = cryptoPortfolio.IdCryptoPortfolio;
                                cryptoTransactionItem.Exchange = string.IsNullOrWhiteSpace(cryptoAddVM.Exchange) ? "ND" : cryptoAddVM.Exchange;
                                cryptoTransactionItem.TransactionType = 1;
                                cryptoTransactionItem.IdCryptoCurrency = resultCryptoCurrency.Value.CryptoCurrencyID;
                                cryptoTransactionItem.Quantity = quantityInput;
                                cryptoTransactionItem.EditedByUser = editedByUser;
                            }

                            cryptoTransactionItems.Add(cryptoTransactionItem);

                            CryptoTransaction cryptoTransactionSell = resultCryptoTransactionSell.Value;

                            if (cryptoTransactionBuy != null)
                            {
                                ResultServiceObject<IEnumerable<CryptoTransactionItem>> resultCryptoTransactionItems = _cryptoTransactionItem.GetByIdCryptoTransaction(cryptoTransactionBuy.IdCryptoTransaction, 1);

                                if (resultCryptoTransactionItems.Success)
                                {
                                    cryptoTransactionItems.AddRange(resultCryptoTransactionItems.Value.ToList());
                                }
                            }
                            else
                            {
                                cryptoTransactionBuy = new CryptoTransaction();
                            }

                            if (cryptoTransactionSell != null)
                            {
                                ResultServiceObject<IEnumerable<CryptoTransactionItem>> resultCryptoTransactionItems = _cryptoTransactionItem.GetByIdCryptoTransaction(cryptoTransactionSell.IdCryptoTransaction, 2);

                                if (resultCryptoTransactionItems.Success)
                                {
                                    cryptoTransactionItems.AddRange(resultCryptoTransactionItems.Value.ToList());
                                }
                            }

                            OrderItems(cryptoTransactionItems);

                            decimal quantity, avgPrice;
                            CalculateAveragePrice(ref cryptoTransactionItems, out quantity, out avgPrice, removeNewItems, lastSync, breakInvalid);

                            cryptoTransactionBuy.AveragePrice = avgPrice;
                            cryptoTransactionBuy.Quantity = quantity;
                            cryptoTransactionBuy.Exchange = string.IsNullOrWhiteSpace(cryptoAddVM.Exchange) ? "ND" : cryptoAddVM.Exchange;
                            cryptoTransactionBuy.IdCryptoPortfolio = cryptoPortfolio.IdCryptoPortfolio;
                            cryptoTransactionBuy.IdCryptoCurrency = resultCryptoCurrency.Value.CryptoCurrencyID;
                            cryptoTransactionBuy.TransactionType = 1;

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

                            if (cryptoTransactionBuy.IdCryptoTransaction == 0)
                            {
                                cryptoTransactionBuy = Insert(cryptoTransactionBuy).Value;
                            }
                            else
                            {
                                Update(cryptoTransactionBuy);
                            }

                            cryptoTransactionItem.IdCryptoTransaction = cryptoTransactionBuy.IdCryptoTransaction;
                            _cryptoTransactionItem.Insert(cryptoTransactionItem);


                            if (calculatePeformance)
                            {
                                _cryptoPortfolioPerformanceService.CalculatePerformance(cryptoPortfolio.IdCryptoPortfolio, _cryptoPortfolioService, _cryptoCurrencyService, _cryptoTransactionService, _cryptoCurrencyPerformanceService);
                            }

                            resultServiceCryptoBuyVM.Success = true;

                        }
                    }
                }
            }

            return resultServiceCryptoBuyVM;
        }

        public void SellCrypto(Guid guidCryptoPortfolio, CryptoAddVM cryptoAddVM, ResultResponseBase resultResponseBase, ICryptoPortfolioService _cryptoPortfolioService, ICryptoTransactionService _cryptoTransactionService,
        ICryptoTransactionItemService _cryptoTransactionItem, ICryptoPortfolioPerformanceService _cryptoPortfolioPerformanceService, ICryptoCurrencyService _cryptoCurrencyService, ICryptoCurrencyPerformanceService _cryptoCurrencyPerformanceService, bool priceAdjust = false,
        bool calculatePeformance = true, bool removeNewItems = false, DateTime? lastSync = null, bool breakInvalid = true, bool editedByUser = false)
        {
            ResultServiceObject<CryptoPortfolio> resultCryptoPortfolio = _cryptoPortfolioService.GetByGuid(guidCryptoPortfolio);

            if (resultCryptoPortfolio.Success && resultCryptoPortfolio.Value != null && cryptoAddVM != null)
            {
                ResultServiceObject<CryptoCurrency> resultCryptoCurrency = _cryptoCurrencyService.GetByGuid(cryptoAddVM.GuidCryptoCurrency);
                decimal quantityInput = 0;
                decimal.TryParse(cryptoAddVM.Quantity.Replace(".", string.Empty), NumberStyles.Currency, new CultureInfo("pt-br"), out quantityInput);

                if (quantityInput > 0 && !string.IsNullOrWhiteSpace(cryptoAddVM.Price) && !string.IsNullOrWhiteSpace(cryptoAddVM.EventDate))
                {
                    CryptoPortfolio cryptoPortfolio = resultCryptoPortfolio.Value;
                    ResultServiceObject<CryptoTransaction> resultCryptoTransactionSell = GetByCryptoPortfolioAndIdCryptoCurrency(cryptoPortfolio.IdCryptoPortfolio, resultCryptoCurrency.Value.CryptoCurrencyID, 2);
                    ResultServiceObject<CryptoTransaction> resultCryptoTransactionBuy = GetByCryptoPortfolioAndIdCryptoCurrency(cryptoPortfolio.IdCryptoPortfolio, resultCryptoCurrency.Value.CryptoCurrencyID, 1);

                    List<CryptoTransactionItem> cryptoTransactionItems = new List<CryptoTransactionItem>();

                    if (resultCryptoTransactionSell.Success && resultCryptoTransactionBuy.Success)
                    {
                        CryptoTransaction cryptoTransactionSell = resultCryptoTransactionSell.Value;
                        CryptoTransaction cryptoTransactionBuy = resultCryptoTransactionBuy.Value;

                        decimal price = 0;
                        decimal.TryParse(cryptoAddVM.Price.Replace(".", string.Empty), NumberStyles.Currency, new CultureInfo("pt-br"), out price);

                        DateTime eventDate;

                        if (!DateTime.TryParseExact(cryptoAddVM.EventDate, "dd/MM/yyyy HH:mm", new CultureInfo("pt-br"), DateTimeStyles.None, out eventDate))
                        {
                            if (!DateTime.TryParseExact(cryptoAddVM.EventDate, "dd/MM/yyyy", new CultureInfo("pt-br"), DateTimeStyles.None, out eventDate))
                            {
                                if (!DateTime.TryParseExact(cryptoAddVM.EventDate, "dd/MM/yy", new CultureInfo("pt-br"), DateTimeStyles.None, out eventDate))
                                {
                                    resultResponseBase.ErrorMessages.Add("A data da operação é inválida");
                                }
                            }
                        }

                        CryptoTransactionItem cryptoTransactionItem = new CryptoTransactionItem();

                        if (resultResponseBase.ErrorMessages.Count.Equals(0))
                        {
                            cryptoTransactionItem.AveragePrice = price;
                            if (eventDate.TimeOfDay == TimeSpan.Zero)
                            {
                                cryptoTransactionItem.EventDate = eventDate.Add(DateTime.Now.TimeOfDay);
                            }
                            else
                            {
                                cryptoTransactionItem.EventDate = eventDate;
                            }

                            cryptoTransactionItem.IdCryptoPortfolio = cryptoPortfolio.IdCryptoPortfolio;
                            cryptoTransactionItem.Exchange = string.IsNullOrWhiteSpace(cryptoAddVM.Exchange) ? "ND" : cryptoAddVM.Exchange;
                            cryptoTransactionItem.TransactionType = 2;
                            cryptoTransactionItem.IdCryptoCurrency = resultCryptoCurrency.Value.CryptoCurrencyID;
                            cryptoTransactionItem.Quantity = quantityInput;
                            cryptoTransactionItem.EditedByUser = editedByUser;

                            decimal acquisitionPrice = 0;

                            if (!string.IsNullOrWhiteSpace(cryptoAddVM.AcquisitionPrice) && decimal.TryParse(cryptoAddVM.AcquisitionPrice.Replace(".", string.Empty), NumberStyles.Currency, new CultureInfo("pt-br"), out acquisitionPrice))
                            {
                                cryptoTransactionItem.AcquisitionPrice = acquisitionPrice;
                            }
                            else if (cryptoTransactionBuy != null)
                            {
                                cryptoTransactionItem.AcquisitionPrice = cryptoTransactionBuy.AveragePrice;
                            }
                        }

                        cryptoTransactionItems.Add(cryptoTransactionItem);

                        if (cryptoTransactionSell != null)
                        {
                            ResultServiceObject<IEnumerable<CryptoTransactionItem>> resultCryptoTransactionItems = _cryptoTransactionItem.GetByIdCryptoTransaction(cryptoTransactionSell.IdCryptoTransaction, 2);

                            if (resultCryptoTransactionItems.Success)
                            {
                                cryptoTransactionItems.AddRange(resultCryptoTransactionItems.Value.ToList());
                            }
                        }
                        else
                        {
                            cryptoTransactionSell = new CryptoTransaction();
                        }

                        if (cryptoTransactionBuy != null)
                        {
                            ResultServiceObject<IEnumerable<CryptoTransactionItem>> resultCryptoTransactionItems = _cryptoTransactionItem.GetByIdCryptoTransaction(cryptoTransactionBuy.IdCryptoTransaction, 1);

                            if (resultCryptoTransactionItems.Success)
                            {
                                cryptoTransactionItems.AddRange(resultCryptoTransactionItems.Value.ToList());
                            }
                        }
                        else
                        {
                            cryptoTransactionBuy = new CryptoTransaction();
                        }

                        OrderItems(cryptoTransactionItems);

                        decimal numberOfShares, avgPrice;
                        bool valid = CalculateAveragePrice(ref cryptoTransactionItems, out numberOfShares, out avgPrice, removeNewItems, lastSync, breakInvalid);

                        List<CryptoTransactionItem> cryptoTransactionBuyItems = cryptoTransactionItems.Where(op => op.TransactionType == 1).ToList();

                        DateTime firstEventDate = DateTime.MinValue;

                        if (cryptoTransactionBuyItems.Count() > 0)
                        {
                            firstEventDate = cryptoTransactionBuyItems.OrderBy(op => op.EventDate).First().EventDate;
                        }

                        decimal quantitySell = cryptoTransactionItems.Where(op => op.TransactionType == 2 && op.EventDate >= firstEventDate).Sum(opItemTmp => opItemTmp.Quantity);
                        decimal quantityBuy = cryptoTransactionBuyItems.Sum(opItemTmp => opItemTmp.Quantity);

                        if (quantitySell <= quantityBuy && valid)
                        {
                            if (resultResponseBase.ErrorMessages.Count.Equals(0))
                            {
                                decimal totalPriceSell = cryptoTransactionItems.Where(op => op.TransactionType == 2).Sum(opItemTmp => (opItemTmp.AveragePrice * opItemTmp.Quantity));

                                cryptoTransactionSell.AveragePrice = quantitySell == 0 ? 0 : totalPriceSell / quantitySell;
                                cryptoTransactionSell.Quantity = quantitySell;
                                cryptoTransactionSell.Exchange = string.IsNullOrWhiteSpace(cryptoAddVM.Exchange) ? "ND" : cryptoAddVM.Exchange;
                                cryptoTransactionSell.IdCryptoPortfolio = cryptoPortfolio.IdCryptoPortfolio;
                                cryptoTransactionSell.IdCryptoCurrency = resultCryptoCurrency.Value.CryptoCurrencyID;
                                cryptoTransactionSell.TransactionType = 2;

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

                                if (cryptoTransactionSell.IdCryptoTransaction == 0)
                                {
                                    cryptoTransactionSell = Insert(cryptoTransactionSell).Value;
                                }
                                else
                                {
                                    Update(cryptoTransactionSell);
                                }

                                cryptoTransactionItem.IdCryptoTransaction = cryptoTransactionSell.IdCryptoTransaction;
                                _cryptoTransactionItem.Insert(cryptoTransactionItem);

                                if (cryptoTransactionBuy != null)
                                {
                                    cryptoTransactionBuy.Quantity = numberOfShares;
                                    cryptoTransactionBuy.AveragePrice = avgPrice;

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

                                    Update(cryptoTransactionBuy);
                                }

                                if (calculatePeformance)
                                {
                                    _cryptoPortfolioPerformanceService.CalculatePerformance(cryptoPortfolio.IdCryptoPortfolio, _cryptoPortfolioService, _cryptoCurrencyService, _cryptoTransactionService, _cryptoCurrencyPerformanceService);
                                }

                                resultResponseBase.Success = true;
                            }
                        }
                        else
                        {
                            resultResponseBase.ErrorMessages.Add("Quantidade vendida informada é maior do que a quantidade que você possui para este ativo");
                        }
                    }
                }
            }
        }

        public ResultResponseBase EditBuyTransaction(Guid guidCryptoPortfolio, CryptoEditVM cryptoEditVM, ICryptoPortfolioService _cryptoPortfolioService, ICryptoTransactionItemService _cryptoTransactionItemService,
            ICryptoPortfolioPerformanceService _cryptoPortfolioPerformanceService, ICryptoCurrencyService _cryptoCurrencyService, ICryptoCurrencyPerformanceService _cryptoCurrencyPerformanceService, ICryptoTransactionService _cryptoTransactionService)
        {
            ResultResponseBase resultResponseBase = new ResultResponseBase();
            resultResponseBase.Success = false;

            ResultServiceObject<CryptoPortfolio> resultCryptoPortfolio = _cryptoPortfolioService.GetByGuid(guidCryptoPortfolio);
            List<CryptoTransactionItem> cryptoTransactionItems = new List<CryptoTransactionItem>();

            if (resultCryptoPortfolio.Success && resultCryptoPortfolio.Value != null && cryptoEditVM != null)
            {
                ResultServiceObject<CryptoCurrency> resultCryptoCurrency = _cryptoCurrencyService.GetByGuid(cryptoEditVM.GuidCryptoCurrency);
                decimal quantityInput = 0;
                decimal.TryParse(cryptoEditVM.Quantity.Replace(".", string.Empty), NumberStyles.Currency, new CultureInfo("pt-br"), out quantityInput);

                if (quantityInput > 0 && !string.IsNullOrWhiteSpace(cryptoEditVM.Price) && !string.IsNullOrWhiteSpace(cryptoEditVM.EventDate))
                {
                    CryptoPortfolio cryptoPortfolio = resultCryptoPortfolio.Value;
                    ResultServiceObject<CryptoTransaction> resultCryptoTransactionBuy = GetByCryptoPortfolioAndIdCryptoCurrency(cryptoPortfolio.IdCryptoPortfolio, resultCryptoCurrency.Value.CryptoCurrencyID, 1);
                    ResultServiceObject<CryptoTransaction> resultCryptoTransactionSell = GetByCryptoPortfolioAndIdCryptoCurrency(cryptoPortfolio.IdCryptoPortfolio, resultCryptoCurrency.Value.CryptoCurrencyID, 2);

                    if (resultCryptoTransactionBuy.Success && resultCryptoTransactionSell.Success)
                    {
                        CryptoTransaction cryptoTransactionBuy = resultCryptoTransactionBuy.Value;
                        CryptoTransaction cryptoTransactionSell = resultCryptoTransactionSell.Value;

                        if (cryptoTransactionBuy != null)
                        {
                            ResultServiceObject<IEnumerable<CryptoTransactionItem>> resultCryptoTransactionItems = _cryptoTransactionItemService.GetByIdCryptoTransaction(cryptoTransactionBuy.IdCryptoTransaction, 1);

                            if (resultCryptoTransactionItems.Success)
                            {
                                cryptoTransactionItems.AddRange(resultCryptoTransactionItems.Value.ToList());
                            }
                        }
                        else
                        {
                            cryptoTransactionBuy = new CryptoTransaction();
                        }

                        if (cryptoTransactionSell != null)
                        {
                            ResultServiceObject<IEnumerable<CryptoTransactionItem>> resultCryptoTransactionItems = _cryptoTransactionItemService.GetByIdCryptoTransaction(cryptoTransactionSell.IdCryptoTransaction, 2);

                            if (resultCryptoTransactionItems.Success)
                            {
                                cryptoTransactionItems.AddRange(resultCryptoTransactionItems.Value.ToList());
                            }
                        }

                        decimal quantityBuy = quantityInput;
                        decimal price = 0;
                        decimal.TryParse(cryptoEditVM.Price.Replace(".", string.Empty), NumberStyles.Currency, new CultureInfo("pt-br"), out price);

                        CryptoTransactionItem cryptoTransactionItemDb = null;

                        if (cryptoTransactionItems != null && cryptoTransactionItems.Count > 0)
                        {
                            cryptoTransactionItemDb = cryptoTransactionItems.FirstOrDefault(op => op.GuidCryptoTransactionItem == cryptoEditVM.GuidCryptoTransactionItem);

                            if (cryptoTransactionItemDb != null)
                            {
                                DateTime eventDate;
                                if (!DateTime.TryParseExact(cryptoEditVM.EventDate, "dd/MM/yyyy", new CultureInfo("pt-br"), DateTimeStyles.None, out eventDate))
                                {
                                    if (!DateTime.TryParseExact(cryptoEditVM.EventDate, "dd/MM/yy", new CultureInfo("pt-br"), DateTimeStyles.None, out eventDate))
                                    {
                                        resultResponseBase.ErrorMessages.Add("A data da operação é inválida");
                                    }
                                }

                                cryptoTransactionItemDb.Quantity = quantityBuy;
                                cryptoTransactionItemDb.AveragePrice = price;
                                cryptoTransactionItemDb.EventDate = eventDate.Add(DateTime.Now.TimeOfDay);
                                cryptoTransactionItemDb.Exchange = string.IsNullOrWhiteSpace(cryptoEditVM.Exchange) ? "ND" : cryptoEditVM.Exchange;
                            }

                            quantityBuy = cryptoTransactionItems.Where(op => op.TransactionType == 1).Sum(opItemTmp => opItemTmp.Quantity);
                        }

                        decimal quantityCalc = 0;
                        decimal avgPriceCalc = 0;
                        bool valid = CalculateAveragePrice(ref cryptoTransactionItems, out quantityCalc, out avgPriceCalc);

                        decimal quantitySell = 0;

                        if (cryptoTransactionSell != null)
                        {
                            quantitySell = cryptoTransactionSell.Quantity;
                        }

                        if (quantitySell <= quantityBuy && valid)
                        {
                            if (resultResponseBase.ErrorMessages.Count.Equals(0))
                            {
                                if (cryptoTransactionItemDb != null)
                                {
                                    _cryptoTransactionItemService.Update(cryptoTransactionItemDb);
                                }

                                cryptoTransactionBuy.AveragePrice = avgPriceCalc;
                                cryptoTransactionBuy.Quantity = quantityCalc;
                                cryptoTransactionBuy.Exchange = string.IsNullOrWhiteSpace(cryptoTransactionBuy.Exchange) ? "ND" : cryptoTransactionBuy.Exchange;
                                cryptoTransactionBuy.IdCryptoPortfolio = cryptoPortfolio.IdCryptoPortfolio;
                                cryptoTransactionBuy.IdCryptoCurrency = resultCryptoCurrency.Value.CryptoCurrencyID;
                                cryptoTransactionBuy.TransactionType = 1;

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

                                if (cryptoTransactionBuy.IdCryptoTransaction == 0)
                                {
                                    cryptoTransactionBuy = Insert(cryptoTransactionBuy).Value;
                                }
                                else
                                {
                                    Update(cryptoTransactionBuy);
                                }

                                _cryptoPortfolioPerformanceService.CalculatePerformance(cryptoPortfolio.IdCryptoPortfolio, _cryptoPortfolioService, _cryptoCurrencyService, _cryptoTransactionService, _cryptoCurrencyPerformanceService);

                                resultResponseBase.Success = true;
                            }
                        }
                        else
                        {
                            resultResponseBase.ErrorMessages.Add("Quantidade vendida informada é maior do que a quantidade que você possui para este ativo");
                        }
                    }
                }
            }

            return resultResponseBase;
        }

        public void EditSellTransaction(Guid guidCryptoPortfolio, CryptoEditVM cryptoEditVM, ResultResponseBase resultResponseBase, ICryptoPortfolioService _cryptoPortfolioService, ICryptoTransactionItemService _cryptoTransactionItemService,
            ICryptoPortfolioPerformanceService _cryptoPortfolioPerformanceService, ICryptoCurrencyService _cryptoCurrencyService, ICryptoCurrencyPerformanceService _cryptoCurrencyPerformanceService, ICryptoTransactionService _cryptoTransactionService)
        {
            ResultServiceObject<CryptoPortfolio> resultCryptoPortfolio = _cryptoPortfolioService.GetByGuid(guidCryptoPortfolio);

            if (resultCryptoPortfolio.Success && resultCryptoPortfolio.Value != null && cryptoEditVM != null)
            {
                ResultServiceObject<CryptoCurrency> resultCryptoCurrency = _cryptoCurrencyService.GetByGuid(cryptoEditVM.GuidCryptoCurrency);
                decimal quantityInput = 0;
                decimal.TryParse(cryptoEditVM.Quantity.Replace(".", string.Empty), NumberStyles.Currency, new CultureInfo("pt-br"), out quantityInput);

                if (quantityInput > 0 && !string.IsNullOrWhiteSpace(cryptoEditVM.Price) && !string.IsNullOrWhiteSpace(cryptoEditVM.EventDate))
                {

                    CryptoPortfolio cryptoPortfolio = resultCryptoPortfolio.Value;
                    ResultServiceObject<CryptoTransaction> resultCryptoTransactionSell = GetByCryptoPortfolioAndIdCryptoCurrency(cryptoPortfolio.IdCryptoPortfolio, resultCryptoCurrency.Value.CryptoCurrencyID, 2);
                    ResultServiceObject<CryptoTransaction> resultCryptoTransactionBuy = GetByCryptoPortfolioAndIdCryptoCurrency(cryptoPortfolio.IdCryptoPortfolio, resultCryptoCurrency.Value.CryptoCurrencyID, 1);
                    List<CryptoTransactionItem> cryptoTransactionItems = new List<CryptoTransactionItem>();

                    if (resultCryptoTransactionSell.Success && resultCryptoTransactionBuy.Success)
                    {
                        CryptoTransaction cryptoTransactionSell = resultCryptoTransactionSell.Value;
                        CryptoTransaction cryptoTransactionBuy = resultCryptoTransactionBuy.Value;

                        if (cryptoTransactionBuy != null)
                        {
                            if (cryptoTransactionSell != null)
                            {
                                ResultServiceObject<IEnumerable<CryptoTransactionItem>> resultCryptoTransactionSellItems = _cryptoTransactionItemService.GetByIdCryptoTransaction(cryptoTransactionSell.IdCryptoTransaction, 2);

                                if (resultCryptoTransactionSellItems.Success)
                                {
                                    cryptoTransactionItems.AddRange(resultCryptoTransactionSellItems.Value.ToList());
                                }
                            }
                            else
                            {
                                cryptoTransactionSell = new CryptoTransaction();
                            }

                            if (cryptoTransactionBuy != null)
                            {
                                ResultServiceObject<IEnumerable<CryptoTransactionItem>> resultCryptoTransactionBuyItems = _cryptoTransactionItemService.GetByIdCryptoTransaction(cryptoTransactionBuy.IdCryptoTransaction, 1);

                                if (resultCryptoTransactionBuyItems.Success)
                                {
                                    cryptoTransactionItems.AddRange(resultCryptoTransactionBuyItems.Value.ToList());
                                }
                            }

                            decimal quantitySell = quantityInput;
                            decimal price = 0;
                            decimal.TryParse(cryptoEditVM.Price.Replace(".", string.Empty), NumberStyles.Currency, new CultureInfo("pt-br"), out price);
                            decimal quantityBuy = 0;
                            decimal totalPrice = quantitySell * price;

                            CryptoTransactionItem cryptoTransactionItemDb = null;

                            if (cryptoTransactionItems != null && cryptoTransactionItems.Count > 0)
                            {
                                cryptoTransactionItemDb = cryptoTransactionItems.FirstOrDefault(op => op.GuidCryptoTransactionItem == cryptoEditVM.GuidCryptoTransactionItem);

                                if (cryptoTransactionItemDb != null)
                                {
                                    DateTime eventDate;

                                    if (!DateTime.TryParseExact(cryptoEditVM.EventDate, "dd/MM/yyyy", new CultureInfo("pt-br"), DateTimeStyles.None, out eventDate))
                                    {
                                        resultResponseBase.ErrorMessages.Add("A data da operação é inválida");
                                    }

                                    cryptoTransactionItemDb.Quantity = quantitySell;
                                    cryptoTransactionItemDb.AveragePrice = price;
                                    cryptoTransactionItemDb.EventDate = eventDate.Add(DateTime.Now.TimeOfDay);
                                    cryptoTransactionItemDb.Exchange = string.IsNullOrWhiteSpace(cryptoEditVM.Exchange) ? "ND" : cryptoEditVM.Exchange;
                                }

                                quantitySell = cryptoTransactionItems.Where(op => op.TransactionType == 2).Sum(opItemTmp => opItemTmp.Quantity);
                                quantityBuy = cryptoTransactionItems.Where(op => op.TransactionType == 1).Sum(opItemTmp => opItemTmp.Quantity);
                            }

                            decimal quantityCalc = 0;
                            decimal avgPriceCalc = 0;
                            bool valid = CalculateAveragePrice(ref cryptoTransactionItems, out quantityCalc, out avgPriceCalc);

                            if (quantitySell <= quantityBuy && valid)
                            {
                                if (resultResponseBase.ErrorMessages.Count.Equals(0))
                                {
                                    if (cryptoTransactionItemDb != null)
                                    {
                                        decimal acquisitionPrice = 0;

                                        if (!string.IsNullOrWhiteSpace(cryptoEditVM.AcquisitionPrice) && decimal.TryParse(cryptoEditVM.AcquisitionPrice.Replace(".", string.Empty), NumberStyles.Currency, new CultureInfo("pt-br"), out acquisitionPrice))
                                        {
                                            cryptoTransactionItemDb.AcquisitionPrice = acquisitionPrice;
                                        }
                                        else if (cryptoTransactionBuy != null)
                                        {
                                            cryptoTransactionItemDb.AcquisitionPrice = cryptoTransactionBuy.AveragePrice;
                                        }

                                        _cryptoTransactionItemService.Update(cryptoTransactionItemDb);
                                    }

                                    cryptoTransactionSell.AveragePrice = quantitySell == 0 ? 0 : totalPrice / quantitySell;
                                    cryptoTransactionSell.Quantity = quantitySell;
                                    cryptoTransactionSell.Exchange = string.IsNullOrWhiteSpace(cryptoTransactionSell.Exchange) ? "ND" : cryptoTransactionSell.Exchange;
                                    cryptoTransactionSell.IdCryptoPortfolio = cryptoPortfolio.IdCryptoPortfolio;
                                    cryptoTransactionSell.IdCryptoCurrency = resultCryptoCurrency.Value.CryptoCurrencyID;
                                    cryptoTransactionSell.TransactionType = 2;

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

                                    if (cryptoTransactionSell.IdCryptoTransaction == 0)
                                    {
                                        cryptoTransactionSell = Insert(cryptoTransactionSell).Value;
                                    }
                                    else
                                    {
                                        Update(cryptoTransactionSell);
                                    }

                                    if (cryptoTransactionBuy != null)
                                    {
                                        cryptoTransactionBuy.Quantity = quantityCalc;
                                        cryptoTransactionBuy.AveragePrice = avgPriceCalc;

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

                                        Update(cryptoTransactionBuy);
                                    }

                                    _cryptoPortfolioPerformanceService.CalculatePerformance(cryptoPortfolio.IdCryptoPortfolio, _cryptoPortfolioService, _cryptoCurrencyService, _cryptoTransactionService, _cryptoCurrencyPerformanceService);

                                    resultResponseBase.Success = true;
                                }
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

        public ResultServiceObject<IEnumerable<CryptoTransactionDetailsView>> GetDetailsByIdCryptoPortfolio(long idCryptoPortfolio, int transactionType, DateTime? startDate = null, DateTime? endDate = null)
        {
            ResultServiceObject<IEnumerable<CryptoTransactionDetailsView>> resultService = new ResultServiceObject<IEnumerable<CryptoTransactionDetailsView>>();

            IEnumerable<CryptoTransactionDetailsView> operations = _uow.CryptoTransactionViewRepository.GetDetailsByIdCryptoPortfolio(idCryptoPortfolio, transactionType, startDate, endDate);

            resultService.Value = operations;

            return resultService;
        }

        public ResultServiceObject<IEnumerable<CryptoTransactionDetailsView>> GetDetailsByIdCryptoSubPortfolio(long idCryptoPortfolio, long idCryptoSubPortfolio, int transactionType, DateTime? startDate = null, DateTime? endDate = null)
        {
            ResultServiceObject<IEnumerable<CryptoTransactionDetailsView>> resultService = new ResultServiceObject<IEnumerable<CryptoTransactionDetailsView>>();

            IEnumerable<CryptoTransactionDetailsView> operations = _uow.CryptoTransactionViewRepository.GetDetailsByIdCryptoSubportfolio(idCryptoPortfolio, idCryptoSubPortfolio, transactionType, startDate, endDate);

            resultService.Value = operations;

            return resultService;
        }

        public ResultServiceObject<CryptoTransaction> GetByGuid(Guid guidCryptoTransaction)
        {
            ResultServiceObject<CryptoTransaction> resultService = new ResultServiceObject<CryptoTransaction>();

            IEnumerable<CryptoTransaction> operations = _uow.CryptoTransactionRepository.Select(p => p.GuidCryptoTransaction == guidCryptoTransaction);

            resultService.Value = operations.FirstOrDefault();

            return resultService;
        }

        public ResultServiceObject<int> CountCryptoCurrencyByUser(string idUser)
        {
            ResultServiceObject<int> resultService = new ResultServiceObject<int>();

            int count = _uow.CryptoTransactionRepository.CountCryptoCurrencyByUser(idUser);

            resultService.Value = count;

            return resultService;
        }
    }
}
