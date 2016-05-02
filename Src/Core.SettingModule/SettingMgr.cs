using System;
using System.Reflection;
using System.ComponentModel;

using Core.SDK.Setting;
using Core.SDK.Log;
using Core.SDK.Setting.Attributes;
using System.Collections;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using System.Globalization;
using System.Drawing;
using Core.UtilsModule;

namespace Core.SettingModule
{
    public class SettingMgr : ISettingMgr
    {
        public SettingMgr(ILogMgr logMgr)
        {
            _logMgr = logMgr;
            _logger = _logMgr.GetLogger("SettingMgr");
            _Culture = CultureInfo.CreateSpecificCulture("en-En");
            _logger.Info("Create.");
        }
       
        public ISettingReadWriter ReadWriteProvider
        {
            get
            {
                return _ReadWriteProvider;
            }
            set
            {
                _ReadWriteProvider = value;
            }
        }

        public void LoadSetting(string userName, string sectionName, string subSectionName, object settings)
        {
            string log = string.Format("Load settings. UserName = {0}; Section = {1}; SubSection = {2};", userName, sectionName, subSectionName);
            _logger.Debug(log);

            CheckArguments(userName, sectionName, subSectionName, settings);

            ReadWriteProvider.Load(userName.ToUpper(), sectionName.ToUpper(), subSectionName.ToUpper());

            foreach (PropertyInfo property in settings.GetType().GetProperties(BindingFlags.Instance | BindingFlags.Public))
            {
                if (IsIgnorAttributeProperty(property)) continue;

                Type propertyType = property.PropertyType;                
                if (!IsSupportForConvertType(propertyType)) continue;

                LoadValue(settings, property, propertyType);
            }
        }

        public void SaveSettings(string userName, string sectionName, string subSectionName, object settings)
        {
            SaveSetting(userName, sectionName, subSectionName, settings, true);
        }

        public void SaveSetting(string userName, string sectionName, string subSectionName, object settings, bool isCommit)
        {
            string log = string.Format("Save settings. UserName = {0}; Section = {1}; SubSection = {2};", userName, sectionName, subSectionName);
            _logger.Debug(log);

            CheckArguments(userName, sectionName, subSectionName, settings);            

            foreach (PropertyInfo property in settings.GetType().GetProperties(BindingFlags.Instance | BindingFlags.Public))
            {
                Type propertyType = property.PropertyType;

                if (!IsSupportForConvertType(propertyType)) continue;

                SaveValue(settings, property, propertyType);
            }

            ReadWriteProvider.Save(userName.ToUpper(), sectionName.ToUpper(), subSectionName.ToUpper(), isCommit);            
        }

        public void ClearSettings(string userName, string sectionName, bool isCommit) 
        {
            ReadWriteProvider.Clear(userName, sectionName, isCommit);
        }

        public void LoadSettingProperty(string userName, string sectionName, string subSectionName, object settings, string settingName)
        { 
            throw new NotSupportedException();
        }
        public void SaveSettingProperty(string userName, string sectionName, string subSectionName, object settings, string settingName)
        {
            throw new NotSupportedException();
        }



        #region private

        ILogMgr _logMgr;
        ILogger _logger;
        ISettingReadWriter _ReadWriteProvider;
        CultureInfo _Culture;
        readonly string _separator = "#@#";
        string _sectionName;
        string _subSectionName;
        string _userName;

        void CheckArguments(string username, string sectionName, string subSectionName, object settings)
        {
            _userName = username;
            _sectionName = sectionName;
            _subSectionName = subSectionName;
            if (_ReadWriteProvider == null) throw new InvalidOperationException("ReadWriteProvider must be set.");
            if (Hlp.IsNullOrWhiteSpace(sectionName)) throw new ArgumentException("Section's name must be set.");
            if (Hlp.IsNullOrWhiteSpace(subSectionName)) throw new ArgumentException("Subsection's name must be set.");
            if (settings == null) throw new ArgumentNullException("Setting class can not be null.");            
        }

