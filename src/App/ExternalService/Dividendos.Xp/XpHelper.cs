using Dividendos.Xp.Interface;
using Dividendos.Xp.Interface.Model;
using Newtonsoft.Json;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Interactions;
using OpenQA.Selenium.Support.UI;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Dynamic;
using System.Globalization;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;

namespace Dividendos.Xp
{
    public class XpHelper : IXpHelper
    {
        public async Task<ImportXpResult> ImportFromXptAsync(string account, string password, string xpToken)
        {
            ImportXpResult importXpResult = new ImportXpResult();
            var chromeOptions = new ChromeOptions();
            chromeOptions.AddArguments("--no-sandbox");
            chromeOptions.AddArguments("--disable-dev-shm-usage");

            string jwtToken = string.Empty;

            using (var driver = new ChromeDriver(chromeOptions))
            {
                driver.Manage().Window.Size = new Size(1920, 1080);
                driver.Manage().Window.Position = new Point(0, 0);
                driver.Manage().Window.Maximize();
                driver.Manage().Timeouts().PageLoad = TimeSpan.FromMinutes(2);

                if (string.IsNullOrWhiteSpace(account))
                {
                    importXpResult.Message = "Conta deve ser informada";
                    importXpResult.ApiResult = "Conta deve ser informada";
                    importXpResult.Success = false;
                    return importXpResult;
                }

                driver.Navigate().GoToUrl(@"https://portal.xpi.com.br/");

                WaitUntilPageIsLoaded(driver, "document.URL", "xpi", 30000);
                //Wait(1000, 1000, driver);

                WaitUntilElementIsLoaded(driver, "document.querySelector('#txtLogin')", 90000);
                var inputIdentifier = driver.FindElement(By.Id("txtLogin"));
                inputIdentifier.SendKeys(account);

                Wait(1000, 1000, driver);

                WaitUntilElementIsLoaded(driver, "document.querySelector('#btnOkLogin')", 60000);
                var btnOkLogin = driver.FindElement(By.Id("btnOkLogin"));

                ClickElement(driver, btnOkLogin);

                Wait(1000, 1000, driver);

                WebElement txtPass = null;

                try
                {
                    WaitUntilElementIsLoaded(driver, "document.querySelector('#txtPass')", 60000);
                    txtPass = (WebElement)driver.FindElement(By.Id("txtPass"));
                }
                catch (Exception)
                {
                }

                if (string.IsNullOrWhiteSpace(password))
                {
                    importXpResult.Message = "Senha deve ser informada";
                    importXpResult.ApiResult = "Senha deve ser informada";
                    importXpResult.Success = false;
                    return importXpResult;
                }

                if (txtPass != null)
                {
                    WaitUntilElementIsLoaded(driver, "document.querySelector('#btnPass0')", 60000);
                    var pass0 = driver.FindElement(By.Id("btnPass0"));

                    WaitUntilElementIsLoaded(driver, "document.querySelector('#btnPass1')", 60000);
                    var pass1 = driver.FindElement(By.Id("btnPass1"));

                    WaitUntilElementIsLoaded(driver, "document.querySelector('#btnPass2')", 60000);
                    var pass2 = driver.FindElement(By.Id("btnPass2"));

                    WaitUntilElementIsLoaded(driver, "document.querySelector('#btnPass3')", 60000);
                    var pass3 = driver.FindElement(By.Id("btnPass3"));

                    WaitUntilElementIsLoaded(driver, "document.querySelector('#btnPass4')", 60000);
                    var pass4 = driver.FindElement(By.Id("btnPass4"));

                    string xpPass = password;

                    //Wait(10000, 10000, driver);

                    foreach (char item in xpPass)
                    {
                        try
                        {
                            if (pass0.Text.Contains(item))
                            {
                                ClickElement(driver, pass0);
                            }

                            if (pass1.Text.Contains(item))
                            {
                                ClickElement(driver, pass1);
                            }

                            if (pass2.Text.Contains(item))
                            {
                                ClickElement(driver, pass2);
                            }

                            if (pass3.Text.Contains(item))
                            {
                                ClickElement(driver, pass3);
                            }

                            if (pass4.Text.Contains(item))
                            {
                                ClickElement(driver, pass4);
                            }
                        }
                        catch (Exception ex)
                        {
                            continue;
                            //throw;
                        }

                    }

                    WaitUntilElementIsLoaded(driver, "document.querySelector('#btnEntrar')", 60000);
                    var btnEntrar = driver.FindElement(By.Id("btnEntrar"));

                    Wait(1000, 1000, driver);
                    ClickElement(driver, btnEntrar);

                    Wait(1000, 1000, driver);

                    WebElement txtXpToken = null;

                    try
                    {
                        WaitUntilElementIsLoaded(driver, "document.querySelector('#txtXpToken')", 60000);
                        txtXpToken = (WebElement)driver.FindElement(By.Id("txtXpToken"));
                    }
                    catch (Exception)
                    {
                    }

                    if (txtXpToken != null)
                    {

                        if (string.IsNullOrWhiteSpace(xpToken))
                        {
                            importXpResult.Message = "Token deve ser informado";
                            importXpResult.ApiResult = "Token deve ser informado";
                            importXpResult.Success = false;
                            return importXpResult;
                        }

                        txtXpToken.SendKeys(xpToken);

                        var btnValidarXpToken = driver.FindElement(By.Id("btnValidarXpToken"));
                        Wait(1000, 1000, driver);
                        ClickElement(driver, btnValidarXpToken);

                        Wait(1000, 1000, driver);

                        driver.Navigate().GoToUrl(@"https://experiencia.xpi.com.br/conta-corrente/extrato/#/");
                        WaitUntilPageIsLoaded(driver, "document.URL", "extrato", 30000);

                        //Wait(1000, 1000, driver);

                        var jsToBeExecuted = "return sessionStorage.getItem('token_data_header')";
                        var token = ((IJavaScriptExecutor)driver).ExecuteScript(jsToBeExecuted);

                        if (token != null)
                        {
                            dynamic resultAPI = JsonConvert.DeserializeObject<ExpandoObject>(token.ToString());

                            jwtToken = resultAPI.token;
                            importXpResult.JwtToken = resultAPI.token;
                            importXpResult.Success = true;
                        }
                        else
                        {
                            importXpResult.Message = "Token inválido. Tente novamente";
                            importXpResult.ApiResult = "Token inválido. Tente novamente";
                            importXpResult.Success = false;
                            return importXpResult;
                        }
                    }
                    else
                    {
                        importXpResult.Message = "Senha inválida. Tente novamente";
                        importXpResult.ApiResult = "Senha inválida. Tente novamente";
                        importXpResult.Success = false;
                        return importXpResult;
                    }
                }
                else
                {
                    importXpResult.Message = "Código do cliente inválido. Tente novamente";
                    importXpResult.ApiResult = "Código do cliente inválido. Tente novamente";
                    importXpResult.Success = false;
                    return importXpResult;
                }
            }


            if (importXpResult.Success)
            {
                List<dynamic> symbols = new List<dynamic>();
                XpPortfolio xpPortfolio = await GetSymbols(jwtToken, importXpResult);

                if (xpPortfolio != null)
                {
                    if (xpPortfolio.Investments != null && xpPortfolio.Investments.Count > 0)
                    {
                        foreach (Investment investment in xpPortfolio.Investments)
                        {
                            foreach (InvestmentChild investmentChild in investment.Children)
                            {
                                foreach (ChildChild child in investmentChild.Children)
                                {
                                    symbols.Add(new { Symbol = child.SurrogateId, Type = investment.Type, Name = child.Name });
                                }
                            }
                        }
                    }

                    if (xpPortfolio.Earnings != null && xpPortfolio.Earnings.Count > 0)
                    {
                        importXpResult.Dividends = new List<XpDividend>();

                        foreach (Earning earning in xpPortfolio.Earnings)
                        {
                            if (earning.Children != null && earning.Children.Count > 0)
                            {
                                foreach (EarningChild earningChild in earning.Children)
                                {
                                    if (earningChild.Children != null && earningChild.Children.Count > 0)
                                    {
                                        if (!string.IsNullOrWhiteSpace(earningChild.Children.First().ProvisionDate))
                                        {
                                            int year = Convert.ToInt32(earningChild.Children.First().ProvisionDate.Substring(0, 4));
                                            int month = Convert.ToInt32(earningChild.Children.First().ProvisionDate.Substring(5, 2));
                                            int day = Convert.ToInt32(earningChild.Children.First().ProvisionDate.Substring(8, 2));

                                            decimal netValue = 0;
                                            decimal.TryParse(earningChild.Children.First().ProvisionValue, NumberStyles.Currency, CultureInfo.InvariantCulture, out netValue);

                                            XpDividend xpDividend = new XpDividend();
                                            xpDividend.EventDate = new DateTime(year, month, day);
                                            xpDividend.NetValue = netValue;
                                            xpDividend.Symbol = earningChild.Name;
                                            xpDividend.Type = earningChild.Children.First().Event;

                                            importXpResult.Dividends.Add(xpDividend);
                                        }
                                    }
                                }
                            }
                        }
                    }
                }

                List<XpPosition> xpPositions = new List<XpPosition>();
                List<XpInvestment> xpInvestments = new List<XpInvestment>();

                if (symbols != null && symbols.Count > 0)
                {
                    foreach (var symbol in symbols)
                    {
                        if (symbol.Type != "1" && symbol.Type != "2" && symbol.Type != "6")
                        {
                            XpPosition xpPosition = await GetPosition(jwtToken, symbol, importXpResult);

                            if (xpPosition != null)
                            {
                                xpPositions.Add(xpPosition);
                            }
                        }
                        else
                        {
                            XpInvestment xpInvestment = await GetOtherInvestments(jwtToken, symbol, importXpResult);

                            if (xpInvestment != null)
                            {
                                xpInvestment.Name = symbol.Name;
                                xpInvestments.Add(xpInvestment);
                            }
                        }
                    }
                }

                if (xpPositions != null && xpPositions.Count > 0)
                {
                    importXpResult.Orders = new List<XpOrder>();

                    foreach (XpPosition xpPosition in xpPositions)
                    {
                        if (xpPosition.Position != null)
                        {
                            decimal avgPrice = 0;
                            decimal.TryParse(xpPosition.Position.PositionValue.AverageCost, NumberStyles.Currency, CultureInfo.InvariantCulture, out avgPrice);

                            if (avgPrice == 0)
                            {
                                decimal.TryParse(xpPosition.Position.PositionValue.UnitPrice, NumberStyles.Currency, CultureInfo.InvariantCulture, out avgPrice);
                            }

                            decimal quantity = 0;
                            decimal.TryParse(xpPosition.Position.PositionValue.TotalQuantity, NumberStyles.Currency, CultureInfo.InvariantCulture, out quantity);

                            if (quantity > 0)
                            {
                                XpOrder xpOrder = new XpOrder();
                                xpOrder.AveragePrice = avgPrice;
                                xpOrder.EventDate = DateTime.Now;
                                xpOrder.IdOperationType = 1;
                                xpOrder.NumberOfShares = quantity;
                                xpOrder.Symbol = RemoveFractionalLetter(xpPosition.Position.PositionValue.Name);

                                importXpResult.Orders.Add(xpOrder);
                            }
                        }
                    }
                }

                if (xpInvestments != null && xpInvestments.Count > 0)
                {
                    importXpResult.Funds = new List<XpFund>();
                    importXpResult.Bonds = new List<XpBond>();

                    foreach (XpInvestment xpInvestment in xpInvestments)
                    {
                        if (xpInvestment.Type == "2")
                        {
                            if (xpInvestment.Description != null && xpInvestment.Description.DescriptionValue != null && xpInvestment.YieldData != null)
                            {
                                XpBond xpBond = new XpBond();
                                xpBond.InvestmenType = "2";
                                decimal netValue = 0;
                                decimal.TryParse(xpInvestment.YieldData.NetClosingValue, NumberStyles.Currency, CultureInfo.InvariantCulture, out netValue);

                                xpBond.Value = netValue;
                                xpBond.Issuer = xpInvestment.Description.DescriptionValue.IssuerName;
                                xpBond.Name = xpInvestment.Name;

                                importXpResult.Bonds.Add(xpBond);
                            }
                        }
                        else if (xpInvestment.Type == "1")
                        {

                            if (xpInvestment.YieldData != null)
                            {
                                decimal netValue = 0;
                                decimal.TryParse(xpInvestment.YieldData.NetClosingValue, NumberStyles.Currency, CultureInfo.InvariantCulture, out netValue);

                                XpFund xpFund = new XpFund();
                                xpFund.Value = netValue;
                                xpFund.Name = xpInvestment.Name;

                                importXpResult.Funds.Add(xpFund);
                            }
                        }
                        else if (xpInvestment.Type == "6")
                        {
                            if (xpInvestment.Position != null && xpInvestment.Position.PositionValue2 != null)
                            {
                                decimal netValue = 0;
                                decimal.TryParse(xpInvestment.Position.PositionValue2.Position, NumberStyles.Currency, CultureInfo.InvariantCulture, out netValue);

                                XpFund xpFund = new XpFund();
                                xpFund.Value = netValue;
                                xpFund.Name = xpInvestment.Name;

                                importXpResult.Funds.Add(xpFund);
                            }
                        }

                    }
                }

                try
                {
                    await GetPastDividends(jwtToken, "2021", importXpResult);
                    await GetNextDividends(jwtToken, importXpResult);
                    await GetStocksSummary(jwtToken, importXpResult);
                    await GetFiisSummary(jwtToken, importXpResult);
                }
                catch
                {
                }
            }

            return importXpResult;
        }

