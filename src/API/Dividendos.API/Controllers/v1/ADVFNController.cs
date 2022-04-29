using Dividendos.API.Model.Request;
using Dividendos.API.Model.Request.DividendCalendar;
using Dividendos.API.Model.Request.Stock;
using Dividendos.API.Model.Response;
using Dividendos.API.Model.Response.Common;
using Dividendos.API.Model.Response.v1;
using Dividendos.Application.Interface;
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
    public class ADVFNController : BaseController
    {
        private readonly IDividendCalendarApp _dividendCalendarApp;

        public ADVFNController(IDividendCalendarApp dividendCalendarApp)
        {
            _dividendCalendarApp = dividendCalendarApp;
        }

        [AllowAnonymous]
        [HttpGet]
        [ProducesResponseType(200)]
        [ProducesResponseType(400)]
        public async Task<IActionResult> Get([FromHeader(Name = "X-Auth-Token")] string token, [FromQuery] int year, [FromQuery] CountryType countryType, [FromQuery] DividendCalendarType dividendCalendarType)
        {
            if (!ModelState.IsValid)
            {
                return Response(NotifyModelStateErrors());
            }

            ResultResponseObject<IEnumerable<DividendCalendarVM>> resultServiceObject = _dividendCalendarApp.GetListByYear(token, year, countryType, dividendCalendarType);

            return Response(resultServiceObject);
        }
    }
}
