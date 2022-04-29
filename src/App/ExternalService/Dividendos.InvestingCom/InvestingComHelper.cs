using Dividendos.InvestingCom.Interface;
using Dividendos.InvestingCom.Interface.Model;
using Newtonsoft.Json;
using RestSharp;
using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace Dividendos.InvestingCom
{
    public class InvestingComHelper : IInvestingComHelper
    {
        public List<StockSplitImport> GetSplitsEvents(int idCountry, int year, int month, int lastDay)
        {
            List<StockSplitImport> stockSplits = new List<StockSplitImport>();
            var client = new RestClient("https://br.investing.com/stock-split-calendar/Service/getCalendarFilteredData");
            client.Timeout = -1;
            var request = new RestRequest(Method.POST);
            request.AddHeader("accept", " */*");
            request.AddHeader("content-length", " 101");
            request.AddHeader("content-type", " application/x-www-form-urlencoded");
            request.AddHeader("origin", " https://br.investing.com");
            request.AddHeader("referer", " https://br.investing.com/stock-split-calendar/");
            request.AddHeader("sec-ch-ua", " \"Google Chrome\";v=\"93\", \" Not;A Brand\";v=\"99\", \"Chromium\";v=\"93\"");
            request.AddHeader("sec-ch-ua-mobile", " ?0");
            request.AddHeader("sec-ch-ua-platform", " \"Windows\"");
            request.AddHeader("sec-fetch-dest", " empty");
            request.AddHeader("sec-fetch-mode", " cors");
            request.AddHeader("sec-fetch-site", " same-origin");
            client.UserAgent = " Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/93.0.4577.63 Safari/537.36";
            request.AddHeader("x-requested-with", " XMLHttpRequest");

            

            var body = string.Format(@"country%5B%5D={0}&dateFrom={1}-{2}-01&dateTo={1}-{2}-{3}&currentTab=custom&submitFilters=1&limit_from=0", idCountry == 1 ? "32" : "5", year, month < 10 ? "0" + month.ToString() : month, lastDay < 10 ? "0" + lastDay.ToString() : lastDay);
            request.AddParameter(" application/x-www-form-urlencoded", body, ParameterType.RequestBody);


            IRestResponse response = client.Execute(request);

            if (response.IsSuccessful)
            {
                dynamic resultAPI = JsonConvert.DeserializeObject<ExpandoObject>(response.Content);

                if (resultAPI != null && resultAPI.data != null)
                {
                    string data = HttpUtility.UrlDecode(resultAPI.data);

                    if (!string.IsNullOrWhiteSpace(data))
                    {
                        data = data.Replace("\n", string.Empty).Replace("\t", string.Empty);
                        List<string> rows = data.Split("<tr>").ToList();

                        if (rows != null && rows.Count() > 1)
                        {
                            DateTime? lastSplitDate = null;


                            for (int i = 1; i < rows.Count(); i++)
                            {
                                StockSplitImport stockSplit = new StockSplitImport();
                                List<string> columns = rows[i].Split("</td>").ToList();

                                if (columns.Count() > 2)
                                {
                                    int startIndex = columns[0].IndexOf('>');

                                    string splitDate = columns[0].Substring(startIndex + 1);

                                    startIndex = columns[1].IndexOf("blank>");

                                    string symbol = columns[1].Substring(startIndex + 6).Replace("</a>)", string.Empty);

                                    startIndex = columns[2].IndexOf('>');
                                    string ratio = columns[2].Substring(startIndex + 1);

                                    string[] ratios = ratio.Split(':');


                                    if (!string.IsNullOrWhiteSpace(splitDate))
                                    {
                                        stockSplit.SplitDate = new DateTime(Convert.ToInt32(splitDate.Substring(6)), Convert.ToInt32(splitDate.Substring(3, 2)), Convert.ToInt32(splitDate.Substring(0, 2)));
                                        lastSplitDate = stockSplit.SplitDate;
                                    }
                                    else if (lastSplitDate.HasValue)
                                    {
                                        stockSplit.SplitDate = lastSplitDate.Value;
                                    }

                                    stockSplit.Symbol = symbol;
                                    stockSplit.ProportionFrom = Convert.ToDecimal(ratios[0], CultureInfo.InvariantCulture);
                                    stockSplit.ProportionTo = Convert.ToDecimal(ratios[1], CultureInfo.InvariantCulture);

                                    stockSplits.Add(stockSplit);
                                }
                            }
                        }
                    }
                }
            }

            return stockSplits;
        }

        public List<InvestingIndicator> GetIndicator()
        {
            List<InvestingIndicator> investingIndicators = new List<InvestingIndicator>();
            InvestingIndicator investingIndicator = new InvestingIndicator();

            CultureInfo cultureInfo = new CultureInfo("pt-br");
            //OilWTI
            string value = GetIndicator("8849");

            investingIndicator.IdIndicatorType = 14;

            if (!string.IsNullOrWhiteSpace(value))
            {
                investingIndicator.Price = Convert.ToDecimal(value.Replace(".", string.Empty), cultureInfo);
            }

            investingIndicators.Add(investingIndicator);

            //OilBrent
            value = GetIndicator("8833");

            investingIndicator = new InvestingIndicator();
            investingIndicator.IdIndicatorType = 15;

            if (!string.IsNullOrWhiteSpace(value))
            {
                investingIndicator.Price = Convert.ToDecimal(value.Replace(".", string.Empty), cultureInfo);
            }

            investingIndicators.Add(investingIndicator);

            //Gold
            value = GetIndicator("8830");

            investingIndicator = new InvestingIndicator();
            investingIndicator.IdIndicatorType = 16;

            if (!string.IsNullOrWhiteSpace(value))
            {
                investingIndicator.Price = Convert.ToDecimal(value.Replace(".", string.Empty), cultureInfo);
            }

            investingIndicators.Add(investingIndicator);

            //Silver
            value = GetIndicator("8836");

            investingIndicator = new InvestingIndicator();
            investingIndicator.IdIndicatorType = 17;

            if (!string.IsNullOrWhiteSpace(value))
            {
                investingIndicator.Price = Convert.ToDecimal(value.Replace(".", string.Empty), cultureInfo);
            }

            investingIndicators.Add(investingIndicator);

            //Copper
            value = GetIndicator("8831");

            investingIndicator = new InvestingIndicator();
            investingIndicator.IdIndicatorType = 18;

            if (!string.IsNullOrWhiteSpace(value))
            {
                investingIndicator.Price = Convert.ToDecimal(value.Replace(".", string.Empty), cultureInfo);
            }

            investingIndicators.Add(investingIndicator);

            //Natural Gas
            value = GetIndicator("8862");

            investingIndicator = new InvestingIndicator();
            investingIndicator.IdIndicatorType = 19;

            if (!string.IsNullOrWhiteSpace(value))
            {
                investingIndicator.Price = Convert.ToDecimal(value.Replace(".", string.Empty), cultureInfo);
            }

            investingIndicators.Add(investingIndicator);

            //Coffee
            value = GetIndicator("8832");

            investingIndicator = new InvestingIndicator();
            investingIndicator.IdIndicatorType = 20;

            if (!string.IsNullOrWhiteSpace(value))
            {
                investingIndicator.Price = Convert.ToDecimal(value.Replace(".", string.Empty), cultureInfo);
            }

            investingIndicators.Add(investingIndicator);

            //Soy
            value = GetIndicator("8916");

            investingIndicator = new InvestingIndicator();
            investingIndicator.IdIndicatorType = 21;

            if (!string.IsNullOrWhiteSpace(value))
            {
                investingIndicator.Price = Convert.ToDecimal(value.Replace(".", string.Empty), cultureInfo);
            }

            investingIndicators.Add(investingIndicator);

            //Cocoa
            value = GetIndicator("8894");

            investingIndicator = new InvestingIndicator();
            investingIndicator.IdIndicatorType = 22;

            if (!string.IsNullOrWhiteSpace(value))
            {
                investingIndicator.Price = Convert.ToDecimal(value.Replace(".", string.Empty), cultureInfo);
            }

            investingIndicators.Add(investingIndicator);

            //Sugar
            value = GetIndicator("8869");

            investingIndicator = new InvestingIndicator();
            investingIndicator.IdIndicatorType = 23;

            if (!string.IsNullOrWhiteSpace(value))
            {
                investingIndicator.Price = Convert.ToDecimal(value.Replace(".", string.Empty), cultureInfo);
            }

            investingIndicators.Add(investingIndicator);

            //AUD/USD
            value = GetIndicator("5");

            investingIndicator = new InvestingIndicator();
            investingIndicator.IdIndicatorType = 24;

            if (!string.IsNullOrWhiteSpace(value))
            {
                investingIndicator.Price = Convert.ToDecimal(value.Replace(".", string.Empty), cultureInfo);
            }

            investingIndicators.Add(investingIndicator);

            //USD/BRL
            //value = GetIndicator("2103");

            //EUR/BRL
            //GetIndicator("1617");

            //EUR/USD
            value = GetIndicator("1");

            investingIndicator = new InvestingIndicator();
            investingIndicator.IdIndicatorType = 25;

            if (!string.IsNullOrWhiteSpace(value))
            {
                investingIndicator.Price = Convert.ToDecimal(value.Replace(".", string.Empty), cultureInfo);
            }

            investingIndicators.Add(investingIndicator);

            ////USD/JPY
            value = GetIndicator("3");

            investingIndicator = new InvestingIndicator();
            investingIndicator.IdIndicatorType = 26;

            if (!string.IsNullOrWhiteSpace(value))
            {
                investingIndicator.Price = Convert.ToDecimal(value.Replace(".", string.Empty), cultureInfo);
            }

            investingIndicators.Add(investingIndicator);

            ////GBP/USD
            value = GetIndicator("2");

            investingIndicator = new InvestingIndicator();
            investingIndicator.IdIndicatorType = 27;

            if (!string.IsNullOrWhiteSpace(value))
            {
                investingIndicator.Price = Convert.ToDecimal(value.Replace(".", string.Empty), cultureInfo);
            }

            ////GBP/BRL
            value = GetIndicator("1736");

            investingIndicator = new InvestingIndicator();
            investingIndicator.IdIndicatorType = 28;

            if (!string.IsNullOrWhiteSpace(value))
            {
                investingIndicator.Price = Convert.ToDecimal(value.Replace(".", string.Empty), cultureInfo);
            }

            investingIndicators.Add(investingIndicator);

            ////CAD/BRL
            value = GetIndicator("1523");

            investingIndicator = new InvestingIndicator();
            investingIndicator.IdIndicatorType = 29;

            if (!string.IsNullOrWhiteSpace(value))
            {
                investingIndicator.Price = Convert.ToDecimal(value.Replace(".", string.Empty), cultureInfo);
            }

            investingIndicators.Add(investingIndicator);

            ////DAX
            //GetIndicator("172");

            ////Euro Stoxx 50
            //GetIndicator("175");

            ////AEX
            //GetIndicator("168");

            ////NIKKEI 225
            //GetIndicator("178");

            return investingIndicators;
        }

        private string GetIndicator(string pairId)
        {
            string value = string.Empty;
            var client = new RestClient(string.Format("https://br.investing.com/common/technical_studies/technical_studies_data.php?action=get_studies&pair_ID={0}&time_frame=3600", pairId));
            client.Timeout = -1;
            var request = new RestRequest(Method.GET);
            request.AddHeader("accept", " */*");
            request.AddHeader("referer", " https://br.investing.com/");
            request.AddHeader("sec-ch-ua", " \" Not;A Brand\";v=\"99\", \"Google Chrome\";v=\"97\", \"Chromium\";v=\"97\"");
            request.AddHeader("sec-ch-ua-mobile", " ?0");
            request.AddHeader("sec-ch-ua-platform", " \"Windows\"");
            request.AddHeader("sec-fetch-dest", " empty");
            request.AddHeader("sec-fetch-mode", " cors");
            request.AddHeader("sec-fetch-site", " same-origin");
            client.UserAgent = " Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/97.0.4692.71 Safari/537.36";
            request.AddHeader("x-requested-with", " XMLHttpRequest");


            IRestResponse response = client.Execute(request);

            if(response.IsSuccessful)
            {
                string[] values = response.Content.Split('*');

                if (values.Length > 2)
                {
                    string price = values[2];
                    int index = price.IndexOf('=');
                    value = price.Substring(index + 1);
                }
            }

            return value;
        }
    
    }
}
