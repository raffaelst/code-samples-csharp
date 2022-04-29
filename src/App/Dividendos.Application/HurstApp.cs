using Dividendos.Repository.Interface.UoW;
using Dividendos.Service.Interface;
using Dividendos.Application.Interface;
using AutoMapper;
using Dividendos.Hurst.Interface;
using Dividendos.API.Model.Request;

namespace Dividendos.Application
{
    public class HurstApp : IHurstApp
    {
        private readonly IUnitOfWork _uow;
        private readonly IMapper _mapper;
        private readonly IGlobalAuthenticationService _globalAuthenticationService;
        private readonly IUserService _userService;
        private readonly IHurstHelper _hurstHelper;

        public HurstApp(IUnitOfWork uow,
                            IMapper mapper,
                            IGlobalAuthenticationService globalAuthenticationService,
                            IUserService userService,
                            IHurstHelper hurstHelper)
        {
            _uow = uow;
            _mapper = mapper;
            _globalAuthenticationService = globalAuthenticationService;
            _userService = userService;
            _hurstHelper = hurstHelper;
        }

        public void SendEvent(DataVM eventData)
        {
            using (_uow.Create())
            {
                var user = _userService.GetByID(_globalAuthenticationService.IdUser);

                string phoneNumber = null;

                if (string.IsNullOrEmpty(user.Value.PhoneNumber))
                {
                    phoneNumber = eventData.PhoneNumber;
                }
                else
                {
                    phoneNumber = user.Value.PhoneNumber;
                }

                _hurstHelper.SendEvent(user.Value.Name, user.Value.Email, phoneNumber);
            }
        }
    }
}
