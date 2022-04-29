using FluentValidation.Results;
using Dividendos.Entity.Entities;
using Dividendos.Entity.Model;
using Dividendos.Entity.Views;
using Dividendos.Repository.Interface;
using Dividendos.Repository.Interface.UoW;
using Dividendos.Service.Interface;
using Dividendos.Service.Validator.Common;
using System;
using System.Collections.Generic;
using System.Data.SqlTypes;
using System.Linq;
using System.Threading.Tasks;
using Dividendos.Entity.Enum;

namespace Dividendos.Service
{
    public class DividendService : BaseService, IDividendService
    {
        public DividendService(IUnitOfWork uow)
        {
            _uow = uow;
        }

        public ResultServiceObject<Dividend> GetById(long idDividend)
        {
            ResultServiceObject<Dividend> resultService = new ResultServiceObject<Dividend>();

            IEnumerable<Dividend> dividends = _uow.DividendRepository.Select(p => p.IdDividend == idDividend);

            resultService.Value = dividends.FirstOrDefault();

            return resultService;
        }

        public ResultServiceObject<IEnumerable<DividendView>> GetByPortfolio(long idPortfolio)
        {
            ResultServiceObject<IEnumerable<DividendView>> resultService = new ResultServiceObject<IEnumerable<DividendView>>();

            IEnumerable<DividendView> dividends = _uow.DividendViewRepository.GetByPortfolio(idPortfolio);

            resultService.Value = dividends;

            return resultService;
        }

        public ResultServiceObject<IEnumerable<DividendView>> GetAllAutomaticByPortfolio(long idPortfolio)
        {
            ResultServiceObject<IEnumerable<DividendView>> resultService = new ResultServiceObject<IEnumerable<DividendView>>();

            IEnumerable<DividendView> dividends = _uow.DividendViewRepository.GetByPortfolio(idPortfolio, true);

            resultService.Value = dividends;

            return resultService;
        }

        public ResultServiceObject<IEnumerable<DividendView>> GetAllActiveByPortfolio(long idPortfolio)
        {
            ResultServiceObject<IEnumerable<DividendView>> resultService = new ResultServiceObject<IEnumerable<DividendView>>();

            IEnumerable<DividendView> dividends = _uow.DividendViewRepository.GetByPortfolio(idPortfolio, null, true);

            resultService.Value = dividends;

            return resultService;
        }

        public ResultServiceObject<IEnumerable<DividendView>> GetAllActiveByPortfolioRangeDate(long idPortfolio, DateTime? startDate, DateTime? endDate)
        {
            ResultServiceObject<IEnumerable<DividendView>> resultService = new ResultServiceObject<IEnumerable<DividendView>>();

            IEnumerable<DividendView> dividends = _uow.DividendViewRepository.GetByPortfolio(idPortfolio, null, true, startDate, endDate);

            resultService.Value = dividends;

            return resultService;
        }

        public ResultServiceObject<IEnumerable<DividendView>> GetAllByDate(DateTime startDate, DateTime endDate)
        {
            ResultServiceObject<IEnumerable<DividendView>> resultService = new ResultServiceObject<IEnumerable<DividendView>>();

            IEnumerable<DividendView> dividends = _uow.DividendViewRepository.GetAllByDate(startDate, endDate);

            resultService.Value = dividends;

            return resultService;
        }

        public ResultServiceObject<IEnumerable<DividendView>> GetAllActiveBySubPortfolioRangeDate(long idSubPortfolio, DateTime? startDate, DateTime? endDate)
        {
            ResultServiceObject<IEnumerable<DividendView>> resultService = new ResultServiceObject<IEnumerable<DividendView>>();

            IEnumerable<DividendView> dividends = _uow.DividendViewRepository.GetAllActiveBySubPortfolio(idSubPortfolio, startDate, endDate);

            resultService.Value = dividends;

            return resultService;
        }

