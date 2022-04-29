using Dapper.Contrib.Extensions;
using Dividendos.Entity.Enum;
using System;

namespace Dividendos.Entity.Views
{
    public class InsightView
    {
        public string Position { get; set; }

        public string Description { get; set; }

        public string LogoURL { get; set; }
    }
}
