using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Core.UtilsModule;

namespace Core.XtraCompositeModule.Utils
{
    public class EnumFormatter<T> : IFormatProvider, ICustomFormatter
    {
        public EnumFormatter()
        {
            _enumDescs = typeof(T).EnumToPairIList();
        }

        public object GetFormat(Type formatType)
        {
            if (formatType == typeof(ICustomFormatter))
                return this;
            else
                return null;
        }
        public string Format(string format, object arg, IFormatProvider formatProvider)
        {
            int x = (int)arg;
            return _enumDescs[x].Value;
        }

        IList<KeyValuePair<Enum, string>> _enumDescs;
    }
}
