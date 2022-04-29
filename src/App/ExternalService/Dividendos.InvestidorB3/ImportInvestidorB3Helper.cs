using Dividendos.InvestidorB3.Config;
using Dividendos.InvestidorB3.Interface;
using Dividendos.InvestidorB3.Interface.Model;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Data.SqlTypes;
using System.Diagnostics;
using System.Text;
using System.Linq;
using System.Threading.Tasks;
using System.Globalization;
using K.Logger;
using System.Threading;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Dynamic;
using Dividendos.InvestidorB3.Interface.Model.Response;
using Dividendos.InvestidorB3.Interface.Model.Response.EquitiesPosition;
using Dividendos.InvestidorB3.Interface.Model.Response.EquitiesMovement;
using Dividendos.InvestidorB3.Interface.Model.Response.ProvisionedEvent;
using Dividendos.InvestidorB3.Interface.Model.Response.AssetTrading;

namespace Dividendos.InvestidorB3
{
    public class ImportInvestidorB3Helper : IImportInvestidorB3Helper
    {
        string _urlBasePortalInvestidorB3;
        string _urlEndPointLogin;
        string _clientId;
        string _clientSecret;
        string _scope;
        string _uRLAuthB3;
        HttpClient _httpClient;

        public ImportInvestidorB3Helper(string urlBasePortalInvestidorB3, string urlEndPointLogin, string clientId, string clientSecret, string scope, string uRLAuthB3, HttpClient httpClient)
        {
            _urlBasePortalInvestidorB3 = urlBasePortalInvestidorB3;
            _urlEndPointLogin = urlEndPointLogin;
            _clientId = clientId;
            _clientSecret = clientSecret;
            _scope = scope;
            _uRLAuthB3 = uRLAuthB3;
            _httpClient = httpClient;
        }

        public string GetAutorizationToken()
        {
            string token = null;

            using (var request = new HttpRequestMessage(new HttpMethod("POST"), _urlEndPointLogin))
            {
                var contentList = new List<string>();
                contentList.Add($"grant_type={Uri.EscapeDataString("client_credentials")}");
                contentList.Add($"client_id={Uri.EscapeDataString(_clientId)}");
                contentList.Add($"client_secret={Uri.EscapeDataString(_clientSecret)}");
                contentList.Add($"scope={Uri.EscapeDataString(_scope)}");
                request.Content = new StringContent(string.Join("&", contentList));
                request.Content.Headers.ContentType = MediaTypeHeaderValue.Parse("application/x-www-form-urlencoded");

                HttpResponseMessage response = SendAsync(request);

                dynamic resultAPI = JsonConvert.DeserializeObject<ExpandoObject>(response.Content.ReadAsStringAsync().Result);

                token = resultAPI.access_token;
                var expireIn = resultAPI.expires_in;
            }

            return token;
        }

        public void Healthcheck(string bearer)
        {
            using (var request = new HttpRequestMessage(new HttpMethod("GET"), string.Concat(_urlBasePortalInvestidorB3, "api/acesso/healthcheck")))
            {
                request.Headers.TryAddWithoutValidation("Authorization", string.Concat("Bearer ", bearer));

                HttpResponseMessage response = SendAsync(request);
            }
        }

        public string GetURLAuthB3()
        {
            return this._uRLAuthB3;
        }

        public Interface.Model.Response.UpdatedProduct.Root UdpateProduct(string bearer, Interface.Model.Request.Product investidorB3Product, DateTime startReferenceDate, DateTime endReferenceDate, int page)
        {
            Interface.Model.Response.UpdatedProduct.Root rootResponse = null;

            using (var request = new HttpRequestMessage(new HttpMethod("GET"), string.Format("{0}api/updated-product/v1/investors?product={1}&referenceStartDate={2}&referenceEndDate={3}&page={4}", _urlBasePortalInvestidorB3, investidorB3Product, startReferenceDate.ToString("yyyy-MM-dd"), endReferenceDate.ToString("yyyy-MM-dd"), page)))
            {
                request.Headers.TryAddWithoutValidation("Authorization", string.Concat("Bearer ", bearer));

                HttpResponseMessage response = SendAsync(request);

                rootResponse = JsonConvert.DeserializeObject<Interface.Model.Response.UpdatedProduct.Root>(response.Content.ReadAsStringAsync().Result);
            }

            return rootResponse;
        }

        public Interface.Model.Response.AssetTrading.Root AssetsTrading(string bearer, string documentNumber, DateTime startReferenceDate, DateTime endReferenceDate, int page)
        {
            Interface.Model.Response.AssetTrading.Root rootResponse = null;

            using (var request = new HttpRequestMessage(new HttpMethod("GET"), string.Format("{0}api/assets-trading/v1/investors/{1}?referenceStartDate={2}&referenceEndDate={3}&page={4}", _urlBasePortalInvestidorB3, documentNumber, startReferenceDate.ToString("yyyy-MM-dd"), endReferenceDate.ToString("yyyy-MM-dd"), page)))
            {
                request.Headers.TryAddWithoutValidation("Authorization", string.Concat("Bearer ", bearer));

                HttpResponseMessage response = SendAsync(request);

                rootResponse = JsonConvert.DeserializeObject<Interface.Model.Response.AssetTrading.Root>(response.Content.ReadAsStringAsync().Result);
            }

            return rootResponse;
        }

        private HttpResponseMessage SendAsync(HttpRequestMessage request, bool throwEx500 = true)
        {
            HttpResponseMessage response = new HttpResponseMessage();

            //try
            //{
                response = _httpClient.SendAsync(request).Result;

                if (response.StatusCode == System.Net.HttpStatusCode.InternalServerError && throwEx500)
                {
                    throw new Exception(response.Content.ReadAsStringAsync().Result);
                }
            //}
            //catch (Exception ex)
            //{

            //    throw;
            //}



            return response;
        }

        public Interface.Model.Response.Collateral.Root Collaterals(string bearer, string documentNumber, DateTime referenceDate, int page)
        {
            Interface.Model.Response.Collateral.Root rootResponse = null;

            using (var request = new HttpRequestMessage(new HttpMethod("GET"), string.Format("{0}api/collaterals/v1/investors/{1}?referenceDate={2}&page={3}", _urlBasePortalInvestidorB3, documentNumber, referenceDate.ToString("yyyy-MM-dd"), page)))
            {
                request.Headers.TryAddWithoutValidation("Authorization", string.Concat("Bearer ", bearer));

                HttpResponseMessage response = SendAsync(request);

                rootResponse = JsonConvert.DeserializeObject<Interface.Model.Response.Collateral.Root>(response.Content.ReadAsStringAsync().Result);
            }

            return rootResponse;
        }

