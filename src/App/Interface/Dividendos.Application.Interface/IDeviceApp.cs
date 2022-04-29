using Dividendos.API.Model.Request.Device;
using Dividendos.API.Model.Response.Common;
using System.Threading.Tasks;

namespace Dividendos.Application.Interface
{
    public interface IDeviceApp
    {
        ResultResponseBase AddNew(DeviceVM deviceAdd);
        ResultResponseBase AddNewTokenGoogle(DeviceVM deviceAdd);
    }
}
