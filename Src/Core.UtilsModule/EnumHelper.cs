using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Reflection;
using System.Collections;
using System.ComponentModel;

namespace Core.UtilsModule
{    
    public static class EnumHelper
    {     
        public static string GetEnumDescription(this Enum value)
        {
            if (value == null)
            {
                throw new ArgumentNullException("value");
            }

            string description = value.ToString();
            FieldInfo fieldInfo = value.GetType().GetField(description);
            DescriptionAttribute[] attributes = (DescriptionAttribute[])fieldInfo.GetCustomAttributes(typeof(DescriptionAttribute), false);

            if (attributes != null && attributes.Length > 0)
            {
                description = attributes[0].Description;
            }
            return description;
        }

      
        public static IList EnumToIList(this Type type)
        {
            if (type == null)
            {
                throw new ArgumentNullException("type");
            }

            ArrayList list = new ArrayList();
            Array enumValues = Enum.GetValues(type);

            foreach (Enum value in enumValues)
            {
                list.Add(new KeyValuePair<Enum, string>(value, GetEnumDescription(value)));
            }

            return list;
        }

        public static IList<KeyValuePair<Enum, string>> EnumToPairIList(this Type type)
        {
            if (type == null)
            {
                throw new ArgumentNullException("type");
            }

            List<KeyValuePair<Enum, string>> list = new List<KeyValuePair<Enum, string>>();
            Array enumValues = Enum.GetValues(type);

            foreach (Enum value in enumValues)
            {
                list.Add(new KeyValuePair<Enum, string>(value, GetEnumDescription(value)));
            }

            return list;
        }

        public static IList<KeyValuePair<Int32, string>> EnumToPairInt32IList(this Type type)
        {
            if (type == null)
            {
                throw new ArgumentNullException("type");
            }

            List<KeyValuePair<Int32, string>> list = new List<KeyValuePair<Int32, string>>();
            Array enumValues = Enum.GetValues(type);

            foreach (Enum value in enumValues)
            {
                list.Add(new KeyValuePair<Int32, string>(Convert.ToInt32(value), GetEnumDescription(value)));
            }

            return list;
        }
    }
}
