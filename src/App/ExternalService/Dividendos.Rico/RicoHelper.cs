using Dividendos.Rico.Interface;
using Dividendos.Rico.Interface.Model;
using Newtonsoft.Json;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Support.UI;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Drawing;
using System.Dynamic;
using System.Globalization;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace Dividendos.Rico
{
    public class RicoHelper : IRicoHelper
    {
        public ImportRicoResult ImportFromRico(string identifier, string password, string token, DateTime? lastEventDate, bool getContactDetails)
        {
            ImportRicoResult importRicoResult = new ImportRicoResult();
            importRicoResult.Success = true;

            string jwtToken = Login(identifier, password, token, importRicoResult);

            if (jwtToken != null && importRicoResult.Success)
            {
                GetAssetsSummary(importRicoResult, jwtToken);

                GetPastDividends(lastEventDate, importRicoResult, jwtToken);

                if (getContactDetails)
                {
                    GetContactDetails(importRicoResult, jwtToken);
                }

                importRicoResult.Success = true;

            }
            else
            {
                importRicoResult.Message = "Token incorreto";
                importRicoResult.Message = "Token incorreto";
                importRicoResult.Success = false;
                return importRicoResult;
            }

            return importRicoResult;
        }

        public ImportRicoResult RestoreDividends(string identifier, string password, string token)
        {
            ImportRicoResult importRicoResult = new ImportRicoResult();
            importRicoResult.Success = true;

            string jwtToken = Login(identifier, password, token, importRicoResult);

            if (jwtToken != null && importRicoResult.Success)
            {
                importRicoResult.Dividends = new List<RicoDividend>();
                GetPastDividends(null, importRicoResult, jwtToken);
            }

            return importRicoResult;
        }

        private string Login(string identifier, string password, string token, ImportRicoResult importRicoResult)
        {
            string jwtToken = string.Empty;
            var chromeOptions = new ChromeOptions();
            chromeOptions.AddArguments("--no-sandbox");
            chromeOptions.AddArguments("--disable-dev-shm-usage");

            using (var driver = new ChromeDriver(chromeOptions))
            {
                driver.Manage().Window.Size = new Size(1920, 1080);
                driver.Manage().Window.Position = new Point(0, 0);
                driver.Manage().Window.Maximize();
                driver.Manage().Timeouts().PageLoad = TimeSpan.FromMinutes(2);

                string errorText = string.Empty;
                driver.Navigate().GoToUrl(@"https://www.rico.com.vc/login/");
                WaitUntilPageIsLoaded(driver, "document.URL", "login", 30000);

                WaitUntilElementIsLoaded(driver, "document.querySelector('.width-100.fontsize-4.color-dove-grey.bordercolor-mine-shaft.input__error')", 60000);
                var inputAccount = driver.FindElement(By.CssSelector(".width-100.fontsize-4.color-dove-grey.bordercolor-mine-shaft.input__error"));
                inputAccount.SendKeys(identifier);

                WaitUntilElementIsLoaded(driver, "document.querySelector('.paddingbottom-3.rounded.paddingtop-3.button.fontsize-3.btn-primary.margintop-4.button')", 60000);
                var inputAccountOk = driver.FindElement(By.CssSelector(".paddingbottom-3.rounded.paddingtop-3.button.fontsize-3.btn-primary.margintop-4.button"));
                inputAccountOk.Click();

                //Wait(6500, 6500, driver);

                ReadOnlyCollection<IWebElement> inputPwd = null;

                try
                {
                    WaitUntilElementIsLoaded(driver, "document.querySelector('.Keyboard__KeyboardButton-knpmRc.jzLThf')", 60000);
                    inputPwd = driver.FindElements(By.CssSelector(".Keyboard__KeyboardButton-knpmRc.jzLThf"));
                    var msg = driver.FindElement(By.CssSelector(".margintop-25px.soma-description.hydrated"));
                    errorText = msg.Text;
                }
                catch
                {
                }

                if (inputPwd != null && inputPwd.Count > 0)
                {
                    foreach (char pwd in password)
                    {
                        foreach (var btnPwd in inputPwd)
                        {
                            if (btnPwd.Text.Contains(pwd))
                            {
                                btnPwd.Click();
                                break;
                            }
                        }
                    }

                    WaitUntilElementIsLoaded(driver, "document.querySelector('.paddingbottom-3.rounded.paddingtop-3.button.fontsize-3.btn-primary.marginvertical-1.button.btn-primary')", 60000);
                    var inputPwdOk = driver.FindElement(By.CssSelector(".paddingbottom-3.rounded.paddingtop-3.button.fontsize-3.btn-primary.marginvertical-1.button.btn-primary"));
                    inputPwdOk.Click();

                    IWebElement inputRicoToken = null;

                    try
                    {
                        WaitUntilElementIsLoaded(driver, "document.querySelector('.width-100.fontsize-4.color-dove-grey.bordercolor-mine-shaft.token-input')", 60000);
                        inputRicoToken = driver.FindElement(By.CssSelector(".width-100.fontsize-4.color-dove-grey.bordercolor-mine-shaft.token-input"));
                    }
                    catch
                    {
                    }

                    if (inputRicoToken != null)
                    {
                        inputRicoToken.SendKeys(token);

                        WaitUntilElementIsLoaded(driver, "document.querySelector('.paddingbottom-3.rounded.paddingtop-3.button.fontsize-3.btn-primary.margintop-4.button')", 60000);
                        var inputRicoTokenOk = driver.FindElement(By.CssSelector(".paddingbottom-3.rounded.paddingtop-3.button.fontsize-3.btn-primary.margintop-4.button"));
                        inputRicoTokenOk.Click();

                        //Wait(60000, 60000, driver);

                        try
                        {
                            WaitUntilElementIsLoaded(driver, "document.querySelector('.margintop-25px.soma-description.hydrated')", 6000);
                            var msg = driver.FindElement(By.CssSelector(".margintop-25px.soma-description.hydrated"));
                            errorText = msg.Text;
                        }
                        catch
                        {
                        }

                        if (string.IsNullOrWhiteSpace(errorText))
                        {
                            WaitUntilPageIsLoaded(driver, "document.URL", "https://arealogada.rico.com.vc", 60000);
                            Wait(30000, 30000, driver);

                            var jsToBeExecuted3 = "return sessionStorage.getItem('Authorization');";
                            var jwtTokenInput3 = ((IJavaScriptExecutor)driver).ExecuteScript(jsToBeExecuted3);

                            if (jwtTokenInput3 != null)
                            {
                                jwtToken = jwtTokenInput3.ToString();
                            }
                        }
                        else
                        {
                            importRicoResult.Message = string.IsNullOrWhiteSpace(errorText) ? "Senha ou token incorreto" : errorText;
                            importRicoResult.Message = string.IsNullOrWhiteSpace(errorText) ? "Senha ou token incorreto" : errorText;
                            importRicoResult.Success = false;
                        }
                    }
                    else
                    {
                        importRicoResult.Message = string.IsNullOrWhiteSpace(errorText) ? "Senha incorreta" : errorText;
                        importRicoResult.Message = string.IsNullOrWhiteSpace(errorText) ? "Senha incorreta" : errorText;
                        importRicoResult.Success = false;
                    }
                }
                else
                {
                    importRicoResult.Message = string.IsNullOrWhiteSpace(errorText) ? "Cliente não encontrado" : errorText;
                    importRicoResult.Message = string.IsNullOrWhiteSpace(errorText) ? "Cliente não encontrado" : errorText;
                    importRicoResult.Success = false;
                }
            }

            return jwtToken;
        }

        private void GetContactDetails(ImportRicoResult importRicoResult, object jwtTokenInput3)
        {
            List<RicoContactDetailApi> ricoContactDetailsApi = GetContactDetails(jwtTokenInput3.ToString());

            if (ricoContactDetailsApi != null && ricoContactDetailsApi.Count > 0)
            {
                ricoContactDetailsApi = ricoContactDetailsApi.Where(item => item.Label.Contains("iniciais") || item.Label.Contains("Telefones") || item.Label.Contains("Documento")).ToList();

                if (ricoContactDetailsApi != null && ricoContactDetailsApi.Count > 0)
                {
                    importRicoResult.ContactDetails = new RicoContactDetails();
                    importRicoResult.ContactPhones = new List<RicoContactPhone>();

                    foreach (RicoContactDetailApi ricoContactDetailApi in ricoContactDetailsApi)
                    {
                        foreach (ContactDetailItem contactDetailItem in ricoContactDetailApi.Items)
                        {
                            if (contactDetailItem.Label.ToLower() == "nome")
                            {
                                importRicoResult.ContactDetails.Name = contactDetailItem.Value;
                            }

                            if (contactDetailItem.Label.ToLower() == "e-mail")
                            {
                                importRicoResult.ContactDetails.Email = contactDetailItem.Value;
                            }

                            if (contactDetailItem.Id.ToLower() == "documentnumber")
                            {
                                importRicoResult.ContactDetails.DocumentNumber = contactDetailItem.Value;
                            }

                            if (contactDetailItem.Id.ToLower() == "phonenumber")
                            {
                                importRicoResult.ContactPhones.Add(new RicoContactPhone { PhoneNumber = contactDetailItem.Value });
                            }

                            if (contactDetailItem.Id.ToLower() == "cellphonenumber")
                            {
                                importRicoResult.ContactPhones.Add(new RicoContactPhone { PhoneNumber = contactDetailItem.Value });
                            }
                        }
                    }
                }
            }
        }

        private void GetPastDividends(DateTime? lastEventDate, ImportRicoResult importRicoResult, object jwtTokenInput3)
        {
            DateTime startDate = new DateTime(2015, 1, 1);

            if (lastEventDate.HasValue)
            {
                startDate = lastEventDate.Value.Date;
            }

            string start = startDate.ToString("yyyy-MM-dd");
            string end = DateTime.Now.ToString("yyyy-MM-dd");

            RicoPastDividend ricoPastDividend = GetPastDividends(jwtTokenInput3.ToString(), start, end);

            if (ricoPastDividend != null && ricoPastDividend.Items != null && ricoPastDividend.Items.Count > 0)
            {
                foreach (Item item in ricoPastDividend.Items)
                {
                    string description = item.Description;

                    if (!string.IsNullOrEmpty(description))
                    {
                        description = description.Replace("* PROV * ", string.Empty);
                    }

                    decimal netValue = 0;
                    decimal.TryParse(item.Value, NumberStyles.Currency, new CultureInfo("en-us"), out netValue);


                    if (description != null && description.ToLower().Contains("juros"))
                    {
                        string[] rowValues = description.Split(" ");
                        DateTime paymentDate = DateTime.Now;

                        RicoDividend ricoDividend = new RicoDividend();
                        ricoDividend.Type = rowValues[0];
                        ricoDividend.Symbol = rowValues[5];
                        ricoDividend.NetValue = netValue;
                        ricoDividend.GrossValue = netValue;


                        if (DateTime.TryParse(item.TransactionDate, new CultureInfo("pt-br"), DateTimeStyles.None, out paymentDate))
                        {
                            ricoDividend.EventDate = paymentDate;
                        }

                        importRicoResult.Dividends.Add(ricoDividend);

                    }

                    if (description != null && description.ToLower().Contains("dividendos"))
                    {
                        string[] rowValues = description.Split(" ");
                        DateTime paymentDate = DateTime.Now;

                        RicoDividend ricoDividend = new RicoDividend();
                        ricoDividend.Type = rowValues[0];
                        ricoDividend.Symbol = rowValues[3];
                        ricoDividend.NetValue = netValue;
                        ricoDividend.GrossValue = netValue;

                        if (DateTime.TryParse(item.TransactionDate, new CultureInfo("pt-br"), DateTimeStyles.None, out paymentDate))
                        {
                            ricoDividend.EventDate = paymentDate;
                        }

                        importRicoResult.Dividends.Add(ricoDividend);

                    }

                    if (description != null && description.ToLower().Contains("rendimentos"))
                    {
                        string[] rowValues = description.Split(" ");
                        DateTime paymentDate = DateTime.Now;

                        RicoDividend ricoDividend = new RicoDividend();
                        ricoDividend.Type = rowValues[0];
                        ricoDividend.Symbol = rowValues[3];
                        ricoDividend.NetValue = netValue;
                        ricoDividend.GrossValue = netValue;

                        if (DateTime.TryParse(item.TransactionDate, new CultureInfo("pt-br"), DateTimeStyles.None, out paymentDate))
                        {
                            ricoDividend.EventDate = paymentDate;
                        }

                        importRicoResult.Dividends.Add(ricoDividend);

                    }

                }
            }
        }

        private void GetAssetsSummary(ImportRicoResult importRicoResult, object jwtTokenInput3)
        {
            #region Test
            //string json = "{\"metadata\":{\"dateHour\":\"2022-04-27T14:21:34.4400933-03:00\",\"accountNumber\":5419322},\"totalValue\":78071.81,\"allocation\":78.47,\"grossValue\":78071.81,\"totalInvestedValue\":60129.5,\"balance\":{\"availableToWithdrawal\":16793.49,\"totalValue\":61278.32,\"grossValue\":78071.81,\"availableValue\":22284.17,\"projections\":{\"d1\":-5490.68,\"d2\":0,\"d3\":0}},\"positions\":[{\"product\":\"COE\",\"productName\":\"COE\",\"totalValue\":0,\"allocation\":0,\"grossValue\":0,\"error\":false,\"investedValue\":0,\"itens\":[]},{\"product\":\"FUNDS\",\"productName\":\"Fundos de Investimentos\",\"totalValue\":0,\"allocation\":0,\"grossValue\":0,\"error\":false,\"itens\":[],\"investedValue\":0},{\"product\":\"PENSION_FUNDS\",\"productName\":\"Previd\u00EAncia Privada\",\"totalValue\":0,\"allocation\":0,\"grossValue\":0,\"error\":false,\"investedValue\":0},{\"product\":\"FIXED_INCOME\",\"productName\":\"Renda Fixa\",\"totalValue\":0,\"allocation\":0,\"grossValue\":0,\"error\":false,\"investedValue\":0,\"itens\":[]},{\"product\":\"STOCK\",\"productName\":\"A\u00E7\u00F5es\",\"totalValue\":0,\"allocation\":36.8,\"grossValue\":28731,\"error\":false,\"investedValue\":28731,\"itens\":[{\"stockCode\":\"CPLE6\",\"price\":7.64,\"quantity\":1000,\"totalValue\":7640,\"averagePrice\":6.06,\"profitability\":26.072607260726073,\"averagePriceStatus\":1},{\"stockCode\":\"JBSS3\",\"price\":39.09,\"quantity\":100,\"totalValue\":3909,\"averagePrice\":38.5,\"profitability\":1.5324675324675325,\"averagePriceStatus\":1},{\"stockCode\":\"SAPR11\",\"price\":19.97,\"quantity\":500,\"totalValue\":9985,\"averagePrice\":18.55,\"profitability\":7.654986522911051,\"averagePriceStatus\":1},{\"stockCode\":\"TAEE4\",\"price\":15.24,\"quantity\":300,\"totalValue\":4572,\"averagePrice\":14.896667,\"profitability\":2.3047638777184183,\"averagePriceStatus\":1},{\"stockCode\":\"TRPL4\",\"price\":26.25,\"quantity\":100,\"totalValue\":2625,\"averagePrice\":23.28,\"profitability\":12.757731958762887,\"averagePriceStatus\":1}]},{\"product\":\"BMF\",\"productName\":\"BMF\",\"totalValue\":0,\"allocation\":0,\"grossValue\":0,\"error\":false,\"investedValue\":0,\"itens\":[]},{\"product\":\"FII\",\"productName\":\"Fundos Imobili\u00E1rios\",\"totalValue\":0,\"allocation\":40.2,\"grossValue\":31398.5,\"error\":false,\"investedValue\":31398.5,\"itens\":[{\"stockCode\":\"TGAR11\",\"price\":119.17,\"quantity\":50,\"totalValue\":5958.5,\"averagePrice\":119.4,\"profitability\":-0.19262981574539365,\"averagePriceStatus\":1},{\"stockCode\":\"VSLH11\",\"price\":9.28,\"quantity\":0,\"totalValue\":0,\"averagePrice\":0,\"profitability\":0,\"averagePriceStatus\":1},{\"stockCode\":\"XPML11\",\"price\":100.5,\"quantity\":50,\"totalValue\":5025,\"averagePrice\":100.0896,\"profitability\":0.41003261078074044,\"averagePriceStatus\":1},{\"stockCode\":\"HABT11\",\"price\":103.33,\"quantity\":50,\"totalValue\":5166.5,\"averagePrice\":114.48,\"profitability\":-9.73969252271139,\"averagePriceStatus\":1},{\"stockCode\":\"KNSC11\",\"price\":93.99,\"quantity\":0,\"totalValue\":0,\"averagePrice\":0,\"profitability\":0,\"averagePriceStatus\":1},{\"stockCode\":\"MXRF11\",\"price\":9.89,\"quantity\":500,\"totalValue\":4945,\"averagePrice\":9.98,\"profitability\":-0.9018036072144289,\"averagePriceStatus\":1},{\"stockCode\":\"BTLG11\",\"price\":102.8,\"quantity\":50,\"totalValue\":5140,\"averagePrice\":103.75,\"profitability\":-0.9156626506024096,\"averagePriceStatus\":1},{\"stockCode\":\"RZTR11\",\"price\":103.27,\"quantity\":50,\"totalValue\":5163.5,\"averagePrice\":102.2662,\"profitability\":0.9815559784171114,\"averagePriceStatus\":1}]},{\"product\":\"OPTIONS\",\"productName\":\"Op\u00E7\u00F5es\",\"totalValue\":0,\"allocation\":0,\"grossValue\":0,\"error\":false,\"investedValue\":0,\"itens\":[]},{\"product\":\"FLEX_OPTIONS\",\"productName\":\"Op\u00E7\u00F5es Flex\u00EDveis\",\"totalValue\":0,\"allocation\":0,\"grossValue\":0,\"error\":false,\"investedValue\":0,\"itens\":[]},{\"product\":\"TREASURY\",\"productName\":\"Tesouro Direto\",\"totalValue\":0,\"allocation\":0,\"grossValue\":0,\"error\":false,\"investedValue\":0,\"itens\":[]},{\"product\":\"GOLD\",\"productName\":\"Ouro\",\"totalValue\":0,\"allocation\":0,\"grossValue\":0,\"error\":false,\"investedValue\":0,\"itens\":[]},{\"product\":\"TERMS\",\"productName\":\"Termos\",\"totalValue\":0,\"allocation\":0,\"grossValue\":0,\"error\":false,\"investedValue\":0,\"itens\":[]},{\"product\":\"SECURITIES_LENDING\",\"productName\":\"Aluguel de A\u00E7\u00F5es e FIIs\",\"totalValue\":0,\"allocation\":0,\"grossValue\":0,\"error\":false,\"investedValue\":0,\"itens\":[]},{\"product\":\"REMUNERATED_CUSTODY\",\"productName\":\"Cust\u00F3dia Remunerada\",\"totalValue\":0,\"allocation\":0,\"grossValue\":0,\"error\":false,\"investedValue\":0,\"itens\":[]},{\"product\":\"EARNINGS\",\"productName\":\"Proventos Provisionados\",\"totalValue\":0,\"allocation\":1.47,\"grossValue\":1148.82,\"error\":false,\"investedValue\":0,\"itens\":[{\"stockCode\":\"ALUP11\",\"quantity\":100,\"type\":\"DIVIDENDO\",\"paymentDate\":\"2022-05-31T03:00:00.000Z\",\"paymentDateString\":\"31/05/2022\",\"grossValue\":45,\"netValue\":0,\"incomeTaxValue\":45},{\"stockCode\":\"ALUP11\",\"quantity\":100,\"type\":\"DIVIDENDO\",\"paymentDate\":\"2022-08-31T03:00:00.000Z\",\"paymentDateString\":\"31/08/2022\",\"grossValue\":45,\"netValue\":0,\"incomeTaxValue\":45},{\"stockCode\":\"ALUP11\",\"quantity\":100,\"type\":\"DIVIDENDO\",\"paymentDate\":\"2022-11-30T03:00:00.000Z\",\"paymentDateString\":\"30/11/2022\",\"grossValue\":33,\"netValue\":0,\"incomeTaxValue\":33},{\"stockCode\":\"CPLE6\",\"quantity\":1000,\"type\":\"JUROS SOBRE CAPITAL PROPRIO\",\"paymentDate\":\"9999-12-31T03:00:00.000Z\",\"paymentDateString\":\"31/12/9999\",\"grossValue\":107.23,\"netValue\":0,\"incomeTaxValue\":107.23},{\"stockCode\":\"ENBR3\",\"quantity\":100,\"type\":\"JUROS SOBRE CAPITAL PROPRIO\",\"paymentDate\":\"2022-06-30T03:00:00.000Z\",\"paymentDateString\":\"30/06/2022\",\"grossValue\":78.92,\"netValue\":0,\"incomeTaxValue\":78.92},{\"stockCode\":\"ENBR3\",\"quantity\":100,\"type\":\"DIVIDENDO\",\"paymentDate\":\"2022-06-30T03:00:00.000Z\",\"paymentDateString\":\"30/06/2022\",\"grossValue\":140.44,\"netValue\":0,\"incomeTaxValue\":140.44},{\"stockCode\":\"EUCA4\",\"quantity\":200,\"type\":\"JUROS SOBRE CAPITAL PROPRIO\",\"paymentDate\":\"9999-12-31T03:00:00.000Z\",\"paymentDateString\":\"31/12/9999\",\"grossValue\":56.6,\"netValue\":0,\"incomeTaxValue\":56.6},{\"stockCode\":\"SAPR11\",\"quantity\":1000,\"type\":\"JUROS SOBRE CAPITAL PROPRIO\",\"paymentDate\":\"4000-12-31T03:00:00.000Z\",\"paymentDateString\":\"31/12/4000\",\"grossValue\":506.12,\"netValue\":0,\"incomeTaxValue\":506.12},{\"stockCode\":\"SAPR11\",\"quantity\":500,\"type\":\"JUROS SOBRE CAPITAL PROPRIO\",\"paymentDate\":\"9999-12-31T03:00:00.000Z\",\"paymentDateString\":\"31/12/9999\",\"grossValue\":292.75,\"netValue\":0,\"incomeTaxValue\":292.75}]},{\"product\":\"STRUCTURED_PRODUCTS\",\"productName\":\"Produtos Estruturados\",\"totalValue\":0,\"allocation\":0,\"grossValue\":0,\"error\":false,\"investedValue\":0,\"itens\":[]}]}";

            //var jsonSerializerSettings = new JsonSerializerSettings();
            //jsonSerializerSettings.Culture = CultureInfo.InvariantCulture;
            //jsonSerializerSettings.MissingMemberHandling = MissingMemberHandling.Ignore;
            //RicoSummary ricoSummary = JsonConvert.DeserializeObject<RicoSummary>(json, jsonSerializerSettings);
            #endregion

            RicoSummary ricoSummary = GetSummaryPosition(jwtTokenInput3.ToString(), importRicoResult);
            importRicoResult.Orders = new List<RicoOrder>();
            importRicoResult.RicoWarnings = new List<string>();
            importRicoResult.Dividends = new List<RicoDividend>();
            importRicoResult.Bonds = new List<RicoBond>();
            importRicoResult.Funds = new List<RicoFund>();

            if (ricoSummary != null && ricoSummary.Positions != null && ricoSummary.Positions.Count > 0)
            {
                List<Position> positionsAssets = ricoSummary.Positions.Where(pos => pos.Product == "STOCK" || pos.Product == "FII").ToList();

                if (positionsAssets != null && positionsAssets.Count > 0)
                {
                    foreach (Position position in positionsAssets)
                    {
                        if (position.Itens != null && position.Itens.Count > 0)
                        {
                            foreach (Iten iten in position.Itens)
                            {
                                decimal averagePrice = 0;
                                decimal.TryParse(iten.AveragePrice, NumberStyles.Currency, new CultureInfo("en-us"), out averagePrice);

                                decimal quantity = 0;
                                decimal.TryParse(iten.Quantity, NumberStyles.Currency, new CultureInfo("en-us"), out quantity);

                                RicoOrder ricoOrder = new RicoOrder();
                                ricoOrder.AveragePrice = averagePrice;
                                ricoOrder.NumberOfShares = quantity;
                                ricoOrder.Symbol = iten.StockCode;
                                ricoOrder.IdOperationType = 1;
                                ricoOrder.EventDate = DateTime.Now;

                                importRicoResult.Orders.Add(ricoOrder);
                            }
                        }
                    }
                }

                List<Position> earnings = ricoSummary.Positions.Where(pos => pos.Product == "EARNINGS").ToList();

                if (earnings != null && earnings.Count > 0)
                {
                    foreach (Position position in earnings)
                    {
                        if (position.Itens != null && position.Itens.Count > 0)
                        {
                            foreach (Iten iten in position.Itens)
                            {
                                decimal grossValue = 0;
                                decimal.TryParse(iten.GrossValue, NumberStyles.Currency, new CultureInfo("en-us"), out grossValue);

                                DateTime paymentDate = DateTime.Now;

                                string type = iten.Type;

                                if (iten.Type.ToLower().Contains("dividendo"))
                                {
                                    type = "dividendos";
                                }
                                else if (iten.Type.ToLower().Contains("juros"))
                                {
                                    type = "juros";
                                }
                                else if (iten.Type.ToLower().Contains("rendimento"))
                                {
                                    type = "rendimentos";
                                }

                                RicoDividend ricoDividend = new RicoDividend();
                                ricoDividend.Type = type;
                                ricoDividend.Symbol = iten.StockCode;
                                ricoDividend.NetValue = grossValue;
                                ricoDividend.GrossValue = grossValue;


                                if (DateTime.TryParse(iten.PaymentDateString, new CultureInfo("pt-br"), DateTimeStyles.None, out paymentDate))
                                {
                                    ricoDividend.EventDate = paymentDate;
                                }

                                importRicoResult.Dividends.Add(ricoDividend);
                            }
                        }
                    }
                }


                List<Position> bonds = ricoSummary.Positions.Where(pos => pos.Product == "FIXED_INCOME" || pos.Product == "TREASURY").ToList();

                if (bonds != null && bonds.Count > 0)
                {
                    foreach (Position position in bonds)
                    {
                        if (position.Itens != null && position.Itens.Count > 0)
                        {
                            foreach (Iten iten in position.Itens)
                            {
                                decimal grossValue = 0;

                                if (position.Product == "FIXED_INCOME")
                                {
                                    decimal.TryParse(iten.CurrentGrossValue, NumberStyles.Currency, new CultureInfo("en-us"), out grossValue);
                                }
                                else if (position.Product == "TREASURY")
                                {
                                    decimal.TryParse(iten.GrossValue, NumberStyles.Currency, new CultureInfo("en-us"), out grossValue);
                                }

                                RicoBond ricoBond = new RicoBond();
                                ricoBond.Name = iten.Name;
                                ricoBond.Issuer = iten.Name;
                                ricoBond.Value = grossValue;

                                importRicoResult.Bonds.Add(ricoBond);
                            }
                        }
                    }
                }

                List<Position> funds = ricoSummary.Positions.Where(pos => pos.Product == "FUNDS").ToList();

                if (funds != null && funds.Count > 0)
                {
                    foreach (Position position in funds)
                    {
                        if (position.Itens != null && position.Itens.Count > 0)
                        {
                            foreach (Iten iten in position.Itens)
                            {
                                decimal grossValue = 0;
                                decimal.TryParse(iten.CurrentTotalValueGross, NumberStyles.Currency, new CultureInfo("en-us"), out grossValue);

                                RicoFund ricoFund = new RicoFund();
                                ricoFund.Name = iten.Name;
                                ricoFund.Value = grossValue;

                                importRicoResult.Funds.Add(ricoFund);
                            }
                        }
                    }
                }

                List<Position> otherPositions = ricoSummary.Positions.Where(pos => pos.Product != "STOCK" && pos.Product != "FII" && pos.Itens != null && pos.Itens.Count > 0).ToList();

                if (otherPositions != null && otherPositions.Count > 0)
                {
                    foreach (Position position in otherPositions)
                    {
                        importRicoResult.RicoWarnings.Add(position.Product);
                    }
                }
            }
        }

        private List<RicoDividend> GetPastDividends(ChromeDriver driver)
        {
            List<RicoDividend> ricoDividends = new List<RicoDividend>();

            try
            {
                driver.Navigate().GoToUrl(@"https://arealogada.rico.com.vc/conta/extrato");
                WaitUntilPageIsLoaded(driver, "document.URL", "extrato", 10500);
                Wait(7500, 7500, driver);

                WaitUntilElementIsLoaded(driver, "document.querySelector('#transactions-statement')", 60000);
                var divTable = driver.FindElement(By.Id("transactions-statement"));

                if (divTable != null)
                {
                    List<IWebElement> lstTbodyElem = new List<IWebElement>(divTable.FindElements(By.TagName("tbody")));

                    if (lstTbodyElem != null && lstTbodyElem.Count > 0)
                    {
                        foreach (var elemTr in lstTbodyElem)
                        {
                            try
                            {
                                List<IWebElement> lstTrElem = new List<IWebElement>(elemTr.FindElements(By.TagName("tr")));

                                if (lstTrElem != null && lstTrElem.Count > 0)
                                {
                                    for (int i = 0; i < lstTrElem.Count; i++)
                                    {
                                        List<IWebElement> dividendTdElems = new List<IWebElement>(elemTr.FindElements(By.TagName("td")));

                                        if (dividendTdElems != null && dividendTdElems.Count > 0)
                                        {
                                            string strRowData = dividendTdElems[2 + (i * 5)].Text;

                                            if (strRowData != null && strRowData.ToLower().Contains("juros"))
                                            {
                                                string[] rowValues = strRowData.Split(" ");
                                                DateTime paymentDate = DateTime.Now;

                                                decimal netValue = 0;
                                                decimal.TryParse(dividendTdElems[3 + (i * 5)].Text, NumberStyles.Currency, new CultureInfo("pt-br"), out netValue);

                                                RicoDividend ricoDividend = new RicoDividend();
                                                ricoDividend.Type = rowValues[3];
                                                ricoDividend.Symbol = rowValues[8];
                                                ricoDividend.NetValue = netValue;
                                                ricoDividend.GrossValue = netValue;

                                                string dateText = dividendTdElems[0 + (i * 5)].Text;

                                                if (DateTime.TryParse(dateText, new CultureInfo("pt-br"), DateTimeStyles.None, out paymentDate))
                                                {
                                                    ricoDividend.EventDate = paymentDate;
                                                }

                                                ricoDividends.Add(ricoDividend);

                                            }

                                            if (strRowData != null && strRowData.ToLower().Contains("dividendos"))
                                            {
                                                string[] rowValues = strRowData.Split(" ");
                                                DateTime paymentDate = DateTime.Now;

                                                decimal netValue = 0;
                                                decimal.TryParse(dividendTdElems[3 + (i * 5)].Text, NumberStyles.Currency, new CultureInfo("pt-br"), out netValue);

                                                RicoDividend ricoDividend = new RicoDividend();
                                                ricoDividend.Type = rowValues[3];
                                                ricoDividend.Symbol = rowValues[6];
                                                ricoDividend.NetValue = netValue;
                                                ricoDividend.GrossValue = netValue;

                                                string dateText = dividendTdElems[0 + (i * 5)].Text;

                                                if (DateTime.TryParse(dateText, new CultureInfo("pt-br"), DateTimeStyles.None, out paymentDate))
                                                {
                                                    ricoDividend.EventDate = paymentDate;
                                                }

                                                ricoDividends.Add(ricoDividend);

                                            }

                                            if (strRowData != null && strRowData.ToLower().Contains("rendimentos"))
                                            {
                                                string[] rowValues = strRowData.Split(" ");
                                                DateTime paymentDate = DateTime.Now;

                                                decimal netValue = 0;
                                                decimal.TryParse(dividendTdElems[3 + (i * 5)].Text, NumberStyles.Currency, new CultureInfo("pt-br"), out netValue);

                                                RicoDividend ricoDividend = new RicoDividend();
                                                ricoDividend.Type = rowValues[0];
                                                ricoDividend.Symbol = rowValues[3];
                                                ricoDividend.NetValue = netValue;
                                                ricoDividend.GrossValue = netValue;

                                                string dateText = dividendTdElems[0 + (i * 5)].Text;

                                                if (DateTime.TryParse(dateText, new CultureInfo("pt-br"), DateTimeStyles.None, out paymentDate))
                                                {
                                                    ricoDividend.EventDate = paymentDate;
                                                }

                                                ricoDividends.Add(ricoDividend);

                                            }
                                        }
                                    }
                                }
                            }
                            catch (Exception)
                            {

                            }
                        }
                    }
                }
            }
            catch
            {
            }

            return ricoDividends;
        }

        private void GetContact(ChromeDriver driver, ImportRicoResult importRicoResult)
        {
            try
            {
                Wait(3500, 3500, driver);
                driver.Navigate().GoToUrl(@"https://arealogada.rico.com.vc/conta/meus-dados");
                Wait(3500, 3500, driver);

                ReadOnlyCollection<IWebElement> tabs = driver.FindElements(By.CssSelector(".soma-tab.hydrated"));

                if (tabs != null && tabs.Count > 0)
                {
                    tabs[1].Click();

                    ReadOnlyCollection<IWebElement> info = driver.FindElements(By.CssSelector(".list.with-divider"));

                    if (info != null && info.Count > 0)
                    {
                        importRicoResult.ContactDetails = new RicoContactDetails();
                        importRicoResult.ContactPhones = new List<RicoContactPhone>();

                        for (int i = 0; i < info.Count; i++)
                        {
                            string text = info[i].Text;

                            if (!string.IsNullOrWhiteSpace(info[i].Text) && (info[i].Text.Contains("Nome:")))
                            {
                                text = text.Replace("\r\n", string.Empty);
                                int startIndex = text.IndexOf("Nome:") + 5;
                                int endIndex = text.IndexOf("E-mail:") - startIndex;

                                string name = text.Substring(startIndex, endIndex);

                                importRicoResult.ContactDetails.Name = name;
                            }

                            if (!string.IsNullOrWhiteSpace(info[i].Text) && (info[i].Text.Contains("E-mail:")))
                            {
                                text = text.Replace("\r\n", string.Empty);
                                int startIndex = text.IndexOf("E-mail:") + 7;
                                int endIndex = text.IndexOf("Data de nascimento:") - startIndex;

                                string name = text.Substring(startIndex, endIndex);

                                importRicoResult.ContactDetails.Email = name;
                            }

                            if (!string.IsNullOrWhiteSpace(info[i].Text) && (info[i].Text.Contains("Número do CPF:")))
                            {
                                text = text.Replace("\r\n", string.Empty);
                                int startIndex = text.IndexOf("Número do CPF:") + 14;
                                int endIndex = text.IndexOf("Data de emissão:") - startIndex;

                                string name = text.Substring(startIndex, endIndex);

                                importRicoResult.ContactDetails.DocumentNumber = name;
                            }

                            if (!string.IsNullOrWhiteSpace(info[i].Text) && (info[i].Text.Contains("Número de Telefone:")))
                            {
                                text = text.Replace("\r\n", string.Empty);
                                int startIndex = text.IndexOf("Número de Telefone:") + 19;
                                int endIndex = text.IndexOf("Número de Celular:") - startIndex;

                                string name = text.Substring(startIndex, endIndex);

                                RicoContactPhone ricoContactPhone = new RicoContactPhone();
                                ricoContactPhone.PhoneNumber = name;

                                importRicoResult.ContactPhones.Add(ricoContactPhone);
                            }

                            if (!string.IsNullOrWhiteSpace(info[i].Text) && (info[i].Text.Contains("Número de Celular:")))
                            {
                                text = text.Replace("\r\n", string.Empty);
                                int startIndex = text.IndexOf("Número de Celular:") + 18;
                                int endIndex = text.IndexOf("Contato Alternativo:") - startIndex;

                                string name = text.Substring(startIndex, endIndex);

                                RicoContactPhone ricoContactPhone = new RicoContactPhone();
                                ricoContactPhone.PhoneNumber = name;

                                importRicoResult.ContactPhones.Add(ricoContactPhone);
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
            }
        }

        private List<RicoOrder> GetFiis(ChromeDriver driver, ImportRicoResult importRicoResult)
        {
            List<RicoOrder> ricoOrders = new List<RicoOrder>();

            try
            {
                //Wait(3500, 3500, driver);
                driver.Navigate().GoToUrl(@"https://arealogada.rico.com.vc/fundos-imobiliarios");
                WaitUntilPageIsLoaded(driver, "document.URL", "fundos-imobiliarios", 60000);
                Wait(7500, 7500, driver);

                CloseDialog(driver);

                WaitUntilElementIsLoaded(driver, "document.querySelector('.dynamic-table.stocks-positions-table')", 10000);
                var enterEasytokenButton6 = driver.FindElement(By.CssSelector(".dynamic-table.stocks-positions-table"));
                List<IWebElement> lstTbodyElem = new List<IWebElement>(enterEasytokenButton6.FindElements(By.TagName("tbody")));

                foreach (var elemTr in lstTbodyElem)
                {
                    try
                    {
                        List<IWebElement> lstTrElem = new List<IWebElement>(elemTr.FindElements(By.TagName("tr")));

                        if (lstTrElem.Count > 0)
                        {
                            for (int i = 0; i < lstTrElem.Count; i++)
                            {
                                List<IWebElement> lstTdElem1 = new List<IWebElement>(elemTr.FindElements(By.TagName("td")));
                                RicoOrder ricoOrder = new RicoOrder();

                                string strRowData = lstTdElem1[3].Text;

                                try
                                {
                                    lstTdElem1 = new List<IWebElement>(elemTr.FindElements(By.TagName("td")));

                                    decimal avgPrice = 0;
                                    decimal.TryParse(lstTdElem1[3 + (i * 8)].Text.Replace("R$", string.Empty), NumberStyles.Currency, new CultureInfo("pt-br"), out avgPrice);

                                    decimal quantity = 0;
                                    decimal.TryParse(lstTdElem1[5 + (i * 8)].Text, NumberStyles.Currency, new CultureInfo("pt-br"), out quantity);

                                    ricoOrder.Symbol = lstTdElem1[i * (7 + 1)].Text;
                                    ricoOrder.AveragePrice = avgPrice;
                                    ricoOrder.IdOperationType = 1;
                                    ricoOrder.NumberOfShares = quantity;
                                    ricoOrder.EventDate = DateTime.Now;

                                    ricoOrders.Add(ricoOrder);

                                    importRicoResult.ApiResult += " " + string.Format("{0} {1} {2}", ricoOrder.Symbol, ricoOrder.AveragePrice, ricoOrder.NumberOfShares);
                                }
                                catch
                                {
                                }
                            }
                        }
                    }
                    catch
                    {
                    }
                }
            }
            catch
            {
            }

            return ricoOrders;
        }

        private List<RicoDividend> GetFiisDividends(ChromeDriver driver)
        {
            List<RicoDividend> ricoDividends = new List<RicoDividend>();

            try
            {
                driver.Navigate().GoToUrl(@"https://arealogada.rico.com.vc/fundos-imobiliarios");
                WaitUntilPageIsLoaded(driver, "document.URL", "fundos-imobiliarios", 60000);
                Wait(7500, 7500, driver);

                CloseDialog(driver);

                WaitUntilElementIsLoaded(driver, "document.querySelector('#links-section')", 10000);
                var tabs = driver.FindElement(By.Id("links-section"));

                WaitUntilElementIsLoaded(driver, "document.querySelector('.soma-tabs')", 10000);
                var somaTab = tabs.FindElement(By.CssSelector(".soma-tabs"));

                WaitUntilElementIsLoaded(driver, "document.querySelector('.soma-tab.hydrated')", 10000);
                List<IWebElement> divTabs = new List<IWebElement>(somaTab.FindElements(By.CssSelector(".soma-tab.hydrated")));

                Wait(3500, 3500, driver);

                if (divTabs != null && divTabs.Count > 0)
                {
                    foreach (var divTab in divTabs)
                    {
                        string tabText = divTab.Text;

                        if (!string.IsNullOrEmpty(tabText) && tabText.ToLower().Contains("provisionados"))
                        {
                            divTab.Click();
                            //Wait(10500, 10500, driver);

                            WaitUntilElementIsLoaded(driver, "document.querySelector('.dynamic-table')", 10000);
                            var dividendTable = driver.FindElement(By.CssSelector(".dynamic-table"));

                            var dividendBody = dividendTable.FindElement(By.TagName("tbody"));

                            if (dividendBody != null)
                            {
                                List<IWebElement> dividendTrElem = new List<IWebElement>(dividendBody.FindElements(By.TagName("tr")));

                                if (dividendTrElem != null && dividendTrElem.Count > 0)
                                {
                                    for (int i = 0; i < dividendTrElem.Count; i++)
                                    {
                                        List<IWebElement> dividendTdElems = new List<IWebElement>(dividendBody.FindElements(By.TagName("td")));


                                        if (dividendTdElems != null && dividendTdElems.Count > 0)
                                        {
                                            DateTime paymentDate = DateTime.Now;

                                            decimal quantity = 0;
                                            decimal.TryParse(dividendTdElems[2 + (i * 7)].Text, NumberStyles.Currency, new CultureInfo("pt-br"), out quantity);

                                            decimal netValue = 0;
                                            decimal.TryParse(dividendTdElems[5 + (i * 7)].Text, NumberStyles.Currency, new CultureInfo("pt-br"), out netValue);

                                            int baseQuantity = 0;
                                            int.TryParse(dividendTdElems[2 + (i * 7)].Text, NumberStyles.Currency, new CultureInfo("pt-br"), out baseQuantity);

                                            decimal grossValue = 0;
                                            decimal.TryParse(dividendTdElems[3 + (i * 7)].Text, NumberStyles.Currency, new CultureInfo("pt-br"), out grossValue);

                                            RicoDividend ricoDividend = new RicoDividend();

                                            ricoDividend.Type = dividendTdElems[i * (7)].Text;
                                            ricoDividend.Symbol = dividendTdElems[1 + (i * 7)].Text;
                                            ricoDividend.NetValue = netValue;
                                            ricoDividend.BaseQuantity = baseQuantity;
                                            ricoDividend.GrossValue = grossValue;

                                            string dateText = dividendTdElems[6 + (i * 7)].Text;

                                            if (DateTime.TryParse(dateText, new CultureInfo("pt-br"), DateTimeStyles.None, out paymentDate))
                                            {
                                                ricoDividend.EventDate = paymentDate;
                                            }

                                            ricoDividends.Add(ricoDividend);
                                        }
                                    }
                                }
                            }
                        }
                    }
                }
            }
            catch
            {
            }

            return ricoDividends;
        }

        private List<RicoOrder> GetStocks(ChromeDriver driver, ImportRicoResult importRicoResult)
        {
            List<RicoOrder> ricoOrders = new List<RicoOrder>();

            try
            {
                //Wait(3500, 3500, driver);
                driver.Navigate().GoToUrl(@"https://arealogada.rico.com.vc/acoes");
                WaitUntilPageIsLoaded(driver, "document.URL", "acoes", 30000);
                Wait(7500, 7500, driver);

                CloseDialog(driver);

                WaitUntilElementIsLoaded(driver, "document.querySelector('.dynamic-table.stocks-positions-table')", 20000);
                var enterEasytokenButton5 = driver.FindElement(By.CssSelector(".dynamic-table.stocks-positions-table"));
                List<IWebElement> lstTbodyElem = new List<IWebElement>(enterEasytokenButton5.FindElements(By.TagName("tbody")));

                foreach (var elemTr in lstTbodyElem)
                {
                    try
                    {
                        List<IWebElement> lstTrElem = new List<IWebElement>(elemTr.FindElements(By.TagName("tr")));

                        if (lstTrElem.Count > 0)
                        {
                            for (int i = 0; i < lstTrElem.Count; i++)
                            {
                                List<IWebElement> lstTdElem1 = new List<IWebElement>(elemTr.FindElements(By.TagName("td")));
                                RicoOrder ricoOrder = new RicoOrder();

                                string strRowData = lstTdElem1[3].Text;

                                try
                                {
                                    lstTdElem1 = new List<IWebElement>(elemTr.FindElements(By.TagName("td")));

                                    decimal avgPrice = 0;
                                    decimal.TryParse(lstTdElem1[3 + (i * 8)].Text.Replace("R$", string.Empty), NumberStyles.Currency, new CultureInfo("pt-br"), out avgPrice);

                                    decimal quantity = 0;
                                    decimal.TryParse(lstTdElem1[5 + (i * 8)].Text, NumberStyles.Currency, new CultureInfo("pt-br"), out quantity);

                                    ricoOrder.Symbol = lstTdElem1[i * (7 + 1)].Text;
                                    ricoOrder.AveragePrice = avgPrice;
                                    ricoOrder.IdOperationType = 1;
                                    ricoOrder.NumberOfShares = quantity;
                                    ricoOrder.EventDate = DateTime.Now;

                                    ricoOrders.Add(ricoOrder);

                                    importRicoResult.ApiResult += " " + string.Format("{0} {1} {2}", ricoOrder.Symbol, ricoOrder.AveragePrice, ricoOrder.NumberOfShares);
                                }
                                catch
                                {
                                }
                            }
                        }
                    }
                    catch
                    {
                    }
                }
            }
            catch
            {
            }


            return ricoOrders;
        }

        private void CloseDialog(ChromeDriver driver)
        {
            try
            {
                WaitUntilElementIsLoaded(driver, "document.querySelector('.position-absolute.cursor-pointer.icon')", 2000);
                List<IWebElement> closeDialog = new List<IWebElement>(driver.FindElements(By.CssSelector(".position-absolute.cursor-pointer.icon")));

                if (closeDialog != null && closeDialog.Count > 0)
                {
                    foreach (IWebElement element in closeDialog)
                    {
                        element.Click();
                    }
                }
            }
            catch
            {
            }
        }

        private static string RemoveFractionalLetter(string stock)
        {
            if (!string.IsNullOrEmpty(stock) && (stock[stock.Length - 1].ToString().ToUpper() == "F"))
            {
                stock = stock.Remove(stock.Length - 1);
            }

            return stock;
        }

        public void Wait(double delay, double interval, IWebDriver driver)
        {
            // Causes the WebDriver to wait for at least a fixed delay
            var now = DateTime.Now;
            var wait = new WebDriverWait(driver, TimeSpan.FromMilliseconds(delay));
            wait.PollingInterval = TimeSpan.FromMilliseconds(interval);
            wait.Until(wd => (DateTime.Now - now) - TimeSpan.FromMilliseconds(delay) > TimeSpan.Zero);
        }

        private List<RicoOrder> GetStocksAndFiis(ChromeDriver driver, ImportRicoResult importRicoResult)
        {
            List<RicoOrder> ricoOrders = new List<RicoOrder>();

            try
            {
                //Wait(3500, 3500, driver);
                driver.Navigate().GoToUrl(@"https://www.rico.com.vc/arealogada/acoes");
                WaitUntilPageIsLoaded(driver, "document.URL", "acoes", 30000);
                Wait(7500, 7500, driver);

                WaitUntilElementIsLoaded(driver, "document.querySelector('.dynamic-table.stocks-positions-table')", 6000);
                var enterEasytokenButton5 = driver.FindElement(By.CssSelector(".dynamic-table.stocks-positions-table"));
                List<IWebElement> lstTbodyElem = new List<IWebElement>(enterEasytokenButton5.FindElements(By.TagName("tbody")));

                foreach (var elemTr in lstTbodyElem)
                {
                    try
                    {
                        List<IWebElement> lstTrElem = new List<IWebElement>(elemTr.FindElements(By.TagName("tr")));

                        if (lstTrElem.Count > 0)
                        {
                            for (int i = 0; i < lstTrElem.Count; i++)
                            {
                                List<IWebElement> lstTdElem1 = new List<IWebElement>(elemTr.FindElements(By.TagName("td")));
                                RicoOrder ricoOrder = new RicoOrder();

                                string strRowData = lstTdElem1[3].Text;

                                try
                                {
                                    lstTdElem1 = new List<IWebElement>(elemTr.FindElements(By.TagName("td")));

                                    decimal avgPrice = 0;
                                    decimal.TryParse(lstTdElem1[1 + (i * 6)].Text.Replace("R$", string.Empty), NumberStyles.Currency, new CultureInfo("pt-br"), out avgPrice);

                                    decimal quantity = 0;
                                    decimal.TryParse(lstTdElem1[3 + (i * 6)].Text, NumberStyles.Currency, new CultureInfo("pt-br"), out quantity);

                                    ricoOrder.Symbol = lstTdElem1[i * (5 + 1)].Text;
                                    ricoOrder.AveragePrice = avgPrice;
                                    ricoOrder.IdOperationType = 1;
                                    ricoOrder.NumberOfShares = quantity;
                                    ricoOrder.EventDate = DateTime.Now;

                                    string test = lstTdElem1[3 + (i * 6)].Text;

                                    ricoOrders.Add(ricoOrder);

                                    importRicoResult.ApiResult += " " + string.Format("{0} {1} {2}", ricoOrder.Symbol, ricoOrder.AveragePrice, ricoOrder.NumberOfShares);
                                }
                                catch
                                {
                                }
                            }
                        }
                    }
                    catch
                    {
                    }
                }
            }
            catch
            {
            }


            return ricoOrders;
        }


        public void WaitUntilPageIsLoaded(IWebDriver driver, string jsScript, string condition, double timeout, bool contains = true)
        {
            try
            {
                var now = DateTime.Now;
                var wait = new WebDriverWait(driver, TimeSpan.FromMilliseconds(timeout));

                wait.Until(driver => {

                    bool conditionOk = false;

                    var jsExecuted = ((IJavaScriptExecutor)driver).ExecuteScript(string.Format("return {0}", jsScript));

                    if (jsExecuted != null)
                    {
                        if (contains)
                        {
                            conditionOk = jsExecuted.ToString().Contains(condition);
                        }
                        else
                        {
                            conditionOk = jsExecuted.ToString().ToLower().Equals(condition);
                        }

                    }

                    return conditionOk;
                });
            }
            catch
            {
            }
        }

        public void WaitUntilElementIsLoaded(IWebDriver driver, string jsScript, double timeout)
        {
            try
            {
                var now = DateTime.Now;
                var wait = new WebDriverWait(driver, TimeSpan.FromMilliseconds(timeout));

                wait.Until(driver => {

                    bool conditionOk = false;

                    var jsExecuted = ((IJavaScriptExecutor)driver).ExecuteScript(string.Format("return {0}", jsScript));

                    if (jsExecuted != null)
                    {
                        conditionOk = true;
                    }

                    return conditionOk;
                });
            }
            catch
            {
            }
        }

        public void WaitUntilElementListIsLoaded(IWebDriver driver, string jsScript, double timeout)
        {
            try
            {
                var now = DateTime.Now;
                var wait = new WebDriverWait(driver, TimeSpan.FromMilliseconds(timeout));

                wait.Until(driver => {

                    bool conditionOk = false;

                    var jsExecuted = (IReadOnlyCollection<IWebElement>)((IJavaScriptExecutor)driver).ExecuteScript(string.Format("return {0}", jsScript));

                    if (jsExecuted != null && jsExecuted.Count > 0)
                    {
                        conditionOk = true;
                    }

                    return conditionOk;
                });
            }
            catch
            {
            }
        }

        public RicoSummary GetSummaryPosition(string jwtToken, ImportRicoResult importRicoResult)
        {
            RicoSummary ricoSummary = null;
            var handler = new HttpClientHandler();
            handler.UseCookies = false;

            using (var httpClient = new HttpClient(handler))
            {
                using (var request = new HttpRequestMessage(new HttpMethod("GET"), "https://api.rico.com.vc/portal/v1/summary-position"))
                {
                    request.Headers.TryAddWithoutValidation("authorization", jwtToken);
                    request.Headers.TryAddWithoutValidation("ocp-apim-subscription-key", "54cff412ccd84c0db195a8e4955b3072");

                    var response = httpClient.SendAsync(request).Result;

                    if (response.IsSuccessStatusCode)
                    {
                        string result = response.Content.ReadAsStringAsync().Result;

                        var jsonSerializerSettings = new JsonSerializerSettings();
                        jsonSerializerSettings.Culture = CultureInfo.InvariantCulture;
                        jsonSerializerSettings.MissingMemberHandling = MissingMemberHandling.Ignore;
                        ricoSummary = JsonConvert.DeserializeObject<RicoSummary>(result, jsonSerializerSettings);

                        importRicoResult.ApiResult = result;
                    }
                }
            }

            return ricoSummary;
        }

        public RicoPastDividend GetPastDividends(string jwtToken, string startDate, string endDate)
        {
            RicoPastDividend ricoPastDividend = null;
            var handler = new HttpClientHandler();
            handler.UseCookies = false;

            using (var httpClient = new HttpClient(handler))
            {
                using (var request = new HttpRequestMessage(new HttpMethod("GET"), string.Format("https://api.rico.com.vc/portal/v1/finance/statement/?startDate={0}&endDate={1}&offset=1", startDate, endDate)))
                {
                    request.Headers.TryAddWithoutValidation("authorization", jwtToken);
                    request.Headers.TryAddWithoutValidation("ocp-apim-subscription-key", "54cff412ccd84c0db195a8e4955b3072");

                    var response = httpClient.SendAsync(request).Result;

                    if (response.IsSuccessStatusCode)
                    {
                        var jsonSerializerSettings = new JsonSerializerSettings();
                        jsonSerializerSettings.Culture = CultureInfo.InvariantCulture;
                        jsonSerializerSettings.MissingMemberHandling = MissingMemberHandling.Ignore;
                        ricoPastDividend = JsonConvert.DeserializeObject<RicoPastDividend>(response.Content.ReadAsStringAsync().Result, jsonSerializerSettings);
                    }
                }
            }

            return ricoPastDividend;
        }

        public List<RicoContactDetailApi> GetContactDetails(string jwtToken)
        {
            List<RicoContactDetailApi> ricoContactDetailApis = null;
            var handler = new HttpClientHandler();
            handler.UseCookies = false;

            using (var httpClient = new HttpClient(handler))
            {
                using (var request = new HttpRequestMessage(new HttpMethod("GET"), "https://api.rico.com.vc/portal/v1/customer/account-details"))
                {
                    request.Headers.TryAddWithoutValidation("authorization", jwtToken);
                    request.Headers.TryAddWithoutValidation("ocp-apim-subscription-key", "54cff412ccd84c0db195a8e4955b3072");

                    var response = httpClient.SendAsync(request).Result;

                    if (response.IsSuccessStatusCode)
                    {
                        var jsonSerializerSettings = new JsonSerializerSettings();
                        jsonSerializerSettings.Culture = CultureInfo.InvariantCulture;
                        jsonSerializerSettings.MissingMemberHandling = MissingMemberHandling.Ignore;
                        ricoContactDetailApis = JsonConvert.DeserializeObject<List<RicoContactDetailApi>>(response.Content.ReadAsStringAsync().Result, jsonSerializerSettings);
                    }
                }
            }

            return ricoContactDetailApis;
        }
    }
}