        public Interface.Model.Response.ProvisionedEvent.Root ProvisionedEvents(string bearer, string documentNumber, DateTime referenceDate, int page)
        {
            Interface.Model.Response.ProvisionedEvent.Root rootResponse = null;

            using (var request = new HttpRequestMessage(new HttpMethod("GET"), string.Format("{0}api/provisioned-events/v1/investors/{1}?referenceDate={2}&page={3}", _urlBasePortalInvestidorB3, documentNumber, referenceDate.ToString("yyyy-MM-dd"), page)))
            {
                request.Headers.TryAddWithoutValidation("Authorization", string.Concat("Bearer ", bearer));

                HttpResponseMessage response = SendAsync(request);

                rootResponse = JsonConvert.DeserializeObject<Interface.Model.Response.ProvisionedEvent.Root>(response.Content.ReadAsStringAsync().Result);
            }

            return rootResponse;
        }

        public Interface.Model.Response.PublicOffer.Root PublicOffers(string bearer, string documentNumber, DateTime startReferenceDate, DateTime endReferenceDate, int page)
        {
            Interface.Model.Response.PublicOffer.Root rootResponse = null;

            using (var request = new HttpRequestMessage(new HttpMethod("GET"), string.Format("{0}api/public-offers/v1/investors/{4}?referenceStartDate={2}&referenceEndDate={3}&page={4}", _urlBasePortalInvestidorB3, documentNumber, startReferenceDate.ToString("yyyy-MM-dd"), endReferenceDate.ToString("yyyy-MM-dd"), page)))
            {
                request.Headers.TryAddWithoutValidation("Authorization", string.Concat("Bearer ", bearer));

                HttpResponseMessage response = SendAsync(request);

                rootResponse = JsonConvert.DeserializeObject<Interface.Model.Response.PublicOffer.Root>(response.Content.ReadAsStringAsync().Result);
            }

            return rootResponse;
        }


        public Interface.Model.Response.SecurityLendingPosition.Root PositionSecuritiesLending(string bearer, string documentNumber, DateTime referenceDate, int page)
        {
            Interface.Model.Response.SecurityLendingPosition.Root rootResponse = null;

            using (var request = new HttpRequestMessage(new HttpMethod("GET"), string.Format("{0}api/position/v1/securities-lending/investors/{1}?referenceDate={2}&page={3}", _urlBasePortalInvestidorB3, documentNumber, referenceDate.ToString("yyyy-MM-dd"), page)))
            {
                request.Headers.TryAddWithoutValidation("Authorization", string.Concat("Bearer ", bearer));

                HttpResponseMessage response = SendAsync(request);

                rootResponse = JsonConvert.DeserializeObject<Interface.Model.Response.SecurityLendingPosition.Root>(response.Content.ReadAsStringAsync().Result);
            }

            return rootResponse;
        }

        public Interface.Model.Response.TreasuryBondsPosition.Root PositionTreasuryBonds(string bearer, string documentNumber, DateTime referenceDate, int page)
        {
            Interface.Model.Response.TreasuryBondsPosition.Root rootResponse = null;

            using (var request = new HttpRequestMessage(new HttpMethod("GET"), string.Format("{0}api/position/v1/treasury-bonds/investors/{1}?referenceDate={2}&page={3}", _urlBasePortalInvestidorB3, documentNumber, referenceDate.ToString("yyyy-MM-dd"), page)))
            {
                request.Headers.TryAddWithoutValidation("Authorization", string.Concat("Bearer ", bearer));

                HttpResponseMessage response = SendAsync(request, false);

                rootResponse = JsonConvert.DeserializeObject<Interface.Model.Response.TreasuryBondsPosition.Root>(response.Content.ReadAsStringAsync().Result);
            }

            return rootResponse;
        }

        public Interface.Model.Response.EquitiesPosition.Root PositionEquities(string bearer, string documentNumber, DateTime referenceDate, int page)
        {
            Interface.Model.Response.EquitiesPosition.Root rootResponse = null;

            using (var request = new HttpRequestMessage(new HttpMethod("GET"), string.Format("{0}api/position/v1/equities/investors/{1}?referenceDate={2}&page={3}", _urlBasePortalInvestidorB3, documentNumber, referenceDate.ToString("yyyy-MM-dd"), page)))
            {
                request.Headers.TryAddWithoutValidation("Authorization", string.Concat("Bearer ", bearer));

                HttpResponseMessage response = SendAsync(request);

                var jsonSerializerSettings = new JsonSerializerSettings();
                jsonSerializerSettings.Culture = CultureInfo.InvariantCulture;
                jsonSerializerSettings.MissingMemberHandling = MissingMemberHandling.Ignore;
                rootResponse = JsonConvert.DeserializeObject<Interface.Model.Response.EquitiesPosition.Root>(response.Content.ReadAsStringAsync().Result, jsonSerializerSettings);

                if (rootResponse != null)
                {
                    rootResponse.ResponseJson = response.Content.ReadAsStringAsync().Result;
                }
            }

            return rootResponse;
        }

        public Interface.Model.Response.FixedIncomePosition.Root PositionFixedIncome(string bearer, string documentNumber, DateTime referenceDate, int page)
        {
            Interface.Model.Response.FixedIncomePosition.Root rootResponse = null;

            using (var request = new HttpRequestMessage(new HttpMethod("GET"), string.Format("{0}api/position/v1/fixed-income/investors/{1}?referenceDate={2}&page={3}", _urlBasePortalInvestidorB3, documentNumber, referenceDate.ToString("yyyy-MM-dd"), page)))
            {
                request.Headers.TryAddWithoutValidation("Authorization", string.Concat("Bearer ", bearer));

                HttpResponseMessage response = SendAsync(request, false);

                rootResponse = JsonConvert.DeserializeObject<Interface.Model.Response.FixedIncomePosition.Root>(response.Content.ReadAsStringAsync().Result);
            }

            return rootResponse;
        }

