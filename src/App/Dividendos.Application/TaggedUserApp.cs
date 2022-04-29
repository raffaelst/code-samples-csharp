using Dividendos.Bacen.Interface;
using Dividendos.Bacen.Interface.Model;
using Dividendos.Repository.Interface.UoW;
using Dividendos.Service.Interface;
using System.Linq;
using System.Collections.Generic;
using System.Text;
using Dividendos.Entity.Model;
using Dividendos.Entity.Entities;
using System;
using System.Globalization;
using K.Logger;
using Dividendos.Application.Interface;
using System.Threading.Tasks;
using Dividendos.API.Model.Response.Common;
using Dividendos.API.Model.Response;
using AutoMapper;
using Dividendos.Entity.Enum;
using Dividendos.Entity.Views;
using Microsoft.Extensions.Caching.Distributed;
using Newtonsoft.Json;
using Dividendos.API.Model.Request.Goals;
using Dividendos.API.Model.Request;
using Dividendos.RDStation.Interface;

namespace Dividendos.Application
{
    public class TaggedUserApp : ITaggedUserApp
    {
        private readonly IUnitOfWork _uow;
        private readonly ILogger _logger;
        private readonly IMapper _mapper;
        private readonly IGlobalAuthenticationService _globalAuthenticationService;
        private readonly ITaggedUserService _taggedUser;
        private readonly ICacheService _cacheService;
        private readonly IRDStationHelper _rDStationHelper;

        public TaggedUserApp(IUnitOfWork uow,
                            ILogger logger,
                            IMapper mapper,
                            IGlobalAuthenticationService globalAuthenticationService,
                            ICacheService cacheService,
                            ITaggedUserService taggedUser,
                            IRDStationHelper rDStationHelper)
        {
            _uow = uow;
            _logger = logger;
            _mapper = mapper;
            _globalAuthenticationService = globalAuthenticationService;
            _cacheService = cacheService;
            _taggedUser = taggedUser;
            _rDStationHelper = rDStationHelper;
        }

        public ResultResponseObject<TaggedUserVM> Add(TaggedUserVM taggedUserVM)
        {
            ResultResponseObject<TaggedUserVM> result = new ResultResponseObject<TaggedUserVM>();

            using (_uow.Create())
            {
                TaggedUser taggedUser = _mapper.Map<TaggedUser>(taggedUserVM);
                taggedUser.UserID = _globalAuthenticationService.IdUser;
                ResultServiceObject<TaggedUser> resultServiceObject = _taggedUser.Add(taggedUser);

                result = _mapper.Map<ResultResponseObject<TaggedUserVM>>(resultServiceObject);
                _rDStationHelper.SendEvent(null, _globalAuthenticationService.Email, _globalAuthenticationService.IdUser, null, taggedUserVM.TagValue);
            }

            return result;
        }
    }
}
