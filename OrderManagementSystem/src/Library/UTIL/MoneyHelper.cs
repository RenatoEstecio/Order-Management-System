using System;
using System.Collections.Generic;
using System.Text;

namespace Library.UTIL
{
    public static class MoneyHelper
    {
        public static decimal Round(decimal value)
        {
            return Math.Round(
                value,
                2,
                MidpointRounding.AwayFromZero);
        }
    }
}
