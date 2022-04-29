using System;
using System.Collections.Generic;

namespace Dividendos.Nasdaq.Interface.Model
{
    public class Headers
    {
        public string symbol { get; set; }
        public string companyName { get; set; }
        public string dividend_Ex_Date { get; set; }
        public string payment_Date { get; set; }
        public string record_Date { get; set; }
        public string dividend_Rate { get; set; }
        public string indicated_Annual_Dividend { get; set; }
        public string announcement_Date { get; set; }
    }

    public class Row
    {
        public string companyName { get; set; }
        public string symbol { get; set; }
        public string dividend_Ex_Date { get; set; }
        public string payment_Date { get; set; }
        public string record_Date { get; set; }
        public double dividend_Rate { get; set; }
        public double indicated_Annual_Dividend { get; set; }
        public string announcement_Date { get; set; }
    }

    public class Calendar
    {
        public Headers headers { get; set; }
        public List<Row> rows { get; set; }
    }

    public class Timeframe
    {
        public DateTime minDate { get; set; }
        public DateTime maxDate { get; set; }
    }

    public class Data
    {
        public Calendar calendar { get; set; }
        public Timeframe timeframe { get; set; }
    }

    public class Status
    {
        public int rCode { get; set; }
        public object bCodeMessage { get; set; }
        public object developerMessage { get; set; }
    }

    public class Root
    {
        public Data data { get; set; }
        public object message { get; set; }
        public Status status { get; set; }
    }
}
