using Dividendos.Bacen.Interface;
using Dividendos.Bacen.Interface.Model;
using Dividendos.Repository.Interface.UoW;
using Dividendos.Service.Interface;
using System.Linq;
using System.Collections.Generic;
using System.Text;
using Dividendos.Entity.Model;
using Dividendos.Entity.Entities;
using System;
using System.Globalization;
using K.Logger;
using Dividendos.Application.Interface;
using System.Threading.Tasks;
using Dividendos.API.Model.Response.Common;
using Dividendos.API.Model.Response;
using AutoMapper;
using Dividendos.Entity.Enum;
using Dividendos.Entity.Views;
using Dividendos.Application.Interface.Model;

namespace Dividendos.Application
{
    public class StockSubscriptionApp : IStockSubscriptionApp
    {
        private readonly IUnitOfWork _uow;
        private readonly IPortfolioService _portfolioService;
        private readonly ISubPortfolioService _subPortfolioService;
        private readonly ILogger _logger;
        private readonly IMapper _mapper;

        public StockSubscriptionApp(IUnitOfWork uow,
                            IPortfolioService portfolioService,
                            ISubPortfolioService subPortfolioService,
                            ILogger logger,
                            IMapper mapper)
        {
            _uow = uow;
            _portfolioService = portfolioService;
            _subPortfolioService = subPortfolioService;
            _logger = logger;
            _mapper = mapper;
        }

        public ResultResponseObject<IEnumerable<PortfolioStatementViewModel>> GetPortfolioStatementSubscriptionView(Guid guidPortfolioSub)
        {
            ResultResponseObject<IEnumerable<PortfolioStatementViewModel>> resultServiceObject = new ResultResponseObject<IEnumerable<PortfolioStatementViewModel>>();
            List<PortfolioStatementViewModel> resultModel = new List<PortfolioStatementViewModel>();

            using (_uow.Create())
            {
                ResultServiceObject<Portfolio> resultPortfolio = _portfolioService.GetByGuid(guidPortfolioSub);
                ResultServiceObject<SubPortfolio> resultSubPortfolio = _subPortfolioService.GetByGuid(guidPortfolioSub);

                if (resultSubPortfolio.Success && resultPortfolio.Success)
                {
                    ResultServiceObject<IEnumerable<PortfolioStatementView>> result = new ResultServiceObject<IEnumerable<PortfolioStatementView>>();
                    Portfolio portfolio = resultPortfolio.Value;
                    SubPortfolio subportfolio = resultSubPortfolio.Value;

                    if (portfolio != null)
                    {
                        result = _portfolioService.GetByPortfolioSubscription(portfolio.GuidPortfolio);
                    }
                    else if (subportfolio != null)
                    {
                        result = _portfolioService.GetBySubportfolioSubscription(subportfolio.GuidSubPortfolio);
                    }

                    if (result.Success && result.Value != null && result.Value.Count() > 0)
                    {
                        foreach (PortfolioStatementView portfolioView in result.Value)
                        {
                            decimal perc = portfolioView.PerformancePerc * 100;
                            PortfolioStatementViewModel portfolioViewModel = new PortfolioStatementViewModel();
                            portfolioViewModel.IdStock = portfolioView.IdStock;
                            portfolioViewModel.GuidOperation = portfolioView.GuidOperation;
                            portfolioViewModel.Company = portfolioView.Company;
                            portfolioViewModel.Segment = portfolioView.Segment;
                            portfolioViewModel.Symbol = portfolioView.Symbol;
                            portfolioViewModel.Logo = portfolioView.Logo;
                            portfolioViewModel.PerformancePerc = GetSignal(perc) + perc.ToString("n2", new CultureInfo("pt-br"));
                            portfolioViewModel.AveragePrice = portfolioView.AveragePrice.ToString("n2", new CultureInfo("pt-br"));
                            portfolioViewModel.MarketPrice = portfolioView.MarketPrice.ToString("n2", new CultureInfo("pt-br"));
                            portfolioViewModel.NetValue = GetSignal(portfolioView.NetValue) + portfolioView.NetValue.ToString("n2", new CultureInfo("pt-br"));
                            portfolioViewModel.TotalDividends = portfolioView.TotalDividends.ToString("n2", new CultureInfo("pt-br"));
                            portfolioViewModel.NumberOfShares = portfolioView.NumberOfShares.ToString("0.#############################", new CultureInfo("pt-br"));
                            portfolioViewModel.TotalMarket = portfolioView.TotalMarket.ToString("n2", new CultureInfo("pt-br"));
                            portfolioViewModel.Total = portfolioView.Total.ToString("n2", new CultureInfo("pt-br"));

                            portfolioViewModel.PerformancePercN = perc;
                            portfolioViewModel.AveragePriceN = portfolioView.AveragePrice;
                            portfolioViewModel.MarketPriceN = portfolioView.MarketPrice;
                            portfolioViewModel.NetValueN = portfolioView.NetValue;
                            portfolioViewModel.TotalDividendsN = portfolioView.TotalDividends;
                            portfolioViewModel.NumberOfSharesN = portfolioView.NumberOfShares;
                            portfolioViewModel.TotalMarketN = portfolioView.TotalMarket;
                            portfolioViewModel.TotalN = portfolioView.Total;
                            portfolioViewModel.LastUpdated = DateTime.Now.ToString("HH:mm:ss");

                            resultModel.Add(portfolioViewModel);
                        }
                    }

                }
            }

            resultServiceObject.Value = resultModel;
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
