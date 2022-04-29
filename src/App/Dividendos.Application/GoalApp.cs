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
using Microsoft.Extensions.Caching.Distributed;
using Newtonsoft.Json;
using Dividendos.API.Model.Request.Goals;

namespace Dividendos.Application
{
    public class GoalApp : IGoalApp
    {
        private readonly IUnitOfWork _uow;
        private readonly IGoalService _goalService;
        private readonly ILogger _logger;
        private readonly IMapper _mapper;
        private readonly IGlobalAuthenticationService _globalAuthenticationService;
        private readonly ICacheService _cacheService;
        private readonly IPortfolioService _portfolioService;
        private readonly ISubPortfolioService _subPortfolioService;
        public GoalApp(IUnitOfWork uow,
                            IGoalService goalService,
                            ILogger logger,
                            IMapper mapper,
                            IGlobalAuthenticationService globalAuthenticationService,
                            IPortfolioService portfolioService,
                            ISubPortfolioService subPortfolioService,
                            ICacheService cacheService)
        {
            _uow = uow;
            _goalService = goalService;
            _logger = logger;
            _mapper = mapper;
            _globalAuthenticationService = globalAuthenticationService;
            _portfolioService = portfolioService;
            _subPortfolioService = subPortfolioService;
            _cacheService = cacheService;
        }

        public ResultResponseObject<GoalVM> Add(GoalRequest goalRequest)
        {
            ResultResponseObject<GoalVM> result = new ResultResponseObject<GoalVM>();

            using (_uow.Create())
            {
                Goal goal = new Goal();
                DateTime limitDate;

                if (DateTime.TryParseExact(goalRequest.Limit, "dd/MM/yyyy", new CultureInfo("pt-br"), DateTimeStyles.None, out limitDate))
                {
                    goal.Limit = limitDate;
                    goal.Name = goalRequest.Name;
                    decimal totalValue = 0;
                    decimal.TryParse(goalRequest.TotalValue.Replace(".", string.Empty), NumberStyles.Currency, new CultureInfo("pt-br"), out totalValue);

                    goal.Value = totalValue;
                    long idPortfolioOrSubPortfolio = 0;

                    var portfolio = _portfolioService.GetByGuid(Guid.Parse(goalRequest.PortfolioGuid));

                    if (portfolio.Value != null)
                    {
                        idPortfolioOrSubPortfolio = portfolio.Value.IdPortfolio;
                    }
                    else
                    {
                        idPortfolioOrSubPortfolio = _subPortfolioService.GetByGuid(Guid.Parse(goalRequest.PortfolioGuid)).Value.IdSubPortfolio;
                    }

                    goal.PortfolioID = idPortfolioOrSubPortfolio;
                    ResultServiceObject<Goal> resultService = _goalService.Add(goal);
                    result = _mapper.Map<ResultResponseObject<GoalVM>>(resultService);
                }
                else
                {
                    result.ErrorMessages.Add("Data inválida");
                }
            }

            return result;
        }

        public ResultResponseBase Delete(string goalGuid)
        {
            ResultResponseBase resultService = new ResultResponseBase() { Success = false };

            using (_uow.Create())
            {
                ResultServiceObject<Goal> resultServiceProduct = _goalService.GetByGuid(Guid.Parse(goalGuid));

                if (resultServiceProduct.Value != null)
                {
                    _goalService.Delete(resultServiceProduct.Value);

                    resultService = new ResultResponseBase() { Success = true };
                }
            }

            return resultService;
        }

        public ResultResponseObject<IEnumerable<GoalVM>> GetAllByUser()
        {
            ResultResponseObject<IEnumerable<GoalVM>> result = null;

            using (_uow.Create())
            {
                ResultServiceObject<IEnumerable<GoalView>> resultService = _goalService.GetAllByUser(_globalAuthenticationService.IdUser);

                foreach (var itemGoalView in resultService.Value)
                {
                    if (itemGoalView.ValueReached < itemGoalView.Value)
                    {
                        itemGoalView.PercentageReached = ((itemGoalView.ValueReached * 100) / itemGoalView.Value);
                    }
                    else
                    {
                        itemGoalView.PercentageReached = 100;
                    }
                }

                result = _mapper.Map<ResultResponseObject<IEnumerable<GoalVM>>>(resultService);
            }

            return result;
        }
    }
}
