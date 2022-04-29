using Newtonsoft.Json;
using System;
using System.Collections.Generic;

namespace Dividendos.RDStation.Interface.Model.Request
{

    public class Payload
    {
        public string conversion_identifier { get; set; }
        public string name { get; set; }
        public string email { get; set; }
        public string client_tracking_id { get; set; }
        public string traffic_source { get; set; }
        public string mobile_phone { get; set; }
        public List<string> tags { get; set; }
        public bool available_for_mailing { get; set; }
    }

    public class Root
    {
        public string event_type { get; set; }
        public string event_family { get; set; }
        public Payload payload { get; set; }
    }
}
