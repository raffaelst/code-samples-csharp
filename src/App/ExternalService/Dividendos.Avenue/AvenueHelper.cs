using Dividendos.Avenue.Interface;
using Dividendos.Avenue.Interface.Model;
using Newtonsoft.Json;
using SimpleBrowser;
using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Globalization;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;

namespace Dividendos.Avenue
{
    public class AvenueHelper : IAvenueHelper
    {
        private const string PROXY_IP = "172.31.22.177";

        public async Task<ImportAvenueResult> ValidateUser(string user, string password)
        {
			ImportAvenueResult importAvenueResult = new ImportAvenueResult();

            HttpClientHandler httpClientHandler = new HttpClientHandler()
            {
                Proxy = new WebProxy(string.Format("{0}:{1}", PROXY_IP, "21218"), false)
            };

            using (var httpClient = new HttpClient(httpClientHandler))
			{
				using (var request = new HttpRequestMessage(new HttpMethod("POST"), "https://pit-api.avenue.us/auth/login"))
				{
					request.Content = new StringContent("{\"username\":\"" + user + "\",\"password\":\"" + password + "\"}");

					request.Content.Headers.ContentType = MediaTypeHeaderValue.Parse("application/json");

                    var response = await httpClient.SendAsync(request);

					if (response.IsSuccessStatusCode)
					{
						dynamic resultAPI = JsonConvert.DeserializeObject<ExpandoObject>(response.Content.ReadAsStringAsync().Result);

						importAvenueResult.Session = resultAPI.session;
						importAvenueResult.Success = true;
                        importAvenueResult.Challenge = resultAPI.challenge;
                        importAvenueResult.AccessToken = resultAPI.access_token;
                        importAvenueResult.IdToken = resultAPI.id_token;

                    }
                    else
                    {
                        importAvenueResult.ApiResult = response.Content.ReadAsStringAsync().Result;
                    }
				}
			}

			return importAvenueResult;
		}

