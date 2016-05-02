using System;
using Microsoft.Win32;
using System.Windows.Forms;

using Core.SDK.Setting;

namespace Core.SettingModule
{
    public class RegSettingReadWriter : ISettingReadWriter
    {
        public RegSettingReadWriter()
        {
            _regKey = Application.UserAppDataRegistry;        
        }

        public RegSettingReadWriter(RegistryKey key)
        {
            _regKey = key;
        }

        RegistryKey _regKey;

        public void Load(string username, string sectionName, string subSectionName)
        { }
        public void Save(string username, string sectionName, string subSectionName)
        { }
        public void Save(string username, string sectionName, string subSectionName, bool isCommit)
        { }
        public void Clear(string username, string sectionName, bool isCommit)
        { }

        public bool ReadValue(string settingName, out string settingValue)
        {
            settingValue = _regKey.GetValue(settingName).ToString();
            return true;
        }

        public bool ReadValue(string settingName, out byte[] settingValue)
        {
            settingValue = (byte[])_regKey.GetValue(settingName);
            return true;
        }

        public void WriteValue(string settingName, string settingValue)
        {
            //if (_regKey.i) _regKey.DeleteValue(settingName);
            _regKey.SetValue(settingName, settingValue);
        }

        public void WriteValue(string settingName, byte[] settingValue)
        {
            _regKey.DeleteValue(settingName);
            _regKey.SetValue(settingName, settingValue);
        }
    }
}
