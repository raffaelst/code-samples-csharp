using Dividendos.API.Model.Common.Interface;
using Dividendos.API.Model.Response.Common;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Linq;

namespace Dividendos.API.Controllers
{
    public class BaseController : Controller
    {

        protected new IActionResult Response(IResultResponse resultResponse = null)
        {
            if (resultResponse != null && resultResponse.Success)
            {
                resultResponse.ErrorMessages = null;

                return Ok(resultResponse);
            }

            return BadRequest(resultResponse);
        }

        protected IResultResponse NotifyModelStateErrors()
        {
            ResultResponseBase resultResponseModel = new ResultResponseBase();

            var erros = ModelState.Values.SelectMany(v => v.Errors);

            if (erros != null)
            {
                if (resultResponseModel.ErrorMessages == null)
                {
                    resultResponseModel.ErrorMessages = new List<string>();
                }

                resultResponseModel.ErrorMessages.ToList().AddRange(erros.Select(x => x.ErrorMessage));
            }
            else
            {
                resultResponseModel.ErrorMessages = null;
            }

            return resultResponseModel;
        }
    }
}
