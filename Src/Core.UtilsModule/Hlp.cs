using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Core.SDK.Dom;
using Core.SDK.Interface;

namespace Core.UtilsModule
{
    public static class Hlp
    {
        public static string ToString(object o)
        {
            if (o == null) return "Null"; else return o.ToString();
        }

        public static string BoolToNumsStr(bool? b)
        {
            if (b == true) return "1"; else return "0";
        }

        public static string BoolToStr(bool? b)
        {
            if (b == true) return "true"; else return "false";
        }

        public static string GetExceptionText(Exception ex)
        {
            if (ex == null) return string.Empty;

            StringBuilder sb = new StringBuilder();
            sb.AppendLine(ex.Message);
            sb.AppendLine("-------------------------");
            sb.AppendLine("Stack: ");
            sb.AppendLine(ex.StackTrace);
            sb.AppendLine("-------------------------");
            Exception innEx = ex.InnerException;
            while (innEx != null)
            {
                sb.AppendLine("InnerException: ");
                sb.AppendLine(innEx.Message);
                sb.AppendLine("Stack: ");
                sb.AppendLine(innEx.StackTrace);
                sb.AppendLine("-------------------------");
                innEx = innEx.InnerException;
            }
            return sb.ToString();
        }

        public static byte[] CopyByteAray(byte[] bytes)
        {
            if (bytes == null) return null;

            if (bytes.Length == 0) return new byte[0];

            byte[] result = new byte[bytes.Length];
            Buffer.BlockCopy(bytes, 0, result, 0, result.Length);
            return result;
        }

        public static bool EqualsByteAray(byte[] bytes1, byte[] bytes2)
        {
            if (bytes1 == null && bytes2 == null) return true;

            if (bytes1 != null) return bytes1.SequenceEqual(bytes2);
            else return false;
        }

        public static bool IsNullOrWhiteSpace(string str)
        {
            return (string.IsNullOrEmpty(str) || (string.Equals(str.Replace(" ", string.Empty), string.Empty)));
        }

        public static bool EqualsList<T>(List<T> listA, List<T> listB)
        {
            if (object.Equals(listA, listB)) return true;
            if (listA != null && listA.Except(listB).Count() > 0) return false;
            if (listB != null && listB.Except(listA).Count() > 0) return false;
            return true;
        }
        public static bool EqualsBindingList<T>(BindingCollection<T> listA, BindingCollection<T> listB)
        {
            if (object.Equals(listA, listB)) return true;
            if (listA != null && listA.Except(listB).Count() > 0) return false;
            if (listB != null && listB.Except(listA).Count() > 0) return false;
            return true;
        }

        public static bool EqualsFilter<T>(T t1, T t2) where T : class
        {
            if (t2 == null) return true;
            if (t1 == null) return false;

            return t1.Equals(t2);
        }

        public static bool FullEqualsFilter<T>(T t1, T t2) where T : class
        {
            if (t2 == null) return true;
            if (t1 == null) return false;

            IFullEquatable<T> obj = t1 as IFullEquatable<T>;
            if (obj != null) return obj.FullEquals(t2);
            else throw new InvalidCastException("Can not cast to IFullEquatable<T>."); 
        }
    }
}
