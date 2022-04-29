using Dividendos.Bacen.Interface;
using Dividendos.Bacen.Interface.Model;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Dividendos.Bacen
{
    public class ImportBacenHelper : IImportBacenHelper
    {
        public IEnumerable<Indicator> ImportIndicatorsAsync()
        {
            List<Indicator> indicators = new List<Indicator>();
            indicators.AddRange( GetIPCA());
            indicators.AddRange( GetCDI());
            indicators.AddRange( GetPoupanca());

            return indicators;
        }

        private static IEnumerable<Indicator> GetPoupanca()
        {
            List<Indicator> indicators = new List<Indicator>();

            wsBacen.FachadaWSSGSClient fachadaWSSGSClient = new wsBacen.FachadaWSSGSClient();
            int year = DateTime.Now.Year;
            DateTime target = new DateTime(year, 1, 1);

            while (target <= DateTime.Today)
            {
                IEnumerable<DateTime> dates = AllDatesInMonth(year, target.Month);

                foreach (DateTime date in dates)
                {
                    try
                    {
                        wsBacen.getValorResponse res =  fachadaWSSGSClient.getValorAsync(196, date.ToString("dd/MM/yyyy")).Result;

                        if (res != null)
                        {
                            Indicator indicator = new Indicator();
                            indicator.IndicatorType = 5;
                            indicator.PeriodType = 1;
                            indicator.Percentage = res.getValorReturn;
                            indicator.TradeTime = date.ToString("dd/MM/yyyy");
                            indicator.Points = "0";
                            indicators.Add(indicator);
                        }
                    }
                    catch
                    {
                        continue;
                    }
                }

                target = target.AddMonths(1);
            }

            return indicators;
        }

        private static IEnumerable<Indicator> GetCDI()
        {
            List<Indicator> indicators = new List<Indicator>();

            wsBacen.FachadaWSSGSClient fachadaWSSGSClient = new wsBacen.FachadaWSSGSClient();
            int year = DateTime.Now.Year;
            DateTime target = new DateTime(year, 1, 1);

            while (target <= DateTime.Today)
            {
                IEnumerable<DateTime> dates = AllDatesInMonth(year, target.Month);

                foreach (DateTime date in dates)
                {
                    try
                    {
                        string dt = date.ToString("dd/MM/yyyy");
                        long bacenCode = 12;
                        wsBacen.getValorResponse res =  fachadaWSSGSClient.getValorAsync(bacenCode, dt).Result;

                        if (res != null)
                        {
                            Indicator indicator = new Indicator();
                            indicator.IndicatorType = 4;
                            indicator.PeriodType = 1;
                            indicator.Percentage = res.getValorReturn;
                            indicator.TradeTime = date.ToString("dd/MM/yyyy");
                            indicator.Points = "0";
                            indicators.Add(indicator);
                        }
                    }
                    catch
                    {
                        continue;
                    }
                }

                target = target.AddMonths(1);
            }

            return indicators;
        }

        private static IEnumerable<Indicator> GetIPCA()
        {
            List<Indicator> indicators = new List<Indicator>();
            wsBacen.FachadaWSSGSClient fachadaWSSGSClient = new wsBacen.FachadaWSSGSClient();

            DateTime target = new DateTime(DateTime.Now.Year, 1, 1);

            while (target <= DateTime.Today)
            {
                string month = string.Empty;

                if (target.Month < 10)
                {
                    month = string.Format("0{0}", target.Month);
                }
                else
                {
                    month = target.Month.ToString();
                }

                try
                {
                    string tradeDate = string.Format("01/{0}/2019", month);
                    wsBacen.getValorResponse res =  fachadaWSSGSClient.getValorAsync(433, tradeDate).Result;

                    if (res != null)
                    {
                        Indicator indicator = new Indicator();
                        indicator.IndicatorType = 3;
                        indicator.PeriodType = 2;
                        indicator.Percentage = res.getValorReturn;
                        indicator.TradeTime = tradeDate;
                        indicator.Points = "0";
                        indicators.Add(indicator);
                    }
                }
                catch
                {
                    target = target.AddMonths(1);
                    continue;
                }

                target = target.AddMonths(1);
            }

            return indicators;
        }

        public static IEnumerable<DateTime> AllDatesInMonth(int year, int month)
        {
            int days = DateTime.DaysInMonth(year, month);
            for (int day = 1; day <= days; day++)
            {
                yield return new DateTime(year, month, day);
            }
        }
    }
}
