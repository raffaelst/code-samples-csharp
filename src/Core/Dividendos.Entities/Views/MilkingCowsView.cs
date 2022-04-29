using System;
using System.ComponentModel.DataAnnotations;

namespace Dividendos.Entity.Entities
{
    public class MilkingCowsView
    {

        public int Position { get; set; }


        public string Symbol { get; set; }


        public string Name { get; set; }

        public string Logo { get; set; }

        public string CurrentPrice { get; set; }

        public string Yield { get; set; }
    }
}