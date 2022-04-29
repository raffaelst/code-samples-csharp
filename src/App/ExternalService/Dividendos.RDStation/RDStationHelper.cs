using Dividendos.RDStation.Config;
using Dividendos.RDStation.Interface;
using Dividendos.RDStation.Interface.Model;
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
using Dividendos.RDStation.Interface.Model.Response;
using Dividendos.RDStation.Interface.Model.Request;

namespace Dividendos.RDStation
{
    public class RDStationB3Helper : IRDStationHelper
    {
        string _urlBase;
        string _apiKey;
        HttpClient _httpClient;

        public RDStationB3Helper(string urlBase, string apiKey, HttpClient httpClient)
        {
            _urlBase = urlBase;
            _apiKey = apiKey;
            _httpClient = httpClient;
        }

        public void SendEvent(string name, string email, string userId, List<string> tags, string phoneNumber, EventType eventType)
        {
            Thread thread = new Thread(() => SendEventBase(name, email, userId, tags, eventType, phoneNumber, null));
            thread.Start();
        }

        public void SendEvent(string name, string email, string userId, List<string> tags, string customEventType)
        {
            Thread thread = new Thread(() => SendEventBase(name, email, userId, tags, null, null, customEventType));
            thread.Start();
        }

        private void SendEventBase(string name, string email, string userId, List<string> tags, EventType? eventType, string phoneNumber, string customEventType)
        {
            try
            {
                if (!string.IsNullOrEmpty(email))
                {
                    using (var request = new HttpRequestMessage(new HttpMethod("POST"), string.Format("{0}platform/conversions?api_key={1}", _urlBase, _apiKey)))
                    {
                        Root root = new Root();
                        root.event_type = "CONVERSION";
                        root.event_family = "CDP";
                        Payload payload = new Payload();
                        payload.available_for_mailing = true;
                        payload.tags = tags;
                        payload.mobile_phone = phoneNumber;
                        //payload.client_tracking_id = userId;
                        payload.name = name;
                        payload.email = email;
                        if (customEventType != null)
                        {
                            payload.conversion_identifier = customEventType;
                        }
                        else
                        {
                            payload.conversion_identifier = eventType.ToString();
                        }
                        root.payload = payload;
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
