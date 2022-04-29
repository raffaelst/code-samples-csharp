using Dapper.Contrib.Extensions;
using Dividendos.Entity.Entities;
using Dividendos.Repository.GenericRepository;
using Dividendos.Repository.Interface;
using Dividendos.Repository.Interface.GenericRepository;
using Dividendos.Repository.Interface.UoW;
using Dapper;
using System.Linq;
using System.Collections.Generic;

namespace Dividendos.Repository.Repository
{
    public class DeviceRepository : Repository<Device>, IDeviceRepository
    {
        private IUnitOfWork _unitOfWork;

        public DeviceRepository(IUnitOfWork unitOfWork) : base(unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public IEnumerable<Device> GetAdminDevices()
        {
            string sql = @"select Device.IdDevice,Device.IdUser,Device.PushToken,Device.LastUpdatedDate,Device.GuidDevice,Device.Name, Device.PushTokenFirebase, Device.UniqueId from Device
                            inner join AspNetUsers on AspNetUsers.Id = Device.IdUser
                            inner join AspNetUserRoles on AspNetUserRoles.UserId = AspNetUsers.Id
                            inner join AspNetRoles on AspNetRoles.Id = AspNetUserRoles.RoleId
                            where AspNetRoles.Name = 'Administrator'  and Device.Active = 1";

            IEnumerable<Device> dividends = _unitOfWork.Connection.Query<Device>(sql, null, _unitOfWork.Transaction);

            return dividends;
        }


        public IEnumerable<Device> GetByUserAndOffSetVersion(string idUser, int version)
        {
            string sql = @"select Device.IdDevice,Device.IdUser,Device.PushToken,Device.LastUpdatedDate,Device.GuidDevice,Device.Name, Device.PushTokenFirebase, Device.AppVersion, Device.Active, Device.UniqueId from Device
                            where CAST(REPLACE(AppVersion, '.', '') AS INT) > @Version AND IdUser = @IdUser AND Active = 1";

            IEnumerable<Device> dividends = _unitOfWork.Connection.Query<Device>(sql, new { IdUser = idUser, Version = version }, _unitOfWork.Transaction);

            return dividends;
        }

        public Device GetByUserAndDeviceUniqueID(string idUser, string deviceUniqueID)
        {
            string sql = @"select Device.IdDevice,Device.IdUser,Device.PushToken,Device.LastUpdatedDate,Device.GuidDevice,Device.Name, Device.PushTokenFirebase, Device.AppVersion, Device.Active, Device.UniqueId from Device
                            where IdUser = @IdUser AND Active = 1 AND Device.UniqueId = @DeviceUniqueID";

            Device dividends = _unitOfWork.Connection.Query<Device>(sql, new { IdUser = idUser, DeviceUniqueID = deviceUniqueID }, _unitOfWork.Transaction).FirstOrDefault();

            return dividends;
        }
    }
}
