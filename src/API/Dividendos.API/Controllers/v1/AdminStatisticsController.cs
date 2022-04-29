using Dividendos.API.Model.Request;
using Dividendos.API.Model.Request.Stock;
using Dividendos.API.Model.Response;
using Dividendos.API.Model.Response.Common;
using Dividendos.Application.Interface;
using Dividendos.CrossCutting.Identity.Models;
using Dividendos.Finance.Interface;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Dividendos.API.Controllers.v1
{
    [Authorize("Bearer")]
    [Authorize(Roles = "Administrator")]
    [Route("admin-statistics")]
    [ApiController]
    public class AdminStatisticsController : BaseController
    {
        private readonly IStockApp _traderApp;
        private readonly IDividendCalendarApp _dividendCalendarApp;
        private readonly IUserApp _userApp;
        private readonly IFollowStockApp _dividend;
        private readonly IRelevantFactApp _spreadsheetsHelper;

        public AdminStatisticsController(IStockApp traderApp,
            IDividendCalendarApp dividendCalendarApp,
            IUserApp userApp,
            IFollowStockApp dividend,
            IRelevantFactApp spreadsheetsHelper)
        {
            _traderApp = traderApp;
            _dividendCalendarApp = dividendCalendarApp;
            _userApp = userApp;
            _dividend = dividend;
            _spreadsheetsHelper = spreadsheetsHelper;
        }

        [HttpGet]
        [ProducesResponseType(200)]
        [ProducesResponseType(400)]
        public async Task<IActionResult> GetGlobalStatistics()
        {

            if (!ModelState.IsValid)
            {
                return Response(NotifyModelStateErrors());
            }

            _dividendCalendarApp.GetAndUpdateFromNasdaq(DateTime.Now.AddMonths(-2), DateTime.Now.AddMonths(10));
            //_traderApp.ImportMarketMover(MakertMoversType.TopDividendYieldREITsEUA);
            //_traderApp.ImportMarketMover(MakertMoversType.TopDividendPaidREITsEUA);
            //_dividendCalendarApp.ImportMarketMover(MakertMoversType.BiggestHighsStocksBRAll);
            //ResultResponseObject<IEnumerable<TraderSummaryVM>> resultServiceObject = _traderApp.GetByLoggedUser();

            return Response();
        }
    }
}