        public async Task<ImportAvenueResult> ImportAvenue(string user, string password, string token, string challenge, string sessiondId, DateTime? lastEventDate, bool getContactDetails)
        {
            ImportAvenueResult importAvenueResult = new ImportAvenueResult();

            if (!string.IsNullOrWhiteSpace(sessiondId))
            {
                if (string.IsNullOrWhiteSpace(challenge))
                {
                    challenge = "SOFTWARE_TOKEN_MFA";
                }

                if (challenge != "SOFTWARE_TOKEN_MFA" && challenge != "SMS_MFA")
                {
                    challenge = "CUSTOM_CHALLENGE";
                }

                await GetChallengepwdreq(user, sessiondId, token, challenge, importAvenueResult);
            }
            else
            {
                importAvenueResult = await ValidateUser(user, password);
            }

            if (importAvenueResult.Success)
            {
                List<AvenueTransaction> avenueTransactions = new List<AvenueTransaction>();
                DateTime now = DateTime.Now.Date;
                DateTime? startDate = new DateTime(2015, 1, 1);

                if (lastEventDate.HasValue)
                {
                    startDate = lastEventDate.Value.Date.AddHours(3);

                    //if (lastEventDate.Value.Date == now)
                    //{
                    //    startDate = lastEventDate.Value.Date.AddHours(3);
                    //}
                    //else
                    //{
                    //    startDate = lastEventDate.Value.Date.AddDays(1).AddHours(3);
                    //}
                }

                DateTime endDate = startDate.Value.Date.AddYears(1);

                if (endDate > now)
                {
                    endDate = now;
                }

                while (startDate.Value <= now)
                {
                    avenueTransactions.AddRange(await GetTransactions(importAvenueResult.AccessToken, importAvenueResult.IdToken, startDate.Value, endDate, importAvenueResult));

                    startDate = endDate.AddDays(1);

                    endDate = startDate.Value.Date.AddYears(1);

                    if (endDate > now)
                    {
                        endDate = now;
                    }
                }

                #region Teste

                //var jsonSerializerSettings2 = new JsonSerializerSettings();
                //jsonSerializerSettings2.Culture = CultureInfo.InvariantCulture;
                //jsonSerializerSettings2.MissingMemberHandling = MissingMemberHandling.Ignore;

                //string result = "[  {   \"transactionDate\": \"2021-09-13\",   \"creditDate\": {    \"seconds\": 1631678400   },   \"description\": \"US Corretagem\",   \"amount\": 1.5,   \"transactionId\": \"7b740e22-ce61-4fa7-9b46-644d45e93933\",   \"comment\": \"{\\\"items\\\": {\\\"Side\\\": \\\"Buy\\\", \\\"Price\\\": \\\"150.12\\\", \\\"Symbol\\\": \\\"AAPL\\\", \\\"ClOrdId\\\": \\\"20210913000050580\\\", \\\"Quantity\\\": \\\"2\\\", \\\"Operation\\\": \\\"Fill\\\", \\\"Commission\\\": \\\"1.5\\\", \\\"HistoryCode\\\": \\\"4013\\\"}}\",   \"entryDate\": {    \"seconds\": 1631544667   },   \"Type\": \"D\",   \"reffered_balance\": 7.66  },  {   \"transactionDate\": \"2021-09-13\",   \"creditDate\": {    \"seconds\": 1631678400   },   \"description\": \"US D\u00E9bito compra de ativo\",   \"amount\": 300.24,   \"transactionId\": \"cc31a184-ecc9-4897-97ab-b73ffa477003\",   \"comment\": \"{\\\"items\\\": {\\\"Side\\\": \\\"Buy\\\", \\\"Price\\\": \\\"150.12\\\", \\\"Symbol\\\": \\\"AAPL\\\", \\\"ClOrdId\\\": \\\"20210913000050580\\\", \\\"Quantity\\\": \\\"2\\\", \\\"Operation\\\": \\\"Fill\\\", \\\"Commission\\\": \\\"1.5\\\", \\\"HistoryCode\\\": \\\"4001\\\"}}\",   \"entryDate\": {    \"seconds\": 1631544667   },   \"Type\": \"D\",   \"reffered_balance\": -292.58  } ]";
                //avenueTransactions = JsonConvert.DeserializeObject<List<AvenueTransaction>>(result, jsonSerializerSettings2);

                #endregion

                if (avenueTransactions != null && avenueTransactions.Count() > 0)
                {
                    avenueTransactions = avenueTransactions.Where(trt => trt.Description.Contains("compra") || trt.Description.Contains("venda") || trt.Description.Contains("venda") || trt.Description.Contains("dividendos")).ToList();

                    if (avenueTransactions != null && avenueTransactions.Count() > 0)
                    {
                        importAvenueResult.Orders = new List<AvenueOrder>();
                        importAvenueResult.Dividends = new List<AvenueDividend>();

                        foreach (AvenueTransaction avenueTransaction in avenueTransactions)
                        {
                            var jsonSerializerSettings = new JsonSerializerSettings();
                            jsonSerializerSettings.Culture = CultureInfo.InvariantCulture;
                            jsonSerializerSettings.MissingMemberHandling = MissingMemberHandling.Ignore;
                            AvenueTransactionItem avenueTransactionItem = JsonConvert.DeserializeObject<AvenueTransactionItem>(avenueTransaction.Comment, jsonSerializerSettings);

                            if (avenueTransactionItem != null && avenueTransactionItem.Items != null && !string.IsNullOrWhiteSpace(avenueTransactionItem.Items.Symbol))
                            {
                                if (avenueTransaction.Description.Contains("compra") || (avenueTransaction.Description.Contains("venda")))
                                {
                                    decimal avgPrice = 0;
                                    decimal.TryParse(avenueTransactionItem.Items.Price, NumberStyles.Currency, CultureInfo.InvariantCulture, out avgPrice);

                                    DateTime eventDate = DateTime.Now;
                                    DateTime.TryParse(avenueTransaction.TransactionDate, CultureInfo.InvariantCulture, DateTimeStyles.None, out eventDate);

                                    decimal quantity = 0;
                                    decimal.TryParse(avenueTransactionItem.Items.Quantity, NumberStyles.Currency, CultureInfo.InvariantCulture, out quantity);

                                    AvenueOrder avenueOrder = new AvenueOrder();
                                    avenueOrder.AveragePrice = avgPrice;
                                    avenueOrder.EventDate = eventDate;
                                    avenueOrder.NumberOfShares = quantity;
                                    avenueOrder.Symbol = avenueTransactionItem.Items.Symbol;
                                    avenueOrder.IdOperationType = avenueTransaction.Description.Contains("compra") ? 1 : 2;
                                    avenueOrder.TransactionId = avenueTransaction.TransactionId;

                                    importAvenueResult.Orders.Add(avenueOrder);
                                }
                                else if (avenueTransaction.Description.Contains("dividendos"))
                                {
                                    decimal avgPrice = 0;
                                    decimal.TryParse(avenueTransaction.Amount, NumberStyles.Currency, CultureInfo.InvariantCulture, out avgPrice);

                                    DateTime eventDate = DateTime.Now;
                                    DateTime.TryParse(avenueTransaction.TransactionDate, CultureInfo.InvariantCulture, DateTimeStyles.None, out eventDate);

                                    AvenueDividend avenueDividend = new AvenueDividend();
                                    avenueDividend.EventDate = eventDate;
                                    avenueDividend.NetValue = avgPrice;
                                    avenueDividend.Symbol = avenueTransactionItem.Items.Symbol;
                                    avenueDividend.TransactionId = avenueTransaction.TransactionId;

                                    importAvenueResult.Dividends.Add(avenueDividend);
                                }

                            }
                        }
                    }
                }

                if (getContactDetails)
                {
                    AvenueProfile avenueProfile = await GetFullProfile(importAvenueResult.AccessToken, importAvenueResult.IdToken, importAvenueResult);

                    if (avenueProfile != null)
                    {
                        importAvenueResult.ContactDetails = new AvenueContactDetails();
                        importAvenueResult.ContactPhones = new List<AvenueContactPhone>();

                        if (avenueProfile.AddressInfo != null)
                        {
                            importAvenueResult.ContactDetails.AddressNumber = avenueProfile.AddressInfo.Number;
                            importAvenueResult.ContactDetails.StreetName = avenueProfile.AddressInfo.Street;
                            importAvenueResult.ContactDetails.Complement = avenueProfile.AddressInfo.Complement;
                            importAvenueResult.ContactDetails.Neighborhood = avenueProfile.AddressInfo.Neighborhood;
                            importAvenueResult.ContactDetails.City = avenueProfile.AddressInfo.City;
                            importAvenueResult.ContactDetails.StateCode = avenueProfile.AddressInfo.State;
                            importAvenueResult.ContactDetails.PostalCode = avenueProfile.AddressInfo.PostalCode;
                        }

                        if (avenueProfile.BasicInfo != null)
                        {
                            importAvenueResult.ContactDetails.BirthCity = avenueProfile.BasicInfo.BirthCity;
                            importAvenueResult.ContactDetails.Email = avenueProfile.BasicInfo.Email;
                            importAvenueResult.ContactDetails.MotherName = avenueProfile.BasicInfo.MotherName;
                            importAvenueResult.ContactDetails.SpouseName = avenueProfile.BasicInfo.SpouseName;
                            importAvenueResult.ContactDetails.Gender = avenueProfile.BasicInfo.Gender;

                            if (!string.IsNullOrWhiteSpace(avenueProfile.BasicInfo.MainPhone))
                            {
                                AvenueContactPhone avenueContactPhone = new AvenueContactPhone();
                                avenueContactPhone.PhoneNumber = avenueProfile.BasicInfo.MainPhone;
                                importAvenueResult.ContactPhones.Add(avenueContactPhone);
                            }

                            if (!string.IsNullOrWhiteSpace(avenueProfile.BasicInfo.AlternatePhone))
                            {
                                AvenueContactPhone avenueContactPhone = new AvenueContactPhone();
                                avenueContactPhone.PhoneNumber = avenueProfile.BasicInfo.AlternatePhone;
                                importAvenueResult.ContactPhones.Add(avenueContactPhone);
                            }
                        }

                        if (avenueProfile.EmploymentInfo != null)
                        {
                            importAvenueResult.ContactDetails.OcupationDesc = avenueProfile.EmploymentInfo.Position;
                            importAvenueResult.ContactDetails.CompanyName = avenueProfile.EmploymentInfo.Company;
                        }

                        if (avenueProfile.InvestorProfileInfo != null)
                        {
                            importAvenueResult.ContactDetails.MonthlyIncome = avenueProfile.InvestorProfileInfo.MonthlyIncome;
                            importAvenueResult.ContactDetails.BankDepositAmount = avenueProfile.InvestorProfileInfo.NetWorth;
                        }

                        if (avenueProfile.JointAccount != null && !string.IsNullOrWhiteSpace(avenueProfile.JointAccount.MainPhone))
                        {
                            AvenueContactPhone avenueContactPhone = new AvenueContactPhone();
                            avenueContactPhone.PhoneNumber = avenueProfile.JointAccount.MainPhone;
                            importAvenueResult.ContactPhones.Add(avenueContactPhone);
                        }
                    }
                }
            }

            return importAvenueResult;
        }
        