        public Interface.Model.Response.DerivativesPosition.Root PositionDerivatives(string bearer, string documentNumber, DateTime referenceDate, int page)
        {
            Interface.Model.Response.DerivativesPosition.Root rootResponse = null;

            using (var request = new HttpRequestMessage(new HttpMethod("GET"), string.Format("{0}api/position/v1/derivatives/investors/{1}?referenceDate={2}&page={3}", _urlBasePortalInvestidorB3, documentNumber, referenceDate.ToString("yyyy-MM-dd"), page)))
            {
                request.Headers.TryAddWithoutValidation("Authorization", string.Concat("Bearer ", bearer));

                HttpResponseMessage response = SendAsync(request);

                rootResponse = JsonConvert.DeserializeObject<Interface.Model.Response.DerivativesPosition.Root>(response.Content.ReadAsStringAsync().Result);
            }

            return rootResponse;
        }

        public Interface.Model.Response.SecurityLendingMovement.Root MovementSecuritiesLending(string bearer, string documentNumber, DateTime startReferenceDate, DateTime endReferenceDate, int page)
        {
            Interface.Model.Response.SecurityLendingMovement.Root rootResponse = null;

            using (var request = new HttpRequestMessage(new HttpMethod("GET"), string.Format("{0}api/movement/v1/securities-lending/investors/{1}?referenceStartDate={2}&referenceEndDate={3}&page={4}", _urlBasePortalInvestidorB3, documentNumber, startReferenceDate.ToString("yyyy-MM-dd"), endReferenceDate.ToString("yyyy-MM-dd"), page)))
            {
                request.Headers.TryAddWithoutValidation("Authorization", string.Concat("Bearer ", bearer));

                HttpResponseMessage response = SendAsync(request);

                rootResponse = JsonConvert.DeserializeObject<Interface.Model.Response.SecurityLendingMovement.Root>(response.Content.ReadAsStringAsync().Result);
            }

            return rootResponse;
        }

        public Interface.Model.Response.TreasuryBondsMovement.Root MovementTreasuryBonds(string bearer, string documentNumber, DateTime startReferenceDate, DateTime endReferenceDate, int page)
        {
            Interface.Model.Response.TreasuryBondsMovement.Root rootResponse = null;

            using (var request = new HttpRequestMessage(new HttpMethod("GET"), string.Format("{0}api/movement/v1/treasury-bonds/investors/{1}?referenceStartDate={2}&referenceEndDate={3}&page={4}", _urlBasePortalInvestidorB3, documentNumber, startReferenceDate.ToString("yyyy-MM-dd"), endReferenceDate.ToString("yyyy-MM-dd"), page)))
            {
                request.Headers.TryAddWithoutValidation("Authorization", string.Concat("Bearer ", bearer));

                HttpResponseMessage response = SendAsync(request);

                rootResponse = JsonConvert.DeserializeObject<Interface.Model.Response.TreasuryBondsMovement.Root>(response.Content.ReadAsStringAsync().Result);
            }

            return rootResponse;
        }

        public Interface.Model.Response.EquitiesMovement.Root MovementEquities(string bearer, string documentNumber, DateTime startReferenceDate, DateTime endReferenceDate, int page)
        {
            Interface.Model.Response.EquitiesMovement.Root rootResponse = null;

            using (var request = new HttpRequestMessage(new HttpMethod("GET"), string.Format("{0}api/movement/v1/equities/investors/{1}?referenceStartDate={2}&referenceEndDate={3}&page={4}", _urlBasePortalInvestidorB3, documentNumber, startReferenceDate.ToString("yyyy-MM-dd"), endReferenceDate.ToString("yyyy-MM-dd"), page)))
            {
                request.Headers.TryAddWithoutValidation("Authorization", string.Concat("Bearer ", bearer));

                HttpResponseMessage response = SendAsync(request);

                rootResponse = JsonConvert.DeserializeObject<Interface.Model.Response.EquitiesMovement.Root>(response.Content.ReadAsStringAsync().Result);
            }

            return rootResponse;
        }

        public Interface.Model.Response.FixedIncomeMovement.Root MovementFixedIncome(string bearer, string documentNumber, DateTime startReferenceDate, DateTime endReferenceDate, int page)
        {
            Interface.Model.Response.FixedIncomeMovement.Root rootResponse = null;

            using (var request = new HttpRequestMessage(new HttpMethod("GET"), string.Format("{0}api/movement/v1/fixed-income/investors/{1}?referenceStartDate={2}&referenceEndDate={3}&page={4}", _urlBasePortalInvestidorB3, documentNumber, startReferenceDate.ToString("yyyy-MM-dd"), endReferenceDate.ToString("yyyy-MM-dd"), page)))
            {
                request.Headers.TryAddWithoutValidation("Authorization", string.Concat("Bearer ", bearer));

                HttpResponseMessage response = SendAsync(request);

                rootResponse = JsonConvert.DeserializeObject<Interface.Model.Response.FixedIncomeMovement.Root>(response.Content.ReadAsStringAsync().Result);
            }

            return rootResponse;
        }

        public Interface.Model.Response.DerivativeMovement.Root MovementDerivaties(string bearer, string documentNumber, DateTime startReferenceDate, DateTime endReferenceDate, int page)
        {
            Interface.Model.Response.DerivativeMovement.Root rootResponse = null;

            using (var request = new HttpRequestMessage(new HttpMethod("GET"), string.Format("{0}api/movement/v1/derivatives/investors/{1}?referenceStartDate={2}&referenceEndDate={3}&page={4}", _urlBasePortalInvestidorB3, documentNumber, startReferenceDate.ToString("yyyy-MM-dd"), endReferenceDate.ToString("yyyy-MM-dd"), page)))
            {
                request.Headers.TryAddWithoutValidation("Authorization", string.Concat("Bearer ", bearer));

                HttpResponseMessage response = SendAsync(request);

                rootResponse = JsonConvert.DeserializeObject<Interface.Model.Response.DerivativeMovement.Root>(response.Content.ReadAsStringAsync().Result);
            }

            return rootResponse;
        }

