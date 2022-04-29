using Dividendos.Entity.Entities;
using Dividendos.Entity.Model;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Dividendos.Service.Interface
{
    public interface IDeviceService : IBaseService
    {
        ResultServiceObject<Device> Add(Device device);
        ResultServiceObject<IEnumerable<Device>> GetByUser(string idUser);

        ResultServiceObject<IEnumerable<Device>> GetByTokenPush(string tokenPush);

        void Inactivate(Device device);

        ResultServiceObject<Device> GetById(long id);

        ResultServiceObject<Device> Update(Device device);

        ResultServiceObject<IEnumerable<Device>> GetAdminDevices();
        ResultServiceObject<IEnumerable<Device>> GetAllNewVersion();
        ResultServiceObject<IEnumerable<Device>> GetByUserAndOffSetVersion(string idUser, string version);
        ResultServiceObject<Device> GetByUserAndDeviceUniqueID(string idUser, string deviceUniqueID);
    }
}
