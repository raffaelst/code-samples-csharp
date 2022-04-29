using Dividendos.Passfolio.Interface;
using Dividendos.Passfolio.Interface.Model;
using K.Logger;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Globalization;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Reflection;
using System.Threading;

namespace Dividendos.Passfolio
{
	public class PassfolioHelper : IPassfolioHelper
	{
        /// <summary>
        /// Step 1 
        /// </summary>
        /// <param name="email"></param>
        /// <param name="password"></param>
        /// <returns></returns>
		public string Session(string email, string password)
		{
            string token = null;

            using (var httpClient = new HttpClient())
            {
                using (var request = new HttpRequestMessage(new HttpMethod("POST"), "https://api.passfolio.us/sessions"))
                {
                    request.Headers.TryAddWithoutValidation("Passfolio-App-Version", "AAAAAAAA");
                    SessionRequest sessionRequest = new SessionRequest() { email = email, password = password };
                    request.Content = new StringContent(JsonConvert.SerializeObject(sessionRequest));
                    request.Content.Headers.ContentType = MediaTypeHeaderValue.Parse("application/json");

                    var response = httpClient.SendAsync(request).Result;

                    if (response.IsSuccessStatusCode)
                    {
                        SessionResponse sessionResponse = JsonConvert.DeserializeObject<SessionResponse>(response.Content.ReadAsStringAsync().Result);

                        token = sessionResponse.token;
                    }
                }
            }

            return token;
        }

        /// <summary>
        /// Step 2
        /// </summary>
        /// <param name="auth"></param>
        /// <returns></returns>
        public AuthenticatorResponse Authenticators(string auth)
        {
            AuthenticatorResponse authenticatorResponse;

            using (var httpClient = new HttpClient())
            {
                using (var request = new HttpRequestMessage(new HttpMethod("GET"), "https://api.passfolio.us/authenticators"))
                {
                    request.Headers.TryAddWithoutValidation("authorization", auth);
                    request.Headers.TryAddWithoutValidation("Passfolio-App-Version", "AAAAAAAA");

                    var response = httpClient.SendAsync(request).Result;

                    if (response.IsSuccessStatusCode)
                    {

                    }

                    IEnumerable<AuthenticatorResponse> authenticatorsAvailables = JsonConvert.DeserializeObject<IEnumerable<AuthenticatorResponse>>(response.Content.ReadAsStringAsync().Result);

                    authenticatorResponse = authenticatorsAvailables.FirstOrDefault();
                }
            }


            return authenticatorResponse;
        }

        /// <summary>
        /// Step 3
        /// </summary>
        /// <param name="auth"></param>
        /// <param name="authenticatorID"></param>
        public void SendCode(string auth, string authenticatorID)
        {
            using (var httpClient = new HttpClient())
            {
                using (var request = new HttpRequestMessage(new HttpMethod("POST"), string.Concat("https://api.passfolio.us/authenticators/sendCode/", authenticatorID)))
                {
                    request.Headers.TryAddWithoutValidation("authorization", auth);
                    request.Headers.TryAddWithoutValidation("Passfolio-App-Version", "AAAAAAAA");

                    var response = httpClient.SendAsync(request).Result;

                    if (response.IsSuccessStatusCode)
                    {

                    }

                    AuthenticatorResponse authenticatorResponse = JsonConvert.DeserializeObject<AuthenticatorResponse>(response.Content.ReadAsStringAsync().Result);
                }
            }
        }

        /// <summary>
        /// Step 4
        /// </summary>
        /// <param name="auth"></param>
        /// <param name="code"></param>
        /// <param name="authenticatorID"></param>
        /// <returns></returns>
        public bool SessionMFA(string auth, string code, string authenticatorID)
        {
            bool validationSucess = false;

            using (var httpClient = new HttpClient())
            {
                using (var request = new HttpRequestMessage(new HttpMethod("POST"), "https://api.passfolio.us/sessions/mfa"))
                {
                    request.Headers.TryAddWithoutValidation("Passfolio-App-Version", "AAAAAAAA");
                    request.Headers.TryAddWithoutValidation("authorization", auth);

                    SessionMFARequest sessionMFARequest = new SessionMFARequest() { authenticatorId = authenticatorID, code = code };
                    request.Content = new StringContent(JsonConvert.SerializeObject(sessionMFARequest));
                    request.Content.Headers.ContentType = MediaTypeHeaderValue.Parse("application/json");

                    var response = httpClient.SendAsync(request).Result;

                    if (response.IsSuccessStatusCode)
                    {
                        validationSucess = true;
                    }
                }
            }

            return validationSucess;
        }


