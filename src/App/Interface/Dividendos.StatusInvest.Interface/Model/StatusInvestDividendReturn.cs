using System;
using System.Collections.Generic;

namespace Dividendos.StatusInvest.Interface.Model
{
    public class DateCom
    {
        public string code { get; set; }
        public string companyName { get; set; }
        public string companyNameClean { get; set; }
        public int companyId { get; set; }
        public string resultAbsoluteValue { get; set; }
        public string dateCom { get; set; }
        public string paymentDividend { get; set; }
        public string earningType { get; set; }
        public string dy { get; set; }
        public int recentEvents { get; set; }
        public int recentReports { get; set; }
        public string uRLClear { get; set; }
    }

    public class DatePayment
    {
        public string code { get; set; }
        public string companyName { get; set; }
        public string companyNameClean { get; set; }
        public int companyId { get; set; }
        public string resultAbsoluteValue { get; set; }
        public string dateCom { get; set; }
        public string paymentDividend { get; set; }
        public string earningType { get; set; }
        public string dy { get; set; }
        public int recentEvents { get; set; }
        public int recentReports { get; set; }
        public string uRLClear { get; set; }
    }

    public class Root
    {
        public string from { get; set; }
        public string controller { get; set; }
        public bool close { get; set; }
        public List<DateCom> dateCom { get; set; }
        public List<DatePayment> datePayment { get; set; }
        public List<object> provisioned { get; set; }
    }


}
