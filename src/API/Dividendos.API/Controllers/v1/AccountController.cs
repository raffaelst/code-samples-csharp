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

namespace Dividendos.API.Controllers.v1
{
    [Authorize("Bearer")]
    [Route("[controller]")]
    [ApiController]
    public class AccountController : BaseController
    {
        private readonly IUserApp _userApp;


        public AccountController(IUserApp userApp)
        {
            _userApp = userApp;
        }

        /// <summary>
        /// Recovery password
        /// </summary>
        /// <param name="email"></param>
        /// <returns></returns>
        [AllowAnonymous]
        [HttpPost]
        [Route("recovery")]
        public async Task<IActionResult> RecoveryPassword([FromBody]RecoveryPassword recoveryPassword)
        {
            if (!ModelState.IsValid)
            {
                return Response(NotifyModelStateErrors());
            }

            ResultResponseStringModel resultService = new ResultResponseStringModel();

            resultService =  _userApp.RecoveryPassword(recoveryPassword.Email);

            return Response(resultService);
        }


        /// <summary>
        /// Reset password
        /// </summary>
        /// <param name="token"></param>
        /// <param name="email"></param>
        /// <param name="newPassword"></param>
        /// <returns></returns>
        [AllowAnonymous]
        [HttpPost]
        [Route("reset")]
        public async Task<IActionResult> ResetPassword([FromBody]ResetPassword resetPassword)
        {
            if (!ModelState.IsValid)
            {
                return Response(NotifyModelStateErrors());
            }

            ResultResponseStringModel resultService = new ResultResponseStringModel();

            resultService =  _userApp.ResetPassword(resetPassword.Email, resetPassword.Token, resetPassword.NewPassword);

            return Response(resultService);
        }


        /// <summary>
        /// Register a new user on dividendos.me
        /// </summary>
        /// <param name="userRegister"></param>
        /// <returns></returns>
        [AllowAnonymous]
        [HttpPost]
        [ProducesResponseType(200)]
        [ProducesResponseType(400)]
        public async Task<IActionResult> Post([FromBody]UserRegisterVM userRegister)
        {
            if (!ModelState.IsValid)
            {
                return Response(NotifyModelStateErrors());
            }

            ResultResponseStringModel resultResponseStringModel = _userApp.RegisterNewUser(userRegister);

            return Response(resultResponseStringModel);
        }

        /// <summary>
        /// Register a new user on dividendos.me (using a affiliate)
        /// </summary>
        /// <param name="userRegister"></param>
        /// <returns></returns>
        [AllowAnonymous]
        [HttpPost("affiliate-program")]
        [ProducesResponseType(200)]
        [ProducesResponseType(400)]
        public async Task<IActionResult> CreateFromAffiliate([FromBody]UserRegisterAffiliateVM userRegister)
        {
            if (!ModelState.IsValid)
            {
                return Response(NotifyModelStateErrors());
            }

            ResultResponseStringModel resultResponseStringModel = _userApp.RegisterNewUserFromAffiliate(userRegister);

            return Response(resultResponseStringModel);
        }

        // <summary>
        // Get account details by logged user
        // </summary>
        // <returns></returns>
        [HttpGet]
        [ProducesResponseType(200)]
        [ProducesResponseType(400)]
        public async Task<IActionResult> Get()
        {
            if (!ModelState.IsValid)
            {
                return Response(NotifyModelStateErrors());
            }

            ResultResponseObject<UserVM> resultResponse = _userApp.GetAccountDetails();

            return Response(resultResponse);
        }

        /// <summary>
        /// Change password of user
        /// </summary>
        /// <param name="changePassword"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("change-password")]
        public async Task<IActionResult> ChangePassword([FromBody]ChangePassword changePassword)
        {
            if (!ModelState.IsValid)
            {
                return Response(NotifyModelStateErrors());
            }

            ResultResponseStringModel resultService = new ResultResponseStringModel();

            resultService = _userApp.ChangePassword(changePassword.CurrentPassword, changePassword.NewPassword);

            return Response(resultService);
        }

        [HttpPut]
        [ProducesResponseType(200)]
        [ProducesResponseType(400)]
        public async Task<IActionResult> Put([FromBody] UserVM userVM)
        {
            if (!ModelState.IsValid)
            {
                return Response(NotifyModelStateErrors());
            }

            ResultResponseBase resultResponseBase = _userApp.ChangeUserName(userVM.Name);

            return Response(resultResponseBase);
        }

        // <summary>
        // delete logged account
        // </summary>
        // <returns></returns>
        [HttpDelete]
        [ProducesResponseType(200)]
        [ProducesResponseType(400)]
        public async Task<IActionResult> Delete()
        {
            if (!ModelState.IsValid)
            {
                return Response(NotifyModelStateErrors());
            }

            ResultResponseBase resultResponse = _userApp.DeleteAccount(null);

            return Response(resultResponse);
        }

        // <summary>
        // delete specific account
        // </summary>
        // <returns></returns>
        [HttpDelete("{userID}/delete")]
        [Authorize(Roles = "Administrator,Suporte,Interns")]
        [ProducesResponseType(200)]
        [ProducesResponseType(400)]
        public async Task<IActionResult> DeleteSpecific(string userID)
        {
            if (!ModelState.IsValid)
            {
                return Response(NotifyModelStateErrors());
            }

            ResultResponseBase resultResponse = _userApp.DeleteAccount(userID);

            return Response(resultResponse);
        }
    }
}