        public ImportPassfolioResult Import(string auth, ILogger logger)
        {
            Thread.CurrentThread.CurrentCulture = new CultureInfo("en-us");
            Thread.CurrentThread.CurrentUICulture = new CultureInfo("en-us");

            ImportPassfolioResult importPassfolioResult = new ImportPassfolioResult() { Imported = false };
            importPassfolioResult.ListStockPortfolio = new List<StockPassfolioOperation>();
            importPassfolioResult.ListStockOperation = new List<StockPassfolioOperation>();
            importPassfolioResult.ListDividend = new List<DividendPassfolioImport>();
            importPassfolioResult.ListCrypto = new List<CryptoPassfolioImport>();

            try
            {
                using (var httpClient = new HttpClient())
                {
                    //portfolio
                    using (var request = new HttpRequestMessage(new HttpMethod("GET"), "https://performance.api.passfolio.us/portfolioPerformance/assetStats/multi?assetType=stock&symbols=ALL"))
                    {
                        request.Headers.TryAddWithoutValidation("Passfolio-App-Version", "AAAAAAAA");
                        request.Headers.TryAddWithoutValidation("authorization", auth);

                        var response = httpClient.SendAsync(request).Result;

                        if (response.IsSuccessStatusCode)
                        {
                            importPassfolioResult.Imported = true;

                            dynamic resultAPI = JsonConvert.DeserializeObject<ExpandoObject>(response.Content.ReadAsStringAsync().Result);

                            IDictionary<string, object> propertyValues = resultAPI;

                            foreach (var property in propertyValues)
                            {
                                dynamic value = property.Value;
                                importPassfolioResult.ListStockPortfolio.Add(new StockPassfolioOperation()
                                {
                                    Broker = "Passfolio",
                                    Symbol = property.Key,
                                    NumberOfShares = decimal.Parse(value.quantity.bignumber),
                                    AveragePrice = decimal.Parse(value.averageCostUSD.bignumber)
                                });
                            }
                        }
                        else
                        {
                            if (response.StatusCode == System.Net.HttpStatusCode.Unauthorized)
                            {
                                importPassfolioResult.PasswordWrong = true;
                                importPassfolioResult.Message = "Falha ao integrar com a Passfolio! Crendenciais de acesso inválidas.";
                                return importPassfolioResult;
                            }
                        }
                    }

                    //Operations
                    using (var request = new HttpRequestMessage(new HttpMethod("GET"), "https://api.passfolio.us/drivewealth/account/transactions?from=2008-07-17&to=2023-01-01"))
                    {
                        request.Headers.TryAddWithoutValidation("Passfolio-App-Version", "AAAAAAAA");
                        request.Headers.TryAddWithoutValidation("authorization", auth);

                        var response = httpClient.SendAsync(request).Result;

                        if (response.IsSuccessStatusCode)
                        {
                            importPassfolioResult.Imported = true;

                            dynamic resultAPI = JsonConvert.DeserializeObject<List<ExpandoObject>>(response.Content.ReadAsStringAsync().Result);

                            foreach (var item in resultAPI)
                            {
                                if (IsPropertyExist(item, "instrument"))
                                {
                                    importPassfolioResult.ListStockOperation.Add(new StockPassfolioOperation()
                                    {
                                        Broker = "Passfolio",
                                        Symbol = item.instrument.symbol,
                                        NumberOfShares = decimal.Round((decimal)item.fillQty, 6),
                                        AveragePrice = decimal.Round((decimal)item.fillPx, 6),
                                        EventDate = item.tranWhen,
                                        OperationType = item.finTranTypeID == "SPUR" || item.finTranTypeID == "XTRF" ? 1 : 2
                                    });
                                }
                            }
                        }
                        else
                        {
                            importPassfolioResult.Imported = false;
                            importPassfolioResult.Message = "Falha ao integrar com a Passfolio! Crendenciais de acesso inválidas.";
                        }
                    }

                    //Dividend
                    using (var request = new HttpRequestMessage(new HttpMethod("GET"), "https://api.passfolio.us/dividends/history"))
                    {
                        request.Headers.TryAddWithoutValidation("Passfolio-App-Version", "AAAAAAAA");
                        request.Headers.TryAddWithoutValidation("authorization", auth);

                        var response = httpClient.SendAsync(request).Result;

                        if (response.IsSuccessStatusCode)
                        {
                            importPassfolioResult.Imported = true;

                            dynamic resultAPI = JsonConvert.DeserializeObject<List<ExpandoObject>>(response.Content.ReadAsStringAsync().Result);

                            foreach (var item in resultAPI)
                            {
                                if (item != null)
                                {
                                    DateTime paymentDate = item.createdAt;

                                    decimal dividendNetValue = 0;

                                    if (IsPropertyExist(item.tax, "amount"))
                                    {
                                        dividendNetValue = decimal.Parse(item.amount.bignumber) + decimal.Parse(item.tax.amount.bignumber);
                                    }
                                    else
                                    {
                                        dividendNetValue = decimal.Parse(item.amount.bignumber);
                                    }

                                    importPassfolioResult.ListDividend.Add(new DividendPassfolioImport()
                                    {
                                        Broker = "Passfolio",
                                        Symbol = item.symbol,
                                        NetValue = dividendNetValue,
                                        GrossValue = decimal.Parse(item.amount.bignumber),
                                        PaymentDate = paymentDate.Date,
                                        DividendType = "DIVIDENDO",
                                    });
                                }
                            }
                        }
                        else
                        {
                            importPassfolioResult.Imported = false;
                            importPassfolioResult.Message = "Falha ao integrar com a Passfolio! Crendenciais de acesso inválidas.";
                        }
                    }

                    //Cripto
                    using (var request = new HttpRequestMessage(new HttpMethod("POST"), "https://api.passfolio.us/gemini/balances"))
                    {
                        request.Headers.TryAddWithoutValidation("Passfolio-App-Version", "AAAAAAAA");
                        request.Headers.TryAddWithoutValidation("authorization", auth);

                        var response = httpClient.SendAsync(request).Result;

                        if (response.IsSuccessStatusCode)
                        {
                            importPassfolioResult.Imported = true;

                            dynamic resultAPI = JsonConvert.DeserializeObject<List<ExpandoObject>>(response.Content.ReadAsStringAsync().Result);

                            foreach (var item in resultAPI)
                            {
                                if (item != null)
                                {
                                    importPassfolioResult.ListCrypto.Add(new CryptoPassfolioImport()
                                    {
                                        Type = item.type,
                                        Amount = decimal.Parse(item.amount),
                                        Available = decimal.Parse(item.available),
                                        AvailableForWithdrawal = decimal.Parse(item.availableForWithdrawal),
                                        Currency = item.currency,
                                    });
                                }
                            }
                        }
                    }
                }

                List<StockPassfolioOperation> listStockPortfolio = new List<StockPassfolioOperation>();

                foreach (var itemStockPortfolio in importPassfolioResult.ListStockPortfolio)
                {
                    if (!itemStockPortfolio.NumberOfShares.Equals(0))
                    {
                        listStockPortfolio.Add(itemStockPortfolio);
                    }
                }

                importPassfolioResult.ListStockPortfolio = listStockPortfolio;

                List<StockPassfolioOperation> listStockOperation = new List<StockPassfolioOperation>();

                foreach (var itemStockOperation in importPassfolioResult.ListStockOperation)
                {
                    if (!itemStockOperation.NumberOfShares.Equals(0))
                    {
                        listStockOperation.Add(itemStockOperation);
                    }
                }

                importPassfolioResult.ListStockOperation = listStockOperation;
            }
            catch (Exception ex)
            {
                logger.SendErrorAsync(ex);
            }

            return importPassfolioResult;
        }

