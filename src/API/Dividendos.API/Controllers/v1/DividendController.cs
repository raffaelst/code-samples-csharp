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

namespace Dividendos.API.Controllers.v1
{
    [Authorize("Bearer")]
    [Route("[controller]")]
    [ApiController]
    public class DividendController : BaseController
    {
        private readonly IDividendApp _dividendApp;

        public DividendController(IDividendApp dividendApp)
        {
            _dividendApp = dividendApp;
        }

        /// <summary>
        /// Create a new Dividend
        /// </summary>
        /// <returns></returns>
        [HttpPost("{idStock}")]
        [ProducesResponseType(200)]
        [ProducesResponseType(400)]
        public async Task<IActionResult> PostDividend(long idStock, DividendAddVM dividendAddVM)
        {
            if (!ModelState.IsValid)
            {
                return Response(NotifyModelStateErrors());
            }

            ResultResponseObject<DividendAddVM> resultResponse = _dividendApp.AddDividend(idStock, dividendAddVM);

            return Response(resultResponse);
        }

        /// <summary>
        /// Edit Dividend
        /// </summary>
        /// <returns></returns>
        [HttpPut("{idDividend}")]
        [ProducesResponseType(200)]
        [ProducesResponseType(400)]
        public async Task<IActionResult> EditDividend(long idDividend, DividendEditVM dividendEditVM)
        {
            if (!ModelState.IsValid)
            {
                return Response(NotifyModelStateErrors());
            }

            ResultResponseObject<DividendEditVM> resultResponse = _dividendApp.EditDividend(idDividend, dividendEditVM);

            return Response(resultResponse);
        }

        /// <summary>
        /// Delete a Dividend
        /// </summary>
        /// <returns></returns>
        [HttpDelete("{idDividend}")]
        [ProducesResponseType(200)]
        [ProducesResponseType(400)]
        public async Task<IActionResult> DeleteDividend(long idDividend)
        {

            if (!ModelState.IsValid)
            {
                return Response(NotifyModelStateErrors());
            }

            ResultResponseBase resultResponseBase = _dividendApp.InactiveDividend(idDividend);

            return Response(resultResponseBase);
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

            ResultResponseObject<IEnumerable<NextDividendVM>> resultServiceObject = _dividendApp.GetNextDividendsByLoggedUser();

            return Response(resultServiceObject);
        }

        /// <summary>
        /// Get Dividend Details
        /// </summary>
        /// <returns></returns>
        [HttpGet("dividenddetails/{guidPortfolioSub}")]
        [ProducesResponseType(200)]
        [ProducesResponseType(400)]
        public async Task<IActionResult> GetDividendDetails(Guid guidPortfolioSub, [FromQuery] string startDate, [FromQuery] string endDate)
        {
            if (!ModelState.IsValid)
            {
                return Response(NotifyModelStateErrors());
            }

            ResultResponseBase resultResponse = _dividendApp.GetDividendView(guidPortfolioSub, startDate, endDate);

            return Response(resultResponse);
        }

        /// <summary>
        /// Get Dividend Yield List
        /// </summary>
        /// <returns></returns>
        [HttpGet("dividendyieldlist/{guidPortfolioSub}")]
        [ProducesResponseType(200)]
        [ProducesResponseType(400)]
        public async Task<IActionResult> GetDividendYieldList(Guid guidPortfolioSub, [FromQuery] string startDate, [FromQuery] string endDate, [FromQuery] long? idStock = null, [FromQuery] int? idStockType = null)
        {
            if (!ModelState.IsValid)
            {
                return Response(NotifyModelStateErrors());
            }

            ResultResponseBase resultResponse = _dividendApp.GetDividendYieldList(guidPortfolioSub, startDate, endDate, idStock, idStockType);

            return Response(resultResponse);
        }

        /// <summary>
        /// Get Dividend List
        /// </summary>
        /// <returns></returns>
        [HttpGet("dividendlist/{guidPortfolioSub}")]
        [ProducesResponseType(200)]
        [ProducesResponseType(400)]
        public async Task<IActionResult> GetDividendList(Guid guidPortfolioSub, [FromQuery] int year, [FromQuery] int month, [FromQuery] long? idStock = null, [FromQuery] int? idStockType = null)
        {
            if (!ModelState.IsValid)
            {
                return Response(NotifyModelStateErrors());
            }

            ResultResponseBase resultResponse = _dividendApp.GetDividendList(guidPortfolioSub, year, month, idStock, idStockType);

            return Response(resultResponse);
        }

        /// <summary>
        /// Get Ranking Div Yield
        /// </summary>
        /// <returns></returns>
        [HttpGet("rankingdividendyield/{guidPortfolioSub}")]
        [ProducesResponseType(200)]
        [ProducesResponseType(400)]
        public async Task<IActionResult> GetRankingDividendYield(Guid guidPortfolioSub, [FromQuery] int year, [FromQuery] int? month, [FromQuery] long? idStock = null, [FromQuery] int? idStockType = null)
        {
            if (!ModelState.IsValid)
            {
                return Response(NotifyModelStateErrors());
            }

            ResultResponseBase resultResponse = _dividendApp.GetRankingDividendYield(guidPortfolioSub, year, month, idStock, idStockType);

            return Response(resultResponse);
        }

        /// <summary>
        /// Restore Dividends
        /// </summary>
        /// <returns></returns>
        [HttpPut("restoredividends/{guidportfolio}")]
        [ProducesResponseType(200)]
        [ProducesResponseType(400)]
        public async Task<IActionResult> RestoreDividends(Guid guidportfolio)
        {
            if (!ModelState.IsValid)
            {
                return Response(NotifyModelStateErrors());
            }

            ResultResponseBase resultResponse = _dividendApp.RestorePastDividends(guidportfolio, null);

            return Response(resultResponse);
        }
    }
}