        private static string RemoveFractionalLetter(string stock)
        {
            if (!string.IsNullOrEmpty(stock) && (stock[stock.Length - 1].ToString().ToUpper() == "F"))
            {
                stock = stock.Remove(stock.Length - 1);
            }

            return stock;
        }

        private static async Task<XpPosition> GetPosition(string jwtToken, dynamic symbol, ImportXpResult importXpResult)
        {
            XpPosition xpPosition = null;

            var handler = new HttpClientHandler();
            handler.UseCookies = false;

            using (var httpClient = new HttpClient(handler))
            {
                string url = string.Format("https://api.xpi.com.br/my-portfolio/v3/old/products/{0}/assets/{1}", symbol.Type, symbol.Symbol);

                using (var request = new HttpRequestMessage(new HttpMethod("GET"), url))
                {
                    request.Headers.TryAddWithoutValidation("authorization", "Bearer " + jwtToken);
                    request.Headers.TryAddWithoutValidation("ocp-apim-subscription-key", "d8b0fb4a6a964315a22534bc59255218");
                    request.Headers.TryAddWithoutValidation("channel", "1");

                    request.Content = new StringContent("");
                    request.Content.Headers.ContentType = MediaTypeHeaderValue.Parse("application/x-www-form-urlencoded");

                    var response = await httpClient.SendAsync(request);
                    string responseContent = null;

                    if (response.IsSuccessStatusCode)
                    {
                        responseContent = await response.Content.ReadAsStringAsync();
                        xpPosition = JsonConvert.DeserializeObject<XpPosition>(responseContent);
                    }

                    if (string.IsNullOrWhiteSpace(importXpResult.ApiResult))
                    {
                        importXpResult.ApiResult = responseContent;
                    }
                    else
                    {
                        importXpResult.ApiResult = importXpResult.ApiResult + response.Content;
                    }
                }
            }

            return xpPosition;
        }

