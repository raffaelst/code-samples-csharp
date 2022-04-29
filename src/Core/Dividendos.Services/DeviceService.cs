using FluentValidation.Results;
using Dividendos.Entity.Entities;
using Dividendos.Entity.Model;
using Dividendos.Repository.Interface;
using Dividendos.Repository.Interface.UoW;
using Dividendos.Service.Interface;
using Dividendos.Service.Validator;
using Dividendos.Service.Validator.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Dividendos.Service
{
    public class DeviceService : BaseService, IDeviceService
    {
        public DeviceService(IUnitOfWork uow)
        {
            _uow = uow;
        }

        public ResultServiceObject<Device> Add(Device device)
        {
            DeviceValidator validator = new DeviceValidator();
            ValidationResult results = validator.Validate(device);

            ResultServiceObject<Device> resultService = new ResultServiceObject<Device>(results, _uow);

            if (results.IsValid)
            {
                device.LastUpdatedDate = DateTime.Now;
                device.GuidDevice = Guid.NewGuid();
                device.Active = true;
                device.IdDevice = _uow.DeviceRepository.Insert(device);
                resultService.Value = device;
            }

            return resultService;
        }


        public void Inactivate(Device device)
        {
            device.Active = false;
            _uow.DeviceRepository.Update(device);
        }

        public ResultServiceObject<IEnumerable<Device>> GetByTokenPush(string tokenPush)
        {
            StringValidador stringValidador = new StringValidador();
            ValidationResult results = stringValidador.Validate(tokenPush);

            ResultServiceObject<IEnumerable<Device>> resultService = new ResultServiceObject<IEnumerable<Device>>(results, _uow);

            IEnumerable<Device> devices = _uow.DeviceRepository.Select(item => item.PushToken == tokenPush && item.Active == true);

            resultService.Value = devices;

            return resultService;
        }

        public ResultServiceObject<IEnumerable<Device>> GetByUser(string idUser)
        {
            StringValidador stringValidador = new StringValidador();
            ValidationResult results = stringValidador.Validate(idUser);

            ResultServiceObject<IEnumerable<Device>> resultService = new ResultServiceObject<IEnumerable<Device>>(results, _uow);


            IEnumerable<Device> devices = _uow.DeviceRepository.Select(item => item.IdUser == idUser && item.Active == true);

            resultService.Value = devices;

            return resultService;
        }

        public ResultServiceObject<Device> GetByUserAndDeviceUniqueID(string idUser, string deviceUniqueID)
        {
            StringValidador stringValidador = new StringValidador();
            ValidationResult results = stringValidador.Validate(idUser);

            ResultServiceObject<Device> resultService = new ResultServiceObject<Device>(results, _uow);


            Device device = _uow.DeviceRepository.GetByUserAndDeviceUniqueID(idUser, deviceUniqueID);

            resultService.Value = device;

            return resultService;
        }

        public ResultServiceObject<IEnumerable<Device>> GetByUserAndOffSetVersion(string idUser, string version)
        {
            StringValidador stringValidador = new StringValidador();
            ValidationResult results = stringValidador.Validate(idUser);

            ResultServiceObject<IEnumerable<Device>> resultService = new ResultServiceObject<IEnumerable<Device>>(results, _uow);


            IEnumerable<Device> devices = _uow.DeviceRepository.GetByUserAndOffSetVersion(idUser, int.Parse(version.Replace(".", "")));

            resultService.Value = devices;

            return resultService;
        }

        public ResultServiceObject<Device> GetById(long id)
        {
            IdValidator idValidator = new IdValidator();
            ValidationResult results = idValidator.Validate(id);

            ResultServiceObject<Device> resultService = new ResultServiceObject<Device>(results, _uow);


            Device device = _uow.DeviceRepository.GetById(id);

            resultService.Value = device;

            return resultService;
        }


        public ResultServiceObject<Device> Update(Device device)
        {
            IdValidator idValidator = new IdValidator();
            ValidationResult results = idValidator.Validate(device.IdDevice);

            ResultServiceObject<Device> resultService = new ResultServiceObject<Device>(results, _uow);

            Device deviceUpdated = _uow.DeviceRepository.Update(device);

            resultService.Value = deviceUpdated;

            return resultService;
        }

        public ResultServiceObject<IEnumerable<Device>> GetAdminDevices()
        {
            ResultServiceObject<IEnumerable<Device>> resultService = new ResultServiceObject<IEnumerable<Device>>();
            IEnumerable<Device> devices = _uow.DeviceRepository.GetAdminDevices();

            resultService.Value = devices;

            return resultService;
        }

        public ResultServiceObject<IEnumerable<Device>> GetAllNewVersion()
        {
            ResultServiceObject<IEnumerable<Device>> resultService = new ResultServiceObject<IEnumerable<Device>>();

            IEnumerable<Device> devices = _uow.DeviceRepository.Select(item => item.Active == true && item.AppVersion != null);

            resultService.Value = devices;

            return resultService;
        }
    }
}
