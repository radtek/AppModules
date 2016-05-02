using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Core.UtilsModule
{
    public static class DateHlp
    {
        public static DateTime GetDateTime_hhmm()
        {
            return new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day, DateTime.Now.Hour, DateTime.Now.Minute, 0);
        }

        public static DateTime GetDateTime_hhmmss()
        {
            return new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day, DateTime.Now.Hour, DateTime.Now.Minute, DateTime.Now.Second);
        }        
        
        public static bool Between(DateTime? date, DateTime? dateStart, DateTime? dateEnd)
        {
            if (!date.HasValue) return false;
            if (!dateStart.HasValue) return false;

            return (dateStart.Value <= date && (!dateEnd.HasValue || dateEnd >= date));
        }
    }
}
