using System;
using System.Collections.Generic;
using System.Text;

namespace Dividendos.API.Model.Response
{
    public class AdvertiserVM
    {
        public string AdvertiserID { get; set; }

        public string Text { get; set; }

        public string BackGroundColor { get; set; }

        public string FontColor { get; set; }

        public bool HasLinkToDetails { get; set; }

        public bool Active { get; set; }

        public Guid AdvertiserGuid { get; set; }
    }
}