        private void LoadValue(object settings, PropertyInfo property, Type propertyType)
        {
            try
            {
                if (propertyType == typeof(byte[]))
                {
                    byte[] b = null;
                    if (ReadWriteProvider.ReadValue(property.Name, out b))
                        if (b != null) property.SetValue(settings, b, null);
                        else SetNullValue(property, settings);
                    else SetNullValue(property, settings);
                }
                else
                {
                    string strValue = "";
                    if (ReadWriteProvider.ReadValue(property.Name, out strValue) == true)
                        if (!string.IsNullOrEmpty(strValue))
                        {
                            if (IsGenericListType(propertyType))
                                SetListFromString(settings, property, propertyType, strValue);
                            else
                            {
                                object val = TypeDescriptor.GetConverter(propertyType).ConvertFromString(null, _Culture, strValue);
                                property.SetValue(settings, val, null);
                            }
                        }
                        else SetNullValue(property, settings);
                    else SetNullValue(property, settings);
                }
            }
            catch (Exception e)
            {
                _logger.Error(e);
                throw;
            }
        }        

        private void SaveValue(object settings, PropertyInfo property, Type propertyType)
        {
            try
            {
                if (propertyType == typeof(byte[]))
                {
                    byte[] b = (byte[])property.GetValue(settings, null);
                    ReadWriteProvider.WriteValue(property.Name, b);
                }
                else
                {
                    string strValue = "";
                    if (property.GetValue(settings, null) == null)
                        strValue = "";
                    else if (IsGenericListType(propertyType))
                    {
                        strValue = GetStringFromList(settings, property, propertyType, strValue);
                    }
                    else
                        strValue = ConvertToString(propertyType, property.GetValue(settings, null));

                    ReadWriteProvider.WriteValue(property.Name, strValue);
                }
            }
            catch (Exception e)
            {
                _logger.Error(e);
                throw;
            }
        }        

        string ConvertToString(Type type, object value)
        {
            return TypeDescriptor.GetConverter(type).ConvertToString(null, _Culture, value);
        }

        T ConvertFromString<T>(T type, string value)
        {
            return (T)TypeDescriptor.GetConverter(type).ConvertFromString(null, _Culture, value);
        }       

        void SetNullValue(PropertyInfo property, object settings)
        {
            if (ExistNotChangedIfNullAttribute(property)) return;           

            if (ExistDefaultValueAttribute(property))
            {
                SetToDefaultValue(property, settings);
                return;
            }

            if (ExistEmptyValueAttribute(property))
            {
                SetToEmptyValue(property, settings);
                return;
            }

            Type propertyType = property.PropertyType;

            if (IsNullable(propertyType))
                property.SetValue(settings, null, null);
            else if (!HasDefaultCtor(propertyType))
            {
                _logger.Warn(string.Format("Process NullToEmpty attribute. Type {0} has no default ctor. Set to null.", propertyType));
                property.SetValue(settings, null, null);
            }
            else if (propertyType.IsValueType)
                property.SetValue(settings, Activator.CreateInstance(propertyType), null);
            else property.SetValue(settings, null, null);
        }

        private void SetToEmptyValue(PropertyInfo property, object settings)
        {            
            Type propertyType = property.PropertyType;
            if (propertyType == typeof(string))
                property.SetValue(settings, string.Empty, null);
            else if (propertyType == typeof(byte[]))
                property.SetValue(settings, new byte[] {}, null);
            else if (propertyType == typeof(Font))
                property.SetValue(settings, new Font("Arial", 12), null);
            else if (!propertyType.IsValueType && !HasDefaultCtor(propertyType))
            {
                _logger.Warn(string.Format("Process NullToEmpty attribute. Type {0} has no default ctor. Set to null.", propertyType));
                property.SetValue(settings, null, null);
            }
            else
                property.SetValue(settings, Activator.CreateInstance(propertyType), null);
        }

        private bool HasDefaultCtor(Type propertyType)
        {
            ConstructorInfo[] ctors = propertyType.GetConstructors();
            foreach (ConstructorInfo ctor in ctors)
            { 
                ParameterInfo[] p =  ctor.GetParameters();
                if (p.Length == 0) return true;
            }
            return false;
        }

        private void SetToDefaultValue(PropertyInfo property, object settings)
        {
            NullToDefaultAttribute attribute = GetAttribute<NullToDefaultAttribute>(property);
            Type propertyType = property.PropertyType;
            if (propertyType == typeof(byte[]))
                property.SetValue(settings, attribute.ByteArrayDefValue, null);
            else if (IsGenericListType(propertyType))
                SetListFromString(settings, property, propertyType, attribute.StringDefValue);
            else
            {
                object val = TypeDescriptor.GetConverter(propertyType).ConvertFromString(null, _Culture, attribute.StringDefValue);
                property.SetValue(settings, val, null);
            }
                
        }

