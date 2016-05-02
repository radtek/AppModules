using System;
using Core.SDK.Db;
using Core.SDK.Log;
using Core.SDK.Setting;
using Core.OracleModule.Properties;
using System.Data;
using System.Collections.Generic;

namespace Core.OracleModule.Utils
{ 
    public class OraDBSettingReadWriter : ISettingReadWriter
    {

        public OraDBSettingReadWriter(IDbMgr DBMgr, ILogMgr logMgr)
        {
            _DBMgr = DBMgr;
            _logMgr = logMgr;
            _logger = _logMgr.GetLogger("DBSettingReadWriter");
            CreateDBCommands();
            _settingsList = new List<SettingData>();
        }

        public void Load(string username, string sectionName, string subSectionName)
        {
            ClearLocalData();
            ((OraParamString)_LoadCommand.Params[Settings.Default.UsernameParam]).ParamValue = username;
            ((OraParamString)_LoadCommand.Params[Settings.Default.SectionParam]).ParamValue = sectionName;
            ((OraParamString)_LoadCommand.Params[Settings.Default.SubSectionParam]).ParamValue = subSectionName;           

            _DBMgr.Execute(_LoadCommand);
            DataTable dt = ((OraParamRefCursor)_LoadCommand.Params[Settings.Default.SettingCursorParam]).ParamValue;
            
            foreach (DataRow row in dt.Rows)
            { 
                string name = row[Settings.Default.SettingNameColumn].ToString();
                byte[] blob = row[Settings.Default.SettingValueBLOBColumn] is DBNull? null : (byte[])row[Settings.Default.SettingValueBLOBColumn];
                if (blob != null && blob.Length > 0)
                    _settingsList.Add(new SettingData(name, blob));
                else
                {
                    string value = row[Settings.Default.SettingValueCLOBColumn].ToString();
                    if (string.IsNullOrEmpty(value)) value = row[Settings.Default.SettingValueColumn].ToString();
                    _settingsList.Add(new SettingData(name, value));
                }                
            }
        }        

        public void Save(string username, string sectionName, string subSectionName, bool isCommit)
        {
            if (isCommit)
                using (DbTransaction transaction = new DbTransaction(_DBMgr))
                {
                    SaveInternal(username, sectionName, subSectionName);
                    transaction.Success = true;                    
                }
            else SaveInternal(username, sectionName, subSectionName);
            ClearLocalData();

        }

        public void Clear(string username, string sectionName, bool isCommit)
        {
            if (isCommit)
                using (DbTransaction transaction = new DbTransaction(_DBMgr))
                {
                    ClearInternal(username, sectionName);
                    transaction.Success = true;                    
                }
            else ClearInternal(username, sectionName);
            ClearLocalData();
        }

        public bool ReadValue(string settingName, out string settingValue)
        {
            SettingData setting = FindSettingByName(settingName);
            bool result = false;
            if (setting == null)
            {
                settingValue = string.Empty;
                result = false;
            }
            else if (string.IsNullOrEmpty(setting.Value) && string.IsNullOrEmpty(setting.Clob))
            {
                settingValue = string.Empty;
                result = true;
            }            
            else
            {
                settingValue = string.IsNullOrEmpty(setting.Clob) ? setting.Value : setting.Clob;
                result = true;
            }
            _logger.Debug(string.Format("Read. Find = {0}. {1} = {2}", result, settingName, settingValue));
            return result; 
        }        

        public bool ReadValue(string settingName, out byte[] settingValue)
        {
            SettingData setting = FindSettingByName(settingName);
            bool result = false;
            if (setting == null)
            {
                settingValue = null;
                result = false;
            }
            else if ((setting.Blob == null) || (setting.Blob.Length == 0))
            {
                settingValue = null;
                result = true;
            }
            else
            {
                settingValue = setting.Blob;
                result = true;
            }
            int size = settingValue == null ? 0 : settingValue.Length;
            _logger.Debug(string.Format("Read. Find = {0}. {1} = size({2})", result, settingName, size));
            return result;
        }

        public void WriteValue(string settingName, string settingValue)
        {
            _logger.Debug(string.Format("Write. {0} = {1}", settingName, settingValue));
            _settingsList.Add(new SettingData(settingName, settingValue));
        }

        public void WriteValue(string settingName, byte[] settingValue)
        {
            int size = settingValue == null ? 0 : settingValue.Length;
            _logger.Debug(string.Format("Write. {0} = size({1})", settingName, size));
            _settingsList.Add(new SettingData(settingName, settingValue));
        }

        #region private

        IDbMgr _DBMgr;
        ILogMgr _logMgr;
        ILogger _logger;
        List<SettingData> _settingsList;

        OraCommand _SaveCommand;
        OraCommand _LoadCommand;
        OraCommand _ClearCommand;

        class SettingData
        {
            public SettingData(string name, string value)
            {
                Name = name;
                if (!string.IsNullOrEmpty(value) && value.Length > 1000)
                    Clob = value;
                else Value = value;
            }

            public SettingData(string name, byte[] value)
            {
                Name = name;
                Blob = value;
            }

            public string Name {get; set;}
            public string Value { get; set; }
            public string Clob { get; set; }
            public byte[] Blob { get; set; }
        }

