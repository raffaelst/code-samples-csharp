using AutoMapper;
using K.Logger;
using Dividendos.API.Model.Response;
using Dividendos.API.Model.Response.Common;

using Dividendos.Application.Base;
using Dividendos.Application.Interface;
using Dividendos.Entity.Entities;
using Dividendos.Entity.Model;
using Dividendos.Finance.Interface;
using Dividendos.Finance.Interface.Model;
using Dividendos.Repository.Interface.UoW;
using Dividendos.Service.Interface;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;
using Dividendos.Application.Interface.Model;
using Dividendos.Entity.Views;
using Dividendos.TradeMap.Interface;
using System.IO;
using Dividendos.Entity.Enum;
using Dividendos.TradeMap.Interface.Model;
using Dividendos.API.Model.Response.v1.CompanyIndicator;

namespace Dividendos.Application
{
    public class CompanyIndicatorsApp : BaseApp, ICompanyIndicatorsApp
    {
        private readonly IMapper _mapper;
        private readonly IUnitOfWork _uow;
        private readonly ICompanyService _companyService;
        private readonly ITradeMapHelper _iTradeMapHelper;
        private readonly ILogoService _logoService;
        private readonly ISystemSettingsService _systemSettingsService;
        private readonly IStockService _stockService;
        private readonly ICompanyIndicatorsService _companyIndicatorsService;
        private readonly IGlobalAuthenticationService _globalAuthenticationService;

        public CompanyIndicatorsApp(IMapper mapper,
            ICompanyService companyService,
            IUnitOfWork uow,
            ITradeMapHelper iTradeMapHelper,
            ILogoService logoService,
            ISystemSettingsService systemSettingsService,
            IStockService stockService,
            ICompanyIndicatorsService companyIndicatorsService,
            IGlobalAuthenticationService globalAuthenticationService)
        {
            _mapper = mapper;
            _uow = uow;
            _companyService = companyService;
            _iTradeMapHelper = iTradeMapHelper;
            _logoService = logoService;
            _systemSettingsService = systemSettingsService;
            _stockService = stockService;
            _companyIndicatorsService = companyIndicatorsService;
            _globalAuthenticationService = globalAuthenticationService;
        }

        public void ImportCompanyIndicators()
        {
            List<Entity.Entities.Stock> stocks = new List<Entity.Entities.Stock>();
            using (_uow.Create())
            {
                ResultServiceObject<IEnumerable<Entity.Entities.Stock>> resultStock = _stockService.GetAll();

                if (resultStock.Value != null && resultStock.Value.Count() > 0)
                {
                    stocks = resultStock.Value.ToList();
                }
            }

            if (stocks != null && stocks.Count > 0)
            {
                foreach (Entity.Entities.Stock stock in stocks)
                {
                    ImportCompanyIndicators(stock);
                }
            }
        }

