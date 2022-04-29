using Dividendos.API.Model.Request;
using Dividendos.API.Model.Request.Goals;
using Dividendos.API.Model.Response;
using Dividendos.API.Model.Response.Common;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Dividendos.Application.Interface
{
    public interface ITaggedUserApp
    {
        ResultResponseObject<TaggedUserVM> Add(TaggedUserVM taggedUserVM);
    }
}