        void CreateDBCommands()
        {
            _LoadCommand = new OraCommand(Core.OracleModule.Properties.Settings.Default.LoadCommand);
            _LoadCommand.CommandType = CommandType.StoredProcedure;
            
            _LoadCommand.AddDBParam(new OraParamString(Settings.Default.UsernameParam, System.Data.ParameterDirection.Input, ""));
            _LoadCommand.AddDBParam(new OraParamString(Settings.Default.SectionParam, System.Data.ParameterDirection.Input, ""));
            _LoadCommand.AddDBParam(new OraParamString(Settings.Default.SubSectionParam, System.Data.ParameterDirection.Input, ""));
            _LoadCommand.AddDBParam(new OraParamRefCursor(Settings.Default.SettingCursorParam, System.Data.ParameterDirection.Output));


            _SaveCommand = new OraCommand(Core.OracleModule.Properties.Settings.Default.SaveCommand);
            _SaveCommand.CommandType = CommandType.StoredProcedure;

            _SaveCommand.AddDBParam(new OraParamString(Settings.Default.UsernameParam, System.Data.ParameterDirection.Input, ""));
            _SaveCommand.AddDBParam(new OraParamString(Settings.Default.SectionParam, System.Data.ParameterDirection.Input, ""));
            _SaveCommand.AddDBParam(new OraParamString(Settings.Default.SubSectionParam, System.Data.ParameterDirection.Input, ""));
            _SaveCommand.AddDBParam(new OraParamString(Settings.Default.SettingNameParam, System.Data.ParameterDirection.Input, ""));
            _SaveCommand.AddDBParam(new OraParamString(Settings.Default.SettingValueParam, System.Data.ParameterDirection.Input, ""));
            _SaveCommand.AddDBParam(new OraParamCLOB(Settings.Default.SettingValueCLOBParam, System.Data.ParameterDirection.Input, ""));
            _SaveCommand.AddDBParam(new OraParamBLOB(Settings.Default.SettingValueBLOBParam, System.Data.ParameterDirection.Input, (byte[])null));

            _ClearCommand = new OraCommand(Core.OracleModule.Properties.Settings.Default.ClearCommand);
            _ClearCommand.CommandType = CommandType.StoredProcedure;

            _ClearCommand.AddDBParam(new OraParamString(Settings.Default.UsernameParam, System.Data.ParameterDirection.Input, ""));
            _ClearCommand.AddDBParam(new OraParamString(Settings.Default.SectionParam, System.Data.ParameterDirection.Input, ""));            
        }

        void ClearLocalData()
        {
            _settingsList.Clear();
        }

        private SettingData FindSettingByName(string settingName)
        {
            foreach (SettingData setting in _settingsList)
            {
                if (setting.Name == settingName) return setting;
            }
            return null;
        }

        void ClearInternal(string username, string sectionName)
        {
            ((OraParamString)_ClearCommand.Params[Settings.Default.UsernameParam]).ParamValue = username;
            ((OraParamString)_ClearCommand.Params[Settings.Default.SectionParam]).ParamValue = sectionName;
            _DBMgr.Execute(_ClearCommand);
        }

        void SaveInternal(string username, string sectionName, string subSectionName)
        {
            foreach (SettingData setting in _settingsList)
            {
                ((OraParamString)_SaveCommand.Params[Settings.Default.UsernameParam]).ParamValue = username;
                ((OraParamString)_SaveCommand.Params[Settings.Default.SectionParam]).ParamValue = sectionName;
                ((OraParamString)_SaveCommand.Params[Settings.Default.SubSectionParam]).ParamValue = subSectionName;
                ((OraParamString)_SaveCommand.Params[Settings.Default.SettingNameParam]).ParamValue = setting.Name;

                if (setting.Blob != null)
                {
                    ((OraParamString)_SaveCommand.Params[Settings.Default.SettingValueParam]).ParamValue = null;
                    ((OraParamCLOB)_SaveCommand.Params[Settings.Default.SettingValueCLOBParam]).ParamValue = null;
                    ((OraParamBLOB)_SaveCommand.Params[Settings.Default.SettingValueBLOBParam]).ParamValue = setting.Blob;
                }
                else if (!string.IsNullOrEmpty(setting.Clob))
                {
                    ((OraParamString)_SaveCommand.Params[Settings.Default.SettingValueParam]).ParamValue = null;
                    ((OraParamCLOB)_SaveCommand.Params[Settings.Default.SettingValueCLOBParam]).ParamValue = setting.Clob;
                    ((OraParamBLOB)_SaveCommand.Params[Settings.Default.SettingValueBLOBParam]).ParamValue = null;
                }
                else
                {
                    ((OraParamString)_SaveCommand.Params[Settings.Default.SettingValueParam]).ParamValue = setting.Value;
                    ((OraParamCLOB)_SaveCommand.Params[Settings.Default.SettingValueCLOBParam]).ParamValue = null;
                    ((OraParamBLOB)_SaveCommand.Params[Settings.Default.SettingValueBLOBParam]).ParamValue = null;
                }

                _DBMgr.Execute(_SaveCommand);
            }
        }

        #endregion private
    }
}
