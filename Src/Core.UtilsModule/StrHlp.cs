using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Core.UtilsModule
{
    public static class StrHlp
    {
        public static bool Contains(string str1, string str2)
        {
            if (string.IsNullOrEmpty(str2)) return true;
            if (string.IsNullOrEmpty(str1)) return false;

            return str1.Contains(str2);
        }

        public static bool ContainsNoCase(string str1, string str2)
        {
            if (string.IsNullOrEmpty(str2)) return true;
            if (string.IsNullOrEmpty(str1)) return false;

            return str1.ToUpper().Contains(str2.ToUpper());
        }

        public static bool StartsWith(string str1, string str2)
        {
            if (string.IsNullOrEmpty(str2)) return true;
            if (string.IsNullOrEmpty(str1)) return false;

            return str1.StartsWith(str2);
        }

        public static bool StartsWithNoCase(string str1, string str2)
        {
            if (string.IsNullOrEmpty(str2)) return true;
            if (string.IsNullOrEmpty(str1)) return false;

            return str1.ToUpper().StartsWith(str2.ToUpper());
        }
    }
}