        public ImportCeiResult ImportAllInvestments(string documentNumber, bool automaticProcess, DateTime endReferenceDate, DateTime? lastEventDate, DateTime? lastSync, CancellationTokenSource cancellationTokenSource = null)
        {
            ImportCeiResult importCeiResult = new ImportCeiResult();

            try
            {
                DateTime startReferenceDate = DateTime.Now.AddDays(-909);
                DateTime oldDividendsLastDate = startReferenceDate;

                var token = GetAutorizationToken();

                if (lastEventDate.HasValue)
                {
                    startReferenceDate = lastEventDate.Value;
                }

                if (startReferenceDate > endReferenceDate)
                {
                    startReferenceDate = endReferenceDate;
                }

                if (lastSync.HasValue)
                {
                    oldDividendsLastDate = lastSync.Value.AddDays(-4);
                }

                if (oldDividendsLastDate > endReferenceDate)
                {
                    oldDividendsLastDate = endReferenceDate;
                }

                List<StockOperation> stockOperations = GetAssetsTrading(documentNumber, startReferenceDate, endReferenceDate, token);

                List<StockOperation> stocksRent = GetLendingPosition(documentNumber, endReferenceDate, token);

                List<DividendImport> dividendImports = GetDividends(documentNumber, endReferenceDate, token);

                importCeiResult.ListTesouroDireto = GetTreasuryBond(documentNumber, endReferenceDate, token);

                importCeiResult.ListTesouroDireto.AddRange(GetFixedIncome(documentNumber, endReferenceDate, token));

                List<StockOperation> lstStockPortfolio = GetStockPortfolio(documentNumber, endReferenceDate, token, importCeiResult);

                //StockOperation stockOperation = lstStockPortfolio.Where(op => op.Symbol == "TRIS3").FirstOrDefault();
                Tuple<List<StockOperation>, List<DividendImport>> tupResult = GetHistoricalEvents(documentNumber, oldDividendsLastDate, endReferenceDate, token);


                importCeiResult.ListDividend = new List<DividendImport>();
                importCeiResult.ListStockPortfolio = lstStockPortfolio;
                importCeiResult.ListStockPortfolio.AddRange(stocksRent);
                importCeiResult.ListStockOperation = new List<StockOperation>();

                if (dividendImports != null && dividendImports.Count > 0)
                {
                    importCeiResult.ListDividend.AddRange(dividendImports);
                }

                if (tupResult.Item2 != null && tupResult.Item2.Count > 0)
                {
                   // List<DividendImport> div = tupResult.Item2.Where(div => div.PaymentDate == null).ToList();

                    importCeiResult.ListDividend.AddRange(tupResult.Item2);

                    //div = importCeiResult.ListDividend.Where(div => div.Symbol == "ENBR3").ToList();
                }

                if (importCeiResult.ListStockPortfolio != null && importCeiResult.ListStockPortfolio.Count > 0 || CheckNewStocksToPortfolio(stockOperations))
                {
                    importCeiResult.ListStockOperation = stockOperations;
                }

                if (importCeiResult.ListStockOperation.Count.Equals(0) &&
                    importCeiResult.ListStockPortfolio.Count.Equals(0) &&
                    importCeiResult.ListTesouroDireto.Count.Equals(0))
                {
                    importCeiResult.Imported = false;
                    if (string.IsNullOrWhiteSpace(importCeiResult.Message))
                    {
                        importCeiResult.Message = "Ocorreu uma falha durante a integração ou seus ativos ainda não estão no CEI (B3). Por favor, tente novamente em alguns minutos.";
                    }
                }
                else
                {
                    importCeiResult.Imported = true;
                }
            }
            catch (Exception ex)
            {
                importCeiResult.Imported = false;
                importCeiResult.Retry = true;
                importCeiResult.Message = ex.Message;
                //throw;
            }

            return importCeiResult;
        }

        private List<StockOperation> GetAssetsTrading(string documentNumber, DateTime startReferenceDate, DateTime endReferenceDate, string token)
        {
            List<StockOperation> lstStockOperation = new List<StockOperation>();
            Interface.Model.Response.AssetTrading.Root assetsTrading = AssetsTrading(token, documentNumber, startReferenceDate, endReferenceDate, 1);
            List<Periods> assetPeriods = new List<Periods>();
            Links links = null;

            if (assetsTrading != null)
            {
                if (assetsTrading.data != null && assetsTrading.data.periods != null)
                {
                    assetPeriods.Add(assetsTrading.data.periods);
                }

                links = assetsTrading.Links;

                if (links != null)
                {
                    while (links != null && links.next != null)
                    {
                        int index = links.next.LastIndexOf('=');
                        int page = int.Parse(links.next.Substring(index + 1));

                        assetsTrading = AssetsTrading(token, documentNumber, startReferenceDate, endReferenceDate, page);
                        links = null;

                        if (assetsTrading != null)
                        {
                            links = assetsTrading.Links;
                        }

                        if (assetsTrading.data != null && assetsTrading.data.periods != null)
                        {
                            assetPeriods.Add(assetsTrading.data.periods);
                        }
                    }
                }
            }

            if (assetPeriods != null && assetPeriods.Count > 0)
            {
                foreach (Periods period in assetPeriods)
                {
                    if (period.periodLists != null && period.periodLists.Count > 0)
                    {
                        foreach (PeriodList periodList in period.periodLists)
                        {
                            DateTime eventDate;
                            DateTime.TryParseExact(periodList.referenceDate, "yyyy-MM-dd", CultureInfo.InvariantCulture, DateTimeStyles.None, out eventDate);

                            if (periodList.assetTradingList != null && periodList.assetTradingList.Count > 0)
                            {
                                foreach (AssetTrading assetTrading in periodList.assetTradingList)
                                {
                                    StockOperation stockNegotiation = new StockOperation();

                                    decimal avgPrice = 0;
                                    decimal.TryParse(assetTrading.priceValue, NumberStyles.Currency, CultureInfo.InvariantCulture, out avgPrice);

                                    decimal numberOfShares = 0;
                                    decimal.TryParse(assetTrading.tradeQuantity, NumberStyles.Currency, CultureInfo.InvariantCulture, out numberOfShares);


                                    stockNegotiation.Broker = assetTrading.participantName;
                                    stockNegotiation.AveragePrice = avgPrice;
                                    stockNegotiation.Symbol = RemoveFractionalLetter(assetTrading.tickerSymbol);
                                    stockNegotiation.NumberOfShares = numberOfShares;
                                    stockNegotiation.OperationType = assetTrading.side.ToLower() == "compra" ? 1 : 2;
                                    stockNegotiation.EventDate = eventDate;


                                    lstStockOperation.Add(stockNegotiation);
                                }
                            }
                        }
                    }
                }
            }

            return lstStockOperation;
        }

