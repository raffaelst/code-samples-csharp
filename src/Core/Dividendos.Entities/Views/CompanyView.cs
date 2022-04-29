using System;
using System.Collections.Generic;
using System.Text;

namespace Dividendos.Entity.Views
{
    public class CompanyView
    {
        public long IdStock { get; set; }
        public string Company { get; set; }
        public string Symbol { get; set; }
        public string Logo { get; set; }
        public string Segment { get; set; }
    }
}
