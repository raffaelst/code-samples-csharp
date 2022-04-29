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

namespace Dividendos.API.Controllers.v2
{
    [Authorize("Bearer")]
    [ApiVersion("2")]
    [Route("v{version:apiVersion}/[controller]")]
    [ApiController]
    public class BoundsController : BaseController
    {
        private readonly IFinancialProductsApp _financialProductsApp;

        public BoundsController(IFinancialProductsApp financialProductsApp)
        {
            _financialProductsApp = financialProductsApp;
        }

        [HttpGet("summary")]
        [ProducesResponseType(200)]
        [ProducesResponseType(400)]
        public async Task<IActionResult> GetSummary()
        {

            if (!ModelState.IsValid)
            {
                return Response(NotifyModelStateErrors());
            }

            ResultResponseObject<BoundsSummaryVM> resultServiceObject = _financialProductsApp.GetBoundsSummaryByLoggedUser();

            return Response(resultServiceObject);
        }

        [HttpGet("extract")]
        [ProducesResponseType(200)]
        [ProducesResponseType(400)]
        public async Task<IActionResult> GetExtract()
        {

            if (!ModelState.IsValid)
            {
                return Response(NotifyModelStateErrors());
            }

            ResultResponseObject<BoundsWrapperVM> resultServiceObject = _financialProductsApp.GetBoundsExtractByLoggedUser();

            return Response(resultServiceObject);
        }

        [HttpGet("chart")]
        [ProducesResponseType(200)]
        [ProducesResponseType(400)]
        public async Task<IActionResult> GetAllocationChart()
        {

            if (!ModelState.IsValid)
            {
                return Response(NotifyModelStateErrors());
            }

            ResultResponseObject<StockTypeChart> resultServiceObject = _financialProductsApp.GetBoundsChartsByLoggedUser();

            return Response(resultServiceObject);
        }
    }
}
