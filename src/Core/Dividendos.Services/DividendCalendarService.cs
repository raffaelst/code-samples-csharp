using Dividendos.Entity.Entities;
using Dividendos.Entity.Model;
using Dividendos.Entity.Views;
using Dividendos.Repository.Interface.UoW;
using Dividendos.Service.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dividendos.Service
{
    public class DividendCalendarService : BaseService, IDividendCalendarService
    {
          public DividendCalendarService(IUnitOfWork uow)
        {
            _uow = uow;
        }


        public ResultServiceObject<IEnumerable<DividendCalendarView>> GetByDataComLimit(DateTime startDate)
        {
            ResultServiceObject<IEnumerable<DividendCalendarView>> resultService = new ResultServiceObject<IEnumerable<DividendCalendarView>>();

            IEnumerable<DividendCalendarView> dividendCalendarViews = _uow.DividendCalendarViewRepository.GetByDataComLimit(startDate);

            resultService.Value = dividendCalendarViews;

            return resultService;
        }

        public ResultServiceObject<IEnumerable<DividendCalendarWaitApproval>> GetAllWaitApproval()
        {
            ResultServiceObject<IEnumerable<DividendCalendarWaitApproval>> resultService = new ResultServiceObject<IEnumerable<DividendCalendarWaitApproval>>();

            IEnumerable<DividendCalendarWaitApproval> dividendCalendarViews = _uow.DividendCalendarWaitApprovalRepository.GetAll();

            resultService.Value = dividendCalendarViews;

            return resultService;

        }
        public ResultServiceObject<IEnumerable<DividendCalendarView>> GetByPaymentDateAndStock(DateTime paymentStartDate, DateTime paymentEndDate, long idStock)
        {
            ResultServiceObject<IEnumerable<DividendCalendarView>> resultService = new ResultServiceObject<IEnumerable<DividendCalendarView>>();

            IEnumerable<DividendCalendarView> dividendCalendarViews = _uow.DividendCalendarViewRepository.GetByPaymentDateAndStock(paymentStartDate, paymentEndDate, idStock);

            resultService.Value = dividendCalendarViews;

            return resultService;
        }

        public ResultServiceObject<IEnumerable<DividendCalendar>> GetByPaymentDateAndStock(DateTime paymentDate, long idStock)
        {
            ResultServiceObject<IEnumerable<DividendCalendar>> resultService = new ResultServiceObject<IEnumerable<DividendCalendar>>();

            IEnumerable<DividendCalendar> dividendCalendarViews = _uow.DividendCalendarRepository.Select(div => div.PaymentDate == paymentDate && div.IdStock == idStock);

            resultService.Value = dividendCalendarViews;

            return resultService;
        }

        public void Delete(DividendCalendarView dividendCalendarView)
        {
            _uow.DividendCalendarRepository.Delete(dividendCalendarView.DividendCalendarID);
        }

        public void DeleteByStockAllFutureItens(long stockID)
        {
            _uow.DividendCalendarRepository.DeleteByStockAllFutureItens(stockID);
        }

        public void DeleteWaitApproval(long dividendCalendarWaitApprovalID)
        {
            _uow.DividendCalendarWaitApprovalRepository.Delete(dividendCalendarWaitApprovalID);
        }

        public ResultServiceObject<bool> ExistByDateAndStock(long idStock, DateTime datacom, DateTime? paymentDate, decimal value)
        {
            ResultServiceObject<bool> resultService = new ResultServiceObject<bool>();


            DividendCalendarView dividendCalendarView = _uow.DividendCalendarViewRepository.ExistByDateAndStock(idStock, datacom, paymentDate, value);

            if (dividendCalendarView != null && dividendCalendarView.DividendCalendarID != 0)
            {
                resultService.Value = true;
            }
            else
            {
                resultService.Value = false;
            }

            return resultService;
        }

        public ResultServiceObject<bool> ExistWaitingForApproval(long idStock, DateTime datacom, DateTime? paymentDate, decimal value)
        {
            ResultServiceObject<bool> resultService = new ResultServiceObject<bool>();

            DividendCalendarWaitApproval dividendCalendarWaitApproval = _uow.DividendCalendarWaitApprovalRepository.ExistByDateAndStock(idStock, datacom, paymentDate, value);

            if (dividendCalendarWaitApproval != null && dividendCalendarWaitApproval.DividendCalendarWaitApprovalID != 0)
            {
                resultService.Value = true;
            }
            else
            {
                resultService.Value = false;
            }

            return resultService;
        }

        public ResultServiceObject<IEnumerable<DividendCalendar>> Save(List<DividendCalendar> dividendCalendars)
        {
            ResultServiceObject<IEnumerable<DividendCalendar>> resultService = new ResultServiceObject<IEnumerable<DividendCalendar>>();

            List<DividendCalendar> dividendCalendarsReturn = new List<DividendCalendar>();

            foreach (DividendCalendar item in dividendCalendars)
            {
                if (!ExistByDateAndStock(item.IdStock, item.DataCom, item.PaymentDate, item.Value).Value)
                {
                    dividendCalendarsReturn.Add(this.Save(item).Value);
                }
            }

            resultService.Value = dividendCalendarsReturn;

            return resultService;
        }


        public ResultServiceObject<IEnumerable<DividendCalendarWaitApproval>> SaveWaitForApproval(List<DividendCalendarWaitApproval> dividendCalendarsWaitApproval)
        {
            ResultServiceObject<IEnumerable<DividendCalendarWaitApproval>> resultService = new ResultServiceObject<IEnumerable<DividendCalendarWaitApproval>>();

            List<DividendCalendarWaitApproval> dividendCalendarsReturn = new List<DividendCalendarWaitApproval>();

            foreach (DividendCalendarWaitApproval item in dividendCalendarsWaitApproval)
            {
                if (!ExistWaitingForApproval(item.IdStock, item.DataCom, item.PaymentDate, item.Value).Value)
                {
                    dividendCalendarsReturn.Add(this.SaveWaitForApproval(item).Value);
                }
            }

            resultService.Value = dividendCalendarsReturn;

            return resultService;
        }

        public ResultServiceObject<DividendCalendar> Save(DividendCalendar dividendCalendar)
        {
            ResultServiceObject<DividendCalendar> resultService = new ResultServiceObject<DividendCalendar>();
            dividendCalendar.CreatedDate = DateTime.Now;
            dividendCalendar.DividendCalendarID = _uow.DividendCalendarRepository.Insert(dividendCalendar);

            resultService.Value = dividendCalendar;

            return resultService;
        }

        public ResultServiceObject<DividendCalendarWaitApproval> SaveWaitForApproval(DividendCalendarWaitApproval dividendCalendar)
        {
            ResultServiceObject<DividendCalendarWaitApproval> resultService = new ResultServiceObject<DividendCalendarWaitApproval>();
            dividendCalendar.DividendCalendarWaitApprovalID = _uow.DividendCalendarWaitApprovalRepository.Insert(dividendCalendar);

            resultService.Value = dividendCalendar;

            return resultService;
        }

        public ResultServiceObject<IEnumerable<DividendCalendar>> GetDividendRangeDate(long idStock, DateTime minDate, DateTime maxDate)
        {
            ResultServiceObject<IEnumerable<DividendCalendar>> resultService = new ResultServiceObject<IEnumerable<DividendCalendar>>();

            resultService.Value = _uow.DividendCalendarRepository.GetDividendRangeDate(idStock, minDate, maxDate);

            return resultService;
        }
    }
}
