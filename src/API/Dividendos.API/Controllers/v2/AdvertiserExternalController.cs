using Dividendos.API.Model.Request;
using Dividendos.API.Model.Request.Purchase;
using Dividendos.API.Model.Response;
using Dividendos.API.Model.Response.Common;
using Dividendos.API.Model.Response.Purchase;
using Dividendos.Application.Interface;
using Dividendos.CrossCutting.Identity.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Dividendos.API.Controllers.v2
{
    [Authorize("Bearer")]
    [ApiVersion("2")]
    [Route("v{version:apiVersion}/[controller]")]
    [ApiController]
    public class AdvertiserExternalController : BaseController
    {
        private readonly IAdvertiserExternalApp _advertiserExternalApp;

        public AdvertiserExternalController(IAdvertiserExternalApp advertiserExternalApp)
        {
            _advertiserExternalApp = advertiserExternalApp;
        }


        [HttpGet]
        [ProducesResponseType(200)]
        [ProducesResponseType(400)]
        public async Task<IActionResult> Get()
        {
            if (!ModelState.IsValid)
            {
                return Response(NotifyModelStateErrors());
            }

            ResultResponseObject<IEnumerable<AdvertiserExternalVM>> resultServiceObject = _advertiserExternalApp.GetV2();

            return Response(resultServiceObject);
        }

        [HttpGet("{id}/details")]
        [ProducesResponseType(200)]
        [ProducesResponseType(400)]
        public async Task<IActionResult> GetDetails(string id)
        {
            if (!ModelState.IsValid)
            {
                return Response(NotifyModelStateErrors());
            }

            ResultResponseObject<AdvertiserExternalDetailVM> resultServiceObject = _advertiserExternalApp.GetDetailsV2(id);

            return Response(resultServiceObject);
        }
    }
}