        public async Task GetChallengepwdreq(string user, string session, string token, string challenge, ImportAvenueResult importAvenueResult)
        {
            HttpClientHandler httpClientHandler = new HttpClientHandler()
            {
                Proxy = new WebProxy(string.Format("{0}:{1}", PROXY_IP, "21218"), false)
            };

            using (var httpClient = new HttpClient(httpClientHandler))
            {
                using (var request = new HttpRequestMessage(new HttpMethod("POST"), "https://pit-api.avenue.us/auth/challengepwdreq"))
                {
                    string content = "{\n    \"username\": \"" + user + "\",\n    \"session\": \"" + session + "\",\n    \"token\": \"" + token + "\",\n    \"challenge\": \"" + challenge + "\"\n}";
                    request.Content = new StringContent(content);
                    request.Content.Headers.ContentType = MediaTypeHeaderValue.Parse("application/json");

                    var response = await httpClient.SendAsync(request);

                    if (response.IsSuccessStatusCode)
                    {
                        importAvenueResult.Success = true;
                        var responseContent = await response.Content.ReadAsStringAsync();

                        dynamic resultAPI = JsonConvert.DeserializeObject<ExpandoObject>(response.Content.ReadAsStringAsync().Result);

                        importAvenueResult.AccessToken = resultAPI.access_token;
                        importAvenueResult.IdToken = resultAPI.id_token;
                    }
                    else
                    {
                        importAvenueResult.Success = false;
                        importAvenueResult.ApiResult = "request: " + content + " response: " + response.Content.ReadAsStringAsync().Result;
                    }
                }
            }
        }


