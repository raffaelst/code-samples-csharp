using AutoMapper;
using Dividendos.API.Model.Request;
using Dividendos.API.Model.Response.Common;
using Dividendos.Application.Base;
using Dividendos.Application.Interface;
using Dividendos.CrossCutting.Identity.Models;
using Dividendos.Entity.Entities;
using Dividendos.Entity.Model;
using Dividendos.Repository.Interface.UoW;
using Microsoft.AspNetCore.Identity;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Linq;
using Dividendos.API.Model.Messages;
using Dividendos.Service.Interface;

namespace Dividendos.Application
{
    public class DashboardApp : BaseApp, IDashboardApp
    {
        private readonly IUserService _userService;
        private readonly IMapper _mapper;

        private readonly IUnitOfWork _uow;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IGlobalAuthenticationService _globalAuthenticationService;

        public DashboardApp(IUserService userService,
            IMapper mapper,
            IUnitOfWork uow,
            UserManager<ApplicationUser> userManager, 
            IGlobalAuthenticationService globalAuthenticationService)
        {
            _userService = userService;
            _mapper = mapper;
            _uow = uow;
            _userManager = userManager;
            _globalAuthenticationService = globalAuthenticationService;
        }

        public ResultResponseObject<UserVM> GetDashboardDetails()
        {
            ResultResponseObject<UserVM> resultResponse = new ResultResponseObject<UserVM>();

            using (_uow.Create())
            {

                ResultServiceObject<ApplicationUser> resultServiceAspNetUser = _userService.GetAccountDetails(_globalAuthenticationService.IdUser);
                resultResponse = _mapper.Map<ResultResponseObject<UserVM>>(resultServiceAspNetUser);
            }


            return resultResponse;
        }

    }
}