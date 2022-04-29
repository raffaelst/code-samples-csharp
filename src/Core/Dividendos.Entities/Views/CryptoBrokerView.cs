using System;
using System.ComponentModel.DataAnnotations;

namespace Dividendos.Entity.Entities
{
    public class CryptoBrokerView
    {
        public Guid GuidTrader { get; set; }

        public string Name { get; set; }
    }
}