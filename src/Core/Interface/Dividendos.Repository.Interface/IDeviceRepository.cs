using Dividendos.Entity.Entities;
using Dividendos.Repository.Interface.GenericRepository;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Dividendos.Repository.Interface
{
    public interface IDeviceRepository : IRepository<Device>
    {
        IEnumerable<Device> GetAdminDevices();

        IEnumerable<Device> GetByUserAndOffSetVersion(string idUser, int version);

        Device GetByUserAndDeviceUniqueID(string idUser, string deviceUniqueID);
    }
}
