using Dividendos.API.Model.Request.Settings;
using Dividendos.API.Model.Response;
using Dividendos.API.Model.Response.Common;
using Dividendos.Entity.Model;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Dividendos.Application.Interface
{
    public interface ISupportChannelApp
    {
        ResultResponseObject<IEnumerable<SupportChannelVM>> GetAll();
    }
}
