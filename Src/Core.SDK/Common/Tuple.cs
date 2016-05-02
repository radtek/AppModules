using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Core.SDK.Common
{
    public class Tuple<T>
    {
        public Tuple(T t)
        {
            Item1 = t;
        }

        public T Item1 { get; set; }
    }

    public class Tuple<T1, T2>
    {
        public Tuple(T1 t1, T2 t2)
        {
            Item1 = t1;
            Item2 = t2;
        }

        public T1 Item1 { get; set; }
        public T2 Item2 { get; set; }
    }

    public class Tuple<T1, T2, T3>
    {
        public Tuple(T1 t1, T2 t2, T3 t3)
        {
            Item1 = t1;
            Item2 = t2;
            Item3 = t3;
        }

        public T1 Item1 { get; set; }
        public T2 Item2 { get; set; }
        public T3 Item3 { get; set; }
    }
}