        public async Task GetCustomChallenge(string user, string session, string token, string challenge, ImportAvenueResult importAvenueResult)
        {
            HttpClientHandler httpClientHandler = new HttpClientHandler()
            {
                Proxy = new WebProxy(string.Format("{0}:{1}", PROXY_IP, "21218"), false)
            };

            using (var httpClient = new HttpClient(httpClientHandler))
            {
                using (var request = new HttpRequestMessage(new HttpMethod("POST"), "https://pit-api.avenue.us/auth/customchallenge"))
                {
                    request.Content = new StringContent("{\n    \"username\": \"" + user + "\",\n    \"session\": \"" + session + "\",\n    \"token\": \"" + token + "\",\n    \"challenge\": \"" + challenge + "\"\n}");
                    request.Content.Headers.ContentType = MediaTypeHeaderValue.Parse("application/json");

                    var response = await httpClient.SendAsync(request);

                    if (response.IsSuccessStatusCode)
                    {
                        importAvenueResult.Success = true;
                        var responseContent = await response.Content.ReadAsStringAsync();

                        dynamic resultAPI = JsonConvert.DeserializeObject<ExpandoObject>(response.Content.ReadAsStringAsync().Result);

                        importAvenueResult.AccessToken = resultAPI.access_token;
                        importAvenueResult.IdToken = resultAPI.id_token;
                    }
                    else
                    {
                        importAvenueResult.Success = false;
                        importAvenueResult.ApiResult = response.Content.ReadAsStringAsync().Result;
                    }
                }
            }
        }

