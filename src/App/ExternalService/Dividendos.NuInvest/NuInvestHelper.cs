using Dividendos.NuInvest.Interface;
using Dividendos.NuInvest.Interface.Model;
using Newtonsoft.Json;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Interactions;
using OpenQA.Selenium.Support.UI;
using RestSharp;
using SeleniumExtras.WaitHelpers;
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

namespace Dividendos.NuInvest
{
    public class NuInvestHelper : INuInvestHelper
    {
        public ImportNuInvestResult ImportFromNuInvest(string identifier, string password, string token, DateTime? lastEventDate, bool getContactDetails)
        {
            ImportNuInvestResult importNuInvestResult = new ImportNuInvestResult();
            string jwtToken = string.Empty;
            var chromeOptions = new ChromeOptions();
            chromeOptions.AddArguments("--no-sandbox");
            chromeOptions.AddArguments("--disable-dev-shm-usage");

            if (string.IsNullOrWhiteSpace(identifier))
            {
                importNuInvestResult.Message = "Login deve ser informado";
                importNuInvestResult.ApiResult = "Login deve ser informado";
                importNuInvestResult.Success = false;
                return importNuInvestResult;
            }

            if (string.IsNullOrWhiteSpace(password))
            {
                importNuInvestResult.Message = "Senha deve ser informada";
                importNuInvestResult.ApiResult = "Senha deve ser informada";
                importNuInvestResult.Success = false;
                return importNuInvestResult;
            }

            if (string.IsNullOrWhiteSpace(token))
            {
                importNuInvestResult.Message = "EasyToken deve ser informado";
                importNuInvestResult.ApiResult = "EasyToken deve ser informado";
                importNuInvestResult.Success = false;
                return importNuInvestResult;
            }

            using (var driver = new ChromeDriver(chromeOptions))
            {
                driver.Manage().Window.Size = new Size(1920, 1080);
                driver.Manage().Window.Position = new Point(0, 0);
                driver.Manage().Window.Maximize();
                driver.Manage().Timeouts().PageLoad = TimeSpan.FromMinutes(2);

                driver.Navigate().GoToUrl(@"https://www.nuinvest.com.br/autenticacao/");
                WaitUntilPageIsLoaded(driver, "document.URL", "autenticacao", 60000);

                StringBuilder sb = new StringBuilder();

                string identifierFormated = identifier.Trim();

                if (!identifierFormated.Contains("@"))
                {
                    identifierFormated = identifier.Replace(".", string.Empty).Replace("-", string.Empty).Trim();
                }

                sb.Append(@"let formData = new FormData();
                                formData.append('username', '" + identifierFormated + @"');
                                formData.append('password', '" + password.Trim() + @"');
                                formData.append('grant_type', 'password');
                                formData.append('client_id', '876dab2190464884bf9b092aa1407585');
                                formData.append('device_uid', '" + Guid.NewGuid().ToString("N") + @"');
                                formData.append('otp_code', '" + token + @"');


                                (async () => {
                                  const rawResponse = await fetch('https://www.nuinvest.com.br/api/auth/v3/security/token', {
                                                                    method: 'POST',
                                                                    body: formData,
                                                                });
                                  const content = await rawResponse.json();

                                  localStorage.setItem('test', JSON.stringify(content));
                                })();");


                string fetch = sb.ToString();
                var jwtTokenInput2 = ((IJavaScriptExecutor)driver).ExecuteScript(fetch);

                Wait(60000, 60000, driver);

                var jsToBeExecuted3 = "return localStorage.getItem('test')";
                var jwtTokenInput3 = ((IJavaScriptExecutor)driver).ExecuteScript(jsToBeExecuted3);

                importNuInvestResult.ResponseBody = fetch;

                if (jwtTokenInput3 != null)
                {
                    dynamic resultAPI = JsonConvert.DeserializeObject<ExpandoObject>(jwtTokenInput3.ToString());

                    if (((IDictionary<string, Object>)resultAPI).ContainsKey("error"))
                    {
                        string code = resultAPI.error.code;
                        string message = resultAPI.error.message;
                        importNuInvestResult.ApiResult = message;
                        
                        if (code == "AuthenticateTransaction" || code == "INVALIDCREDENTIAL")
                        {
                            importNuInvestResult.Message = message;
                        }
                        else
                        {
                            importNuInvestResult.Message = "Token inválido ou expirado. Tente novamente";
                        }
                    }
                    else if (((IDictionary<string, Object>)resultAPI).ContainsKey("access_token"))
                    {
                        jwtToken = resultAPI.access_token;
                        importNuInvestResult.Success = true;
                    }
                }
                else
                {
                    importNuInvestResult.Message = "Token inválido ou expirado. Tente novamente";
                    importNuInvestResult.ApiResult = "JWT nao encontrado " + driver.Url;
                    importNuInvestResult.Success = false;
                    return importNuInvestResult;
                }






                //WaitUntilElementIsLoaded(driver, "document.querySelector('#username')", 90000);
                //var inputIdentifier = driver.FindElement(By.Id("username"));
                //inputIdentifier.SendKeys(identifier);



                //WaitUntilElementIsLoaded(driver, "document.querySelector('#password')", 90000);
                //var inputPassword = driver.FindElement(By.Id("password"));
                //inputPassword.SendKeys(password);
                //inputPassword.Submit();

                //Wait(1500, 1500, driver);

                //try
                //{
                //    var message2 = driver.FindElement(By.CssSelector(".sc-bbmXgH.ivGmOC"));
                //    string text2 = message2.Text;

                //    importNuInvestResult.ApiResult = text2;

                //    if (!string.IsNullOrWhiteSpace(text2) && text2.Contains("Confira"))
                //    {
                //        importNuInvestResult.Message = "Confira seu login e senha e tente novamente.";
                //        importNuInvestResult.Success = false;
                //        return importNuInvestResult;
                //    }
                //}
                //catch
                //{

                //}

                //WebElement easyToken = null;

                //var jsToBeExecuted2 = "return document.forms[1].querySelector('greg-input-text').shadowRoot.querySelector('#content').querySelector('#greg-input');";

                //try
                //{
                //    WaitUntilElementIsLoaded(driver, "document.forms[1].querySelector('greg-input-text').shadowRoot.querySelector('#content').querySelector('#greg-input')", 90000);
                //    easyToken = (WebElement)((IJavaScriptExecutor)driver).ExecuteScript(jsToBeExecuted2);
                //}
                //catch (Exception)
                //{
                //}

                //if (easyToken != null)
                //{
                //    if (string.IsNullOrWhiteSpace(token))
                //    {
                //        importNuInvestResult.Message = "Token deve ser informado";
                //        importNuInvestResult.ApiResult = "Token deve ser informado";
                //        importNuInvestResult.Success = false;
                //        return importNuInvestResult;
                //    }

                //    easyToken.SendKeys(token);

                //    WaitUntilElementIsLoaded(driver, "document.querySelector('#enterEasytokenButton')", 90000);
                //    var enterEasytokenButton = driver.FindElement(By.Id("enterEasytokenButton"));
                //    ClickElement(driver, enterEasytokenButton);
                //    //enterEasytokenButton.Click();

                //    Wait(20000, 20000, driver);



                //    var jsToBeExecuted4 = "return document.forms[1].querySelector('greg-input-text').shadowRoot.querySelector('#errorText');";
                //    WebElement errorToken = null;

                //    try
                //    {
                //        //WaitUntilElementIsLoaded(driver, "document.forms[1].querySelector('greg-input-text').shadowRoot.querySelector('#errorText')", 90000);
                //        errorToken = (WebElement)((IJavaScriptExecutor)driver).ExecuteScript(jsToBeExecuted4);
                //    }
                //    catch (Exception)
                //    {
                //    }

                //    try
                //    {
                //        if (errorToken != null && (!string.IsNullOrWhiteSpace(errorToken.Text) && errorToken.Text.Contains("menos 6") || errorToken.Text.Contains("inválido")))
                //        {
                //            importNuInvestResult.Message = "Token inválido ou expirado. Tente novamente";
                //            importNuInvestResult.ApiResult = errorToken.Text;
                //            importNuInvestResult.Success = false;
                //            return importNuInvestResult;
                //        }
                //        else
                //        {
                //            try
                //            {
                //                var jsToBeExecuted3 = "return document.querySelector('greg-button');";
                //                WebElement noDeviceButton = (WebElement)((IJavaScriptExecutor)driver).ExecuteScript(jsToBeExecuted3);

                //                noDeviceButton.Submit();
                //            }
                //            catch (Exception)
                //            {
                //            }
                //        }
                //    }
                //    catch (Exception)
                //    {
                //    }

                //}

                //Wait(20000, 20000, driver);
                //driver.Navigate().GoToUrl(@"https://www.nuinvest.com.br/acompanhar/investimentos");
                //WaitUntilPageIsLoaded(driver, "document.URL", "investimentos", 30000);

                //Wait(20000, 20000, driver);


                //var jsToBeExecuted = "return localStorage.getItem('access_token')";
                //var jwtTokenInput = ((IJavaScriptExecutor)driver).ExecuteScript(jsToBeExecuted);

                //if (jwtTokenInput != null)
                //{
                //    jwtToken = jwtTokenInput.ToString();
                //    importNuInvestResult.Success = true;
                //}
                //else
                //{
                //    importNuInvestResult.Message = "Token inválido ou expirado. Tente novamente";
                //    importNuInvestResult.ApiResult = "JWT nao encontrado " + driver.Url;
                //    importNuInvestResult.Success = false;
                //    return importNuInvestResult;
                //}
            }

            DateTime yesterDay = DateTime.Now.Date.AddDays(-1);

            if (importNuInvestResult.Success)
            {
                DateTime? startDate = new DateTime(2015, 1, 1);

                if (lastEventDate.HasValue)
                {
                    startDate = lastEventDate.Value.Date;
                }

                DateTime endDate = startDate.Value.Date.AddDays(88);

                if (endDate > yesterDay)
                {
                    endDate = yesterDay;
                }

                List<Statement> nuInvestStatements = new List<Statement>();

                while (startDate.Value <= yesterDay)
                {
                    NuInvestStatement nuInvestStatement = GetTransactionSummary(startDate.Value, endDate, jwtToken, importNuInvestResult);

                    if (nuInvestStatement != null && nuInvestStatement.Value != null && nuInvestStatement.Value.Statements != null)
                    {
                        nuInvestStatements.AddRange(nuInvestStatement.Value.Statements);
                    }

                    startDate = endDate.AddDays(1);

                    endDate = startDate.Value.Date.AddDays(88);

                    if (endDate > yesterDay)
                    {
                        endDate = yesterDay;
                    }
                }

                importNuInvestResult.Orders = new List<NuInvestOrder>();

                List<NuInvestTransaction> nuInvestTransactions = ImportTransactions(jwtToken, importNuInvestResult);

                if (nuInvestTransactions != null && nuInvestTransactions.Count > 0)
                {
                    foreach (NuInvestTransaction nuInvestTransaction in nuInvestTransactions)
                    {
                        int year = Convert.ToInt32(nuInvestTransaction.LastTime.Substring(0, 4));
                        int month = Convert.ToInt32(nuInvestTransaction.LastTime.Substring(5, 2));
                        int day = Convert.ToInt32(nuInvestTransaction.LastTime.Substring(8, 2));
                        int hour = Convert.ToInt32(nuInvestTransaction.LastTime.Substring(11, 2));
                        int minutes = Convert.ToInt32(nuInvestTransaction.LastTime.Substring(14, 2));
                        int seconds = Convert.ToInt32(nuInvestTransaction.LastTime.Substring(17, 2));

                        nuInvestTransaction.EventDate = new DateTime(year, month, day, hour, minutes, seconds);
                    }

                    nuInvestTransactions = nuInvestTransactions.Where(nuInv => nuInv.Status == "9" && nuInv.EventDate >= startDate.Value).ToList();

                    if (nuInvestTransactions != null && nuInvestTransactions.Count > 0)
                    {
                        nuInvestTransactions = nuInvestTransactions.OrderBy(nuInv => nuInv.EventDate).ToList();

                        foreach (NuInvestTransaction nuInvestTransaction in nuInvestTransactions)
                        {
                            decimal avgPrice = 0;
                            decimal.TryParse(nuInvestTransaction.NegotiatedPrice, NumberStyles.Currency, CultureInfo.InvariantCulture, out avgPrice);

                            decimal quantity = 0;
                            decimal.TryParse(nuInvestTransaction.Quantity, NumberStyles.Currency, CultureInfo.InvariantCulture, out quantity);

                            NuInvestOrder nuInvestOrder = new NuInvestOrder();
                            nuInvestOrder.AveragePrice = avgPrice;
                            nuInvestOrder.EventDate = nuInvestTransaction.EventDate;
                            nuInvestOrder.IdOperationType = nuInvestTransaction.Side == "1" ? 1 : 2;
                            nuInvestOrder.NumberOfShares = quantity;
                            nuInvestOrder.Symbol = RemoveFractionalLetter(nuInvestTransaction.Symbol);
                            nuInvestOrder.TransactionId = nuInvestTransaction.ClOrderId;

                            importNuInvestResult.Orders.Add(nuInvestOrder);
                        }
                    }
                }


                if (nuInvestStatements != null && nuInvestStatements.Count > 0)
                {
                    foreach (Statement statement in nuInvestStatements)
                    {
                        int year = Convert.ToInt32(statement.SettlementDate.Substring(0, 4));
                        int month = Convert.ToInt32(statement.SettlementDate.Substring(5, 2));
                        int day = Convert.ToInt32(statement.SettlementDate.Substring(8, 2));

                        DateTime eventDate = new DateTime(year, month, day);
                        decimal avgPrice = 0;
                        decimal.TryParse(statement.Price, NumberStyles.Currency, CultureInfo.InvariantCulture, out avgPrice);

                        decimal buyQuantity = 0;
                        decimal.TryParse(statement.BuyQuantity, NumberStyles.Currency, CultureInfo.InvariantCulture, out buyQuantity);

                        decimal sellQuantity = 0;
                        decimal.TryParse(statement.SellQuantity, NumberStyles.Currency, CultureInfo.InvariantCulture, out sellQuantity);

                        decimal numberOfShares = buyQuantity > 0 ? buyQuantity : sellQuantity;
                        int idOperationType = buyQuantity > 0 ? 1 : 2;
                        string symbol = RemoveFractionalLetter(statement.Ticker);

                        bool exists = false;
                        if (importNuInvestResult.Orders != null && importNuInvestResult.Orders.Count > 0)
                        {
                            exists = importNuInvestResult.Orders.Exists(ord => ord.EventDate.Date == eventDate.Date && ord.AveragePrice == avgPrice && ord.NumberOfShares == numberOfShares && ord.IdOperationType == idOperationType && ord.Symbol == symbol);
                        }

                        if (!exists)
                        {
                            NuInvestOrder nuInvestOrder = new NuInvestOrder();
                            nuInvestOrder.AveragePrice = avgPrice;
                            nuInvestOrder.NumberOfShares = numberOfShares;
                            nuInvestOrder.EventDate = eventDate;
                            nuInvestOrder.IdOperationType = idOperationType;
                            nuInvestOrder.Symbol = RemoveFractionalLetter(statement.Ticker);

                            importNuInvestResult.Orders.Add(nuInvestOrder);
                        }
                    }
                }

                NuInvestCustody nuInvestCustody = GetOtherInvestments(jwtToken, importNuInvestResult);

                if (nuInvestCustody != null && nuInvestCustody.Investments != null && nuInvestCustody.Investments.Count > 0)
                {
                    try
                    {
                        List<Investment> otherInvestments = nuInvestCustody.Investments.Where(inv => inv.SecurityType != "STOCK").ToList();

                        if (otherInvestments != null && otherInvestments.Count > 0)
                        {
                            importNuInvestResult.Warnings = new List<string>();

                            foreach (Investment position in otherInvestments)
                            {
                                importNuInvestResult.Warnings.Add(position.SecurityType);
                            }

                            if (importNuInvestResult.Warnings.Count > 0)
                            {
                                importNuInvestResult.Warnings = importNuInvestResult.Warnings.Distinct().ToList();
                            }
                        }
                    }
                    catch
                    {
                    }

                    List<Investment> stocks = nuInvestCustody.Investments.Where(inv => inv.SecurityType == "STOCK").ToList();
                    nuInvestCustody.Investments = nuInvestCustody.Investments.Where(inv => inv.SecurityType != "STOCK").ToList();

                    if (nuInvestCustody != null && nuInvestCustody.Investments != null && nuInvestCustody.Investments.Count > 0)
                    {
                        importNuInvestResult.Funds = new List<NuInvestFund>();
                        importNuInvestResult.Bonds = new List<NuInvestBond>();

                        foreach (Investment investment in nuInvestCustody.Investments)
                        {
                            decimal netValue = 0;
                            decimal.TryParse(investment.NetValue, NumberStyles.Currency, CultureInfo.InvariantCulture, out netValue);

                            NuInvestBond nuInvestBond = new NuInvestBond();
                            nuInvestBond.Issuer = investment.NickName;
                            nuInvestBond.Name = string.Format("{0} - {1}", investment.SecurityNameType, investment.Rentability);
                            nuInvestBond.InvestmenType = investment.SecurityType;
                            nuInvestBond.Value = netValue;

                            importNuInvestResult.Bonds.Add(nuInvestBond);
                        }
                    }

                    if (stocks != null && stocks.Count > 0)
                    {
                        importNuInvestResult.OrdersAvgPrice = new List<NuInvestOrder>();

                        foreach (Investment stock in stocks)
                        {
                            decimal quantity = 0;
                            decimal.TryParse(stock.Quantity, NumberStyles.Currency, CultureInfo.InvariantCulture, out quantity);

                            NuInvestOrder nuInvestOrder = new NuInvestOrder();
                            nuInvestOrder.EventDate = DateTime.Now;
                            nuInvestOrder.IdOperationType = 1;
                            nuInvestOrder.NumberOfShares = quantity;
                            nuInvestOrder.Symbol = stock.NickName;

                            NuAvgPrice nuAvgPrice = GetAveragePrice(jwtToken, nuInvestOrder.Symbol, importNuInvestResult);

                            if (nuAvgPrice != null)
                            {
                                decimal avgPrice = 0;
                                decimal.TryParse(nuAvgPrice.AveragePrice, NumberStyles.Currency, CultureInfo.InvariantCulture, out avgPrice);

                                nuInvestOrder.AveragePrice = avgPrice;
                            }

                            importNuInvestResult.OrdersAvgPrice.Add(nuInvestOrder);
                        }
                    }
                }

                PersonalInfoNuInvest personalInfoNuInvest = null;
                AddressNuInvest addressNuInvest = null;
                PatrimonialSituationNuInvest patrimonialSituationNuInvest = null;
                ProfessionalInfoNuInvest professionalInfoNuInvest = null;

                if (getContactDetails)
                {
                    personalInfoNuInvest = GetPersonalInfo(jwtToken, importNuInvestResult);
                    addressNuInvest = GetResidentialAddress(jwtToken, importNuInvestResult);
                    patrimonialSituationNuInvest = GetPatrimonialSituation(jwtToken, importNuInvestResult);
                    professionalInfoNuInvest = GetProfessionalInfo(jwtToken, importNuInvestResult);
                }

                importNuInvestResult.ContactDetails = new NuInvestContactDetails();
                importNuInvestResult.ContactPhones = new List<NuInvestContactPhone>();

                if (personalInfoNuInvest != null && personalInfoNuInvest.PersonalInfo != null)
                {
                    importNuInvestResult.ContactDetails.DocumentNumber = personalInfoNuInvest.PersonalInfo.DocumentNumber;
                    importNuInvestResult.ContactDetails.Name = personalInfoNuInvest.PersonalInfo.Name;
                    importNuInvestResult.ContactDetails.Email = personalInfoNuInvest.PersonalInfo.Email;
                    importNuInvestResult.ContactDetails.Gender = personalInfoNuInvest.PersonalInfo.Gender;
                    importNuInvestResult.ContactDetails.MotherName = personalInfoNuInvest.PersonalInfo.MotherName;
                    importNuInvestResult.ContactDetails.BirthCity = personalInfoNuInvest.PersonalInfo.BirthCity;
                    importNuInvestResult.ContactDetails.BirthDate = personalInfoNuInvest.PersonalInfo.BirthDate;
                    importNuInvestResult.ContactDetails.SpouseName = personalInfoNuInvest.PersonalInfo.SpouseName;
                    importNuInvestResult.ContactDetails.SpouseDocumentNumber = personalInfoNuInvest.PersonalInfo.SpouseDocumentNumber;
                    
                    if (personalInfoNuInvest.PersonalInfo.Phones != null && personalInfoNuInvest.PersonalInfo.Phones.Count() > 0)
                    {
                        foreach (var phone in personalInfoNuInvest.PersonalInfo.Phones)
                        {
                            NuInvestContactPhone nuInvestContactPhone = new NuInvestContactPhone();
                            nuInvestContactPhone.AreaCode = phone.AreaCode;
                            nuInvestContactPhone.CountryCode = phone.CountryCode;
                            nuInvestContactPhone.PhoneNumber = phone.PhoneNumber;
                            importNuInvestResult.ContactPhones.Add(nuInvestContactPhone);
                        }
                    }
                }

                if (addressNuInvest != null)
                {
                    importNuInvestResult.ContactDetails.AddressNumber = addressNuInvest.AddressNumber;
                    importNuInvestResult.ContactDetails.PostalCode = addressNuInvest.PostalCode;
                    importNuInvestResult.ContactDetails.StreetName = addressNuInvest.StreetName;
                    importNuInvestResult.ContactDetails.Complement = addressNuInvest.Complement;
                    importNuInvestResult.ContactDetails.Neighborhood = addressNuInvest.Neighborhood;
                    importNuInvestResult.ContactDetails.City = addressNuInvest.City;
                    importNuInvestResult.ContactDetails.StateCode = addressNuInvest.StateCode;
                    importNuInvestResult.ContactDetails.AdressType = addressNuInvest.Type;
                }

                if (patrimonialSituationNuInvest != null && patrimonialSituationNuInvest.Value != null && patrimonialSituationNuInvest.Value.PatrimonialSituation != null)
                {
                    importNuInvestResult.ContactDetails.MonthlyIncome = patrimonialSituationNuInvest.Value.PatrimonialSituation.MonthlyIncome;
                    importNuInvestResult.ContactDetails.BankDepositAmount = patrimonialSituationNuInvest.Value.PatrimonialSituation.BankDepositAmount;
                }

                if (professionalInfoNuInvest != null)
                {
                    importNuInvestResult.ContactDetails.OcupationDesc = professionalInfoNuInvest.OccupationDescription;
                    importNuInvestResult.ContactDetails.CompanyName = professionalInfoNuInvest.InstitutionName;
                    importNuInvestResult.ContactDetails.CompanyDocumentNumber = professionalInfoNuInvest.CnpjNumber;
                }
            }

            return importNuInvestResult;
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

        private static PatrimonialSituationNuInvest GetPatrimonialSituation(string jwtToken, ImportNuInvestResult importNuInvestResult)
        {
            PatrimonialSituationNuInvest patrimonialSituationNuInvest = new PatrimonialSituationNuInvest();
            var client = new RestClient("https://www.nuinvest.com.br/api/gringott/customers/patrimonialsituation");
            client.Timeout = -1;
            var request = new RestRequest(Method.GET);
            request.AddHeader("authorization", " Bearer " + jwtToken);
            IRestResponse response = client.Execute(request);

            if (response.IsSuccessful)
            {
                var jsonSerializerSettings = new JsonSerializerSettings();
                jsonSerializerSettings.Culture = CultureInfo.InvariantCulture;
                jsonSerializerSettings.MissingMemberHandling = MissingMemberHandling.Ignore;
                patrimonialSituationNuInvest = JsonConvert.DeserializeObject<PatrimonialSituationNuInvest>(response.Content, jsonSerializerSettings);
            }

            if (string.IsNullOrWhiteSpace(importNuInvestResult.ApiResult))
            {
                importNuInvestResult.ApiResult = response.Content;
            }
            else
            {
                importNuInvestResult.ApiResult = importNuInvestResult.ApiResult + response.Content;
            }

            return patrimonialSituationNuInvest;
        }

        private static ProfessionalInfoNuInvest GetProfessionalInfo(string jwtToken, ImportNuInvestResult importNuInvestResult)
        {
            ProfessionalInfoNuInvest professionalInfoNuInvest = new ProfessionalInfoNuInvest();
            var client = new RestClient("https://www.nuinvest.com.br/api/registrar/v2/customers/professionalinfo");
            client.Timeout = -1;
            var request = new RestRequest(Method.GET);
            request.AddHeader("authorization", " Bearer " + jwtToken);
            IRestResponse response = client.Execute(request);

            if (response.IsSuccessful)
            {
                var jsonSerializerSettings = new JsonSerializerSettings();
                jsonSerializerSettings.Culture = CultureInfo.InvariantCulture;
                jsonSerializerSettings.MissingMemberHandling = MissingMemberHandling.Ignore;
                professionalInfoNuInvest = JsonConvert.DeserializeObject<ProfessionalInfoNuInvest>(response.Content, jsonSerializerSettings);
            }

            if (string.IsNullOrWhiteSpace(importNuInvestResult.ApiResult))
            {
                importNuInvestResult.ApiResult = response.Content;
            }
            else
            {
                importNuInvestResult.ApiResult = importNuInvestResult.ApiResult + response.Content;
            }

            return professionalInfoNuInvest;
        }

        private static AddressNuInvest GetResidentialAddress(string jwtToken, ImportNuInvestResult importNuInvestResult)
        {
            AddressNuInvest addressNuInvest = new AddressNuInvest();
            var client = new RestClient("https://www.nuinvest.com.br/api/registrar/v1/customers/residentialaddress");
            client.Timeout = -1;
            var request = new RestRequest(Method.GET);
            request.AddHeader("authorization", " Bearer " + jwtToken);
            IRestResponse response = client.Execute(request);

            if (response.IsSuccessful)
            {
                var jsonSerializerSettings = new JsonSerializerSettings();
                jsonSerializerSettings.Culture = CultureInfo.InvariantCulture;
                jsonSerializerSettings.MissingMemberHandling = MissingMemberHandling.Ignore;
                addressNuInvest = JsonConvert.DeserializeObject<AddressNuInvest>(response.Content, jsonSerializerSettings);
            }

            if (string.IsNullOrWhiteSpace(importNuInvestResult.ApiResult))
            {
                importNuInvestResult.ApiResult = response.Content;
            }
            else
            {
                importNuInvestResult.ApiResult = importNuInvestResult.ApiResult + response.Content;
            }

            return addressNuInvest;
        }

        private static PersonalInfoNuInvest GetPersonalInfo(string jwtToken, ImportNuInvestResult importNuInvestResult)
        {
            PersonalInfoNuInvest personalInfoNuInvest = new PersonalInfoNuInvest();
            var client = new RestClient("https://www.nuinvest.com.br/api/registrar/v3/customers/personalinfo");
            client.Timeout = -1;
            var request = new RestRequest(Method.GET);
            request.AddHeader("authorization", " Bearer " + jwtToken);
            IRestResponse response = client.Execute(request);

            if (response.IsSuccessful)
            {
                var jsonSerializerSettings = new JsonSerializerSettings();
                jsonSerializerSettings.Culture = CultureInfo.InvariantCulture;
                jsonSerializerSettings.MissingMemberHandling = MissingMemberHandling.Ignore;
                personalInfoNuInvest = JsonConvert.DeserializeObject<PersonalInfoNuInvest>(response.Content, jsonSerializerSettings);
            }

            if (string.IsNullOrWhiteSpace(importNuInvestResult.ApiResult))
            {
                importNuInvestResult.ApiResult = response.Content;
            }
            else
            {
                importNuInvestResult.ApiResult = importNuInvestResult.ApiResult + response.Content;
            }

            return personalInfoNuInvest;
        }

        public List<NuInvestTransaction> ImportTransactions(string jwtToken, ImportNuInvestResult importNuInvestResult)
        {
            List<NuInvestTransaction> nuInvestTransactions = new List<NuInvestTransaction>();
            var client = new RestClient("https://www.nuinvest.com.br/api/orders-tracker/Orders?category=2");
            client.Timeout = -1;
            var request = new RestRequest(Method.GET);
            request.AddHeader("authorization", " Bearer " + jwtToken);
            var body = @"";
            request.AddParameter("text/plain", body, ParameterType.RequestBody);
            IRestResponse response = client.Execute(request);

            if (response.IsSuccessful)
            {
                var jsonSerializerSettings = new JsonSerializerSettings();
                jsonSerializerSettings.Culture = CultureInfo.InvariantCulture;
                jsonSerializerSettings.MissingMemberHandling = MissingMemberHandling.Ignore;
                nuInvestTransactions = JsonConvert.DeserializeObject<List<NuInvestTransaction>>(response.Content, jsonSerializerSettings);
            }

            if (string.IsNullOrWhiteSpace(importNuInvestResult.ApiResult))
            {
                importNuInvestResult.ApiResult = response.Content;
            }
            else
            {
                importNuInvestResult.ApiResult = importNuInvestResult.ApiResult + response.Content;
            }


            return nuInvestTransactions;
        }

        public NuInvestStatement GetTransactionSummary(DateTime startDate, DateTime endDate, string jwtToken, ImportNuInvestResult importNuInvestResult)
        {
            NuInvestStatement nuInvestStatement = null;
            string start = startDate.ToString("yyyy-MM-dd");
            string end = endDate.ToString("yyyy-MM-dd");
            string url = string.Format("https://www.nuinvest.com.br/api/gringott/tradingSummary/1?startDate={0}&endDate={1}", start, end);


            var client = new RestClient(url);
            client.Timeout = -1;
            var request = new RestRequest(Method.GET);
            request.AddHeader("authorization", " Bearer " + jwtToken);
            var body = @"";
            request.AddParameter("text/plain", body, ParameterType.RequestBody);
            IRestResponse response = client.Execute(request);


            if (response.IsSuccessful)
            {
                var jsonSerializerSettings = new JsonSerializerSettings();
                jsonSerializerSettings.Culture = CultureInfo.InvariantCulture;
                jsonSerializerSettings.MissingMemberHandling = MissingMemberHandling.Ignore;
                nuInvestStatement = JsonConvert.DeserializeObject<NuInvestStatement>(response.Content, jsonSerializerSettings);
            }

            if (string.IsNullOrWhiteSpace(importNuInvestResult.ApiResult))
            {
                importNuInvestResult.ApiResult = response.Content;
            }
            else
            {
                importNuInvestResult.ApiResult = importNuInvestResult.ApiResult + response.Content;
            }

            return nuInvestStatement;
        }

        public NuInvestCustody GetOtherInvestments(string jwtToken, ImportNuInvestResult importNuInvestResult)
        {
            NuInvestCustody nuInvestCustody = null;
            var client = new RestClient("https://www.nuinvest.com.br/api/samwise/v2/custody-position");
            client.Timeout = -1;
            var request = new RestRequest(Method.GET);
            request.AddHeader("authorization", " Bearer " + jwtToken);
            var body = @"";
            request.AddParameter("text/plain", body, ParameterType.RequestBody);
            IRestResponse response = client.Execute(request);


            if (response.IsSuccessful)
            {
                var jsonSerializerSettings = new JsonSerializerSettings();
                jsonSerializerSettings.Culture = CultureInfo.InvariantCulture;
                jsonSerializerSettings.MissingMemberHandling = MissingMemberHandling.Ignore;
                nuInvestCustody = JsonConvert.DeserializeObject<NuInvestCustody>(response.Content, jsonSerializerSettings);
            }

            if (string.IsNullOrWhiteSpace(importNuInvestResult.ApiResult))
            {
                importNuInvestResult.ApiResult = response.Content;
            }
            else
            {
                importNuInvestResult.ApiResult = importNuInvestResult.ApiResult + response.Content;
            }

            return nuInvestCustody;
        }

        public NuAvgPrice GetAveragePrice(string jwtToken, string symbol, ImportNuInvestResult importNuInvestResult)
        {
            NuAvgPrice avgPrice = null;
            var client = new RestClient(string.Format("https://www.nuinvest.com.br/api/gringott/averagePrice/{0}", symbol));
            client.Timeout = -1;
            var request = new RestRequest(Method.GET);
            request.AddHeader("authorization", " Bearer " + jwtToken);
            var body = @"";
            request.AddParameter("text/plain", body, ParameterType.RequestBody);
            IRestResponse response = client.Execute(request);

            if (response.IsSuccessful)
            {
                var jsonSerializerSettings = new JsonSerializerSettings();
                jsonSerializerSettings.Culture = CultureInfo.InvariantCulture;
                jsonSerializerSettings.MissingMemberHandling = MissingMemberHandling.Ignore;
                avgPrice = JsonConvert.DeserializeObject<NuAvgPrice>(response.Content, jsonSerializerSettings);
            }

            if (string.IsNullOrWhiteSpace(importNuInvestResult.ApiResult))
            {
                importNuInvestResult.ApiResult = response.Content;
            }
            else
            {
                importNuInvestResult.ApiResult = importNuInvestResult.ApiResult + response.Content;
            }

            return avgPrice;
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

        private static void ClickElement(ChromeDriver driver, IWebElement element)
        {
            Actions actionClick = new Actions(driver);
            actionClick.MoveToElement(element).Click().Perform();
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
    }
}
