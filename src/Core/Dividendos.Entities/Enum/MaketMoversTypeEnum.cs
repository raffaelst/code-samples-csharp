using Dapper.Contrib.Extensions;
using System;

namespace Dividendos.Entity.Enum
{
    public enum MaketMoversTypeEnum
    {
        BiggestHighsStocksBR = 1,
        BiggestHighsUS = 2,
        BiggestFallsStocksBR = 3,
        BiggestFallsUS = 4,
        BiggestHighsFIIsBR = 5,
        BiggestFallsFIIsBR = 6,
        TopDividendPaidStocksBR = 7,
        TopDividendPaidUS = 8,
        TopDividendYieldStocksBR = 9,
        TopDividendYieldUS = 10,
        TopDividendPaidFIIsBR = 11,
        TopDividendYieldFIIsBR = 12,
        BiggestHighsStocksBRAll = 13,
        BiggestFallsStocksBRAll = 14,
        China = 15,
        BiggestHighsBDR = 16,
        BiggestFallsBDR = 17,
        TopDividendPaidREITsEUA = 18,
        TopDividendYieldREITsEUA = 19,
    }
}
