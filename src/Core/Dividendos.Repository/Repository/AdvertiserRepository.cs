using Dapper;
using Dividendos.Entity.Entities;
using Dividendos.Repository.GenericRepository;
using Dividendos.Repository.Interface;
using Dividendos.Repository.Interface.UoW;
using System;
using System.Collections.Generic;
using System.Text;

namespace Dividendos.Repository.Repository
{
    public class AdvertiserRepository : Repository<Advertiser>, IAdvertiserRepository
    {
        private IUnitOfWork _unitOfWork;

        public AdvertiserRepository(IUnitOfWork unitOfWork) : base(unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }


        public IEnumerable<Advertiser> GetGeneralAndByUser(string userId, DateTime dateTimeShowOffSet)
        {
            string sql = @"SELECT TOP 1 Advertiser.* FROM Advertiser
				LEFT JOIN AdvertiserUser ON AdvertiserUser.AdvertiserID = Advertiser.AdvertiserID
                WHERE  Advertiser.Active = @Active
				UNION ALL 
                SELECT TOP 1 Advertiser.* FROM Advertiser
				LEFT JOIN AdvertiserUser ON AdvertiserUser.AdvertiserID = Advertiser.AdvertiserID
                WHERE AdvertiserUser.UserID = @IdUser AND AdvertiserUser.ShowUntil >= @ShowUntil AND Advertiser.Active = @Active";

            var advertiser = _unitOfWork.Connection.Query<Advertiser>(sql, new { IdUser = userId, Active = true, ShowUntil = dateTimeShowOffSet }, _unitOfWork.Transaction);

            return advertiser;
        }

        public IEnumerable<Advertiser> GetOnlyGeneral()
        {
            string sql = @"SELECT TOP 1 Advertiser.* FROM Advertiser
				LEFT JOIN AdvertiserUser ON AdvertiserUser.AdvertiserID = Advertiser.AdvertiserID 
                WHERE  Advertiser.Active = @Active  AND AdvertiserUser.AdvertiserUserID IS NULL ORDER BY AdvertiserID DESC";

            var advertiser = _unitOfWork.Connection.Query<Advertiser>(sql, new { Active = true }, _unitOfWork.Transaction);

            return advertiser;
        }
    }
}