        private static async Task<AvenueProfile> GetFullProfile(string accessToken, string idToken, ImportAvenueResult importAvenueResult)
        {
            AvenueProfile avenueProfile = null;

            HttpClientHandler httpClientHandler = new HttpClientHandler()
            {
                Proxy = new WebProxy(string.Format("{0}:{1}", PROXY_IP, "21218"), false)
            };

            using (var httpClient = new HttpClient(httpClientHandler))
            {
                using (var request = new HttpRequestMessage(new HttpMethod("GET"), "https://pit-api.avenue.us/api/profile/full"))
                {
                    request.Headers.TryAddWithoutValidation("accept", "application/json, text/plain, */*");
                    request.Headers.TryAddWithoutValidation("authorization", accessToken);
                    request.Headers.TryAddWithoutValidation("idtoken", idToken);

                    var response = await httpClient.SendAsync(request);

                    if (response.IsSuccessStatusCode)
                    {
                        var responseContent = await response.Content.ReadAsStringAsync();
                        var jsonSerializerSettings = new JsonSerializerSettings();
                        jsonSerializerSettings.Culture = CultureInfo.InvariantCulture;
                        jsonSerializerSettings.MissingMemberHandling = MissingMemberHandling.Ignore;
                        avenueProfile = JsonConvert.DeserializeObject<AvenueProfile>(response.Content.ReadAsStringAsync().Result, jsonSerializerSettings);
                    }
                }
            }

            return avenueProfile;
        }

        public async Task<DateTime?> GetAccountStartDate(string accessToken, string idToken, ImportAvenueResult importAvenueResult)
		{
			DateTime? startDate = null;

            HttpClientHandler httpClientHandler = new HttpClientHandler()
            {
                Proxy = new WebProxy(string.Format("{0}:{1}", PROXY_IP, "21218"), false)
            };

            using (var httpClient = new HttpClient(httpClientHandler))
			{
				using (var request = new HttpRequestMessage(new HttpMethod("GET"), "https://pit-api.avenue.us/api/profile/accountinfo"))
				{
					request.Headers.TryAddWithoutValidation("accept", "application/json, text/plain, */*");
					request.Headers.TryAddWithoutValidation("authorization", accessToken);
					request.Headers.TryAddWithoutValidation("idtoken", idToken);

					var response = await httpClient.SendAsync(request);


					if (response.IsSuccessStatusCode)
					{
						var responseContent = await response.Content.ReadAsStringAsync();

						dynamic resultAPI = JsonConvert.DeserializeObject<ExpandoObject>(response.Content.ReadAsStringAsync().Result);

						startDate = resultAPI.started_plan;
					}
                    else
                    {
                        importAvenueResult.ApiResult = response.Content.ReadAsStringAsync().Result;
                    }
                }
			}

			return startDate;
		}

