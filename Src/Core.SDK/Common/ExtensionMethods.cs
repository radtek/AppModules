using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Core.SDK.Common
{
    public static class ExtensionMethods
    {
        public static string ToStringExt(this object obj)
        {
            if (obj == null) return "Null"; else return obj.ToString();
        }

        public static string ToStateString(this object obj)
        {
            if (obj == null) return "Null"; else return "Ok";
        }

        public static bool IsNullOrEmpty(this string str)
        {
            return string.IsNullOrEmpty(str);
        }

        public static bool IsNullOrWhiteSpace(this string str)
        {
            return string.IsNullOrEmpty(str.Trim()); 
        }
    }
}