        public ResultServiceObject<IEnumerable<DividendView>> GetByNotificationStatusAndPaymentDate(bool morningNotification)
        {
            ResultServiceObject<IEnumerable<DividendView>> resultService = new ResultServiceObject<IEnumerable<DividendView>>();

            DateTime dateTime = DateTime.Now;

            if (!morningNotification)
            {
                dateTime = dateTime.AddDays(1);
            }

            IEnumerable<DividendView> dividends = _uow.DividendViewRepository.GetByNotificationStatusAndPaymentDate(false, dateTime);

            resultService.Value = dividends;

            return resultService;
        }

        public ResultServiceObject<Dividend> Update(Dividend dividend)
        {
            ResultServiceObject<Dividend> resultService = new ResultServiceObject<Dividend>();

            resultService.Value = _uow.DividendRepository.Update(dividend);

            return resultService;
        }

        public ResultServiceObject<Dividend> Insert(Dividend dividend)
        {
            ResultServiceObject<Dividend> resultService = new ResultServiceObject<Dividend>();
            dividend.GuidDividend = Guid.NewGuid();

            if (dividend.DateAdded == DateTime.MinValue)
            {
                dividend.DateAdded = DateTime.Now;
            }

            if (!dividend.PaymentDate.HasValue || dividend.PaymentDate.Value <DateTime.Now.Date)
            {
                dividend.NotificationSent = true;
            }

            if (dividend.PaymentDate.HasValue && dividend.PaymentDate.Value <= DateTime.MinValue)
            {
                dividend.PaymentDate = null;
            }

            dividend.Active = true;
            dividend.IdDividend = _uow.DividendRepository.Insert(dividend);
            resultService.Value = dividend;

            return resultService;
        }

        public ResultServiceObject<IEnumerable<DividendDetailsView>> GetDetailsByPortfolio(long idPortfolio, DateTime? startDate = null, DateTime? endDate = null, long? idStock = null, int? idStockType = null)
        {
            ResultServiceObject<IEnumerable<DividendDetailsView>> resultService = new ResultServiceObject<IEnumerable<DividendDetailsView>>();

            IEnumerable<DividendDetailsView> dividends = _uow.DividendViewRepository.GetDetailsByPortfolio(idPortfolio, startDate, endDate, idStock, idStockType);

            resultService.Value = dividends;

            return resultService;
        }

        public ResultServiceObject<IEnumerable<DividendDetailsView>> GetDetailsBySubportfolio(long idSubPortfolio, DateTime? startDate = null, DateTime? endDate = null, long? idStock = null, int? idStockType = null)
        {
            ResultServiceObject<IEnumerable<DividendDetailsView>> resultService = new ResultServiceObject<IEnumerable<DividendDetailsView>>();

            IEnumerable<DividendDetailsView> dividends = _uow.DividendViewRepository.GetDetailsBySubportfolio(idSubPortfolio, startDate, endDate);

            resultService.Value = dividends;

            return resultService;
        }

        public ResultServiceObject<IEnumerable<DividendView>> GetNextDividendByUser(string user, int amountToReturn)
        {
            ResultServiceObject<IEnumerable<DividendView>> resultService = new ResultServiceObject<IEnumerable<DividendView>>();

            IEnumerable<DividendView> dividends = _uow.DividendViewRepository.GetNextDividendByUser(user, DateTime.Now, amountToReturn);

            foreach (var itemDividendView in dividends)
            {
                itemDividendView.DividendType = Abreviation(itemDividendView.IdDividendType);
            }

            resultService.Value = dividends;

            return resultService;
        }


        public ResultServiceObject<IEnumerable<DividendInfoView>> GetManualPortfoliosWithStock(long idStock)
        {
            ResultServiceObject<IEnumerable<DividendInfoView>> resultService = new ResultServiceObject<IEnumerable<DividendInfoView>>();

            IEnumerable<DividendInfoView> idsPortfolios = _uow.DividendInfoViewRepository.GetManualPortfoliosWithStock(idStock);

            resultService.Value = idsPortfolios;

            return resultService;
        }