        private List<StockOperation> GetLendingPosition(string documentNumber, DateTime endReferenceDate, string token)
        {
            List<StockOperation> lstStockPortfolio = new List<StockOperation>();
            Interface.Model.Response.SecurityLendingPosition.Root lendingPosition = PositionSecuritiesLending(token, documentNumber, endReferenceDate, 1);
            List<Interface.Model.Response.SecurityLendingPosition.SecurityLendingPosition> lendingAllPages = new List<Interface.Model.Response.SecurityLendingPosition.SecurityLendingPosition>();
            Links links = null;

            if (lendingPosition != null)
            {
                if (lendingPosition.data != null && lendingPosition.data.securityLendingPositions != null && lendingPosition.data.securityLendingPositions.Count > 0)
                {
                    lendingAllPages.AddRange(lendingPosition.data.securityLendingPositions);
                }

                links = lendingPosition.Links;

                if (links != null)
                {
                    while (links != null && links.next != null)
                    {
                        int index = links.next.LastIndexOf('=');
                        int page = int.Parse(links.next.Substring(index + 1));

                        lendingPosition = PositionSecuritiesLending(token, documentNumber, endReferenceDate, page);
                        links = null;

                        if (lendingPosition != null)
                        {
                            links = lendingPosition.Links;
                        }

                        if (lendingPosition.data != null && lendingPosition.data.securityLendingPositions != null && lendingPosition.data.securityLendingPositions.Count > 0)
                        {
                            lendingAllPages.AddRange(lendingPosition.data.securityLendingPositions);
                        }
                    }
                }
            }

            if (lendingAllPages != null && lendingAllPages.Count > 0)
            {
                lendingAllPages = lendingAllPages.Where(lend => lend.sideLenderBorrowerName == "Doador").ToList();

                if (lendingAllPages != null && lendingAllPages.Count > 0)
                {
                    foreach (Interface.Model.Response.SecurityLendingPosition.SecurityLendingPosition lendingPos in lendingAllPages)
                    {
                        decimal numberOfShares = 0;
                        decimal.TryParse(lendingPos.securitiesLendingQuantity, NumberStyles.Currency, CultureInfo.InvariantCulture, out numberOfShares);

                        DateTime expirationDate;
                        DateTime.TryParseExact(lendingPos.expirationDate, "yyyy-MM-dd", CultureInfo.InvariantCulture, DateTimeStyles.None, out expirationDate);

                        StockOperation stockPortfolio = new StockOperation();

                        stockPortfolio.Broker = lendingPos.participantName;
                        stockPortfolio.AveragePrice = 0;
                        stockPortfolio.Symbol = RemoveFractionalLetter(lendingPos.tickerSymbol);
                        stockPortfolio.NumberOfShares = numberOfShares;

                        if (expirationDate >= DateTime.Now.Date)
                        {
                            lstStockPortfolio.Add(stockPortfolio);
                        }
                    }
                }
            }

            return lstStockPortfolio;
        }

        private List<TesouroDiretoImport> GetFixedIncome(string documentNumber, DateTime endReferenceDate, string token)
        {
            List<TesouroDiretoImport> tesouroDiretoImports = new List<TesouroDiretoImport>();
            Interface.Model.Response.FixedIncomePosition.Root fixedIncome = PositionFixedIncome(token, documentNumber, endReferenceDate, 1);
            List<Interface.Model.Response.FixedIncomePosition.FixedIncomePosition> fixedIncomeAllPages = new List<Interface.Model.Response.FixedIncomePosition.FixedIncomePosition>();
            Links links = null;

            if (fixedIncome != null)
            {
                if (fixedIncome.data != null && fixedIncome.data.fixedIncomePositions != null && fixedIncome.data.fixedIncomePositions.Count > 0)
                {
                    fixedIncomeAllPages.AddRange(fixedIncome.data.fixedIncomePositions);
                }

                links = fixedIncome.Links;

                if (links != null)
                {
                    while (links != null && links.next != null)
                    {
                        int index = links.next.LastIndexOf('=');
                        int page = int.Parse(links.next.Substring(index + 1));

                        fixedIncome = PositionFixedIncome(token, documentNumber, endReferenceDate, page);
                        links = null;

                        if (fixedIncome != null)
                        {
                            links = fixedIncome.Links;
                        }

                        if (fixedIncome.data != null && fixedIncome.data.fixedIncomePositions != null && fixedIncome.data.fixedIncomePositions.Count > 0)
                        {
                            fixedIncomeAllPages.AddRange(fixedIncome.data.fixedIncomePositions);
                        }
                    }
                }
            }

            if (fixedIncomeAllPages != null && fixedIncomeAllPages.Count > 0)
            {
                foreach (Interface.Model.Response.FixedIncomePosition.FixedIncomePosition fixedInc in fixedIncomeAllPages)
                {
                    TesouroDiretoImport tesouroDiretoImport = new TesouroDiretoImport();

                    decimal netValue = 0;

                    decimal.TryParse(fixedInc.updateValue, NumberStyles.Currency, CultureInfo.InvariantCulture, out netValue);

                    tesouroDiretoImport.Broker = fixedInc.participantName;
                    tesouroDiretoImport.Symbol = fixedInc.tickerSymbol;
                    tesouroDiretoImport.NetValue = netValue;

                    tesouroDiretoImports.Add(tesouroDiretoImport);
                }
            }

            return tesouroDiretoImports;
        }