        private static async Task<XpPortfolio> GetSymbols(string jwtToken, ImportXpResult importXpResult)
        {
            XpPortfolio xpPortfolio = null;

            var handler = new HttpClientHandler();
            handler.UseCookies = false;

            using (var httpClient = new HttpClient(handler))
            {
                using (var request = new HttpRequestMessage(new HttpMethod("GET"), "https://api.xpi.com.br/my-portfolio/v3"))
                {
                    request.Headers.TryAddWithoutValidation("authorization", "Bearer " + jwtToken);
                    request.Headers.TryAddWithoutValidation("ocp-apim-subscription-key", "d8b0fb4a6a964315a22534bc59255218");
                    request.Headers.TryAddWithoutValidation("channel", "1");

                    request.Content = new StringContent("");
                    request.Content.Headers.ContentType = MediaTypeHeaderValue.Parse("application/x-www-form-urlencoded");

                    var response = await httpClient.SendAsync(request);
                    string responseContent = null;

                    if (response.IsSuccessStatusCode)
                    {
                        responseContent = await response.Content.ReadAsStringAsync();
                        xpPortfolio = JsonConvert.DeserializeObject<XpPortfolio>(response.Content.ReadAsStringAsync().Result);
                    }

                    if (string.IsNullOrWhiteSpace(importXpResult.ApiResult))
                    {
                        importXpResult.ApiResult = responseContent;
                    }
                    else
                    {
                        importXpResult.ApiResult = importXpResult.ApiResult + response.Content;
                    }
                }
            }

            return xpPortfolio;
        }