        public ResultServiceObject<IEnumerable<string>> GetAllUsersWithStock(long idStock)
        {
            ResultServiceObject<IEnumerable<string>> resultService = new ResultServiceObject<IEnumerable<string>>();

            IEnumerable<string> idsPortfolios = _uow.DividendInfoViewRepository.GetAllUsersWithStock(idStock);

            resultService.Value = idsPortfolios;

            return resultService;
        }

        public ResultServiceObject<bool> ExistInDataBase(Dividend dividend)
        {
            ResultServiceObject<bool> resultService = new ResultServiceObject<bool>();

            bool exist = _uow.DividendViewRepository.ExistInDataBase(dividend);

            resultService.Value = exist;

            return resultService;
        }


        public string Abreviation(int idDividendType)
        {
            string abreviateValue = string.Empty;

            switch (idDividendType)
            {
                case (int)DividendTypeEnum.Dividend:
                    {
                        abreviateValue = "Div.";
                        break;
                    }
                case (int)DividendTypeEnum.JCP:
                    {
                        abreviateValue = "JCP";
                        break;
                    }
                case (int)DividendTypeEnum.Rendimento:
                    {
                        abreviateValue = "Rend.";
                        break;
                    }
                case (int)DividendTypeEnum.LeilaoFracoes:
                    {
                        abreviateValue = "Leilão";
                        break;
                    }
                case (int)DividendTypeEnum.RestituicoesDeCapital:
                    {
                        abreviateValue = "Rest.";
                        break;
                    }
                case (int)DividendTypeEnum.Amortizacao:
                    {
                        abreviateValue = "Amort.";
                        break;
                    }
                default:
                    break;
            }

            return abreviateValue;
        }

        public ResultServiceObject<IEnumerable<Dividend>> GetByFutureDate(long idPortfolio)
        {
            ResultServiceObject<IEnumerable<Dividend>> resultService = new ResultServiceObject<IEnumerable<Dividend>>();
            DateTime dtFuture = DateTime.Now.AddDays(2);

            resultService.Value = _uow.DividendRepository.Select(p => p.IdPortfolio == idPortfolio && p.PaymentDate >= dtFuture && p.Active == true && p.AutomaticImport == true);

            return resultService;
        }

        public ResultServiceObject<bool> Delete(Dividend dividend)
        {
            ResultServiceObject<bool> resultService = new ResultServiceObject<bool>();
            resultService.Value = _uow.DividendRepository.Delete(dividend);

            return resultService;
        }

