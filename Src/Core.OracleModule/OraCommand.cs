using System.Collections.Generic;
using Core.SDK.Db;
using System.Text;
using System;

namespace Core.OracleModule
{
    public sealed class OraCommand : IDbCommand
    {
        public OraCommand(string commandText)
        {
            _ParamDict = new Dictionary<string, IDbParam>();
            CommandText = commandText;
            CommandType = System.Data.CommandType.Text;
        }

        Dictionary<string, IDbParam> _ParamDict;
        public Dictionary<string, IDbParam> Params
        {
            get { return _ParamDict; }
            set { _ParamDict = value; }
        }

        public void AddDBParam(IDbParam param)
        {
            Params.Add(param.ParamName, param);
        }

        string _CommandText;
        public string CommandText
        {
            get { return _CommandText; }
            set { _CommandText = value; }
        }

        System.Data.CommandType _CommandType;
        public System.Data.CommandType CommandType
        {
            get { return _CommandType; }
            set { _CommandType = value; }
        }

        public override string ToString()
        {
            StringBuilder text = new StringBuilder();
            text.AppendFormat("Command text: {0}", CommandText);

            foreach (KeyValuePair<string, IDbParam> param in Params)
            {
                text.AppendFormat("{0}    {1}", Environment.NewLine, (param.Value == null) ? "ERROR: NULL value IDBParam" : param.Value.ToString());
            }
            return text.ToString();
        } 
    }
}
