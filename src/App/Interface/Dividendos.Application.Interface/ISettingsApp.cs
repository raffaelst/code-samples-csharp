using Dividendos.API.Model.Request.Settings;
using Dividendos.API.Model.Response.Common;
using Dividendos.Entity.Model;
using System.Threading.Tasks;

namespace Dividendos.Application.Interface
{
    public interface ISettingsApp
    {
        ResultResponseBase ChangeSettings(SettingsEditVM settingsEditVM);
        ResultResponseBase ChangeSettingsV2(SettingsEditVM settingsEditVM);
        ResultResponseObject<SettingsEditVM> Get();
    }
}
