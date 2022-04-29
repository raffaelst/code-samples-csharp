using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using AutoMapper;
using Dividendos.Entity.Entities;
using Dividendos.Entity.Enum;
using Dividendos.Entity.Model;
using Dividendos.Repository.Interface.UoW;
using Dividendos.Service.Interface;
using Dividendos.TradeMap.Interface;
using K.Logger;
using System.Linq;
using System.Globalization;
using Dividendos.Application.Interface;
using Dividendos.Application.Base;

namespace Dividendos.Application
{
    public class HolidayApp : BaseApp, IHolidayApp
    {
        private readonly IMapper _mapper;
        private readonly IUnitOfWork _uow;
        private readonly IHolidayService _holidayService;
        private readonly ITradeMapHelper _iTradeMapHelper;
        private readonly ILogger _logger;
        private readonly ISystemSettingsService _systemSettingsService;

        public HolidayApp(IMapper mapper,
            IUnitOfWork uow,
            IHolidayService holidayService,
            ILogger logger,
            ISystemSettingsService systemSettingsService,
            ITradeMapHelper iTradeMapHelper)
        {
            _mapper = mapper;
            _uow = uow;
            _holidayService = holidayService;
            _iTradeMapHelper = iTradeMapHelper;
            _logger = logger;
            _systemSettingsService = systemSettingsService;
        }


        public async Task ImportHolidays()
        {
            try
            {
                string tradeMapCookie = string.Empty;
                List<Holiday> holidays = new List<Holiday>();

                using (_uow.Create())
                {                    
                    ResultServiceObject<Entity.Entities.SystemSettings> resultSettingsCookie = _systemSettingsService.GetByKey(Constants.SYSTEM_SETTINGS_TRADE_MAP_COOKIE);
                    ResultServiceObject<IEnumerable<Holiday>> resultHoliday = _holidayService.GetByCountry(1);

                    if (resultSettingsCookie.Success && resultSettingsCookie.Value != null)
                    {
                        tradeMapCookie = resultSettingsCookie.Value.SettingValue;
                    }

                    if (resultHoliday.Success && resultHoliday.Value != null)
                    {
                        holidays = resultHoliday.Value.ToList();
                    }
                }

                List<string> holidaysTd = await _iTradeMapHelper.GetHolidays(tradeMapCookie);

                if (holidaysTd != null && holidaysTd.Count > 0)
                {
                    foreach (string holidayTd in holidaysTd)
                    {
                        Holiday holiday = null;
                        DateTime dtHoliday = DateTime.ParseExact(holidayTd, "yyyyMMdd", CultureInfo.InvariantCulture);

                        if (holidays != null && holidays.Count > 0)
                        {
                            holiday = holidays.FirstOrDefault(h => h.EventDate.Date == dtHoliday.Date);
                        }

                        if (holiday == null)
                        {
                            using (_uow.Create())
                            {
                                holiday = new Holiday();
                                holiday.EventDate = dtHoliday.Date;
                                holiday.IdCountry = 1;

                                holiday = _holidayService.Insert(holiday).Value;
                            }
                        }
                    }
                }

            }
            catch (Exception exception)
            {
                _logger.SendErrorAsync(exception);
                throw;
            }
        }

    }
}