        public List<string> RestorePastDividends(long idPortfolio, int idCountry, IOperationItemService _operationItemService, IDividendCalendarService _dividendCalendarService, IOperationService _operationService, IStockService _stockService, IPerformanceStockService _performanceStockService, IPortfolioService _portfolioService, DateTime? minDataCom = null, bool? cei = null, long? idOldPortfolio = null, bool? onlyFiis = null)
        {
            int? idStockType = null;

            if (onlyFiis.HasValue && onlyFiis.Value)
            {
                idStockType = 2;
            }

            Portfolio portfolio = _portfolioService.GetById(idPortfolio).Value;
            DateTime yesterday = DateTime.Now.AddDays(-1).Date;
            List<string> changedDivs = new List<string>();
            ResultServiceObject<IEnumerable<DividendView>> resultDividend = GetAllActiveByPortfolio(idPortfolio);
            ResultServiceObject<IEnumerable<StockDivRangeView>> resultServiceStockRange = _operationItemService.GetStockDivRangeView(idPortfolio, idStockType);

            if (resultServiceStockRange.Value != null && resultServiceStockRange.Value.Count() > 0)
            {
                foreach (StockDivRangeView stockDivRangeView in resultServiceStockRange.Value)
                {
                    DateTime minDateCalendar = stockDivRangeView.MinDate;

                    if (minDataCom.HasValue)
                    {
                        minDateCalendar = minDataCom.Value.Date.AddDays(-10);
                    }

                    if (minDateCalendar > yesterday)
                    {
                        minDateCalendar = yesterday;
                    }

                    ResultServiceObject<IEnumerable<DividendCalendar>> resultDivCalendar = _dividendCalendarService.GetDividendRangeDate(stockDivRangeView.IdStock, minDateCalendar, yesterday);

                    if (resultDivCalendar.Value != null && resultDivCalendar.Value.Count() > 0)
                    {
                        resultDivCalendar.Value = resultDivCalendar.Value.Where(div => div.PaymentDate.HasValue).ToList();

                        if (resultDivCalendar.Value != null && resultDivCalendar.Value.Count() > 0)
                        {
                            foreach (DividendCalendar divCalendar in resultDivCalendar.Value)
                            {
                                Stock stock = _stockService.GetById(divCalendar.IdStock).Value;

                                decimal amount = 0;

                                if (cei.HasValue && cei.Value)
                                {
                                    decimal amountSum = _operationService.GetTotalAmount(idPortfolio, divCalendar.DataCom, stockDivRangeView.IdStock);
                                    amount = _performanceStockService.GetTotalAmount(idPortfolio, divCalendar.DataCom, stockDivRangeView.IdStock);

                                    if (amount == 0 && idOldPortfolio.HasValue)
                                    {
                                        amount = _operationService.GetTotalAmount(idOldPortfolio.Value, divCalendar.DataCom, stockDivRangeView.IdStock);
                                    }

                                    if ((amountSum != amount) && DateTime.Now.Date.Subtract(divCalendar.DataCom).TotalDays <= 30)
                                    {
                                        break;
                                    }
                                    else if (amount == 0 && amountSum > 0)
                                    {
                                        amount = amountSum;
                                    }
                                }
                                else
                                {
                                    amount = _operationService.GetTotalAmount(idPortfolio, divCalendar.DataCom, stockDivRangeView.IdStock);
                                }

                                if (amount > 0)
                                {
                                    DividendView dividendView = null;

                                    if (resultDividend.Value != null && resultDividend.Value.Count() > 0)
                                    {
                                        resultDividend.Value = resultDividend.Value.Where(div => div.PaymentDate.HasValue).ToList();

                                        if (resultDividend.Value != null && resultDividend.Value.Count() > 0)
                                        {
                                            if (portfolio.ManualPortfolio && stock.IdStockType == 2)
                                            {
                                                dividendView = resultDividend.Value.FirstOrDefault(div => div.IdStock == stockDivRangeView.IdStock && div.PaymentDate.Value.Date == divCalendar.PaymentDate.Value.Date);
                                            }
                                            else
                                            {
                                                dividendView = resultDividend.Value.FirstOrDefault(div => div.IdStock == stockDivRangeView.IdStock && div.IdDividendType == divCalendar.IdDividendType && div.PaymentDate.Value.Date == divCalendar.PaymentDate.Value.Date);
                                            }
                                        }
                                    }

                                    if (dividendView == null)
                                    {
                                        Dividend dividend = new Dividend();
                                        dividend.Active = true;
                                        dividend.AutomaticImport = true;
                                        dividend.BaseQuantity = (int)amount;

                                        decimal netValue = 0;
                                        decimal grossValue = 0;

                                        if (divCalendar.IdCountry.Equals((int)CountryEnum.EUA))
                                        {
                                            grossValue = divCalendar.Value * amount;
                                            netValue = grossValue * ((decimal)0.70);
                                        }
                                        else
                                        {
                                            if (divCalendar.IdDividendType.Equals((int)DividendTypeEnum.JCP))
                                            {
                                                grossValue = divCalendar.Value * amount;
                                                netValue = grossValue * ((decimal)0.85);
                                            }
                                            else
                                            {
                                                grossValue = divCalendar.Value * amount;
                                                netValue = grossValue;
                                            }
                                        }

                                        dividend.GrossValue = grossValue;
                                        dividend.NetValue = netValue;
                                        dividend.HomeBroker = "Sistema";
                                        dividend.IdDividendType = divCalendar.IdDividendType;
                                        dividend.IdPortfolio = idPortfolio;
                                        dividend.PaymentDate = divCalendar.PaymentDate;
                                        dividend.IdStock = divCalendar.IdStock;

                                        dividend.NetValue = Math.Round(dividend.NetValue, 2);

                                        if (dividend.NetValue >= (decimal)0.01)
                                        {
                                            Insert(dividend);
                                            changedDivs.Add(stock.Symbol);
                                        }
                                    }
                                }
                            }
                        }
                    }
                }
            }

            if (changedDivs.Count > 0)
            {
                changedDivs = changedDivs.Distinct().ToList();
            }

            return changedDivs;
        }

