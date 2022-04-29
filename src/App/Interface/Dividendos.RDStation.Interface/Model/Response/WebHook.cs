using Newtonsoft.Json;
using System;
using System.Collections.Generic;

namespace Dividendos.RDStation.Interface.Model.Response
{
    public class LegalBas
    {
        public string category { get; set; }
        public string type { get; set; }
        public string status { get; set; }
    }

    public class Company
    {
        public string name { get; set; }
    }

    public class Funnel
    {
        public string name { get; set; }
        public string lifecycle_stage { get; set; }
        public bool opportunity { get; set; }
        public string contact_owner_email { get; set; }
        public int interest { get; set; }
        public int fit { get; set; }
        public string origin { get; set; }
    }

    public class Contact
    {
        public string uuid { get; set; }
        public string email { get; set; }
        public string name { get; set; }
        public string job_title { get; set; }
        public string bio { get; set; }
        public string website { get; set; }
        public string personal_phone { get; set; }
        public string mobile_phone { get; set; }
        public string city { get; set; }
        public string facebook { get; set; }
        public string linkedin { get; set; }
        public string twitter { get; set; }
        public List<string> tags { get; set; }
        public List<string> cf_custom_field_example { get; set; }
        public List<LegalBas> legal_bases { get; set; }
        public Company company { get; set; }
        public Funnel funnel { get; set; }
    }

    public class RootWebHook
    {
        public string event_type { get; set; }
        public string entity_type { get; set; }
        public string event_identifier { get; set; }
        public DateTime timestamp { get; set; }
        public DateTime event_timestamp { get; set; }
        public Contact contact { get; set; }
    }
}
