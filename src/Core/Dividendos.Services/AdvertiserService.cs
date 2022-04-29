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
    public class AdvertiserService : BaseService, IAdvertiserService
    {
        public AdvertiserService(IUnitOfWork uow)
        {
            _uow = uow;
        }

        public ResultServiceObject<Advertiser> Get()
        {
            ResultServiceObject<Advertiser> resultService = new ResultServiceObject<Advertiser>();

            Advertiser advertiser = _uow.AdvertiserRepository.GetOnlyGeneral().FirstOrDefault();

            resultService.Value = advertiser;

            return resultService;
        }


        public ResultServiceObject<AdvertiserDetails> GetDetails(string advertiserGuid)
        {
            ResultServiceObject<AdvertiserDetails> resultService = new ResultServiceObject<AdvertiserDetails>();

            AdvertiserDetails advertiserDetails = _uow.AdvertiserDetailsRepository.GetByAdvertiser(advertiserGuid);

            resultService.Value = advertiserDetails;

            return resultService;
        }


        public ResultServiceObject<IEnumerable<Advertiser>> GetGeneralAndByUser(string userId)
        {
            ResultServiceObject<IEnumerable<Advertiser>> resultService = new ResultServiceObject<IEnumerable<Advertiser>>();

            IEnumerable<Advertiser> advertiser = _uow.AdvertiserRepository.GetGeneralAndByUser(userId, DateTime.Now);

            resultService.Value = advertiser;

            return resultService;
        }
    }
}