        public void ImportCompanyIndicators(Entity.Entities.Stock stock)
        {
            try
            {

                string tradeMapCookie = string.Empty;
                Company company = null;

                using (_uow.Create())
                {
                    ResultServiceObject<Entity.Entities.SystemSettings> resultSettingsCookie = _systemSettingsService.GetByKey(Constants.SYSTEM_SETTINGS_TRADE_MAP_COOKIE);

                    if (resultSettingsCookie.Success && resultSettingsCookie.Value != null)
                    {
                        tradeMapCookie = resultSettingsCookie.Value.SettingValue;
                    }

                    company = _companyService.GetById(stock.IdCompany).Value;
                }

                if (company != null)
                {

                    CompanyIndicators companyIndicators = null;

                    if (stock.IdStockType == 2)
                    {
                        CompanyHistoricTd companyHistoricTd = _iTradeMapHelper.ImportCompanyHistoricCorporate(tradeMapCookie, stock.Symbol);
                        CompanyIndicatorFiisTd companyIndicatorTd = _iTradeMapHelper.ImportCompanyIndicatorsFiis(tradeMapCookie, stock.Symbol);
                        companyIndicators = ConvertIndicatorFiis(companyIndicatorTd, companyHistoricTd, companyIndicators);
                    }
                    else if (stock.IdStockType == 1 && stock.IdCountry == 1)
                    {
                        CompanyHistoricTd companyHistoricTd = _iTradeMapHelper.ImportCompanyHistoricCorporate(tradeMapCookie, stock.Symbol);
                        CompanyIndicatorTd companyIndicatorTd = _iTradeMapHelper.ImportCompanyIndicators(tradeMapCookie, stock.Symbol);
                        companyIndicators = ConvertIndicatorBr(companyIndicatorTd, companyHistoricTd, companyIndicators);
                    }
                    else if (stock.IdCountry == 2 || stock.IdStockType == 4)
                    {
                        string symbolFormat = stock.Symbol;

                        if (stock.IdStockType == 4)
                        {
                            symbolFormat = stock.Symbol.Substring(0, stock.Symbol.Length - 2);
                        }

                        CompanyHistoricTd companyHistoricTd = _iTradeMapHelper.ImportCompanyHistoricCorporate(tradeMapCookie, stock.Symbol);
                        CompanyIndicatorUsTd companyIndicatorUsTd = _iTradeMapHelper.ImportCompanyIndicatorsUs(tradeMapCookie, symbolFormat);
                        companyIndicators = ConvertIndicatorUs(companyIndicatorUsTd, companyHistoricTd, companyIndicators);
                    }

                    if (companyIndicators != null)
                    {
                        using (_uow.Create())
                        {
                            if (company.IdCompanyIndicators.HasValue)
                            {
                                companyIndicators.CompanyCode = company.Code;
                                companyIndicators.IdCompanyIndicators = company.IdCompanyIndicators.Value;
                                _companyIndicatorsService.Update(companyIndicators);
                            }
                            else
                            {
                                companyIndicators.CompanyCode = company.Code;
                                companyIndicators = _companyIndicatorsService.Insert(companyIndicators).Value;

                                company.IdCompanyIndicators = companyIndicators.IdCompanyIndicators;
                                _companyService.Update(company);
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                string test = "po";
            }
        }

        private static CompanyIndicators ConvertIndicatorBr(CompanyIndicatorTd companyIndicatorTd, CompanyHistoricTd companyHistoricTd, CompanyIndicators companyIndicators)
        {
            if (companyIndicatorTd != null && companyIndicatorTd.Result != null && companyIndicatorTd.Result.Length > 0)
            {
                companyIndicators = new CompanyIndicators();

                if (companyIndicatorTd.Result.First().IndicesLast != null)
                {
                    companyIndicators.NetWorth = ConvertValue(companyIndicatorTd.Result.First().IndicesLast.NetWorth);
                    companyIndicators.PricePerVpa = ConvertValue(companyIndicatorTd.Result.First().IndicesLast.PricePerVpa);
                    companyIndicators.MarketCap = ConvertValue(companyIndicatorTd.Result.First().IndicesLast.MarketCap);
                }

                if (companyIndicatorTd.Result.First().Indices != null)
                {
                    companyIndicators.TotalAssets = ConvertValue(companyIndicatorTd.Result.First().Indices.TotalAssets);
                    companyIndicators.NetDebt = ConvertValue(companyIndicatorTd.Result.First().Indices.DividaLiquida);
                    companyIndicators.PayoutAnnual = ConvertValue(companyIndicatorTd.Result.First().Indices.PayoutAnnual);
                    companyIndicators.QttyStock = ConvertValue(companyIndicatorTd.Result.First().Indices.QttyStock);
                    companyIndicators.RoaAnnual = ConvertValue(companyIndicatorTd.Result.First().Indices.RoaAnnual);
                    companyIndicators.RoeAnnual = ConvertValue(companyIndicatorTd.Result.First().Indices.RoeAnnual);
                    companyIndicators.RoicAnnual = ConvertValue(companyIndicatorTd.Result.First().Indices.RoicAnnual);
                    companyIndicators.NetProfitAnnual = ConvertValue(companyIndicatorTd.Result.First().Indices.VlProfitAnnual);
                    companyIndicators.PricePerProfit = ConvertValue(companyIndicatorTd.Result.First().Indices.PriceProfitAnnual);
                    companyIndicators.ReferenceDate = DateTime.ParseExact(companyIndicatorTd.Result.First().Indices.DtEntry, "yyyyMMdd", CultureInfo.InvariantCulture);
                }

                if (companyHistoricTd != null && companyHistoricTd.CompanyDividendTd != null)
                {
                    companyIndicators.Dividend12Months = ConvertValue(companyHistoricTd.CompanyDividendTd.DividentUm);
                    companyIndicators.Dividend12MonthsYield = ConvertValue(companyHistoricTd.CompanyDividendTd.DividentYieldUm);
                    companyIndicators.Dividend24Months = ConvertValue(companyHistoricTd.CompanyDividendTd.DividentDois);
                    companyIndicators.Dividend24MonthsYield = ConvertValue(companyHistoricTd.CompanyDividendTd.DividentYieldDois);
                    companyIndicators.Dividend36Months = ConvertValue(companyHistoricTd.CompanyDividendTd.DividentTres);
                    companyIndicators.Dividend36MonthsYield = ConvertValue(companyHistoricTd.CompanyDividendTd.DividentYieldTres);
                }
            }

            return companyIndicators;
        }

        private static CompanyIndicators ConvertIndicatorUs(CompanyIndicatorUsTd companyIndicatorUsTd, CompanyHistoricTd companyHistoricTd, CompanyIndicators companyIndicators)
        {
            if (companyIndicatorUsTd != null && companyIndicatorUsTd.Result != null && companyIndicatorUsTd.Result.Count() > 0)
            {
                companyIndicators = new CompanyIndicators();

                if (companyIndicatorUsTd.Result.First().Finance != null)
                {
                    companyIndicators.NetWorth = ConvertValue(companyIndicatorUsTd.Result.First().Finance.ShareHolderEquity);
                    companyIndicators.TotalAssets = ConvertValue(companyIndicatorUsTd.Result.First().Finance.TotalAssets);
                    companyIndicators.NetProfitAnnual = ConvertValue(companyIndicatorUsTd.Result.First().Finance.NetIncome);
                    companyIndicators.ReferenceDate = DateTime.ParseExact(companyIndicatorUsTd.Result.First().Finance.ReportDate, "yyyyMMdd", CultureInfo.InvariantCulture);

                }

                if (companyIndicatorUsTd.Result.First().MarketData != null)
                {
                    companyIndicators.MarketCap = ConvertValue(companyIndicatorUsTd.Result.First().MarketData.Marketcap);
                }

                if (companyIndicatorUsTd.Result.First().Index != null)
                {
                    companyIndicators.NetDebt = ConvertValue(companyIndicatorUsTd.Result.First().Index.DividaLiquida);
                    companyIndicators.PricePerVpa = ConvertValue(companyIndicatorUsTd.Result.First().Index.PricePerWorth);
                    companyIndicators.PricePerProfit = ConvertValue(companyIndicatorUsTd.Result.First().Index.PricePerProfit);
                    companyIndicators.RoaAnnual = ConvertValue(companyIndicatorUsTd.Result.First().Index.Roa);
                    companyIndicators.RoeAnnual = ConvertValue(companyIndicatorUsTd.Result.First().Index.Roe);
                }

                if (companyHistoricTd != null && companyHistoricTd.CompanyDividendTd != null)
                {
                    companyIndicators.Dividend12Months = ConvertValue(companyHistoricTd.CompanyDividendTd.DividentUm);
                    companyIndicators.Dividend12MonthsYield = ConvertValue(companyHistoricTd.CompanyDividendTd.DividentYieldUm);
                    companyIndicators.Dividend24Months = ConvertValue(companyHistoricTd.CompanyDividendTd.DividentDois);
                    companyIndicators.Dividend24MonthsYield = ConvertValue(companyHistoricTd.CompanyDividendTd.DividentYieldDois);
                    companyIndicators.Dividend36Months = ConvertValue(companyHistoricTd.CompanyDividendTd.DividentTres);
                    companyIndicators.Dividend36MonthsYield = ConvertValue(companyHistoricTd.CompanyDividendTd.DividentYieldTres);
                }
            }

            return companyIndicators;
        }

        private static CompanyIndicators ConvertIndicatorFiis(CompanyIndicatorFiisTd companyIndicatorTd, CompanyHistoricTd companyHistoricTd, CompanyIndicators companyIndicators)
        {
            if (companyIndicatorTd != null && companyIndicatorTd.Result != null && companyIndicatorTd.Result.Monthly.Count > 0)
            {
                companyIndicators = new CompanyIndicators();
                companyIndicators.NetWorth = ConvertValue(companyIndicatorTd.Result.Monthly.First().VlPatrimonyLiq);
                companyIndicators.TotalAssets = ConvertValue(companyIndicatorTd.Result.Monthly.First().VlAsset);
                companyIndicators.VlPatrimonyQuotas = ConvertValue(companyIndicatorTd.Result.Monthly.First().VlPatrimonyQuotas);
                companyIndicators.PricePerVpa = ConvertValue(companyIndicatorTd.Result.Monthly.First().PriceOverVpa);
                companyIndicators.ReferenceDate = DateTime.ParseExact(companyIndicatorTd.Result.Monthly.First().DtReference, "yyyyMM", CultureInfo.InvariantCulture);
                companyIndicators.TotalQuotaHolder = ConvertIntValue(companyIndicatorTd.Result.Monthly.First().TotalQuotaholder);
            }

            if (companyHistoricTd != null && companyHistoricTd.CompanyDividendTd != null)
            {
                if (companyIndicators == null)
                {
                    companyIndicators = new CompanyIndicators();
                }

                companyIndicators.Dividend12Months = ConvertValue(companyHistoricTd.CompanyDividendTd.DividentUm);
                companyIndicators.Dividend12MonthsYield = ConvertValue(companyHistoricTd.CompanyDividendTd.DividentYieldUm);
                companyIndicators.Dividend24Months = ConvertValue(companyHistoricTd.CompanyDividendTd.DividentDois);
                companyIndicators.Dividend24MonthsYield = ConvertValue(companyHistoricTd.CompanyDividendTd.DividentYieldDois);
                companyIndicators.Dividend36Months = ConvertValue(companyHistoricTd.CompanyDividendTd.DividentTres);
                companyIndicators.Dividend36MonthsYield = ConvertValue(companyHistoricTd.CompanyDividendTd.DividentYieldTres);
            }

            return companyIndicators;
        }


        private static decimal ConvertValue(string price)
        {
            decimal priceConvert = 0;
            var style = NumberStyles.Number;
            var culture = CultureInfo.CreateSpecificCulture("en-US");


            if (!string.IsNullOrWhiteSpace(price) && price.Contains("E"))
            {
                string[] priceParts = price.Split("E");

                if (priceParts != null && priceParts.Length >= 1)
                {
                    decimal firstPart = 0;
                    long secondPart = 0;

                    decimal.TryParse(priceParts[0], style, culture, out firstPart);

                    if (priceParts.Length > 1)
                    {
                        long.TryParse(priceParts[1].Replace(".", string.Empty), out secondPart);
                    }

                    price = (firstPart * ((decimal)Math.Pow(10, secondPart))).ToString(culture);
                }
            }

            decimal.TryParse(price, style, culture, out priceConvert);
            return priceConvert;
        }

        private static int ConvertIntValue(string total)
        {
            int totalConvert = 0;
            var style = NumberStyles.Number;
            var culture = CultureInfo.CreateSpecificCulture("en-US");


            if (!string.IsNullOrWhiteSpace(total) && total.Contains("E"))
            {
                string[] priceParts = total.Split("E");

                if (priceParts != null && priceParts.Length >= 1)
                {
                    int firstPart = 0;
                    int secondPart = 0;

                    int.TryParse(priceParts[0].Replace(".", string.Empty), style, culture, out firstPart);

                    if (priceParts.Length > 1)
                    {
                        int.TryParse(priceParts[1].Replace(".", string.Empty), out secondPart);
                    }

                    total = (firstPart * ((int)Math.Pow(10, secondPart))).ToString(culture);
                }
            }

            int.TryParse(total, style, culture, out totalConvert);
            return totalConvert;
        }

        public ResultResponseObject<CompanyIndicatorWrapperVM> GetCompanyIndicators(Guid guidStock)
        {
            ResultResponseObject<CompanyIndicatorWrapperVM> resultResponseObject = new ResultResponseObject<CompanyIndicatorWrapperVM>();
            CompanyIndicatorWrapperVM companyIndicatorWrapperVM = null;
            resultResponseObject.Success = true;

            using (_uow.Create())
            {
                ResultServiceObject<CompanyIndicatorsView> resultCpIndicatorsView = _companyIndicatorsService.GetCompanyIndicators(guidStock);

                if (resultCpIndicatorsView.Value != null)
                {
                    companyIndicatorWrapperVM = new CompanyIndicatorWrapperVM();
                    companyIndicatorWrapperVM.Dividend12Months = resultCpIndicatorsView.Value.Dividend12Months.ToString("n2", new CultureInfo("pt-br"));
                    companyIndicatorWrapperVM.Dividend12MonthsYield = resultCpIndicatorsView.Value.Dividend12MonthsYield.ToString("n2", new CultureInfo("pt-br")) + "%";
                    companyIndicatorWrapperVM.Dividend24Months = resultCpIndicatorsView.Value.Dividend24Months.ToString("n2", new CultureInfo("pt-br"));
                    companyIndicatorWrapperVM.Dividend24MonthsYield = resultCpIndicatorsView.Value.Dividend24MonthsYield.ToString("n2", new CultureInfo("pt-br")) + "%";
                    companyIndicatorWrapperVM.Dividend36Months = resultCpIndicatorsView.Value.Dividend36Months.ToString("n2", new CultureInfo("pt-br"));
                    companyIndicatorWrapperVM.Dividend36MonthsYield = resultCpIndicatorsView.Value.Dividend36MonthsYield.ToString("n2", new CultureInfo("pt-br")) + "%";

                    companyIndicatorWrapperVM.Indicators = new List<IndicatorVM>();

                    if (resultCpIndicatorsView.Value.IdStockType == (int)StockTypeEnum.FII)
                    {
                        IndicatorVM indicatorVM = new IndicatorVM();
                        indicatorVM.Name = "Patrimônio Líquido";
                        indicatorVM.Value = FormatValue(resultCpIndicatorsView.Value.NetWorth);
                        companyIndicatorWrapperVM.Indicators.Add(indicatorVM);

                        indicatorVM = new IndicatorVM();
                        indicatorVM.Name = "Patrimônio Líquido";
                        indicatorVM.Value = resultCpIndicatorsView.Value.VlPatrimonyQuotas.ToString("n2", new CultureInfo("pt-br")); ;
                        companyIndicatorWrapperVM.Indicators.Add(indicatorVM);

                        indicatorVM = new IndicatorVM();
                        indicatorVM.Name = "Ativo Total";
                        indicatorVM.Value = FormatValue(resultCpIndicatorsView.Value.TotalAssets);
                        companyIndicatorWrapperVM.Indicators.Add(indicatorVM);

                        indicatorVM = new IndicatorVM();
                        indicatorVM.Name = "Cotistas";
                        indicatorVM.Value = Math.Round(resultCpIndicatorsView.Value.TotalQuotaHolder).ToString();
                        companyIndicatorWrapperVM.Indicators.Add(indicatorVM);

                        indicatorVM = new IndicatorVM();
                        indicatorVM.Name = "P/VP Cota";
                        indicatorVM.Value = resultCpIndicatorsView.Value.PricePerVpa.ToString("n2", new CultureInfo("pt-br")); ;
                        companyIndicatorWrapperVM.Indicators.Add(indicatorVM);
                    }
                    else if (resultCpIndicatorsView.Value.IdStockType == (int)StockTypeEnum.Stocks)
                    {
                        IndicatorVM indicatorVM = new IndicatorVM();
                        indicatorVM.Name = "Patrimônio Líquido";
                        indicatorVM.Value = FormatValue(resultCpIndicatorsView.Value.NetWorth);
                        companyIndicatorWrapperVM.Indicators.Add(indicatorVM);

                        indicatorVM = new IndicatorVM();
                        indicatorVM.Name = "Ativo Total";
                        indicatorVM.Value = FormatValue(resultCpIndicatorsView.Value.TotalAssets);
                        companyIndicatorWrapperVM.Indicators.Add(indicatorVM);

                        indicatorVM = new IndicatorVM();
                        indicatorVM.Name = "Dívida Líquida";
                        indicatorVM.Value = FormatValue(resultCpIndicatorsView.Value.NetDebt);
                        companyIndicatorWrapperVM.Indicators.Add(indicatorVM);

                        indicatorVM = new IndicatorVM();
                        indicatorVM.Name = "Lucro Líquido";
                        indicatorVM.Value = FormatValue(resultCpIndicatorsView.Value.NetProfitAnnual);
                        companyIndicatorWrapperVM.Indicators.Add(indicatorVM);

                        indicatorVM = new IndicatorVM();
                        indicatorVM.Name = "ROE";
                        indicatorVM.Value = resultCpIndicatorsView.Value.RoeAnnual.ToString("n2", new CultureInfo("pt-br")) + "%";
                        companyIndicatorWrapperVM.Indicators.Add(indicatorVM);

                        indicatorVM = new IndicatorVM();
                        indicatorVM.Name = "ROA";
                        indicatorVM.Value = resultCpIndicatorsView.Value.RoaAnnual.ToString("n2", new CultureInfo("pt-br")) + "%";
                        companyIndicatorWrapperVM.Indicators.Add(indicatorVM);

                        indicatorVM = new IndicatorVM();
                        indicatorVM.Name = "Valor de Mercado";
                        indicatorVM.Value = FormatValue(resultCpIndicatorsView.Value.MarketCap);
                        companyIndicatorWrapperVM.Indicators.Add(indicatorVM);

                        indicatorVM = new IndicatorVM();
                        indicatorVM.Name = "Preço/VPA";
                        indicatorVM.Value = resultCpIndicatorsView.Value.PricePerVpa.ToString("n2", new CultureInfo("pt-br"));
                        companyIndicatorWrapperVM.Indicators.Add(indicatorVM);

                        indicatorVM = new IndicatorVM();
                        indicatorVM.Name = "Preço/Lucro";
                        indicatorVM.Value = resultCpIndicatorsView.Value.PricePerProfit.ToString("n2", new CultureInfo("pt-br"));
                        companyIndicatorWrapperVM.Indicators.Add(indicatorVM);

                        indicatorVM = new IndicatorVM();
                        indicatorVM.Name = "Nº total de papéis";
                        indicatorVM.Value = FormatValue(resultCpIndicatorsView.Value.QttyStock);
                        companyIndicatorWrapperVM.Indicators.Add(indicatorVM);

                        indicatorVM = new IndicatorVM();
                        indicatorVM.Name = "ROIC";
                        indicatorVM.Value = resultCpIndicatorsView.Value.RoicAnnual.ToString("n2", new CultureInfo("pt-br"));
                        companyIndicatorWrapperVM.Indicators.Add(indicatorVM);

                        indicatorVM = new IndicatorVM();
                        indicatorVM.Name = "Payout";
                        indicatorVM.Value = resultCpIndicatorsView.Value.PayoutAnnual.ToString("n2", new CultureInfo("pt-br"));
                        companyIndicatorWrapperVM.Indicators.Add(indicatorVM);
                    }

                }
            }

            resultResponseObject.Value = companyIndicatorWrapperVM;

            return resultResponseObject;
        }

        public ResultResponseObject<IEnumerable<CompanyIndicatorVM>> GetList(API.Model.Request.Stock.StockType stockType, bool onlyMyStocks)
        {
            ResultResponseObject<IEnumerable<CompanyIndicatorVM>> resultResponseObject = new ResultResponseObject<IEnumerable<CompanyIndicatorVM>>();
            CompanyIndicatorWrapperVM companyIndicatorWrapperVM = null;
            resultResponseObject.Success = true;

            using (_uow.Create())
            {
                ResultServiceObject<IEnumerable<CompanyIndicatorsView>> resultCpIndicatorsView = _companyIndicatorsService.GetList(_globalAuthenticationService, onlyMyStocks);

                //if (resultCpIndicatorsView.Value != null)
                //{
                //    companyIndicatorWrapperVM = new CompanyIndicatorWrapperVM();
                //    companyIndicatorWrapperVM.Dividend12Months = resultCpIndicatorsView.Value.Dividend12Months.ToString("n2", new CultureInfo("pt-br"));
                //    companyIndicatorWrapperVM.Dividend12MonthsYield = resultCpIndicatorsView.Value.Dividend12MonthsYield.ToString("n2", new CultureInfo("pt-br")) + "%";
                //    companyIndicatorWrapperVM.Dividend24Months = resultCpIndicatorsView.Value.Dividend24Months.ToString("n2", new CultureInfo("pt-br"));
                //    companyIndicatorWrapperVM.Dividend24MonthsYield = resultCpIndicatorsView.Value.Dividend24MonthsYield.ToString("n2", new CultureInfo("pt-br")) + "%";
                //    companyIndicatorWrapperVM.Dividend36Months = resultCpIndicatorsView.Value.Dividend36Months.ToString("n2", new CultureInfo("pt-br"));
                //    companyIndicatorWrapperVM.Dividend36MonthsYield = resultCpIndicatorsView.Value.Dividend36MonthsYield.ToString("n2", new CultureInfo("pt-br")) + "%";

                //    companyIndicatorWrapperVM.Indicators = new List<IndicatorVM>();

                //    if (resultCpIndicatorsView.Value.IdStockType == (int)StockTypeEnum.FII)
                //    {
                //        IndicatorVM indicatorVM = new IndicatorVM();
                //        indicatorVM.Name = "Patrimônio Líquido";
                //        indicatorVM.Value = FormatValue(resultCpIndicatorsView.Value.NetWorth);
                //        companyIndicatorWrapperVM.Indicators.Add(indicatorVM);

                //        indicatorVM = new IndicatorVM();
                //        indicatorVM.Name = "Patrimônio Líquido";
                //        indicatorVM.Value = resultCpIndicatorsView.Value.VlPatrimonyQuotas.ToString("n2", new CultureInfo("pt-br")); ;
                //        companyIndicatorWrapperVM.Indicators.Add(indicatorVM);

                //        indicatorVM = new IndicatorVM();
                //        indicatorVM.Name = "Ativo Total";
                //        indicatorVM.Value = FormatValue(resultCpIndicatorsView.Value.TotalAssets);
                //        companyIndicatorWrapperVM.Indicators.Add(indicatorVM);

                //        indicatorVM = new IndicatorVM();
                //        indicatorVM.Name = "Cotistas";
                //        indicatorVM.Value = Math.Round(resultCpIndicatorsView.Value.TotalQuotaHolder).ToString();
                //        companyIndicatorWrapperVM.Indicators.Add(indicatorVM);

                //        indicatorVM = new IndicatorVM();
                //        indicatorVM.Name = "P/VP Cota";
                //        indicatorVM.Value = resultCpIndicatorsView.Value.PricePerVpa.ToString("n2", new CultureInfo("pt-br")); ;
                //        companyIndicatorWrapperVM.Indicators.Add(indicatorVM);
                //    }
                //    else if (resultCpIndicatorsView.Value.IdStockType == (int)StockTypeEnum.Stocks)
                //    {
                //        IndicatorVM indicatorVM = new IndicatorVM();
                //        indicatorVM.Name = "Patrimônio Líquido";
                //        indicatorVM.Value = FormatValue(resultCpIndicatorsView.Value.NetWorth);
                //        companyIndicatorWrapperVM.Indicators.Add(indicatorVM);

                //        indicatorVM = new IndicatorVM();
                //        indicatorVM.Name = "Ativo Total";
                //        indicatorVM.Value = FormatValue(resultCpIndicatorsView.Value.TotalAssets);
                //        companyIndicatorWrapperVM.Indicators.Add(indicatorVM);

                //        indicatorVM = new IndicatorVM();
                //        indicatorVM.Name = "Dívida Líquida";
                //        indicatorVM.Value = FormatValue(resultCpIndicatorsView.Value.NetDebt);
                //        companyIndicatorWrapperVM.Indicators.Add(indicatorVM);

                //        indicatorVM = new IndicatorVM();
                //        indicatorVM.Name = "Lucro Líquido";
                //        indicatorVM.Value = FormatValue(resultCpIndicatorsView.Value.NetProfitAnnual);
                //        companyIndicatorWrapperVM.Indicators.Add(indicatorVM);

                //        indicatorVM = new IndicatorVM();
                //        indicatorVM.Name = "ROE";
                //        indicatorVM.Value = resultCpIndicatorsView.Value.RoeAnnual.ToString("n2", new CultureInfo("pt-br")) + "%";
                //        companyIndicatorWrapperVM.Indicators.Add(indicatorVM);

                //        indicatorVM = new IndicatorVM();
                //        indicatorVM.Name = "ROA";
                //        indicatorVM.Value = resultCpIndicatorsView.Value.RoaAnnual.ToString("n2", new CultureInfo("pt-br")) + "%";
                //        companyIndicatorWrapperVM.Indicators.Add(indicatorVM);

                //        indicatorVM = new IndicatorVM();
                //        indicatorVM.Name = "Valor de Mercado";
                //        indicatorVM.Value = FormatValue(resultCpIndicatorsView.Value.MarketCap);
                //        companyIndicatorWrapperVM.Indicators.Add(indicatorVM);

                //        indicatorVM = new IndicatorVM();
                //        indicatorVM.Name = "Preço/VPA";
                //        indicatorVM.Value = resultCpIndicatorsView.Value.PricePerVpa.ToString("n2", new CultureInfo("pt-br"));
                //        companyIndicatorWrapperVM.Indicators.Add(indicatorVM);

                //        indicatorVM = new IndicatorVM();
                //        indicatorVM.Name = "Preço/Lucro";
                //        indicatorVM.Value = resultCpIndicatorsView.Value.PricePerProfit.ToString("n2", new CultureInfo("pt-br"));
                //        companyIndicatorWrapperVM.Indicators.Add(indicatorVM);

                //        indicatorVM = new IndicatorVM();
                //        indicatorVM.Name = "Nº total de papéis";
                //        indicatorVM.Value = FormatValue(resultCpIndicatorsView.Value.QttyStock);
                //        companyIndicatorWrapperVM.Indicators.Add(indicatorVM);

                //        indicatorVM = new IndicatorVM();
                //        indicatorVM.Name = "ROIC";
                //        indicatorVM.Value = resultCpIndicatorsView.Value.RoicAnnual.ToString("n2", new CultureInfo("pt-br"));
                //        companyIndicatorWrapperVM.Indicators.Add(indicatorVM);

                //        indicatorVM = new IndicatorVM();
                //        indicatorVM.Name = "Payout";
                //        indicatorVM.Value = resultCpIndicatorsView.Value.PayoutAnnual.ToString("n2", new CultureInfo("pt-br"));
                //        companyIndicatorWrapperVM.Indicators.Add(indicatorVM);
                //    }

                //}
            }

            //resultResponseObject.Value = companyIndicatorWrapperVM;

            return resultResponseObject;
        }

        public string FormatValue(decimal value)
        {
            string formatValue = "-";
            if (value >= 1000000000 || value < -999999999)
            {
                formatValue = value.ToString("0,,,.##B", new CultureInfo("pt-br"));
            }
            else if (value > 999999 || value < -999999)
            {
                formatValue = value.ToString("0,,.##M", new CultureInfo("pt-br"));
            }
            else if (value > 999 || value < -999)
            {
                formatValue = value.ToString("0,.#K", new CultureInfo("pt-br"));
            }
            else
            {
                formatValue = value.ToString("n2", new CultureInfo("pt-br"));
            }

            return formatValue;
        }
    }
}
