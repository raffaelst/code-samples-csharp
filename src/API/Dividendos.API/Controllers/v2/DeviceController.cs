using Dividendos.API.Model.Request;
using Dividendos.API.Model.Request.Device;
using Dividendos.API.Model.Response.Common;
using Dividendos.Application.Interface;
using Dividendos.CrossCutting.Identity.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace Dividendos.API.Controllers.v2
{
    [Authorize("Bearer")]
    [ApiVersion("2")]
    [Route("v{version:apiVersion}/[controller]")]
    [ApiController]
    public class DeviceController : BaseController
    {
        private readonly IDeviceApp _deviceApp;

        public DeviceController(IDeviceApp deviceApp)
        {
            _deviceApp = deviceApp;
        }


        [HttpPost]
        [ProducesResponseType(200)]
        [ProducesResponseType(400)]
        public async Task<IActionResult> AddNewDevice([FromBody]DeviceVM deviceAdd)
        {

            if (!ModelState.IsValid)
            {
                return Response(NotifyModelStateErrors());
            }

            ResultResponseBase resultResponseBase = _deviceApp.AddNewTokenGoogle(deviceAdd);

            return Response(resultResponseBase);
        }
    }
}