        public static async Task<XpInvestment> GetOtherInvestments(string jwtToken, dynamic symbol, ImportXpResult importXpResult)
        {
            XpInvestment xpInvestments = null;
            var handler = new HttpClientHandler();
            handler.UseCookies = false;

            using (var httpClient = new HttpClient(handler))
            {
                using (var request = new HttpRequestMessage(new HttpMethod("GET"), string.Format("https://api.xpi.com.br/my-portfolio/v3/products/{0}/assets/{1}", symbol.Type, symbol.Symbol)))
                {
                    request.Headers.TryAddWithoutValidation("authorization", "Bearer " + jwtToken);
                    request.Headers.TryAddWithoutValidation("ocp-apim-subscription-key", "d8b0fb4a6a964315a22534bc59255218");
                    request.Headers.TryAddWithoutValidation("channel", "1");

                    request.Content = new StringContent("");
                    request.Content.Headers.ContentType = MediaTypeHeaderValue.Parse("application/x-www-form-urlencoded");

                    var response = await httpClient.SendAsync(request);
                    string responseContent = null;

                    if (response.IsSuccessStatusCode)
                    {
                        responseContent = await response.Content.ReadAsStringAsync();
                        xpInvestments = JsonConvert.DeserializeObject<XpInvestment>(responseContent);
                    }

                    if (string.IsNullOrWhiteSpace(importXpResult.ApiResult))
                    {
                        importXpResult.ApiResult = responseContent;
                    }
                    else
                    {
                        importXpResult.ApiResult = importXpResult.ApiResult + response.Content;
                    }
                }
            }

            return xpInvestments;
        }

