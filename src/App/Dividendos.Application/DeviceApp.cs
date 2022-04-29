using AutoMapper;
using K.Logger;
using Dividendos.API.Model.Request.Device;
using Dividendos.API.Model.Response.Common;

using Dividendos.Application.Base;
using Dividendos.Application.Interface;
using Dividendos.Entity.Entities;
using Dividendos.Entity.Model;
using Dividendos.Repository.Interface.UoW;
using Dividendos.Service.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Dividendos.Application
{
    public class DeviceApp : BaseApp, IDeviceApp
    {

        private readonly IMapper _mapper;
        private readonly IUnitOfWork _uow;
        private readonly IGlobalAuthenticationService _globalAuthenticationService;
        private readonly IDeviceService _deviceService;

        public DeviceApp(IMapper mapper,
            IUnitOfWork uow,
            IGlobalAuthenticationService globalAuthenticationService,
            IDeviceService deviceService,
            ILogger logger)
        {
            _mapper = mapper;
            _uow = uow;
            _globalAuthenticationService = globalAuthenticationService;
            _deviceService = deviceService;
        }

        public ResultResponseBase AddNew(DeviceVM deviceAdd)
        {
            Device device = _mapper.Map<Device>(deviceAdd);

            return AddNewBase(device);
        }

        public ResultResponseBase AddNewTokenGoogle(DeviceVM deviceAdd)
        {
            Device device = _mapper.Map<Device>(deviceAdd);
            device.PushToken = device.PushToken;
            device.PushTokenFirebase = device.PushToken;
            device.AppVersion = deviceAdd.AppVersion;

            return AddNewBase(device);
        }


        private ResultResponseBase AddNewBase(Device device)
        {
            ResultResponseBase resultResponseBase;
            ResultServiceObject<Device> result = new ResultServiceObject<Device>();

            using (_uow.Create())
            {
                ResultServiceObject<IEnumerable<Device>> devices = _deviceService.GetByUser(_globalAuthenticationService.IdUser);

                if (device.UniqueId != null)
                {
                    foreach (Device itemDevice in devices.Value)
                    {
                        if (itemDevice.UniqueId == null)
                        {
                            itemDevice.Active = false;
                            _deviceService.Update(itemDevice);
                        }
                    }

                    ResultServiceObject<Device> deviceFound = _deviceService.GetByUserAndDeviceUniqueID(_globalAuthenticationService.IdUser, device.UniqueId);

                    if (deviceFound.Value == null)
                    {
                        device.IdUser = _globalAuthenticationService.IdUser;
                        result = _deviceService.Add(device);
                    }
                    else
                    {
                        if (!deviceFound.Value.PushToken.Equals(device.PushToken))
                        {
                            deviceFound.Value.PushToken = device.PushToken;
                            deviceFound.Value.PushTokenFirebase = device.PushToken;
                            _deviceService.Update(deviceFound.Value);
                        }
                    }
                }
                else
                {
                    bool addNewDivice = true;
                    foreach (Device itemDevice in devices.Value)
                    {
                        if (itemDevice.PushToken.Equals(device.PushToken) ||
                            (itemDevice.PushTokenFirebase != null &&
                            itemDevice.PushTokenFirebase.Equals(device.PushToken)))
                        {
                            addNewDivice = false;
                        }
                        if (!string.IsNullOrWhiteSpace(device.AppVersion))
                        {
                            if (string.IsNullOrWhiteSpace(itemDevice.AppVersion) || (device.AppVersion != itemDevice.AppVersion))
                            {
                                itemDevice.AppVersion = device.AppVersion;
                                _deviceService.Update(itemDevice);
                            }
                        }
                    }
                    if (addNewDivice)
                    {
                        device.IdUser = _globalAuthenticationService.IdUser;
                        result = _deviceService.Add(device);
                    }
                }
            }

            resultResponseBase = _mapper.Map<ResultResponseBase>(result);

            return resultResponseBase;
        }
    }
}