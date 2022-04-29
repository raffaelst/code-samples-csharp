using AutoMapper;
using Dividendos.API.Model.Response.Common;

using Dividendos.Application.Base;
using Dividendos.Application.Interface;
using Dividendos.B3.Interface;
using Dividendos.B3.Interface.Model;
using Dividendos.Entity.Entities;
using Dividendos.Entity.Model;
using Dividendos.Repository.Interface.UoW;
using Dividendos.Service.Interface;
using K.Logger;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Dividendos.Application
{
    public class SyncQueueApp : BaseApp, ISyncQueueApp
    {
        private readonly ISyncQueueService _syncQueueService;
        private readonly IMapper _mapper;
        private readonly IUnitOfWork _uow;
        private readonly IImportB3Helper _importB3Helper;
        private readonly ITraderService _traderService;
        private readonly ILogger _logger;
        private readonly ICeiLogService _ceiLogService;

        public SyncQueueApp(IMapper mapper,
            IUnitOfWork uow,
            ISyncQueueService syncQueueService,
            IImportB3Helper importB3Helper,
            ITraderService traderService,
            ILogger logger,
            ICeiLogService ceiLogService)
        {
            _syncQueueService = syncQueueService;
            _mapper = mapper;
            _uow = uow;
            _importB3Helper = importB3Helper;
            _traderService = traderService;
            _logger = logger;
            _ceiLogService = ceiLogService;
        }

        public void ClearAllDoneAndInUse()
        {
            using (_uow.Create())
            {
                _syncQueueService.ClearAllDoneAndInUse();
            }
        }


        public void ClearAllLogs()
        {
            using (_uow.Create())
            {
                _ceiLogService.DeleteAll();
            }
        }

    }
}