        public async Task GetPastDividends(string jwtToken, string year, ImportXpResult importXpResult)
        {
            var handler = new HttpClientHandler();
            handler.UseCookies = false;

            using (var httpClient = new HttpClient(handler))
            {
                using (var request = new HttpRequestMessage(new HttpMethod("GET"), string.Format("https://api.xpi.com.br/xp-equities-portfolio-api/v1/Customers/portfolio/earnings/past?type=All&period={0}", year)))
                {
                    request.Headers.TryAddWithoutValidation("authorization", "Bearer " + jwtToken);
                    request.Headers.TryAddWithoutValidation("ocp-apim-subscription-key", "4099b36f826749e1acab295989795688");
                    request.Headers.TryAddWithoutValidation("channel", "1");

                    request.Content = new StringContent("");
                    request.Content.Headers.ContentType = MediaTypeHeaderValue.Parse("application/x-www-form-urlencoded");

                    var response = await httpClient.SendAsync(request);
                    string responseContent = null;

                    if (response.IsSuccessStatusCode)
                    {
                        responseContent = await response.Content.ReadAsStringAsync();
                    }

                    if (!string.IsNullOrWhiteSpace(responseContent))
                    {
                        importXpResult.ResponseBody += string.Format(" Past {0}: {1}", year, responseContent);
                    }
                }
            }
        }

