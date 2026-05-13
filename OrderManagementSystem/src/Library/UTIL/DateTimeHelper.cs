using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Text;

namespace Library.UTIL
{
    public static class DateTimeHelper
    {
        public static DateTime ToSaoPaulo(DateTime? utcDate = null)
        {
            if (utcDate == null)
                utcDate = DateTime.UtcNow;

            var tz = TimeZoneInfo.FindSystemTimeZoneById(
                RuntimeInformation.IsOSPlatform(OSPlatform.Windows)
                    ? "E. South America Standard Time"
                    : "America/Sao_Paulo");

            return TimeZoneInfo.ConvertTimeFromUtc(utcDate.Value, tz);
        }
    }
}
