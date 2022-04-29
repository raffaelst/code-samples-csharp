using Dapper;
using Dividendos.Entity.Entities;
using Dividendos.Repository.GenericRepository;
using Dividendos.Repository.Interface;
using Dividendos.Repository.Interface.UoW;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Dividendos.Repository.Repository
{
    public class AdvertiserExternalDetailRepository : Repository<AdvertiserExternalDetail>, IAdvertiserExternalDetailRepository
    {
        private IUnitOfWork _unitOfWork;

        public AdvertiserExternalDetailRepository(IUnitOfWork unitOfWork) : base(unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public IEnumerable<AdvertiserExternalDetail> GetByAdvertiser(string advertiserGuid)
        {
            string sql = @"SELECT AdvertiserExternalDetail.AdvertiserExternalDetailID,
                AdvertiserExternalDetail.ContentHTML, AdvertiserExternalDetail.AdvertiserExternalID,
                AdvertiserExternalDetail.URL, AdvertiserExternalDetail.CallToActionButtonTitle  FROM AdvertiserExternalDetail
                INNER JOIN AdvertiserExternal ON AdvertiserExternal.AdvertiserExternalID = AdvertiserExternalDetail.AdvertiserExternalID
                WHERE AdvertiserExternal.AdvertiserExternalGuid = @AdvertiserExternalGuid";

            var advertiser = _unitOfWork.Connection.Query<AdvertiserExternalDetail>(sql, new { AdvertiserExternalGuid = advertiserGuid }, _unitOfWork.Transaction);

            return advertiser;
        }

    }
}