        private static bool IsPropertyExist(dynamic settings, string name)
        {
            if (settings is ExpandoObject)
                return ((IDictionary<string, object>)settings).ContainsKey(name);

            return settings.GetType().GetProperty(name) != null;
        }


        public List<Ticker> GetQuotationOfCryptos(string auth)
        {
            List<Ticker> ticker = new List<Ticker>();

            using (var httpClient = new HttpClient())
            {
                using (var request = new HttpRequestMessage(new HttpMethod("GET"), "https://api.passfolio.us/instruments/search?query=assetType%3Acrypto&sortAscending=true&sortBy=symbol&limit=25&page=0&preLimitSortAscending=true"))
                {
                    request.Headers.TryAddWithoutValidation("authorization", auth);
                    request.Headers.TryAddWithoutValidation("Passfolio-App-Version", "AAAAAAAA");

                    var response = httpClient.SendAsync(request).Result;

                    dynamic resultAPI = JsonConvert.DeserializeObject<List<ExpandoObject>>(response.Content.ReadAsStringAsync().Result);

                    if (response.IsSuccessStatusCode)
                    {
                        foreach (var item in resultAPI)
                        {
                            ticker.Add(GetQuotationOfSpecificCrypto(auth, item.symbol));
                        }
                    }
                }
            }

            return ticker;
        }

        private Ticker GetQuotationOfSpecificCrypto(string auth, string crypto)
        {
            Ticker ticker = new Ticker();

            using (var httpClient = new HttpClient())
            {
                using (var request = new HttpRequestMessage(new HttpMethod("GET"), string.Concat("https://api.passfolio.us/assetInfo/symbols?fields=details%2Cquote&crypto=", crypto)))
                {
                    request.Headers.TryAddWithoutValidation("authorization", auth);
                    request.Headers.TryAddWithoutValidation("Passfolio-App-Version", "AAAAAAAA");

                    var response = httpClient.SendAsync(request).Result;

                    if (response.IsSuccessStatusCode)
                    {
                        dynamic resultAPI = JsonConvert.DeserializeObject<ExpandoObject>(response.Content.ReadAsStringAsync().Result);

                        IDictionary<string, object> propertiesFromApi = resultAPI;

                        foreach (var property in propertiesFromApi)
                        {
                            if (property.Key.Equals("crypto"))
                            {
                                dynamic cryptoDetails = property.Value;

                                foreach (var itemCryptoDetail in cryptoDetails)
                                {
                                    var itemCryptoDetailValue = itemCryptoDetail.Value;

                                    foreach (var itemCrypto in itemCryptoDetailValue)
                                    {
                                        if (itemCrypto.Key.Equals("quote"))
                                        {
                                            ticker = new Ticker()
                                            {
                                                name = crypto,
                                                last = (decimal)itemCrypto.Value.price,
                                                open = (decimal)itemCrypto.Value.open
                                            };
                                        }
                                    }
                                }
                            }
                        }
                    }
                }
            }


            return ticker;
        }
    }
}

