using Dividendos.Entity.Entities;
using Dividendos.Entity.Model;
using Dividendos.Repository.Interface.UoW;
using Dividendos.Service.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Dividendos.Service
{
    public class AdvertiserExternalService : BaseService, IAdvertiserExternalService
    {
        public AdvertiserExternalService(IUnitOfWork uow)
        {
            _uow = uow;
        }

        public ResultServiceObject<IEnumerable<AdvertiserExternal>> Get()
        {
            ResultServiceObject<IEnumerable<AdvertiserExternal>> resultService = new ResultServiceObject<IEnumerable<AdvertiserExternal>>();

            IEnumerable<AdvertiserExternal> advertiser = _uow.AdvertiserExternalRepository.GetAllActive();

            resultService.Value = advertiser;

            return resultService;
        }


        public ResultServiceObject<AdvertiserExternalDetail> GetDetails(string advertiserGuid)
        {
            ResultServiceObject<AdvertiserExternalDetail> resultService = new ResultServiceObject<AdvertiserExternalDetail>();

            AdvertiserExternalDetail advertiserDetails = _uow.AdvertiserExternalDetailRepository.GetByAdvertiser(advertiserGuid).FirstOrDefault();

            resultService.Value = advertiserDetails;

            return resultService;
        }
    }
}