        private List<TesouroDiretoImport> GetTreasuryBond(string documentNumber, DateTime endReferenceDate, string token)
        {
            List<TesouroDiretoImport> tesouroDiretoImports = new List<TesouroDiretoImport>();
            Interface.Model.Response.TreasuryBondsPosition.Root treasuryBonds = PositionTreasuryBonds(token, documentNumber, endReferenceDate, 1);

            List<Interface.Model.Response.TreasuryBondsPosition.TreasuryBondsPosition> treasuryBondsAllPages = new List<Interface.Model.Response.TreasuryBondsPosition.TreasuryBondsPosition>();
            Interface.Model.Response.TreasuryBondsPosition.Links links = null;

            if (treasuryBonds != null)
            {
                if (treasuryBonds.data != null && treasuryBonds.data.treasuryBondsPositions != null && treasuryBonds.data.treasuryBondsPositions.Count > 0)
                {
                    treasuryBondsAllPages.AddRange(treasuryBonds.data.treasuryBondsPositions);
                }

                links = treasuryBonds.Links;

                if (links != null)
                {
                    while (links != null && links.next != null)
                    {
                        int index = links.next.LastIndexOf('=');
                        int page = int.Parse(links.next.Substring(index + 1));

                        treasuryBonds = PositionTreasuryBonds(token, documentNumber, endReferenceDate, page);
                        links = null;

                        if (treasuryBonds != null)
                        {
                            links = treasuryBonds.Links;
                        }

                        if (treasuryBonds.data != null && treasuryBonds.data.treasuryBondsPositions != null && treasuryBonds.data.treasuryBondsPositions.Count > 0)
                        {
                            treasuryBondsAllPages.AddRange(treasuryBonds.data.treasuryBondsPositions);
                        }
                    }
                }
            }

            if (treasuryBondsAllPages != null && treasuryBondsAllPages.Count > 0)
            {
                foreach (Interface.Model.Response.TreasuryBondsPosition.TreasuryBondsPosition treasuryBondsPosition in treasuryBondsAllPages)
                {
                    TesouroDiretoImport tesouroDiretoImport = new TesouroDiretoImport();

                    decimal netValue = 0;

                    decimal.TryParse(treasuryBondsPosition.netValue, NumberStyles.Currency, CultureInfo.InvariantCulture, out netValue);

                    tesouroDiretoImport.Broker = treasuryBondsPosition.participantName;
                    tesouroDiretoImport.Symbol = treasuryBondsPosition.tickerSymbol;
                    tesouroDiretoImport.NetValue = netValue;

                    tesouroDiretoImports.Add(tesouroDiretoImport);
                }
            }

            return tesouroDiretoImports;
        }

        private List<DividendImport> GetDividends(string documentNumber, DateTime endReferenceDate, string token)
        {
            List<DividendImport> dividendImports = new List<DividendImport>();
            Interface.Model.Response.ProvisionedEvent.Root provisionedEvents = ProvisionedEvents(token, documentNumber, endReferenceDate, 1);
            List<ProvisionedEvent> provisionedEventsAllPages = new List<ProvisionedEvent>();
            Links links = null;

            if (provisionedEvents != null)
            {
                if (provisionedEvents.data != null && provisionedEvents.data.provisionedEvents != null && provisionedEvents.data.provisionedEvents.Count > 0)
                {
                    provisionedEventsAllPages.AddRange(provisionedEvents.data.provisionedEvents);
                }

                links = provisionedEvents.Links;

                if (links != null)
                {
                    while (links != null && links.next != null)
                    {
                        int index = links.next.LastIndexOf('=');
                        int page = int.Parse(links.next.Substring(index + 1));

                        provisionedEvents = ProvisionedEvents(token, documentNumber, endReferenceDate, page);
                        links = null;

                        if (provisionedEvents != null)
                        {
                            links = provisionedEvents.Links;
                        }

                        if (provisionedEvents.data != null && provisionedEvents.data.provisionedEvents != null && provisionedEvents.data.provisionedEvents.Count > 0)
                        {
                            provisionedEventsAllPages.AddRange(provisionedEvents.data.provisionedEvents);
                        }
                    }
                }
            }

            if (provisionedEventsAllPages != null && provisionedEventsAllPages.Count > 0)
            {
                foreach (ProvisionedEvent provisionedEvent in provisionedEventsAllPages)
                {
                    DividendImport dividendImport = new DividendImport();

                    decimal netValue = 0;
                    decimal grossValue = 0;
                    decimal.TryParse(provisionedEvent.netValue, NumberStyles.Currency, CultureInfo.InvariantCulture, out netValue);
                    decimal.TryParse(provisionedEvent.grossAmount, NumberStyles.Currency, CultureInfo.InvariantCulture, out grossValue);
                    decimal eventQuantity = 0;
                    decimal.TryParse(provisionedEvent.eventQuantity, NumberStyles.Currency, CultureInfo.InvariantCulture, out eventQuantity);

                    dividendImport.Broker = provisionedEvent.participantName;
                    dividendImport.DividendType = provisionedEvent.corporateActionTypeDescription;
                    dividendImport.NetValue = netValue;
                    dividendImport.GrossValue = grossValue;
                    dividendImport.BaseQuantity = Convert.ToInt32(eventQuantity);
                    dividendImport.Symbol = RemoveFractionalLetter(provisionedEvent.tickerSymbol);

                    DateTime eventDate;
                    
                    if (DateTime.TryParseExact(provisionedEvent.paymentDate, "yyyy-MM-dd", CultureInfo.InvariantCulture, DateTimeStyles.None, out eventDate))
                    {
                        dividendImport.PaymentDate = eventDate;
                    }

                    if (dividendImport.PaymentDate <= SqlDateTime.MinValue.Value)
                    {
                        dividendImport.PaymentDate = null;
                    }

                    dividendImports.Add(dividendImport);
                }
            }

            return dividendImports;
        }

