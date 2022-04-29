using Dividendos.API.Model.Request;
using Dividendos.API.Model.Request.Affiliation;
using Dividendos.API.Model.Request.Device;
using Dividendos.API.Model.Response;
using Dividendos.API.Model.Response.Common;
using Dividendos.Application.Interface;
using Dividendos.CrossCutting.Identity.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Dividendos.API.Controllers.v1
{
    [Authorize("Bearer")]
    [Route("[controller]")]
    [ApiController]
    public class AffiliationController : BaseController
    {
        private readonly IAffiliationApp _affiliationApp;

        public AffiliationController(IAffiliationApp affiliationApp)
        {
            _affiliationApp = affiliationApp;
        }


        [HttpGet]
        [ProducesResponseType(200)]
        [ProducesResponseType(400)]
        public async Task<IActionResult> Get([FromQuery] AffiliationType affiliationType)
        {

            if (!ModelState.IsValid)
            {
                return Response(NotifyModelStateErrors());
            }

            ResultResponseObject<IEnumerable<AffiliateProductDetailVM>> resultServiceObject = _affiliationApp.GetByType(affiliationType);

            return Response(resultServiceObject);
        }
    }
}
