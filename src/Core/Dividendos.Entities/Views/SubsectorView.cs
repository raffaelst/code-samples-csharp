using System;
using System.Collections.Generic;
using System.Text;

namespace Dividendos.Entity.Views
{
    public class SubsectorView
    {
        public long IdSubsector { get; set; }
        public long IdSector { get; set; }
        public string Name { get; set; }
        public Guid GuidSubsector { get; set; }
        public decimal TotalMarket { get; set; }
    }
}
