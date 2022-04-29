using Dapper.Contrib.Extensions;
using System;

namespace Dividendos.Entity.Enum
{
    //1	A??es
    //2	Fundos Imobili?rios
    //3	ETFs

    public enum StockTypeEnum
    {
        Stocks = 1,
        FII = 2,
        ETF = 3,
        BDR = 4,
        REIT = 5,
        FIP = 6
    }
}
