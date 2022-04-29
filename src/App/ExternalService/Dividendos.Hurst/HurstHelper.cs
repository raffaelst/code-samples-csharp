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
using Dividendos.Hurst.Interface;
using Dividendos.Hurst.Interface.Model.Request;

namespace Dividendos.Hurst
{
    public class HurstHelper : IHurstHelper
    {
        HttpClient _httpClient;

        public HurstHelper(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public void SendEvent(string name, string email, string phoneNumber)
        {
            Thread thread = new Thread(() => SendEventBase(name, email, phoneNumber));
            thread.Start();
        }

        private void SendEventBase(string name, string email, string phoneNumber)
        {
            try
            {
                if (!string.IsNullOrEmpty(email))
                {
                    using (var request = new HttpRequestMessage(new HttpMethod("POST"), "https://api-hci.hurst.capital/v1/partners/leads"))
                    {
                        Root root = new Root();
                        Data data = new Data();
                        data.email = email;
                        data.name = name;
                        data.phone = phoneNumber;
                        data.tag = "app_dividendos_cpa";
                        data.source = "App Dividendos.me";
                        root.data = data;
                        request.Headers.TryAddWithoutValidation("x-api-key", "EPOP15P8ws7b94KkQIWFW4pVhi8nk8194sQigQf6");

                        request.Content = new StringContent(JsonConvert.SerializeObject(root));
                        request.Content.Headers.ContentType = MediaTypeHeaderValue.Parse("application/json");

                        var response = _httpClient.SendAsync(request).Result;
                    }
                }
            }
            catch (Exception ex)
            {

            }
        }
    }
}
