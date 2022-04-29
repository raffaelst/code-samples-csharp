using Dividendos.Entity.Entities;
using Dividendos.Repository.Interface.UoW;
using Dividendos.Service;
using System;
using System.Collections.Generic;
using System.Text;
using Xunit;

namespace Dividendos.UnityTest
{
    public class TraderTest
    {
        [Fact]
        public async void Save()
        {
            Trader trader = new Trader();
            trader.Identifier = "31171035896s";


        }
    }
}
