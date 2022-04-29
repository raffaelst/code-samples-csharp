using Google.Apis.Auth.OAuth2;
using Google.Apis.Services;
using Google.Apis.Sheets.v4;
using Dividendos.Finance.Interface;
using Dividendos.Finance.Interface.Model;
using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;

namespace Dividendos.Finance
{
    public class SpreadsheetsHelper : ISpreadsheetsHelper
    {
        static readonly string[] Scopes = { SheetsService.Scope.Spreadsheets };
        static readonly string ApplicationName = "Dividendos";
        static readonly string SpreadsheetId = "1oFIv1hyRiMweAfn9NYuT6XI_1XWVDjcodF5Bm9tzb_c";
        static readonly string sheet = "Stock";
        static SheetsService service;

        public SpreadsheetsHelper()
        {
            GoogleCredential credential;
            using (var stream = new FileStream("client_secret.json", FileMode.Open, FileAccess.Read))
            {
                credential = GoogleCredential.FromStream(stream)
                    .CreateScoped(Scopes);
            }

            // Create Google Sheets API service.
            service = new SheetsService(new BaseClientService.Initializer()
            {
                HttpClientInitializer = credential,
                ApplicationName = ApplicationName,
            });

        }

        public ImportFinanceResult ReadEntries()
        {
            ImportFinanceResult importFinanceResult = new ImportFinanceResult();
            List<Stock> stocks = new List<Stock>();
            List<Indicator> indicators = new List<Indicator>();

            var range = $"{sheet}!A:I";
            SpreadsheetsResource.ValuesResource.GetRequest request =
                    service.Spreadsheets.Values.Get(SpreadsheetId, range);

            var response = request.Execute();
            IList<IList<object>> values = response.Values;
            if (values != null && values.Count > 0)
            {
                int index = 1;
                foreach (var row in values)
                {
                    stocks.Add(new Stock() { IdStock = row[0].ToString(), MarketPrice = row[1].ToString(), TradeTime = row[2].ToString(), LastChangePerc = row[4].ToString() });

                    if (index == 1)
                    {
                        Indicator indicator = new Indicator();
                        indicator.IndicatorType = 1;
                        indicator.Percentage = row[6].ToString();
                        indicator.TradeTime = row[7].ToString();
                        indicator.Points = row[8].ToString();

                        indicators.Add(indicator);
                    }

                    if (index == 2)
                    {
                        Indicator indicator = new Indicator();
                        indicator.IndicatorType = 2;
                        indicator.Percentage = row[6].ToString();
                        indicator.TradeTime = row[7].ToString();
                        indicator.Points = row[8].ToString();

                        indicators.Add(indicator);
                    }

                    if (index == 3)
                    {
                        Indicator indicator = new Indicator();
                        indicator.IndicatorType = 6;
                        indicator.Percentage = row[6].ToString();
                        indicator.TradeTime = row[7].ToString();
                        indicator.Points = row[8].ToString();

                        indicators.Add(indicator);
                    }

                    if (index == 4)
                    {
                        Indicator indicator = new Indicator();
                        indicator.IndicatorType = 7;
                        indicator.Percentage = row[6].ToString();
                        indicator.TradeTime = row[7].ToString();
                        indicator.Points = row[8].ToString();

                        indicators.Add(indicator);
                    }

                    if (index == 5)
                    {
                        Indicator indicator = new Indicator();
                        indicator.IndicatorType = 8;
                        indicator.Percentage = row[6].ToString();
                        indicator.TradeTime = row[7].ToString();
                        indicator.Points = row[8].ToString();

                        indicators.Add(indicator);
                    }

                    if (index == 6)
                    {
                        Indicator indicator = new Indicator();
                        indicator.IndicatorType = 9;
                        indicator.Percentage = "0";
                        indicator.TradeTime = DateTime.Now.ToString("dd/MM/yyyy hh:mm:ss");
                        indicator.Points = row[6].ToString();
                        indicators.Add(indicator);
                    }

                    if (index == 7)
                    {
                        Indicator indicator = new Indicator();
                        indicator.IndicatorType = 10;
                        indicator.Percentage = row[6].ToString();
                        indicator.TradeTime = row[7].ToString();
                        indicator.Points = row[8].ToString();
                        indicators.Add(indicator);
                    }

                    if (index == 8)
                    {
                        Indicator indicator = new Indicator();
                        indicator.IndicatorType = 11;
                        indicator.Percentage = row[6].ToString();
                        indicator.TradeTime = row[7].ToString();
                        indicator.Points = row[8].ToString();
                        indicators.Add(indicator);
                    }

                    if (index == 9)
                    {
                        Indicator indicator = new Indicator();
                        indicator.IndicatorType = 12;
                        indicator.Percentage = row[6].ToString();
                        indicator.TradeTime = row[7].ToString();
                        indicator.Points = row[8].ToString();
                        indicators.Add(indicator);
                    }

                    if (index == 10)
                    {
                        Indicator indicator = new Indicator();
                        indicator.IndicatorType = 13;
                        indicator.Percentage = "0";
                        indicator.TradeTime = DateTime.Now.ToString("dd/MM/yyyy hh:mm:ss");
                        indicator.Points = row[6].ToString();
                        indicators.Add(indicator);
                    }

                    index++;
                }
            }
            else
            {
               //Console.WriteLine("No data found.");
            }

            importFinanceResult.ListIndicator = indicators;
            importFinanceResult.ListStock = stocks;

            return importFinanceResult;
        }
    }
}