        private List<StockOperation> GetStockPortfolio(string documentNumber, DateTime endReferenceDate, string token, ImportCeiResult importCeiResult)
        {
            List<StockOperation> lstStockPortfolio = new List<StockOperation>();

            Interface.Model.Response.EquitiesPosition.Root equitiesPosition = PositionEquities(token, documentNumber, endReferenceDate, 1);

            List<EquitiesPosition> equitiesPositionsAllPages = new List<EquitiesPosition>();

            Links links = null;

            if (equitiesPosition != null)
            {
                importCeiResult.Json += " " + equitiesPosition.ResponseJson;

                if (equitiesPosition != null && equitiesPosition.data != null && equitiesPosition.data.equitiesPositions != null && equitiesPosition.data.equitiesPositions.Count > 0)
                {
                    equitiesPositionsAllPages.AddRange(equitiesPosition.data.equitiesPositions);
                }

                links = equitiesPosition.Links;

                if (links != null)
                {
                    while (links != null && links.next != null)
                    {
                        int index = links.next.LastIndexOf('=');
                        int page = int.Parse(links.next.Substring(index + 1));

                        equitiesPosition = PositionEquities(token, documentNumber, endReferenceDate, page);
                        importCeiResult.Json += " " + equitiesPosition.ResponseJson;
                        links = null;

                        if (equitiesPosition != null)
                        {
                            links = equitiesPosition.Links;
                        }

                        if (equitiesPosition != null && equitiesPosition.data != null && equitiesPosition.data.equitiesPositions != null && equitiesPosition.data.equitiesPositions.Count > 0)
                        {
                            equitiesPositionsAllPages.AddRange(equitiesPosition.data.equitiesPositions);
                        }
                    }
                }
            }

            if (equitiesPositionsAllPages != null && equitiesPositionsAllPages.Count > 0)
            {
                foreach (EquitiesPosition equitiesPos in equitiesPositionsAllPages)
                {
                    decimal numberOfShares = 0;
                    decimal.TryParse(equitiesPos.equitiesQuantity, NumberStyles.Currency, CultureInfo.InvariantCulture, out numberOfShares);

                    StockOperation stockPortfolio = new StockOperation();

                    stockPortfolio.Broker = equitiesPos.participantName;
                    stockPortfolio.AveragePrice = 0;
                    stockPortfolio.Symbol = RemoveFractionalLetter(equitiesPos.tickerSymbol);
                    stockPortfolio.NumberOfShares = numberOfShares;

                    lstStockPortfolio.Add(stockPortfolio);
                }
            }

            return lstStockPortfolio;
        }

        public Tuple<List<StockOperation>, List<DividendImport>> GetHistoricalEvents(string documentNumber, DateTime startReferenceDate, DateTime endReferenceDate, string token)
        {
            List<StockOperation> lstStockOperation = new List<StockOperation>();
            List<DividendImport> dividendImports = new List<DividendImport>();

            Tuple<List<StockOperation>, List<DividendImport>> tupResult = new Tuple<List<StockOperation>, List<DividendImport>>(lstStockOperation, dividendImports);

            Interface.Model.Response.EquitiesMovement.Root equitiesMovement = MovementEquities(token, documentNumber, startReferenceDate, endReferenceDate, 1);

            List<EquitiesMovement> equitiesMovementsAllPages = new List<EquitiesMovement>();

            Links links = null;

            if (equitiesMovement != null)
            {
                if (equitiesMovement != null && equitiesMovement.data != null && equitiesMovement.data.equitiesPeriods != null && equitiesMovement.data.equitiesPeriods.equitiesMovements != null && equitiesMovement.data.equitiesPeriods.equitiesMovements.Count > 0)
                {
                    equitiesMovementsAllPages.AddRange(equitiesMovement.data.equitiesPeriods.equitiesMovements);
                }

                links = equitiesMovement.Links;

                if (links != null)
                {
                    while (links != null && links.next != null)
                    {
                        int index = links.next.LastIndexOf('=');
                        int page = int.Parse(links.next.Substring(index + 1));
                        //Thread.Sleep(500);
                        equitiesMovement = MovementEquities(token, documentNumber, startReferenceDate, endReferenceDate, page);
                        links = null;

                        if (equitiesMovement != null)
                        {
                            links = equitiesMovement.Links;
                        }

                        if (equitiesMovement != null && equitiesMovement.data != null && equitiesMovement.data.equitiesPeriods != null && equitiesMovement.data.equitiesPeriods.equitiesMovements != null && equitiesMovement.data.equitiesPeriods.equitiesMovements.Count > 0)
                        {
                            equitiesMovementsAllPages.AddRange(equitiesMovement.data.equitiesPeriods.equitiesMovements);
                        }
                    }
                }
            }

            if (equitiesMovementsAllPages != null && equitiesMovementsAllPages.Count > 0)
            {
                List<EquitiesMovement> equitiesMovementsBuySell = equitiesMovementsAllPages.Where(eq => eq.movementType.Contains("Liquidação") && (eq.operationType.Trim().ToLower() == "debito" || eq.operationType.Trim().ToLower() == "credito")).ToList();

                if (equitiesMovementsBuySell != null && equitiesMovementsBuySell.Count > 0)
                {
                    foreach (EquitiesMovement equitiesMov in equitiesMovementsBuySell)
                    {
                        StockOperation stockNegotiation = new StockOperation();

                        decimal avgPrice = 0;
                        decimal.TryParse(equitiesMov.unitPrice, NumberStyles.Currency, CultureInfo.InvariantCulture, out avgPrice);

                        decimal numberOfShares = 0;
                        decimal.TryParse(equitiesMov.equitiesQuantity, NumberStyles.Currency, CultureInfo.InvariantCulture, out numberOfShares);

                        DateTime eventDate;
                        DateTime.TryParseExact(equitiesMov.referenceDate, "yyyy-MM-dd", CultureInfo.InvariantCulture, DateTimeStyles.None, out eventDate);


                        stockNegotiation.Broker = equitiesMov.participantName;
                        stockNegotiation.AveragePrice = avgPrice;
                        stockNegotiation.Symbol = RemoveFractionalLetter(equitiesMov.tickerSymbol);
                        stockNegotiation.NumberOfShares = numberOfShares;
                        stockNegotiation.OperationType = 1;

                        if (!string.IsNullOrWhiteSpace(equitiesMov.operationType))
                        {
                            if (equitiesMov.operationType.ToLower().Trim() == "debito")
                            {
                                stockNegotiation.OperationType = 2;
                            }
                        }


                        stockNegotiation.EventDate = eventDate;


                        lstStockOperation.Add(stockNegotiation);
                    }
                }

                List<EquitiesMovement> dividends = equitiesMovementsAllPages.Where(eq => !eq.movementType.Contains("Liquidação") && eq.operationType.Trim().ToLower() == "credito").ToList();

                if (dividends != null && dividends.Count > 0)
                {
                    foreach (EquitiesMovement dividend in dividends)
                    {
                        DividendImport dividendImport = new DividendImport();

                        decimal netValue = 0;
                        decimal eventQuantity = 0;
                        decimal.TryParse(dividend.operationValue, NumberStyles.Currency, CultureInfo.InvariantCulture, out netValue);
                        decimal.TryParse(dividend.equitiesQuantity, NumberStyles.Currency, CultureInfo.InvariantCulture, out eventQuantity);

                        if (netValue > 0)
                        {
                            dividendImport.Broker = dividend.participantName;
                            dividendImport.DividendType = dividend.movementType;
                            dividendImport.NetValue = netValue;
                            dividendImport.GrossValue = netValue;
                            dividendImport.BaseQuantity = Convert.ToInt32(eventQuantity);
                            dividendImport.Symbol = RemoveFractionalLetter(dividend.tickerSymbol);

                            DateTime eventDate;

                            if (DateTime.TryParseExact(dividend.referenceDate, "yyyy-MM-dd", CultureInfo.InvariantCulture, DateTimeStyles.None, out eventDate))
                            {
                                dividendImport.PaymentDate = eventDate;
                            }

                            if (dividendImport.PaymentDate <= SqlDateTime.MinValue.Value)
                            {
                                dividendImport.PaymentDate = null;
                            }

                            dividendImports.Add(dividendImport);
                        }
                    }
                }
            }

            return tupResult;
        }