        private bool IsIgnorAttributeProperty(PropertyInfo property)
        {           
            return IsExistAttribute<IgnoreAttribute>(property);
        }

        private bool ExistNotChangedIfNullAttribute(PropertyInfo property)
        {
            return IsExistAttribute<NullNotChangedAttribute>(property);
        }

        private bool ExistDefaultValueAttribute(PropertyInfo property)
        {
            return IsExistAttribute<NullToDefaultAttribute>(property);
        }

        private bool ExistEmptyValueAttribute(PropertyInfo property)
        {
            return IsExistAttribute<NullToEmptyAttribute>(property);
        }

        private bool IsExistAttribute<T>(PropertyInfo property) where T : Attribute
        {
            T[] attributeArray = (T[])property.GetCustomAttributes(typeof(T), false);
            return (attributeArray != null && attributeArray.Length != 0);
        }

        private T GetAttribute<T>(PropertyInfo property) where T : Attribute
        {
            T[] attributeArray = (T[])property.GetCustomAttributes(typeof(T), false);
            if (attributeArray != null && attributeArray.Length != 0)
                return attributeArray[0];
            else return null;
        }

        bool IsSupportForConvertType(Type type)
        {
            if ((type == typeof(byte[])) ||
                IsSupportedBaseType(type) ||
                IsNullable(type, true) ||
                IsGenericListType(type, true)) return true;
            else
            {
                _logger.Warn("Unsupported type " + type.FullName + " (" + _sectionName + "/" + _subSectionName + ").");
                return false;
            }
        }

        bool IsSupportedBaseType(Type type)
        {
            if ((type == typeof(string)) ||
                (type == typeof(Int16)) ||
                (type == typeof(Int32)) ||
                (type == typeof(Int64)) ||
                (type == typeof(float)) ||
                (type == typeof(double)) ||
                (type == typeof(decimal)) ||
                (type == typeof(bool)) ||
                (type == typeof(DateTime)) ||
                (type == typeof(TimeSpan)) ||
                (type == typeof(Size)) ||
                (type == typeof(Point)) ||
                (type == typeof(Color)) ||
                (type == typeof(Byte)) ||
                (type == typeof(Font)) || 
                (type == typeof(Guid)) || 
                type.IsEnum)
            {
                return true;
            }
            return false;
        }

        bool IsNullable(Type type)
        {
            return IsNullable(type, false);
        }

        bool IsNullable(Type type, bool checkParamType)
        {
            Type t = Nullable.GetUnderlyingType(type);
            if (t == null) return false;

            if (checkParamType) return IsSupportedBaseType(t);
            else return true;
        }

        bool IsGenericListType(Type type)
        {
            return IsGenericListType(type, false);
        }

        bool IsGenericListType(Type type, bool checkParamType)
        {
            if (!type.IsGenericType) return false;
           
            Type[] interfaces = type.GetInterfaces();

            Type[] paramTypes = type.GetGenericArguments();
            if (paramTypes.Length > 1) return false;

            bool result = false;
            foreach (Type t in interfaces)
            {
                if (t.Name.StartsWith("IList"))
                {                    
                    result = true;
                    break;
                }
            }

            if (checkParamType) return (result && (IsSupportedBaseType(paramTypes[0])) || IsNullable(paramTypes[0], true));
            else return result;
        }

        private void SetListFromString(object settings, PropertyInfo property, Type propertyType, string strValue)
        {
            Type genericParamType = propertyType.GetGenericArguments()[0];            
            Type newGenericInstance = typeof(List<>).MakeGenericType(genericParamType);
            IList list = Activator.CreateInstance(newGenericInstance) as IList;

            string[] values = strValue.Split(new string[] { _separator }, StringSplitOptions.None);
            foreach (string val in values)
            {
                list.Add(TypeDescriptor.GetConverter(genericParamType).ConvertFromString(null, _Culture, val));
            }
            property.SetValue(settings, list, null);
        }

        private string GetStringFromList(object settings, PropertyInfo property, Type propertyType, string strValue)
        {
            IList list = property.GetValue(settings, null) as IList;
            foreach (object val in list)
            {
                strValue = strValue + ConvertToString(propertyType.GetGenericArguments()[0], val) + _separator;
            }
            strValue = Regex.Replace(strValue, _separator + "$", "");
            return strValue;
        }
        #endregion private
    }
}