		public async Task<List<AvenueTransaction>> GetTransactions(string accessToken, string idToken, DateTime startDate, DateTime endDate, ImportAvenueResult importAvenueResult)
		{
			List<AvenueTransaction> resultAPI = new List<AvenueTransaction>();
            string start = startDate.ToString("yyyy-MM-ddTHH:mm:ss") + "-03:00";
            string end = endDate.ToString("yyyy-MM-ddTHH:mm:ss") + "-03:00";

            HttpClientHandler httpClientHandler = new HttpClientHandler()
            {
                Proxy = new WebProxy(string.Format("{0}:{1}", PROXY_IP, "21218"), false)
            };

            using (var httpClient = new HttpClient(httpClientHandler))
			{
                string url = string.Format("https://pit-api.avenue.us/api/report/statements?startdate={0}&enddate={1}&currency=USD", start, end);

                using (var request = new HttpRequestMessage(new HttpMethod("GET"), url))
				{
					request.Headers.TryAddWithoutValidation("accept", "application/json, text/plain, */*");
					request.Headers.TryAddWithoutValidation("authorization", accessToken);
					request.Headers.TryAddWithoutValidation("idtoken", idToken);

					var response = await httpClient.SendAsync(request);


					if (response.IsSuccessStatusCode)
					{
						var responseContent = await response.Content.ReadAsStringAsync();
                        var jsonSerializerSettings = new JsonSerializerSettings();
						jsonSerializerSettings.Culture = CultureInfo.InvariantCulture;
						jsonSerializerSettings.MissingMemberHandling = MissingMemberHandling.Ignore;
                        resultAPI = JsonConvert.DeserializeObject<List<AvenueTransaction>>(response.Content.ReadAsStringAsync().Result, jsonSerializerSettings);


                        //string result = "[  {   \"transactionDate\": \"2021-09-13\",   \"creditDate\": {    \"seconds\": 1631678400   },   \"description\": \"US Corretagem\",   \"amount\": 1.5,   \"transactionId\": \"7b740e22-ce61-4fa7-9b46-644d45e93933\",   \"comment\": \"{\\\"items\\\": {\\\"Side\\\": \\\"Buy\\\", \\\"Price\\\": \\\"150.12\\\", \\\"Symbol\\\": \\\"AAPL\\\", \\\"ClOrdId\\\": \\\"20210913000050580\\\", \\\"Quantity\\\": \\\"2\\\", \\\"Operation\\\": \\\"Fill\\\", \\\"Commission\\\": \\\"1.5\\\", \\\"HistoryCode\\\": \\\"4013\\\"}}\",   \"entryDate\": {    \"seconds\": 1631544667   },   \"Type\": \"D\",   \"reffered_balance\": 7.66  },  {   \"transactionDate\": \"2021-09-13\",   \"creditDate\": {    \"seconds\": 1631678400   },   \"description\": \"US D\u00E9bito compra de ativo\",   \"amount\": 300.24,   \"transactionId\": \"cc31a184-ecc9-4897-97ab-b73ffa477003\",   \"comment\": \"{\\\"items\\\": {\\\"Side\\\": \\\"Buy\\\", \\\"Price\\\": \\\"150.12\\\", \\\"Symbol\\\": \\\"AAPL\\\", \\\"ClOrdId\\\": \\\"20210913000050580\\\", \\\"Quantity\\\": \\\"2\\\", \\\"Operation\\\": \\\"Fill\\\", \\\"Commission\\\": \\\"1.5\\\", \\\"HistoryCode\\\": \\\"4001\\\"}}\",   \"entryDate\": {    \"seconds\": 1631544667   },   \"Type\": \"D\",   \"reffered_balance\": -292.58  } ]";
                        //resultAPI = JsonConvert.DeserializeObject<List<AvenueTransaction>>(response.Content.ReadAsStringAsync().Result, jsonSerializerSettings);

                        if (resultAPI == null)
                        {
                            resultAPI = new List<AvenueTransaction>();
                        }
                        else
                        {
                            if (string.IsNullOrWhiteSpace(importAvenueResult.ApiResult))
                            {
                                importAvenueResult.ApiResult = response.Content.ReadAsStringAsync().Result;
                            }
                            else
                            {
                                importAvenueResult.ApiResult = importAvenueResult.ApiResult + response.Content.ReadAsStringAsync().Result;
                            }
                        }
                        
					}
                    else
                    {
                        importAvenueResult.ApiResult = response.Content.ReadAsStringAsync().Result;
                    }
                }
			}

			return resultAPI;
		}

        //private static async Task GetHistorical(ImportAvenueResult importAvenueResult, string accessToken, string idToken)
        //{
        //    using (var httpClient = new HttpClient())
        //    {
        //        using (var request = new HttpRequestMessage(new HttpMethod("GET"), "https://pit-api.avenue.us/api/order/historical"))
        //        {
        //            request.Headers.TryAddWithoutValidation("accept", "application/json, text/plain, */*");
        //            request.Headers.TryAddWithoutValidation("authorization", accessToken);
        //            request.Headers.TryAddWithoutValidation("idtoken", idToken);

        //            var response = await httpClient.SendAsync(request);


        //            if (response.IsSuccessStatusCode)
        //            {
        //                var jsonSerializerSettings = new JsonSerializerSettings();
        //                jsonSerializerSettings.Culture = CultureInfo.InvariantCulture;
        //                jsonSerializerSettings.MissingMemberHandling = MissingMemberHandling.Ignore;
        //                AvenueOrder avenueOrder = JsonConvert.DeserializeObject<AvenueOrder>(response.Content.ReadAsStringAsync().Result, jsonSerializerSettings);


        //                if (avenueOrder != null && avenueOrder.Orders != null && avenueOrder.Orders.Count > 0)
        //                {
        //                    importAvenueResult.Orders = new List<Order>();

        //                    foreach (OrderItem orderItem in avenueOrder.Orders)
        //                    {
        //                        Order order = new Order();
        //                        order.AveragePrice = orderItem.AvePx;
        //                        order.NumberOfShares = orderItem.FilledQty;
        //                        order.Symbol = orderItem.Symbol;
        //                        order.EventDate = DateTime.Now;

        //                        if (orderItem.Events != null && orderItem.Events.Count > 0)
        //                        {
        //                            order.EventDate = orderItem.Events.OrderByDescending(item => item.Created).First().Created;
        //                        }

        //                        importAvenueResult.Orders.Add(order);
        //                    }
        //                }
        //            }
        //        }
        //    }
        //}

    }
}
