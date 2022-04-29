using Dividendos.API.Model.Request;
using Dividendos.API.Model.Response;
using Dividendos.API.Model.Response.Bounds;
using Dividendos.API.Model.Response.Common;
using Dividendos.Application.Interface;
using Dividendos.Application.Interface.Model;
using Dividendos.CrossCutting.Identity.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Dividendos.API.Controllers.v1
{
    [Authorize("Bearer")]
    [Route("[controller]")]
    [ApiController]
    public class BoundsController : BaseController
    {
        private readonly IFinancialProductsApp _financialProductsApp;
        private readonly IPortfolioApp _portfolioApp;

        public BoundsController(IFinancialProductsApp financialProductsApp,
            IPortfolioApp portfolioApp)
        {
            _financialProductsApp = financialProductsApp;
            _portfolioApp = portfolioApp;
        }

        [HttpGet("summary")]
        [ProducesResponseType(200)]
        [ProducesResponseType(400)]
        public async Task<IActionResult> GetBoundsSummary()
        {

            if (!ModelState.IsValid)
            {
                return Response(NotifyModelStateErrors());
            }

            ResultResponseObject<BoundsSummaryVM> resultServiceObject = _financialProductsApp.GetFixedIncomeSummaryByLoggedUser();

            return Response(resultServiceObject);
        }

        [HttpGet("extract")]
        [ProducesResponseType(200)]
        [ProducesResponseType(400)]
        public async Task<IActionResult> GetBondsExtract()
        {

            if (!ModelState.IsValid)
            {
                return Response(NotifyModelStateErrors());
            }

            ResultResponseObject<BoundsWrapperVM> resultServiceObject = _financialProductsApp.GetFixedIncomeExtracByLoggedUser();

            return Response(resultServiceObject);
        }

        [HttpGet("allocation-chart")]
        [ProducesResponseType(200)]
        [ProducesResponseType(400)]
        public async Task<IActionResult> GetBoundsAllocationChart()
        {

            if (!ModelState.IsValid)
            {
                return Response(NotifyModelStateErrors());
            }

            ResultResponseObject<StockTypeChart> resultServiceObject = _financialProductsApp.GetFixedIncomeChartsByLoggedUser();

            return Response(resultServiceObject);
        }
    }
}
