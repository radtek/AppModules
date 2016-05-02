using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Core.SDK.Dom
{
    public class EntityIdComparer<T> : IEqualityComparer<T> where T : EntityBase<T>
    {
        public bool Equals(T x, T y)
        {
            if (x == null || y == null) return false;
            else return string.Equals(x.Id, y.Id);
        }

        public int GetHashCode(T obj)
        {
            return obj.Id.GetHashCode();
        }
    }

    public class EntityFullComparer<T> : IEqualityComparer<T> where T : EntityBase<T>
    {
        public bool Equals(T x, T y)
        {
            if (x == null || y == null) return false;
            else return x.FullEquals(y);
        }

        public int GetHashCode(T obj)
        {
            string s = string.Format("{0}-{1}", obj.Id, obj.ToString());
            return s.GetHashCode();
        }
    }
}
