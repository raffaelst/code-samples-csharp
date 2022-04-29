using Dividendos.Clear.Interface;
using Dividendos.Clear.Interface.Model;
using Newtonsoft.Json;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Interactions;
using OpenQA.Selenium.Support.UI;
using SeleniumExtras.WaitHelpers;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Drawing;
using System.Dynamic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace Dividendos.Clear
{
    public class ClearHelper : IClearHelper
    {
        public ImportClearResult ImportFromClear(string identifier, string birthDate, string password, DateTime? lastEventDate, bool getContactDetails, bool getDividends)
        {
            ImportClearResult importClearResult = new ImportClearResult();

            var chromeOptions = new ChromeOptions();
            chromeOptions.AddArguments("--no-sandbox");
            chromeOptions.AddArguments("--disable-dev-shm-usage");


            using (var driver = new ChromeDriver(chromeOptions))
            {
                driver.Manage().Window.Size = new Size(1920, 1080);
                driver.Manage().Window.Position = new Point(0, 0);
                driver.Manage().Window.Maximize();
                driver.Manage().Timeouts().PageLoad = TimeSpan.FromMinutes(2);

                try
                {
                    importClearResult = Login(identifier, birthDate, password, driver);

                    if (importClearResult.Success)
                    {
                        //GET TOKEN
                        dynamic resultAPI = GetJwtToken(driver);

                        importClearResult.OrderItems = GetOperationItems(lastEventDate, resultAPI);
                        //importClearResult.Orders = GetOperations(driver, importClearResult);
                        importClearResult.Orders = GetSwingOperations(driver, importClearResult);

                        importClearResult.Success = true;

                        if (getDividends)
                        {
                            importClearResult.Dividends = GetDividends(lastEventDate, driver, importClearResult);
                        }

                        if (getContactDetails)
                        {
                            GetContact(driver, importClearResult);
                        }
                    }
                }
                catch (Exception ex)
                {
                    driver.Close();
                    importClearResult.DividendException = string.Format("ClearException {0} : {1} : {2} ", ex.Message, ex.InnerException, ex.StackTrace);
                }
            }

            return importClearResult;
        }

        private void GetContact(ChromeDriver driver, ImportClearResult importClearResult)
        {
            try
            {
                driver.Navigate().GoToUrl(@"https://pro.clear.com.br/#gerenciamento/minha-conta/meus-dados");
                Wait(25500, 25500, driver);

                driver.SwitchTo().Frame("content-page");
                ReadOnlyCollection<IWebElement> tabs = driver.FindElements(By.CssSelector(".soma-tab.hydrated"));

                if (tabs != null && tabs.Count > 0)
                {
                    ClickElement(driver, tabs[1]);
                    Wait(500, 500, driver);

                    var email = driver.FindElement(By.Name("email"));

                    if (email != null)
                    {
                        importClearResult.Email = email.GetAttribute("value");
                    }

                    var phone = driver.FindElement(By.Name("telefone"));

                    if (phone != null)
                    {
                        importClearResult.Phone = phone.GetAttribute("value");
                    }
                }
            }
            catch
            {
            }
        }

        public ImportClearResult RestoreDividends(string identifier, string birthDate, string password, DateTime? lastEventDate)
        {
            ImportClearResult importClearResult = new ImportClearResult();

            var chromeOptions = new ChromeOptions();
            chromeOptions.AddArguments("--no-sandbox");
            chromeOptions.AddArguments("--disable-dev-shm-usage");

            using (var driver = new ChromeDriver(chromeOptions))
            {
                driver.Manage().Window.Size = new Size(1920, 1080);
                driver.Manage().Window.Position = new Point(0, 0);
                driver.Manage().Window.Maximize();
                driver.Manage().Timeouts().PageLoad = TimeSpan.FromMinutes(2);

                importClearResult = Login(identifier, birthDate, password, driver);
                if (importClearResult.Success)
                {
                    importClearResult.Dividends = GetDividends(lastEventDate, driver, importClearResult);
                }
            }

            return importClearResult;
        }

        private ImportClearResult Login(string identifier, string birthDate, string password, ChromeDriver driver)
        {
            ImportClearResult importClearResult = new ImportClearResult();

            string errorText = string.Empty;
            driver.Navigate().GoToUrl(@"https://login.clear.com.br/pit/login/");
            //Wait(6500, 6500, driver);

            WaitUntilPageIsLoaded(driver, "document.URL", "login", 30000);

            //driver.Manage().Window.Maximize();

            var inputDocument = driver.FindElement(By.Id("identificationNumber"));
            inputDocument.SendKeys(identifier);
            Wait(100, 100, driver);

            var inputPwd = driver.FindElement(By.Id("password"));
            inputPwd.SendKeys(password);
            Wait(100, 100, driver);


            var inputBirth = driver.FindElement(By.Id("dob"));
            inputBirth.SendKeys(birthDate);


            var btnOk = driver.FindElement(By.CssSelector(".bt_signin"));
            btnOk.Click();

            WaitUntilPageIsLoaded(driver, "document.URL", "home", 180000);
            //Wait(180000, 180000, driver);

            try
            {
                var inputDocumentError = driver.FindElement(By.Id("identificationNumber-error"));

                if (inputDocumentError != null)
                {
                    string errorMessage = inputDocumentError.Text;

                    if (errorMessage.Contains("obrigatório"))
                    {
                        importClearResult.Message = "Número de documento deve ser informado";
                        importClearResult.Message = "Número de documento deve ser informado";
                    }
                    else if (errorMessage.Contains("inválido"))
                    {
                        importClearResult.Message = "Número de documento inválido";
                        importClearResult.Message = "Número de documento inválido";
                    }


                    importClearResult.Success = false;

                    return importClearResult;
                }
            }
            catch
            {
            }

            try
            {
                var inputDocumentError = driver.FindElement(By.Id("password-error"));

                if (inputDocumentError != null)
                {
                    string errorMessage = inputDocumentError.Text;

                    if (errorMessage.Contains("obrigatório"))
                    {
                        importClearResult.Message = "Senha deve ser informada";
                        importClearResult.Message = "Senha deve ser informada";
                    }
                    else if (errorMessage.Contains("inválido"))
                    {
                        importClearResult.Message = "Senha inválida";
                        importClearResult.Message = "Senha inválida";
                    }


                    importClearResult.Success = false;

                    return importClearResult;
                }
            }
            catch
            {
            }

            try
            {
                var birthError = driver.FindElement(By.CssSelector(".text-danger.field-validation-error"));

                if (birthError != null)
                {
                    string errorMessage = birthError.Text;

                    if (errorMessage.Contains("obrigatório"))
                    {
                        importClearResult.Message = "Data de nascimento deve ser informada";
                        importClearResult.Message = "Data de nascimento deve ser informada";
                    }
                    else
                    {
                        errorMessage = errorMessage.Replace("DoB", "data de nascimento");
                        importClearResult.Message = errorMessage;
                        importClearResult.Message = errorMessage;
                    }

                    importClearResult.Success = false;

                    return importClearResult;
                }
            }
            catch
            {
            }

            try
            {
                var validationMessage = driver.FindElement(By.CssSelector(".cont_error_login.validation-summary-errors"));

                if (validationMessage != null)
                {
                    var errorMessage = validationMessage.FindElement(By.TagName("li"));

                    importClearResult.Message = errorMessage.Text;
                    importClearResult.Message = errorMessage.Text;

                    importClearResult.Success = false;

                    return importClearResult;
                }
            }
            catch
            {
            }

            importClearResult.Success = true;


            return importClearResult;
        }

        private List<ClearDividend> GetDividends(DateTime? lastEventDate, ChromeDriver driver, ImportClearResult importClearResult)
        {
            List<ClearDividend> dividends = new List<ClearDividend>();
            DateTime startDate = new DateTime(2019, 1, 1);
            string txtDay = string.Empty;

            try
            {
                driver.Navigate().GoToUrl(@"https://pro.clear.com.br/#minha-conta/extrato");
                WaitUntilPageIsLoaded(driver, "document.URL", "extrato", 180000);
                Wait(10500, 10500, driver);
                //driver.Navigate().GoToUrl(@"https://pro.clear.com.br/#minha-conta/extrato");
                //Wait(60000, 60000, driver);

                driver.SwitchTo().Frame("content-page");

                //CloseModalRlp(driver);

                if (lastEventDate.HasValue)
                {
                    startDate = new DateTime(lastEventDate.Value.Year, lastEventDate.Value.Month, lastEventDate.Value.Day);
                }

                //DateTime endDate = startDate.AddDays(85);

                importClearResult.ResponseBody += " Dividendos: ";

                while (startDate <= DateTime.Now.Date && IsBrowserOpened(driver))
                {
                    try
                    {
                        txtDay = string.Empty;
                        GetDividends(startDate, driver, ref dividends, importClearResult, out txtDay);
                        startDate = startDate.AddDays(60);
                    }
                    catch (Exception ex)
                    {
                        importClearResult.DividendException = string.Format("ClearException {0} : {1} : {2} : date: {3} day: {4}", ex.Message, ex.InnerException, ex.StackTrace, startDate.ToString("dd/MM/yyyy"), txtDay);
                    }
                }

            }
            catch (Exception ex)
            {
                importClearResult.DividendException = string.Format("ClearException {0} : {1} : {2} : date: {3} day: {4}", ex.Message, ex.InnerException, ex.StackTrace, startDate.ToString("dd/MM/yyyy"), txtDay);
            }


            return dividends;
        }

        private void GetDividends(DateTime startDate, ChromeDriver driver, ref List<ClearDividend> dividends, ImportClearResult importClearResult, out string txtDay)
        {
            txtDay = string.Empty;
            //WaitUntilElementIsLoaded(driver, "document.querySelector('.filters-content')", 60000);
            //var tabsPeriod = driver.FindElements(By.CssSelector(".filters__btn"));
            //ClickElement(driver, tabsPeriod[0]);
            //Wait(1000, 1000, driver);

            WaitUntilElementIsLoaded(driver, "document.querySelector('.filter-button.soma-chip.clickable.hydrated')", 60000);
            var infoList = driver.FindElement(By.CssSelector(".filter-button.soma-chip.clickable.hydrated"));
            ClickElement(driver, infoList);
            //Wait(1000, 1000, driver);


            WaitUntilElementListIsLoaded(driver, "window.frames.document.querySelector('.soma-calendar.hydrated').shadowRoot.querySelectorAll('.hydrated')", 120000);
            var calendarJs = "return window.frames.document.querySelector('.soma-calendar.hydrated').shadowRoot.querySelectorAll('.hydrated');";


            var calendarBtn = (IReadOnlyCollection<IWebElement>)((IJavaScriptExecutor)driver).ExecuteScript(calendarJs);
            //Wait(1000, 1000, driver);

            DateTime currentDate = DateTime.Now;

            int totalMonths = ((currentDate.Year - startDate.Year) * 12) + currentDate.Month - startDate.Month;

            for (int i = 0; i < totalMonths; i++)
            {
                ClickElement(driver, calendarBtn.First());
                //Wait(100, 100, driver);
            }

            WaitUntilElementIsLoaded(driver, "window.frames.document.querySelector('.soma-calendar.hydrated').shadowRoot.querySelector('.days')", 60000);
            var calendarDaysJs = "return window.frames.document.querySelector('.soma-calendar.hydrated').shadowRoot.querySelector('.days');";
            WebElement calendarDays = (WebElement)((IJavaScriptExecutor)driver).ExecuteScript(calendarDaysJs);

            List<IWebElement> days = new List<IWebElement>(calendarDays.FindElements(By.TagName("li")));

            if (days != null && days.Count > 0)
            {
                for (int i = 0; i < days.Count; i++)
                {
                    string className = days[i].GetAttribute("class");
                    string texttt = days[i].Text;

                    if (!string.IsNullOrEmpty(texttt) && !className.Contains("disabled"))
                    {
                        txtDay = texttt;

                        WaitUntilElementIsLoaded(driver, "window.frames.document.querySelector('.soma-calendar.hydrated').shadowRoot.innerHTML", 60000);
                        var headerJs = "return window.frames.document.querySelector('.soma-calendar.hydrated').shadowRoot.innerHTML;";
                        string headerCal = (string)((IJavaScriptExecutor)driver).ExecuteScript(headerJs);
                        txtDay = txtDay + " " + headerCal;

                        //Wait(2000, 2000, driver);

                        ClickElement(driver, days[i]);

                        //Wait(2000, 2000, driver);
                        break;
                    }
                }
            }


            for (int i = 0; i < 1; i++)
            {
                ClickElement(driver, calendarBtn.Last());
                Wait(100, 100, driver);
            }

            calendarDays = (WebElement)((IJavaScriptExecutor)driver).ExecuteScript(calendarDaysJs);
            days = new List<IWebElement>(calendarDays.FindElements(By.TagName("li")));


            if (days != null && days.Count > 0)
            {
                for (int i = days.Count - 1; i >= 0; i--)
                {
                    string className = days[i].GetAttribute("class");
                    string texttt = days[i].Text;

                    if (!string.IsNullOrEmpty(texttt) && !className.Contains("disabled"))
                    {
                        txtDay = texttt;

                        WaitUntilElementIsLoaded(driver, "window.frames.document.querySelector('.soma-calendar.hydrated').shadowRoot.innerHTML", 60000);
                        var headerJs = "return window.frames.document.querySelector('.soma-calendar.hydrated').shadowRoot.innerHTML;";
                        string headerCal = (string)((IJavaScriptExecutor)driver).ExecuteScript(headerJs);
                        txtDay = txtDay + " " + headerCal;

                        //Wait(2000, 2000, driver);

                        ClickElement(driver, days[i]);

                        //Wait(2000, 2000, driver);
                        break;
                    }
                }
            }

            //Wait(5000, 5000, driver);

            WaitUntilElementIsLoaded(driver, "document.querySelector('.table-content__item.pointer')", 5000);
            List<IWebElement> lstTrElem = new List<IWebElement>(driver.FindElements(By.CssSelector(".table-content__item.pointer")));

            if (lstTrElem != null && lstTrElem.Count > 0)
            {
                for (int i = 0; i < lstTrElem.Count; i++)
                {
                    string text = lstTrElem[i].Text;

                    if (text.Contains("DIVIDENDOS") || text.Contains("JUROS") || text.Contains("RENDIMENTO"))
                    {
                        string type = string.Empty;

                        if (text.Contains("DIVIDENDOS"))
                        {
                            type = "Dividendos";
                        }
                        else if (text.Contains("JUROS"))
                        {
                            type = "Juros";
                        }
                        else if (text.Contains("RENDIMENTO"))
                        {
                            type = "Rendimento";
                        }

                        string[] statementLine = text.Split("\r\n");

                        if (statementLine != null && statementLine.Length == 5)
                        {
                            DateTime paymentDate = Convert.ToDateTime(statementLine[0], new CultureInfo("pt-br"));

                            decimal netValue = 0;
                            decimal.TryParse(statementLine[2].Replace("R$", string.Empty), NumberStyles.Currency, new CultureInfo("pt-br"), out netValue);

                            string[] symbolData = statementLine[3].Split(" ");
                            string symbol = string.Empty;

                            if (symbolData != null && symbolData.Length > 0)
                            {
                                symbol = symbolData[symbolData.Length - 1];
                            }

                            ClearDividend clearDividend = new ClearDividend();
                            clearDividend.Type = type;
                            clearDividend.EventDate = paymentDate;
                            clearDividend.NetValue = netValue;
                            clearDividend.Symbol = RemoveFractionalLetter(symbol);

                            bool found = false;

                            if (dividends != null && dividends.Count > 0)
                            {
                                found = dividends.Exists(div => div.EventDate == clearDividend.EventDate && div.Type == clearDividend.Type && div.NetValue == clearDividend.NetValue && div.Symbol == clearDividend.Symbol);
                            }

                            if (!found)
                            {
                                importClearResult.ResponseBody += new StringBuilder().AppendFormat("{0} {1} {2}", clearDividend.Symbol, netValue, paymentDate).AppendLine().ToString();
                                dividends.Add(clearDividend);
                            }
                        }
                    }
                }
            }
        }

        private List<ClearOrderItem> GetOperationItems(DateTime? lastEventDate, dynamic resultAPI)
        {
            DateTime now = DateTime.Now;
            List<ClearOrderItem> clearOrderItems = new List<ClearOrderItem>();
            DateTime startDate = now.Date.AddMonths(-3).AddHours(3);

            int test = GetTime(startDate);

            if (lastEventDate.HasValue)
            {
                startDate = lastEventDate.Value.Date.AddHours(3);
            }

            DateTime endDate = startDate.Date.AddDays(6).AddHours(2).AddMinutes(59).AddSeconds(59).AddMilliseconds(999);


            if (endDate > now)
            {
                endDate = now.Date.AddDays(1).AddHours(2).AddMinutes(59).AddSeconds(59).AddMilliseconds(999);
            }

            while (startDate <= endDate && endDate <= now.AddDays(2))
            {
                string startDateUnix = DateTimeToUnixTimeStamp(startDate);

                string endDateUnix = DateTimeToUnixTimeStamp(endDate);

                clearOrderItems.AddRange(GetOperationItens(resultAPI, startDateUnix, endDateUnix));
                //GetOperationItens(resultAPI, "1646017200000", "1646362799999");

                if (endDate.Date == now.Date)
                {
                    startDate = endDate.Date.AddHours(3);
                }
                else
                {
                    startDate = endDate.Date.AddDays(1).AddHours(3);
                }

                endDate = startDate.Date.AddDays(6).AddHours(2).AddMinutes(59).AddSeconds(59).AddMilliseconds(999);

                if (startDate < now && endDate > now)
                {
                    endDate = now.Date.AddDays(1).AddHours(2).AddMinutes(59).AddSeconds(59).AddMilliseconds(999);
                }
            }

            return clearOrderItems;
        }

        private List<ClearOrderItem> GetOperationItens(dynamic resultAPI, string startDate, string endDate)
        {
            List<ClearOrderItem> clearOrderItems = new List<ClearOrderItem>();

            var handler = new HttpClientHandler();
            handler.UseCookies = false;

            using (var httpClient = new HttpClient(handler))
            {
                string url = string.Format("https://api.clear.com.br/trading-orders/v1/orders?Modules=3%2C101%2C10%2C4%2C2&Platforms=&PageSize=25&PageNumber=1&StatusGroup=4&OrderDate={0}&OrderDateEnd={1}", startDate, endDate);

                using (var request = new HttpRequestMessage(new HttpMethod("GET"), url))
                {
                    request.Headers.TryAddWithoutValidation("ocp-apim-subscription-key", "28506c592bae4424bf77cdbaad8b2618");
                    request.Headers.TryAddWithoutValidation("authorization", string.Format("Bearer {0}", resultAPI.AccessToken));

                    var response = httpClient.SendAsync(request).Result;

                    if (response.IsSuccessStatusCode)
                    {
                        ClearStatement clearStatement = JsonConvert.DeserializeObject<ClearStatement>(response.Content.ReadAsStringAsync().Result);

                        if (clearStatement != null && clearStatement.Orders != null && clearStatement.Orders.Count > 0)
                        {
                            foreach (Order order in clearStatement.Orders)
                            {
                                decimal avgPrice = 0;
                                decimal.TryParse(order.AveragePrice, NumberStyles.Currency, CultureInfo.InvariantCulture, out avgPrice);

                                decimal numberOfShares = 0;
                                decimal.TryParse(order.ExecutedQuantity, NumberStyles.Currency, CultureInfo.InvariantCulture, out numberOfShares);

                                //string eventDate = RemoveTrailingZeroes(order.CreatedAt);
                                //string eventDate = order.CreatedAt;

                                ClearOrderItem clearOrderItem = new ClearOrderItem();
                                clearOrderItem.NumberOfShares = numberOfShares;
                                clearOrderItem.AveragePrice = avgPrice;
                                clearOrderItem.EventDate = UnixTimeStampToDateTime(Convert.ToDouble(order.CreatedAt));
                                clearOrderItem.IdOperationType = order.Side == "1" ? 1 : 2;
                                clearOrderItem.Symbol = RemoveFractionalLetter(order.Symbol);
                                clearOrderItem.TransactionId = order.Id;

                                clearOrderItems.Add(clearOrderItem);
                            }
                        }
                    }
                }
            }

            return clearOrderItems;
        }

        private dynamic GetJwtToken(ChromeDriver driver)
        {
            var _cookies = driver.Manage().Cookies.AllCookies;
            string tp = string.Empty;

            foreach (Cookie cookie in _cookies)
            {
                if (cookie.Name == "pit_token")
                {
                    tp = HttpUtility.UrlDecode(cookie.Value);
                }
            }

            dynamic resultAPI = JsonConvert.DeserializeObject<ExpandoObject>(tp);
            return resultAPI;
        }

        private List<ClearOrder> GetOperations(ChromeDriver driver, ImportClearResult importClearResult)
        {
            //driver.Manage().Window.Maximize();
            List<ClearOrder> clearOrders = new List<ClearOrder>();
            //Wait(6500, 6500, driver);
            driver.Navigate().GoToUrl(@"https://pro.clear.com.br/#minha-conta/meus-ativos");
            WaitUntilPageIsLoaded(driver, "document.URL", "meus-ativos", 180000);
            //Wait(70000, 70000, driver);

            driver.SwitchTo().Frame("content-page");

            CloseModalRlp(driver);

            WaitUntilElementIsLoaded(driver, "document.querySelector('.AssetItem.item')", 60000);
            Wait(2000, 2000, driver);

            var inputPwd4 = driver.FindElements(By.CssSelector(".AssetItem.item"));

            if (inputPwd4 != null && inputPwd4.Count > 0)
            {
                for (int i = 0; i < inputPwd4.Count; i++)
                {
                    try
                    {
                        WaitUntilElementIsLoaded(driver, "document.querySelector('.AssetItem.item')", 2000);

                        ClearOrder clearOrder = new ClearOrder();
                        clearOrder.EventDate = DateTime.Now;
                        clearOrder.IdOperationType = 1;

                        inputPwd4 = driver.FindElements(By.CssSelector(".AssetItem.item"));
                        Actions actions = new Actions(driver);
                        actions.MoveToElement(inputPwd4[i]).SendKeys(OpenQA.Selenium.Keys.ArrowDown).Perform();

                        if (i % 2 == 0 || (i > 11 && i < 60))
                        {
                            actions.MoveToElement(inputPwd4[i]).SendKeys(OpenQA.Selenium.Keys.ArrowDown).Perform();
                        }

                        string symbol = inputPwd4[i].Text;
                        clearOrder.Symbol = RemoveFractionalLetter(symbol.Substring(0, symbol.IndexOf("\r")));

                        if (clearOrders != null && clearOrders.Count > 0)
                        {
                            if (clearOrders.Exists(ord => ord.Symbol == clearOrder.Symbol))
                            {
                                continue;
                            }
                        }

                        ClickElement(driver, inputPwd4[i]);

                        WaitUntilElementIsLoaded(driver, "document.querySelector('.info-list')", 1200);
                        //Wait(1200, 1200, driver);

                        var infoList = driver.FindElements(By.CssSelector(".info-list"));

                        if (infoList != null && infoList.Count > 0)
                        {
                            importClearResult.ApiResult += " " + clearOrder.Symbol + infoList[0].Text;
                            List<IWebElement> lstLiElem = new List<IWebElement>(infoList[0].FindElements(By.TagName("li")));

                            if (lstLiElem != null && lstLiElem.Count > 0)
                            {

                                List<IWebElement> lstSpanElem = new List<IWebElement>(lstLiElem[0].FindElements(By.TagName("span")));

                                if (lstSpanElem != null && lstSpanElem.Count > 0)
                                {
                                    clearOrder.NumberOfShares = Convert.ToInt32(lstSpanElem[0].Text.Replace(".", string.Empty), new CultureInfo("pt-br"));
                                }

                                lstSpanElem = new List<IWebElement>(lstLiElem[1].FindElements(By.TagName("span")));

                                if (lstSpanElem != null && lstSpanElem.Count > 0)
                                {
                                    decimal avgPrice = 0;
                                    decimal.TryParse(lstSpanElem[0].Text, NumberStyles.Currency, new CultureInfo("pt-br"), out avgPrice);
                                    clearOrder.AveragePrice = avgPrice;
                                }

                                clearOrders.Add(clearOrder);

                                WaitUntilElementIsLoaded(driver, "document.querySelector('.close-slidebox')", 10000);
                                var btnClose = driver.FindElements(By.CssSelector(".close-slidebox"));

                                for (int k = 0; k < btnClose.Count; k++)
                                {
                                    try
                                    {
                                        ClickElement(driver, btnClose[k]);
                                    }
                                    catch
                                    {
                                    }
                                }
                            }
                        }

                    }
                    catch(Exception ex)
                    {
                        i--;
                        WaitUntilElementIsLoaded(driver, "document.querySelector('.AssetItem.item')", 2500);
                        //Wait(2500, 2500, driver);
                        inputPwd4 = driver.FindElements(By.CssSelector(".AssetItem.item"));
                        continue;
                    }
                }
            }

            return clearOrders;
        }

        private void CloseModalRlp(ChromeDriver driver)
        {
            try
            {
                //WaitUntilElementIsLoaded(driver, "document.querySelector('.rlp_close_button.timer-wrapper')", 20000);
                var closeRlp = driver.FindElement(By.CssSelector(".rlp_close_button.timer-wrapper"));
                ClickElement(driver, closeRlp);
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

        public void Wait(double delay, double interval, IWebDriver driver)
        {
            // Causes the WebDriver to wait for at least a fixed delay
            var now = DateTime.Now;
            var wait = new WebDriverWait(driver, TimeSpan.FromMilliseconds(delay));
            wait.PollingInterval = TimeSpan.FromMilliseconds(interval);
            wait.Until(wd => (DateTime.Now - now) - TimeSpan.FromMilliseconds(delay) > TimeSpan.Zero);
        }

        public DateTime UnixTimeStampToDateTime(double unixTimeStamp)
        {
            // Unix timestamp is seconds past epoch
            DateTime dateTime = new DateTime(1970, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc);

            var brasilia = TimeZoneInfo.FindSystemTimeZoneById("E. South America Standard Time");
            DateTime dtBrasilia = TimeZoneInfo.ConvertTimeFromUtc(dateTime.AddMilliseconds(unixTimeStamp), brasilia);

            return dtBrasilia;
        }

        public string DateTimeToUnixTimeStamp(DateTime date)
        {
            string unixTimestamp = date.Subtract(new DateTime(1970, 1, 1)).TotalMilliseconds.ToString();

            return unixTimestamp;
        }

        private string RemoveTrailingZeroes(string s)
        {
            StringBuilder sb = new StringBuilder(s);
            while (sb.Length > 0 && sb.ToString()[sb.Length - 1] == '0')
            {
                sb = sb.Remove(sb.Length - 1, 1);
            }
            return sb.ToString();
        }

        private int GetTime(DateTime date)
        {
            var time = (date.ToUniversalTime() - new DateTime(1970, 1, 1));
            return (int)(time.TotalMilliseconds + 0.5);
        }

        private static string RemoveFractionalLetter(string stock)
        {
            if (!string.IsNullOrEmpty(stock) && (stock[stock.Length - 1].ToString().ToUpper() == "F"))
            {
                stock = stock.Remove(stock.Length - 1);
            }

            return stock;
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

                    CloseModalRlp((ChromeDriver)driver);
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

                    CloseModalRlp((ChromeDriver)driver);
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

        private void ToggleMenu(ChromeDriver driver, ImportClearResult importClearResult)
        {
            driver.Navigate().GoToUrl(@"https://pro.clear.com.br/#renda-variavel/swing-trade");
            WaitUntilPageIsLoaded(driver, "document.URL", "swing-trade", 180000);
            Wait(10000, 10000, driver);

            WaitUntilElementIsLoaded(driver, "document.querySelector('.expand-toggler.bg-nav-active')", 10000);
            var toggleMenu = driver.FindElement(By.CssSelector(".expand-toggler.bg-nav-active"));
            ClickElement(driver, toggleMenu);
            Wait(10000, 10000, driver);
            //GetSwingOperations(driver, importClearResult);
        }

        private List<ClearOrder> GetSwingOperations(ChromeDriver driver, ImportClearResult importClearResult)
        {
            List<ClearOrder> clearOrders = new List<ClearOrder>();
            driver.Navigate().GoToUrl(@"https://pro.clear.com.br/#renda-variavel/swing-trade");
            WaitUntilPageIsLoaded(driver, "document.URL", "swing-trade", 180000);
            Wait(10000, 10000, driver);
            bool switchFrame = true;


            try
            {
                WaitUntilElementIsLoaded(driver, "document.querySelector('.expand-toggler.bg-nav-active.active')", 10000);
                var toggleMenu = driver.FindElement(By.CssSelector(".expand-toggler.bg-nav-active.active"));
            }
            catch
            {

                driver.SwitchTo().Frame("content-page");

                CloseModalRlp(driver);
                CloseModaSwinglButton(driver);
                CloseModalSwing(driver);
                CloseModaNewAsset(driver);

                ToggleMenu(driver, importClearResult);
                driver.SwitchTo().Frame("content-page");
                switchFrame = false;
            }

            if (switchFrame)
            {
                driver.SwitchTo().Frame("content-page");
            }

            CloseModalRlp(driver);
            CloseModaSwinglButton(driver);
            CloseModalSwing(driver);
            CloseModaNewAsset(driver);

            WaitUntilElementIsLoaded(driver, "document.querySelector('.btn-item.results')", 60000);
            Wait(2000, 2000, driver);


            var btnResults = driver.FindElement(By.CssSelector(".btn-item.results"));
            ClickElement(driver, btnResults);
            Wait(10000, 10000, driver);


            WaitUntilElementListIsLoaded(driver, "document.querySelectorAll('.result-info')", 60000);

            var assets = driver.FindElements(By.CssSelector(".result-info"));


            if (assets != null && assets.Count > 0)
            {
                for (int i = 0; i < assets.Count; i++)
                {
                    ClearOrder clearOrder = new ClearOrder();
                    var spanSymbol = GetInnerHtml(driver, string.Format("document.querySelectorAll('.result-info')[{0}].querySelector('.tit').innerHTML", i), 2000);
                    var spanAmount = GetInnerHtml(driver, string.Format("document.querySelectorAll('.result-info')[{0}].querySelector('.net-qty').innerHTML", i), 2000);
                    var spanAvgPrice = GetInnerHtml(driver, string.Format("document.querySelectorAll('.result-info')[{0}].querySelector('.avg-price').innerHTML", i), 2000);

                    decimal avgPrice = 0;
                    decimal.TryParse(spanAvgPrice.Replace("R$", string.Empty), NumberStyles.Currency, new CultureInfo("pt-br"), out avgPrice);

                    int numberOfShares = 0;
                    int.TryParse(spanAmount.Replace(".", string.Empty), out numberOfShares);

                    clearOrder.Symbol = spanSymbol;
                    clearOrder.NumberOfShares = numberOfShares;
                    clearOrder.AveragePrice = avgPrice;
                    clearOrder.IdOperationType = 1;
                    clearOrder.EventDate = DateTime.Now;
                    clearOrders.Add(clearOrder);

                    importClearResult.ResponseBody += new StringBuilder().AppendFormat("{0} {1} {2}", spanSymbol, spanAmount, spanAvgPrice).AppendLine().ToString();
                }
            }

            return clearOrders;
        }

        public string GetInnerHtml(IWebDriver driver, string jsScript, double timeout)
        {
            string text = string.Empty;

            try
            {
                var now = DateTime.Now;
                var wait = new WebDriverWait(driver, TimeSpan.FromMilliseconds(timeout));

                wait.Until(driver =>
                {

                    CloseModalRlp((ChromeDriver)driver);
                    bool conditionOk = false;

                    var jsExecuted = ((IJavaScriptExecutor)driver).ExecuteScript(string.Format("return {0}", jsScript));

                    if (jsExecuted != null)
                    {
                        text = jsExecuted.ToString();
                        conditionOk = true;
                    }

                    return conditionOk;
                });
            }
            catch
            {
            }

            return text;
        }

        public bool IsBrowserOpened(ChromeDriver driver)
        {
            bool isOpened = true;

            try
            {
                string title = driver.Title;
            }
            catch
            {
                isOpened = false;
            }

            return isOpened;
        }

        private void CloseModalSwing(ChromeDriver driver)
        {
            try
            {
                WaitUntilElementIsLoaded(driver, "document.querySelectorAll('.btn-close.small-docket.close.btn-close-lightbox')", 20000);
                var closeRlp = driver.FindElement(By.CssSelector(".btn-close.small-docket.close.btn-close-lightbox"));
                ClickElement(driver, closeRlp);
            }
            catch
            {
            }
        }

        private void CloseModaSwinglButton(ChromeDriver driver)
        {
            try
            {
                WaitUntilElementIsLoaded(driver, "document.querySelectorAll('.button-container')", 20000);
                var closeRlp = driver.FindElement(By.CssSelector(".button-container"));
                ClickElement(driver, closeRlp);
            }
            catch
            {
            }
        }

        private void CloseModaNewAsset(ChromeDriver driver)
        {
            try
            {
                WaitUntilElementIsLoaded(driver, "document.querySelectorAll('.timer-wrapper')", 20000);

                var btnClose = driver.FindElements(By.CssSelector(".timer-wrapper"));

                for (int k = 0; k < btnClose.Count; k++)
                {
                    try
                    {
                        ClickElement(driver, btnClose[k]);
                    }
                    catch
                    {
                    }
                }
            }
            catch
            {
            }
        }
    }
}