        private static string RemoveFractionalLetter(string stock)
        {
            if (!string.IsNullOrEmpty(stock) && (stock[stock.Length - 1].ToString().ToUpper() == "F"))
            {
                stock = stock.Remove(stock.Length - 1);
            }

            return stock.Trim();
        }

        private static bool CheckNewStocksToPortfolio(List<StockOperation> lstStockOperation)
        {
            bool exists = false;

            List<StockOperation> lstStockPortfolioGrouped = new List<StockOperation>();

            List<StockOperation> stocksOperationGpNew = lstStockOperation.Where(op => op.OperationType == 1 && !op.PriceAdjustNew && op.EventDate.HasValue && DateTime.Now.Subtract(op.EventDate.Value).TotalDays <= 6)
                                                                       .GroupBy(op => op.Symbol).Select(objStockOpGp => new StockOperation
                                                                       {
                                                                           Broker = objStockOpGp.First().Broker,
                                                                           Symbol = objStockOpGp.First().Symbol,
                                                                           NumberOfShares = objStockOpGp.Sum(c => c.NumberOfShares),
                                                                           AveragePrice = 0,
                                                                       }).ToList();

            if (stocksOperationGpNew != null && stocksOperationGpNew.Count > 0)
            {
                foreach (StockOperation stockOpNew in stocksOperationGpNew)
                {
                    if (lstStockPortfolioGrouped.Count == 0)
                    {
                        lstStockPortfolioGrouped.Add(stockOpNew);
                    }
                    else if (!lstStockPortfolioGrouped.Exists(op => op.Symbol == stockOpNew.Symbol))
                    {
                        lstStockPortfolioGrouped.Add(stockOpNew);
                    }
                }
            }

            if (lstStockPortfolioGrouped != null && lstStockPortfolioGrouped.Count > 0)
            {
                exists = true;
            }

            return exists;
        }

        public List<string> CheckProductsUpdate(DateTime startDate, DateTime endDate)
        {
            if (startDate > endDate)
            {
                startDate = endDate;
            }

            List<string> documentNumbers = new List<string>();

            documentNumbers.AddRange(GetAllDocumentsToUpdate(startDate, endDate, InvestidorB3.Interface.Model.Request.Product.AssetsTrading));
            documentNumbers.AddRange(GetAllDocumentsToUpdate(startDate, endDate, InvestidorB3.Interface.Model.Request.Product.SecuritiesLendingPosition));
            documentNumbers.AddRange(GetAllDocumentsToUpdate(startDate, endDate, InvestidorB3.Interface.Model.Request.Product.ProvisionedEvents));
            documentNumbers.AddRange(GetAllDocumentsToUpdate(startDate, endDate, InvestidorB3.Interface.Model.Request.Product.TreasuryBondsPosition));
            documentNumbers.AddRange(GetAllDocumentsToUpdate(startDate, endDate, InvestidorB3.Interface.Model.Request.Product.FixedIncommePosition));
            documentNumbers.AddRange(GetAllDocumentsToUpdate(startDate, endDate, InvestidorB3.Interface.Model.Request.Product.EquitiesMovement));
            documentNumbers.AddRange(GetAllDocumentsToUpdate(startDate, endDate, InvestidorB3.Interface.Model.Request.Product.EquitiesPosition));

            if (documentNumbers != null && documentNumbers.Count > 0)
            {
                documentNumbers = documentNumbers.Distinct().ToList();
            }

            return documentNumbers;
        }

        private List<string> GetAllDocumentsToUpdate(DateTime startDate, DateTime endDate, InvestidorB3.Interface.Model.Request.Product product)
        {
            List<string> documentNumbers = new List<string>();
            var token = GetAutorizationToken();
            Dividendos.InvestidorB3.Interface.Model.Response.UpdatedProduct.Root root = UdpateProduct(token, product, startDate, endDate, 1);
            List<Interface.Model.Response.UpdatedProduct.UpdatedProduct> updatedProductsAllPages = new List<Interface.Model.Response.UpdatedProduct.UpdatedProduct>();
            Dividendos.InvestidorB3.Interface.Model.Response.UpdatedProduct.Links links = null;

            if (root != null)
            {
                if (root != null && root.data != null && root.data.updatedProducts != null && root.data.updatedProducts.Count > 0)
                {
                    updatedProductsAllPages.AddRange(root.data.updatedProducts);
                }

                links = root.links;

                if (links != null)
                {
                    while (links != null && links.next != null)
                    {
                        int index = links.next.LastIndexOf('=');
                        int page = int.Parse(links.next.Substring(index + 1));
                        //Thread.Sleep(500);
                        root = UdpateProduct(token, product, startDate, endDate, page);
                        links = null;

                        if (root != null)
                        {
                            links = root.links;
                        }

                        if (root != null && root.data != null && root.data.updatedProducts != null && root.data.updatedProducts.Count > 0)
                        {
                            updatedProductsAllPages.AddRange(root.data.updatedProducts);
                        }
                    }
                }
            }

            if (updatedProductsAllPages != null && updatedProductsAllPages.Count > 0)
            {
                foreach (Interface.Model.Response.UpdatedProduct.UpdatedProduct updatedProduct in updatedProductsAllPages)
                {
                    documentNumbers.Add(updatedProduct.documentNumber);
                }
            }

            return documentNumbers;
        }
    }
}
