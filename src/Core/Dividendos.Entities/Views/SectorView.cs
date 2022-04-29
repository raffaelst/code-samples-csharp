using System;
using System.Collections.Generic;
using System.Text;

namespace Dividendos.Entity.Views
{
    public class SectorView
    {
        public long IdSector { get; set; }

        public string Name { get; set; }
        public Guid GuidSector { get; set; }
        public decimal TotalMarket { get; set; }
        public int IdCountry { get; set; }
    }
}
