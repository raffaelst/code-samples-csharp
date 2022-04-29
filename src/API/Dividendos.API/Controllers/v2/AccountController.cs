using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using K.MailSender;
using Dividendos.API.Model.Messages;
using Dividendos.API.Model.Request;
using Dividendos.API.Model.Request.Auth;
using Dividendos.API.Model.Response.Common;
using Dividendos.Application.Interface;
using Dividendos.CrossCutting.Identity.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace Dividendos.API.Controllers.v2
{
    [Authorize("Bearer")]
    [ApiVersion("2")]
    [Route("v{version:apiVersion}/[controller]")]
    [ApiController]
    public class AccountController : BaseController
    {
        private readonly IUserApp _userApp;

        public AccountController(IUserApp userApp)
        {
            _userApp = userApp;
        }

        [HttpPut]
        [ProducesResponseType(200)]
        [ProducesResponseType(400)]
        public async Task<IActionResult> Put([FromBody] Model.Request.v2.User.UserVM userVM)
        {
            if (!ModelState.IsValid)
            {
                return Response(NotifyModelStateErrors());
            }

            ResultResponseBase resultResponseBase = _userApp.ChangeUserData(userVM);

            return Response(resultResponseBase);
        }
    }
}