using AutoMapper;
using Dividendos.API.Model.PurchaseAPI;
using Dividendos.API.Model.Request.Purchase;
using Dividendos.API.Model.Response;
using Dividendos.API.Model.Response.Common;
using Dividendos.API.Model.Response.Purchase;
using Dividendos.Application.Base;
using Dividendos.Application.Interface;
using Dividendos.CrossCutting.Identity.Models;
using Dividendos.Entity.Entities;
using Dividendos.Entity.Enum;
using Dividendos.Entity.Model;
using Dividendos.Repository.Interface.UoW;
using Dividendos.Service.Interface;
using K.Logger;
using Microsoft.AspNetCore.Identity;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Security.Cryptography.X509Certificates;
using System.Threading.Tasks;

namespace Dividendos.Application
{
    public class HealthCheckApp : BaseApp, IHealthCheckApp
    {
        private readonly IUnitOfWork _uow;
        private readonly ILogger _logger;
        private readonly IHealthCheckService _healthCheckService;
        public HealthCheckApp(IUnitOfWork uow,
            ILogger logger,
            IHealthCheckService healthCheckService
           )
        {
            _uow = uow;
            _logger = logger;
            _healthCheckService = healthCheckService;
        }

        public ResultResponseObject<bool> GetStatus(string token)
        {
            ResultResponseObject<bool> resultResponseObject = new ResultResponseObject<bool>() { Success = false };

            if (token.Equals("7f173390-8878-4631-a743-7d7ff3b32b08"))
            {
                using (_uow.Create())
                {
                    ResultServiceObject<bool> resultServiceObject = _healthCheckService.GetStatus();


                    if (resultServiceObject.Value == true)
                    {
                        resultResponseObject.Success = true;
                        resultResponseObject.Value = true;
                    }
                }
            }
            else
            {
                _logger.SendInformationAsync(new { IAPHubWebhooks = "Tentativa de acesso ao HealthCheck sem token de acesso" });
            }

            return resultResponseObject;
        }
    }
}