        public async Task GetNextDividends(string jwtToken, ImportXpResult importXpResult)
        {
            var handler = new HttpClientHandler();
            handler.UseCookies = false;

            using (var httpClient = new HttpClient(handler))
            {
                using (var request = new HttpRequestMessage(new HttpMethod("GET"), "https://api.xpi.com.br/xp-equities-portfolio-api/v1/Customers/portfolio/earnings/next"))
                {
                    request.Headers.TryAddWithoutValidation("authorization", "Bearer " + jwtToken);
                    request.Headers.TryAddWithoutValidation("ocp-apim-subscription-key", "4099b36f826749e1acab295989795688");
                    request.Headers.TryAddWithoutValidation("channel", "1");

                    request.Content = new StringContent("");
                    request.Content.Headers.ContentType = MediaTypeHeaderValue.Parse("application/x-www-form-urlencoded");

                    var response = await httpClient.SendAsync(request);
                    string responseContent = null;

                    if (response.IsSuccessStatusCode)
                    {
                        responseContent = await response.Content.ReadAsStringAsync();
                    }

                    if (!string.IsNullOrWhiteSpace(responseContent))
                    {
                        importXpResult.ResponseBody += string.Format(" Next: {0}", responseContent);
                    }
                }
            }
        }

        public async Task GetStocksSummary(string jwtToken, ImportXpResult importXpResult)
        {
            var handler = new HttpClientHandler();
            handler.UseCookies = false;

            using (var httpClient = new HttpClient(handler))
            {
                using (var request = new HttpRequestMessage(new HttpMethod("GET"), "https://api.xpi.com.br/xp-equities-portfolio-api/v1/Customers/portfolio/position/stocks"))
                {
                    request.Headers.TryAddWithoutValidation("authorization", "Bearer " + jwtToken);
                    request.Headers.TryAddWithoutValidation("ocp-apim-subscription-key", "4099b36f826749e1acab295989795688");
                    request.Headers.TryAddWithoutValidation("channel", "1");

                    request.Content = new StringContent("");
                    request.Content.Headers.ContentType = MediaTypeHeaderValue.Parse("application/x-www-form-urlencoded");

                    var response = await httpClient.SendAsync(request);
                    string responseContent = null;

                    if (response.IsSuccessStatusCode)
                    {
                        responseContent = await response.Content.ReadAsStringAsync();
                    }

                    if (!string.IsNullOrWhiteSpace(responseContent))
                    {
                        importXpResult.ResponseBody += string.Format(" Stocks: {0}", responseContent);
                    }
                }
            }
        }

        public async Task GetFiisSummary(string jwtToken, ImportXpResult importXpResult)
        {
            var handler = new HttpClientHandler();
            handler.UseCookies = false;

            using (var httpClient = new HttpClient(handler))
            {
                using (var request = new HttpRequestMessage(new HttpMethod("GET"), "https://api.xpi.com.br/xp-equities-portfolio-api/v1/Customers/portfolio/position/realstatefunds"))
                {
                    request.Headers.TryAddWithoutValidation("authorization", "Bearer " + jwtToken);
                    request.Headers.TryAddWithoutValidation("ocp-apim-subscription-key", "4099b36f826749e1acab295989795688");
                    request.Headers.TryAddWithoutValidation("channel", "1");

                    request.Content = new StringContent("");
                    request.Content.Headers.ContentType = MediaTypeHeaderValue.Parse("application/x-www-form-urlencoded");

                    var response = await httpClient.SendAsync(request);
                    string responseContent = null;

                    if (response.IsSuccessStatusCode)
                    {
                        responseContent = await response.Content.ReadAsStringAsync();
                    }

                    if (!string.IsNullOrWhiteSpace(responseContent))
                    {
                        importXpResult.ResponseBody += string.Format(" Fiis: {0}", responseContent);
                    }
                }
            }
        }


        public void Wait(double delay, double interval, IWebDriver driver)
        {
            // Causes the WebDriver to wait for at least a fixed delay
            var now = DateTime.Now;
            var wait = new WebDriverWait(driver, TimeSpan.FromMilliseconds(delay));
            wait.PollingInterval = TimeSpan.FromMilliseconds(interval);
            wait.Until(wd => (DateTime.Now - now) - TimeSpan.FromMilliseconds(delay) > TimeSpan.Zero);
        }

        public void WaitUntilPageIsLoaded(IWebDriver driver, string jsScript, string condition, double timeout)
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
                        conditionOk = jsExecuted.ToString().Contains(condition);
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

        private static void ClickElement(ChromeDriver driver, IWebElement element)
        {
            Actions actionClick = new Actions(driver);
            actionClick.MoveToElement(element).Click().Perform();
        }

    }
}