        public ResultServiceObject<IEnumerable<Dividend>> GetAutomaticByIdPortfolio(long idPortfolio)
        {
            ResultServiceObject<IEnumerable<Dividend>> resultService = new ResultServiceObject<IEnumerable<Dividend>>();

            IEnumerable<Dividend> dividends = _uow.DividendRepository.Select(p => p.IdPortfolio == idPortfolio && p.AutomaticImport == true);

            resultService.Value = dividends;

            return resultService;
        }

        public ResultServiceObject<IEnumerable<Dividend>> GetActiveByPortfolio(long idPortfolio)
        {
            ResultServiceObject<IEnumerable<Dividend>> resultService = new ResultServiceObject<IEnumerable<Dividend>>();

            IEnumerable<Dividend> dividends = _uow.DividendRepository.Select(p => p.IdPortfolio == idPortfolio && p.Active == true);

            resultService.Value = dividends;

            return resultService;
        }

        public void RemoveDuplicated(long idPortfolio, IDividendCalendarService _dividendCalendarService)
        {
            try
            {
                ResultServiceObject<IEnumerable<Dividend>> resultServiceDiv = GetActiveByPortfolio(idPortfolio);

                if (resultServiceDiv.Value != null && resultServiceDiv.Value.Count() > 0)
                {
                    List<Dividend> dividends = resultServiceDiv.Value.Where(div => div.PaymentDate.HasValue).ToList();

                    if (dividends != null && dividends.Count() > 0)
                    {
                        var dividendsGp = dividends.GroupBy(div => new { div.IdStock, div.PaymentDate, div.IdDividendType }).Select(div => new { Dividend = div.First(), Count = div.Count() }).ToList();

                        if (dividendsGp != null && dividendsGp.Count() > 0)
                        {
                            dividendsGp = dividendsGp.Where(div => div.Count > 1).ToList();

                            if (dividendsGp != null && dividendsGp.Count() > 0)
                            {
                                foreach (var dividendGp in dividendsGp)
                                {
                                    List<Dividend> dividendsSystem = dividends.Where(div => div.HomeBroker == "Sistema" && div.IdStock == dividendGp.Dividend.IdStock && div.PaymentDate == dividendGp.Dividend.PaymentDate && div.IdDividendType == dividendGp.Dividend.IdDividendType).ToList();

                                    if (dividendsSystem != null && dividendsSystem.Count() > 0)
                                    {
                                        if (dividendGp.Count > dividendsSystem.Count())
                                        {
                                            foreach (Dividend dividend in dividendsSystem)
                                            {
                                                Delete(dividend);
                                            }
                                        }
                                        else if (dividendGp.Count == dividendsSystem.Count())
                                        {
                                            bool delete = true;
                                            ResultServiceObject<IEnumerable<DividendCalendar>> resultServiceCal = _dividendCalendarService.GetByPaymentDateAndStock(dividendsSystem.First().PaymentDate.Value, dividendsSystem.First().IdStock);

                                            if (resultServiceCal.Value != null && resultServiceCal.Value.Count() == dividendsSystem.Count())
                                            {
                                                delete = false;
                                            }

                                            if (delete)
                                            {
                                                for (int i = 0; i < dividendsSystem.Count() - 1; i++)
                                                {
                                                    Delete(dividendsSystem[i]);
                                                }
                                            }
                                        }
                                    }
                                }
                            }
                        }
                    }
                }
            }
            catch (Exception)
            {
            }
        }

