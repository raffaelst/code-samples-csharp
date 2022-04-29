using System;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using Dividendos.StatusInvest.Interface;
using Dividendos.StatusInvest.Interface.Model;
using Newtonsoft.Json;
using System.Linq;
using System.Text;
using System.Web;
using System.Collections.Generic;
using System.Net;

namespace Dividendos.StatusInvest
{
    public class StatusInvestHelper : IStatusInvestHelper
    {
        public StatusInvestHelper()
        {
        }

        public int count { get; set; }
        //private const string PROXY_IP = "172.31.1.215";

        public async Task<List<Datum>> GetCompanies(int type)
        {
            List<Datum> lstCompanies = new List<Datum>();
            var handler = new HttpClientHandler();
            handler.UseCookies = false;
            //handler.Proxy = new WebProxy(string.Format("{0}:{1}", PROXY_IP, "21218"), false);

            using (var httpClient = new HttpClient(handler))
            {
                using (var request = new HttpRequestMessage(new HttpMethod("GET"), string.Format("https://statusinvest.com.br/sector/getcompanies?categoryType={0}", type)))
                {                    
                    request.Content = new StringContent("");
                    request.Content.Headers.ContentType = MediaTypeHeaderValue.Parse("application/x-www-form-urlencoded");

                    try
                    {
                        var response = await httpClient.SendAsync(request);

                        if (response.IsSuccessStatusCode)
                        {
                            var responseContent = await response.Content.ReadAsStringAsync();

                            var jsonSerializerSettings = new JsonSerializerSettings();
                            jsonSerializerSettings.MissingMemberHandling = MissingMemberHandling.Ignore;
                            CompanyInfo companyInfo = JsonConvert.DeserializeObject<CompanyInfo>(responseContent, jsonSerializerSettings);

                            if (companyInfo != null && companyInfo.Data != null && companyInfo.Data.Count > 0)
                            {
                                foreach (Datum company in companyInfo.Data)
                                {
                                    count = 0;
                                    string tickerCode = company.Tickers.First().Code;

                                    if (type == 6)
                                    {
                                        company.Logo = null;
                                    }
                                    else
                                    {
                                        company.Logo = await GetLogo64(company.CompanyId, company.Tickers.First().Code, true, type);
                                    }

                                    if (type == 1)
                                    {
                                        company.Segment = await GetSegment(company.UrlClear);
                                    }
                                    else if (type == 2)
                                    {
                                        if (tickerCode.Contains("34"))
                                        {
                                            company.Segment = "Fundos Imobiliários";
                                        }
                                        else
                                        {
                                            company.Segment = await GetSegmentFiis(company.UrlClear);
                                        }
                                    }
                                    else if (type == 6)
                                    {
                                        company.Segment = "ETFs";
                                    }
                                    else if (type == 12)
                                    {
                                        company.Segment = await GetSegmentStock(company.UrlClear);
                                    }
                                    else if (type == 13)
                                    {
                                        company.Segment = await GetSegmentReits(company.UrlClear);
                                    }

                                    lstCompanies.Add(company);
                                }
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        throw ex;
                    }
                }
            }

            return lstCompanies;
        }

        public async Task<List<DividendCalendarItem>> GetDividendCalendar(DateTime startDate, DateTime finalDate, string symbol, AssetsTypeEnum assetsTypeEnum)
        {
            string url = "";

            try
            {
                List<DividendCalendarItem> dividendCalendarItems = new List<DividendCalendarItem>();

                var handler = new HttpClientHandler();
                handler.UseCookies = false;
                //handler.Proxy = new WebProxy(string.Format("{0}:{1}", PROXY_IP, "21218"), false);

                using (var httpClient = new HttpClient(handler))
                {
                    switch (assetsTypeEnum)
                    {
                        case AssetsTypeEnum.Stocks:
                            url = string.Format("https://statusinvest.com.br/acao/getearnings?IndiceCode=&Filter={0}&Start={1}&End={2}", symbol, startDate.ToString("yyyy-MM-dd"), finalDate.ToString("yyyy-MM-dd"));
                            break;
                        case AssetsTypeEnum.FII:
                            url = string.Format("https://statusinvest.com.br/fii/getearnings?IndiceCode=&Filter={0}&Start={1}&End={2}", symbol, startDate.ToString("yyyy-MM-dd"), finalDate.ToString("yyyy-MM-dd"));
                            break;
                        case AssetsTypeEnum.BDR:
                            url = string.Format("https://statusinvest.com.br/bdr/getearnings?IndiceCode=&Filter={0}&Start={1}&End={2}", symbol, startDate.ToString("yyyy-MM-dd"), finalDate.ToString("yyyy-MM-dd"));
                            break;
                    }

                    if (!string.IsNullOrEmpty(url))
                    {
                        using (var request = new HttpRequestMessage(new HttpMethod("GET"), url))
                        {
                            request.Headers.TryAddWithoutValidation("Accept", "*/*");

                            var response = httpClient.SendAsync(request).Result;

                            if (response.IsSuccessStatusCode)
                            {
                                try
                                {
                                    var responseContent = response.Content.ReadAsStringAsync().Result;

                                    var jsonSerializerSettings = new JsonSerializerSettings();
                                    jsonSerializerSettings.MissingMemberHandling = MissingMemberHandling.Ignore;
                                    Root returnFromStatusInvest = JsonConvert.DeserializeObject<Root>(responseContent, jsonSerializerSettings);

                                    if (returnFromStatusInvest.dateCom != null)
                                    {
                                        foreach (var itemCalendar in returnFromStatusInvest.dateCom)
                                        {
                                            dividendCalendarItems.Add(new DividendCalendarItem() { Ticker = itemCalendar.code, PaymentDate = itemCalendar.paymentDividend, DataCom = itemCalendar.dateCom, Value = itemCalendar.resultAbsoluteValue, Type = itemCalendar.earningType });
                                        }

                                        foreach (var itemCalendar in returnFromStatusInvest.datePayment)
                                        {
                                            dividendCalendarItems.Add(new DividendCalendarItem() { Ticker = itemCalendar.code, PaymentDate = itemCalendar.paymentDividend, DataCom = itemCalendar.dateCom, Value = itemCalendar.resultAbsoluteValue, Type = itemCalendar.earningType });
                                        }
                                    }
                                }
                                catch
                                {
                                    //Nada a fazer
                                }
                            }
                            else
                            {
                                throw new Exception("SI response not ok - IsSuccessStatusCode false");
                            }
                        }
                    }
                }

                return dividendCalendarItems;

            }
            catch (Exception ex)
            {
                throw new Exception(string.Format("SI response not ok - message: {0} - URL: {1}", ex.Message, url));
            }
        }

        public async Task<string> GetLogo64(long companyId, string companyCode, bool small, int idStockType)
        {
            string imgType = "avatar";
            string logo64 = string.Empty;

            if (!small)
            {
                imgType = "square";
            }

            string url = string.Format("https://statusinvest.com.br/img/company/{0}/{1}.jpg", imgType, companyId);

            if (idStockType == 12)
            {
                url = string.Format("https://statusinvest.com.br/img/company/stock/{0}/{1}.jpg", imgType, companyId);
            }
            else if (idStockType == 13)
            {
                url = string.Format("https://statusinvest.com.br/img/company/reit/{0}/{1}.jpg", imgType, companyId);
            }
            else if (companyCode.Contains("34") || companyCode.Contains("39"))
            {
                url = string.Format("https://statusinvest.com.br/img/company/bdr/{0}/{1}.jpg", imgType, companyId);
            }

            try
            {
                //HttpClientHandler httpClientHandler = new HttpClientHandler()
                //{
                //    Proxy = new WebProxy(string.Format("{0}:{1}", PROXY_IP, "21218"), false)
                //};

                using (var client = new HttpClient())
                {
                    var bytes = await client.GetByteArrayAsync(url);
                    logo64 = "data:image/jpeg;base64," + Convert.ToBase64String(bytes);
                }
            }
            catch (Exception ex)
            {
                if (count <= 0)
                {
                    count++;
                    logo64 = await GetLogo64(companyId, companyCode, true, idStockType);                    
                }
                else if (count <= 1)
                {
                    count++;
                    logo64 = await GetLogo64(companyId, companyCode, false, idStockType);
                }
            }

            return logo64;
        }

        public async Task<string> GetSegment(string url)
        {
            string segment = string.Empty;

            try
            {
                var handler = new HttpClientHandler();
                handler.UseCookies = false;
                //handler.Proxy = new WebProxy(string.Format("{0}:{1}", PROXY_IP, "21218"), false);

                using (var httpClient = new HttpClient(handler))
                {
                    using (var request = new HttpRequestMessage(new HttpMethod("GET"), string.Format("https://statusinvest.com.br{0}", url)))
                    {
                        var response = await httpClient.SendAsync(request);

                        if (response.IsSuccessStatusCode)
                        {
                            var bytesContent = await response.Content.ReadAsByteArrayAsync();
                            string responseContent = Encoding.UTF8.GetString(bytesContent, 0, bytesContent.Length - 1);

                            string content = DecodeFromUtf8(responseContent);

                            int index = content.IndexOf("Ver outras empresas do segmento", StringComparison.InvariantCulture);

                            string segmentLine = content.Substring(index, 200);

                            int firstIndex = segmentLine.IndexOf("'", StringComparison.InvariantCulture);
                            int lastIndex = segmentLine.LastIndexOf("'", StringComparison.InvariantCulture);

                            segment = HttpUtility.HtmlDecode(segmentLine.Substring(firstIndex + 1, (lastIndex - firstIndex) - 1));
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }

            if (!string.IsNullOrWhiteSpace(segment))
            {
                if (segment.Contains("Alcoólicas"))
                {
                    segment = "Bebidas Alcoólicas";
                }
                else if (segment.Contains("Análises e Diagnósticos"))
                {
                    segment = "Serviços Médico - Hospitalares, Análises e Diagnósticos";
                }
                else if (segment.Contains("Refino e Distribuição"))
                {
                    segment = "Exploração, Refino e Distribuição";
                }
                else if (segment.Contains("Compressores e Outros"))
                {
                    segment = "Motores, Compressores e Outros";
                }
                else if (segment.Contains("Vestuário e Calçados"))
                {
                    segment = "Tecidos, Vestuário e Calçados";
                }
                else if (segment.Contains("Logística"))
                {
                    segment = "Logística";
                }
                else if (segment.Contains("Publicidade"))
                {
                    segment = "Publicidade";
                }
                else if (segment.Contains("Livros e Revistas"))
                {
                    segment = "Jornais, Livros e Revistas";
                }
            }

            return segment;
        }

        public async Task<string> GetSegmentFiis(string url)
        {
            string segment = string.Empty;

            try
            {
                var handler = new HttpClientHandler();
                handler.UseCookies = false;
                //handler.Proxy = new WebProxy(string.Format("{0}:{1}", PROXY_IP, "21218"), false);

                using (var httpClient = new HttpClient(handler))
                {
                    using (var request = new HttpRequestMessage(new HttpMethod("GET"), string.Format("https://statusinvest.com.br{0}", url)))
                    {
                        var response = await httpClient.SendAsync(request);

                        if (response.IsSuccessStatusCode)
                        {
                            var bytesContent = await response.Content.ReadAsByteArrayAsync();
                            string responseContent = Encoding.UTF8.GetString(bytesContent, 0, bytesContent.Length - 1);

                            string content = DecodeFromUtf8(responseContent);

                            string term = "Ver fundos imobiliários que fazem parte do segmento ";
                            int index = content.IndexOf(term, StringComparison.InvariantCulture);

                            if (index == -1)
                            {
                                index = responseContent.IndexOf(term, StringComparison.InvariantCulture);
                            }

                            string segmentLine = content.Substring(index, 200);

                            int firstIndex = term.Length;
                            int lastIndex = segmentLine.IndexOf((char)34, StringComparison.InvariantCulture);

                            segment = HttpUtility.HtmlDecode(segmentLine.Substring(term.Length, (lastIndex - firstIndex)));
                            segment = "FII - " + segment;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }

            return segment;
        }

        public async Task<string> GetSegmentStock(string url)
        {
            string segment = string.Empty;

            try
            {
                var handler = new HttpClientHandler();
                handler.UseCookies = false;
                //handler.Proxy = new WebProxy(string.Format("{0}:{1}", PROXY_IP, "21218"), false);

                using (var httpClient = new HttpClient(handler))
                {
                    using (var request = new HttpRequestMessage(new HttpMethod("GET"), string.Format("https://statusinvest.com.br{0}", url)))
                    {
                        var response = await httpClient.SendAsync(request);

                        if (response.IsSuccessStatusCode)
                        {
                            var bytesContent = await response.Content.ReadAsByteArrayAsync();
                            string responseContent = Encoding.UTF8.GetString(bytesContent, 0, bytesContent.Length - 1);

                            string content = DecodeFromUtf8(responseContent);

                            int index = content.IndexOf("SEGMENTO DE", StringComparison.InvariantCulture);

                            string segmentLine = content.Substring(index, 3000);

                            int firstIndex = segmentLine.IndexOf("value", StringComparison.InvariantCulture);
                            int lastIndex = segmentLine.LastIndexOf("</strong>", StringComparison.InvariantCulture);

                            segment = HttpUtility.HtmlDecode(segmentLine.Substring(firstIndex + 7, (lastIndex - firstIndex) - 7));
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }

            if (!string.IsNullOrWhiteSpace(segment))
            {
                if (segment.Contains("Alcoólicas"))
                {
                    segment = "Bebidas Alcoólicas";
                }
                else if (segment.Contains("Análises e Diagnósticos"))
                {
                    segment = "Serviços Médico - Hospitalares, Análises e Diagnósticos";
                }
                else if (segment.Contains("Refino e Distribuição"))
                {
                    segment = "Exploração, Refino e Distribuição";
                }
                else if (segment.Contains("Compressores e Outros"))
                {
                    segment = "Motores, Compressores e Outros";
                }
                else if (segment.Contains("Vestuário e Calçados"))
                {
                    segment = "Tecidos, Vestuário e Calçados";
                }
                else if (segment.Contains("Logística"))
                {
                    segment = "Logística";
                }
                else if (segment.Contains("Publicidade"))
                {
                    segment = "Publicidade";
                }
                else if (segment.Contains("Livros e Revistas"))
                {
                    segment = "Jornais, Livros e Revistas";
                }
            }

            return segment;
        }

        public async Task<string> GetSegmentReits(string url)
        {
            string segment = string.Empty;

            try
            {
                var handler = new HttpClientHandler();
                handler.UseCookies = false;
                //handler.Proxy = new WebProxy(string.Format("{0}:{1}", PROXY_IP, "21218"), false);

                using (var httpClient = new HttpClient(handler))
                {
                    using (var request = new HttpRequestMessage(new HttpMethod("GET"), string.Format("https://statusinvest.com.br{0}", url)))
                    {
                        var response = await httpClient.SendAsync(request);

                        if (response.IsSuccessStatusCode)
                        {
                            var bytesContent = await response.Content.ReadAsByteArrayAsync();
                            string responseContent = Encoding.UTF8.GetString(bytesContent, 0, bytesContent.Length - 1);

                            string content = DecodeFromUtf8(responseContent);

                            int index = content.IndexOf("SEGMENTO DE", StringComparison.InvariantCulture);

                            string segmentLine = content.Substring(index, 3000);

                            int firstIndex = segmentLine.IndexOf("value", StringComparison.InvariantCulture);
                            int lastIndex = segmentLine.LastIndexOf("</strong>", StringComparison.InvariantCulture);

                            segment = HttpUtility.HtmlDecode(segmentLine.Substring(firstIndex + 7, (lastIndex - firstIndex) - 7));
                            segment = "FII - " + segment;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }

            if (!string.IsNullOrWhiteSpace(segment))
            {
                if (segment.Contains("Alcoólicas"))
                {
                    segment = "Bebidas Alcoólicas";
                }
                else if (segment.Contains("Análises e Diagnósticos"))
                {
                    segment = "Serviços Médico - Hospitalares, Análises e Diagnósticos";
                }
                else if (segment.Contains("Refino e Distribuição"))
                {
                    segment = "Exploração, Refino e Distribuição";
                }
                else if (segment.Contains("Compressores e Outros"))
                {
                    segment = "Motores, Compressores e Outros";
                }
                else if (segment.Contains("Vestuário e Calçados"))
                {
                    segment = "Tecidos, Vestuário e Calçados";
                }
                else if (segment.Contains("Logística"))
                {
                    segment = "Logística";
                }
                else if (segment.Contains("Publicidade"))
                {
                    segment = "Publicidade";
                }
                else if (segment.Contains("Livros e Revistas"))
                {
                    segment = "Jornais, Livros e Revistas";
                }
            }

            return segment;
        }


        public static string DecodeFromUtf8(string utf8String)
        {
            // copy the string as UTF-8 bytes.
            byte[] utf8Bytes = new byte[utf8String.Length];
            for (int i = 0; i < utf8String.Length; ++i)
            {
                //Debug.Assert( 0 <= utf8String[i] && utf8String[i] <= 255, "the char must be in byte's range");
                utf8Bytes[i] = (byte)utf8String[i];
            }

            return Encoding.UTF8.GetString(utf8Bytes, 0, utf8Bytes.Length);
        }
    }
}
