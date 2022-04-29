using Dividendos.API.Model.Request.Settings;
using Dividendos.API.Model.Response;
using Dividendos.API.Model.Response.Common;
using Dividendos.API.Model.Response.v1;
using Dividendos.API.Model.Response.v1.ResearchProduct;
using Dividendos.Entity.Entities;
using Dividendos.Entity.Model;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Dividendos.Application.Interface
{
    public interface IResearchProductApp
    {
        ResultResponseObject<ResearchProductWrapperVM> GetAllActive();
    }
}
