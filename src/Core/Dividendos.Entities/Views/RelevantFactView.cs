using Dapper.Contrib.Extensions;
using System;

namespace Dividendos.Entity.Entities
{
    public class RelevantFactView
    {
        public string URL { get; set; }

        public string CompanyName { get; set;}

        public DateTime ReferenceDate { get; set; }
    }
}
