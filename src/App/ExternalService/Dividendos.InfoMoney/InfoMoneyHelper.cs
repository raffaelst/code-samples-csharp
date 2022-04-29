using Dividendos.InfoMoney.Interface;
using Dividendos.InfoMoney.Interface.Model;
using Newtonsoft.Json;
using System;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;

namespace Dividendos.InfoMoney
{
    public class InfoMoneyHelper : IInfoMoneyHelper
    {
        private const string PROXY_IP = "10.20.31.150";

        public RelevantFactInfoMoney ImportRelevantFacts(string nonce)
        {
            RelevantFactInfoMoney relevantFactInfoMoney = null;

            //HttpClientHandler httpClientHandler = new HttpClientHandler()
            //{
            //    Proxy = new WebProxy(string.Format("{0}:{1}", PROXY_IP, "21218"), false)
            //};

            using (var httpClient = new HttpClient())
            {
                using (var request = new HttpRequestMessage(new HttpMethod("POST"), "https://www.infomoney.com.br/wp-admin/admin-ajax.php"))
                {
                    request.Headers.TryAddWithoutValidation("accept", "application/json, text/javascript, */*; q=0.01");
                    //request.Headers.TryAddWithoutValidation("accept-encoding", "gzip, deflate, br");
                    request.Headers.TryAddWithoutValidation("accept-language", "en-US,en;q=0.9");
                    request.Headers.TryAddWithoutValidation("content-length", "75");
                    request.Headers.TryAddWithoutValidation("origin", "https://www.infomoney.com.br");
                    request.Headers.TryAddWithoutValidation("referer", "https://www.infomoney.com.br/ferramentas/fatos-relevantes/");
                    request.Headers.TryAddWithoutValidation("sec-ch-ua", "\" Not A;Brand\";v=\"99\", \"Chromium\";v=\"98\", \"Google Chrome\";v=\"98\"");
                    request.Headers.TryAddWithoutValidation("sec-ch-ua-mobile", "?0");
                    request.Headers.TryAddWithoutValidation("sec-ch-ua-platform", "\"macOS\"");
                    request.Headers.TryAddWithoutValidation("sec-fetch-dest", "empty");
                    request.Headers.TryAddWithoutValidation("sec-fetch-mode", "cors");
                    request.Headers.TryAddWithoutValidation("sec-fetch-site", "same-origin");
                    request.Headers.TryAddWithoutValidation("user-agent", "Mozilla/5.0 (Macintosh; Intel Mac OS X 10_15_7) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/98.0.4758.80 Safari/537.36");
                    request.Headers.TryAddWithoutValidation("x-requested-with", "XMLHttpRequest");

                    request.Content = new StringContent(string.Format("action=tool_fatos_relevantes&pagination=1&fatos_relevantes_nonce={0}", nonce));
                    request.Content.Headers.ContentType = MediaTypeHeaderValue.Parse("application/x-www-form-urlencoded; charset=UTF-8");

                    var response = httpClient.SendAsync(request).Result;

                    if (response.IsSuccessStatusCode)
                    {
                        relevantFactInfoMoney = JsonConvert.DeserializeObject<RelevantFactInfoMoney>(response.Content.ReadAsStringAsync().Result);
                    }
                }
            }

            return relevantFactInfoMoney;
        }
    }
}
