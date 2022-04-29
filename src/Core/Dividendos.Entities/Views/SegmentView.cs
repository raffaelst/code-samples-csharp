using System;
using System.Collections.Generic;
using System.Text;

namespace Dividendos.Entity.Views
{
    public class SegmentView
    {
        public long IdSegment { get; set; }
        public long IdSubsector { get; set; }

        public string Name { get; set; }
        public Guid GuidSegment { get; set; }
        public decimal TotalMarket { get; set; }
    }
}
