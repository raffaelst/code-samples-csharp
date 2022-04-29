using Dapper.Contrib.Extensions;
using System;

namespace Dividendos.Entity.Entities
{
    public class OperationView
    {

        public long IdOperation { get; set; }

        public bool Selected { get; set; }
        public string Symbol { get; set; }

        public string FullName { get; set; }

        public string Logo { get; set; }

    }
}
