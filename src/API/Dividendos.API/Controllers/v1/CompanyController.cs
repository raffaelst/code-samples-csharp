using Dividendos.API.Model.Response;
using Dividendos.API.Model.Response.Common;
using Dividendos.API.Model.Response.Company;
using Dividendos.Application.Interface;
using Dividendos.Application.Interface.Model;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Dividendos.API.Controllers.v1
{
    [Authorize("Bearer")]
    [Route("[controller]")]
    [ApiController]
    public class CompanyController : BaseController
    {
        private readonly ICompanyApp _companyApp;

        public CompanyController(ICompanyApp companyApp)
        {
            _companyApp = companyApp;
        }

        /// <summary>
        /// Get company by name
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [ProducesResponseType(200)]
        [ProducesResponseType(400)]
        public async Task<IActionResult> GetByName(string name)
        {
            if (!ModelState.IsValid)
            {
                return Response(NotifyModelStateErrors());
            }

            ResultResponseObject<IEnumerable<CompanyVM>> resultResponse = _companyApp.GetByName(name);

            return Response(resultResponse);
        }

        /// <summary>
        /// Update Company Logo
        /// </summary>
        /// <returns></returns>
        [HttpPut("updatecompanieslogo")]
        [Authorize(Roles = "Administrator")]
        [ProducesResponseType(200)]
        [ProducesResponseType(400)]
        public async Task<IActionResult> UpdateComapniesLogo()
        {
            if (!ModelState.IsValid)
            {
                return Response(NotifyModelStateErrors());
            }

            await _companyApp.UpdateCompanyLogo();

            return Response();
        }


        /// <summary>
        /// Generate Company Logo
        /// </summary>
        /// <returns></returns>
        [HttpPut("generatelogo")]
        [Authorize(Roles = "Administrator")]
        [ProducesResponseType(200)]
        [ProducesResponseType(400)]
        public async Task<IActionResult> GenerateLogo()
        {
            if (!ModelState.IsValid)
            {
                return Response(NotifyModelStateErrors());
            }

            await _companyApp.Generatefiles();

            return Response();
        }

    }
}
