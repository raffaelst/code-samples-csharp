using Dividendos.API.Model.Request.Dividend;
using Dividendos.API.Model.Request.Operation;
using Dividendos.API.Model.Response;
using Dividendos.API.Model.Response.Common;

using Dividendos.Application.Interface;
using Microsoft.AspNetCore.Authorization;
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
    public class DividendController : BaseController
    {
        private readonly IDividendApp _dividendApp;

        public DividendController(IDividendApp dividendApp)
        {
            _dividendApp = dividendApp;
        }

        [HttpGet]
        [ProducesResponseType(200)]
        [ProducesResponseType(400)]
        public async Task<IActionResult> GetNextDividend()
        {

            if (!ModelState.IsValid)
            {
                return Response(NotifyModelStateErrors());
            }

            ResultResponseObject<IEnumerable<API.Model.Response.v2.NextDividendVM>> resultServiceObject = _dividendApp.GetNextDividendsByLoggedUserV2();

            return Response(resultServiceObject);
        }

        /// <summary>
        /// Restore Dividends
        /// </summary>
        /// <returns></returns>
        [HttpPut("restoredividends")]
        [ProducesResponseType(200)]
        [ProducesResponseType(400)]
        public async Task<IActionResult> RestoreDividends([FromBody] RestoreDividendVM restoreDividendVM)
        {
            if (!ModelState.IsValid)
            {
                return Response(NotifyModelStateErrors());
            }

            ResultResponseBase resultResponse = _dividendApp.RestorePastDividends(restoreDividendVM.GuidPortfolio, restoreDividendVM.Token);

            return Response(resultResponse);
        }
    }
}