        public void RemoveDuplicatedEasyInvest(long idPortfolio, IDividendCalendarService _dividendCalendarService)
        {
            try
            {
                ResultServiceObject<IEnumerable<Dividend>> resultServiceDiv = GetActiveByPortfolio(idPortfolio);

                if (resultServiceDiv.Value != null && resultServiceDiv.Value.Count() > 0)
                {
                    List<Dividend> dividends = resultServiceDiv.Value.Where(div => div.PaymentDate.HasValue).ToList();

                    if (dividends != null && dividends.Count() > 0)
                    {
                        var dividendsGp = dividends.GroupBy(div => new { div.IdStock, div.PaymentDate, div.IdDividendType }).Select(div => new { Dividend = div.First(), Count = div.Count() }).ToList();

                        if (dividendsGp != null && dividendsGp.Count() > 0)
                        {
                            dividendsGp = dividendsGp.Where(div => div.Count > 1).ToList();

                            if (dividendsGp != null && dividendsGp.Count() > 0)
                            {
                                foreach (var dividendGp in dividendsGp)
                                {
                                    List<Dividend> dividendsSystem = dividends.Where(div => div.HomeBroker == " EASYNVEST - TITULO CV S.A." && div.NetValue == dividendGp.Dividend.NetValue && div.IdStock == dividendGp.Dividend.IdStock && div.PaymentDate == dividendGp.Dividend.PaymentDate && div.IdDividendType == dividendGp.Dividend.IdDividendType).ToList();

                                    if (dividendsSystem != null && dividendsSystem.Count() > 0)
                                    {
                                        if (dividendGp.Count > dividendsSystem.Count())
                                        {
                                            foreach (Dividend dividend in dividendsSystem)
                                            {
                                                Delete(dividend);
                                            }
                                        }
                                        else if (dividendGp.Count == dividendsSystem.Count())
                                        {
                                            bool delete = true;
                                            ResultServiceObject<IEnumerable<DividendCalendar>> resultServiceCal = _dividendCalendarService.GetByPaymentDateAndStock(dividendsSystem.First().PaymentDate.Value, dividendsSystem.First().IdStock);

                                            if (resultServiceCal.Value != null && resultServiceCal.Value.Count() == dividendsSystem.Count())
                                            {
                                                delete = false;
                                            }

                                            if (delete)
                                            {
                                                for (int i = 0; i < dividendsSystem.Count() - 1; i++)
                                                {
                                                    Delete(dividendsSystem[i]);
                                                }
                                            }
                                        }
                                    }
                                }
                            }
                        }
                    }
                }
            }
            catch (Exception)
            {
            }
        }

        public ResultServiceObject<IEnumerable<Dividend>> GetByIdPortfolio(long idPortfolio)
        {
            ResultServiceObject<IEnumerable<Dividend>> resultService = new ResultServiceObject<IEnumerable<Dividend>>();

            IEnumerable<Dividend> dividends = _uow.DividendRepository.Select(p => p.IdPortfolio == idPortfolio);

            resultService.Value = dividends;

            return resultService;
        }

        public ResultServiceObject<IEnumerable<Dividend>> GetByIdPortfolioActive(long idPortfolio)
        {
            ResultServiceObject<IEnumerable<Dividend>> resultService = new ResultServiceObject<IEnumerable<Dividend>>();

            IEnumerable<Dividend> dividends = _uow.DividendRepository.Select(p => p.IdPortfolio == idPortfolio && p.Active == true);

            resultService.Value = dividends;

            return resultService;
        }
    }
}
