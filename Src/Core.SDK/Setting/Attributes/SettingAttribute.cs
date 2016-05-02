using System;
using System.Collections.Generic;
using System.Text;

namespace Core.SDK.Setting.Attributes
{
    public class IgnoreAttribute : Attribute
    { }

    public class NullToEmptyAttribute : Attribute
    { }

    public class NullNotChangedAttribute : Attribute
    { }

    public class NullToDefaultAttribute : Attribute
    {
        public NullToDefaultAttribute(string defValue)
        {
            _strDrfValue = defValue;
        }

        public NullToDefaultAttribute(byte[] defValue)
        {
            _byteDefValue = defValue;
        }

        public string StringDefValue
        {
            get { return _strDrfValue; }
        }

        public byte[] ByteArrayDefValue
        {
            get { return _byteDefValue; }
        }

        string _strDrfValue;
        byte[] _byteDefValue;
    }
}
