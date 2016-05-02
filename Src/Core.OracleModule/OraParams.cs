using System;
using Oracle.DataAccess.Client;
using Oracle.DataAccess.Types;
using System.IO;
using Core.SDK.Db;

namespace Core.OracleModule
{    
    public abstract class OraParamBase<T> : IDbParam
    {
        public OraParamBase(string name, DbType type, System.Data.ParameterDirection direction, T value, int size)
        {
            ParamName = name;
            ParamType = type;
            ParamDirection = direction;
            ParamValue = value;
            ParamSize = size;
        }

        string _Name;
        public string ParamName
        {
            get { return _Name; }
            set { _Name = value; }
        }

        DbType _Type;
        public DbType ParamType
        {            
            get { return _Type; }
            private set { _Type = value; }
        }

        System.Data.ParameterDirection _Direction;
        public System.Data.ParameterDirection ParamDirection
        {
            get { return _Direction; }
            set { _Direction = value; }
        }        

        T _Value;
        public T ParamValue 
        {
            get { return _Value; }
            set { _Value = value; }
        }

        int _Size;
        public int ParamSize
        {
            get { return _Size; }
            set { _Size = value; }
        }

        public void FromDBParam(System.Data.Common.DbParameter param)
        {
            if (ParamDirection == System.Data.ParameterDirection.Input) return;

            OracleParameter oraParam = param as OracleParameter;
            FromOracleParamInternal(oraParam);
        }

        protected abstract void FromOracleParamInternal(OracleParameter param);


        public System.Data.Common.DbParameter ToDBParam()
        {
            OracleParameter oraParam = new OracleParameter();
            oraParam.ParameterName = ParamName;
            oraParam.Direction = ParamDirection;
            oraParam.Size = ParamSize;
            ToOracleParamInternal(oraParam);
            return oraParam;
        }

        protected abstract void ToOracleParamInternal(OracleParameter param);

        public object GetValue()
        {
            return ParamValue;
        }

        /*public TVal GetValue<TVal>()
        {
            if (typeof(TVal) == typeof(T))
            return (TVal)ParamValue;
        }*/

        protected bool IsNull(object val)
        { 
            INullable nullVal = val as INullable;
            return nullVal.IsNull;
        }

        protected abstract string TypeToString();
        protected abstract string ValueToString();
        
        public override string ToString()
        {
            string name = string.IsNullOrEmpty(ParamName) ? "EmptyName" : ParamName;
            string direction = DirectionToString(ParamDirection);
            string typeName = TypeToString();
            string value = ValueToString();
            return string.Format("{0} ({1} {2}) = {3};", name, direction, typeName, value);
        }

        private string DirectionToString(System.Data.ParameterDirection direction)
        {
            string text = "";
            switch (direction)
            {
                case System.Data.ParameterDirection.Input:
                    text = "In";
                    break;
                case System.Data.ParameterDirection.Output:
                    text = "Out";
                    break;
                case System.Data.ParameterDirection.InputOutput:
                    text = "In Out";
                    break;
                case System.Data.ParameterDirection.ReturnValue:
                    text = "Return";
                    break;
                default:
                    text = "Unknown";
                    break;
            }
            return text;
        }

        protected const string NullStringConst = "NULL";
    }

    public sealed class OraParamInt32 : OraParamBase<Int32?>
    {
        public OraParamInt32(string name, System.Data.ParameterDirection direction, Int32? value) : base(name, DbType.Int32, direction, value, 0) { }

        protected override void ToOracleParamInternal(OracleParameter param)
        {
            param.OracleDbType = OracleDbType.Int32;
            param.Value = ParamValue;
        }

        protected override void FromOracleParamInternal(OracleParameter param)
        {
            if (IsNull(param.Value) == true) ParamValue = null;
            else ParamValue = Convert.ToInt32(param.Value.ToString());
        }

        protected override string TypeToString()
        {
            return "Int32";
        }

        protected override string ValueToString()
        {
            return ParamValue.HasValue ? ParamValue.Value.ToString() : NullStringConst;
        }
    }

    public sealed class OraParamDouble : OraParamBase<Double?>
    {
        public OraParamDouble(string name, System.Data.ParameterDirection direction, Double? value) : base(name, DbType.Double, direction, value, 0) { }

        protected override void ToOracleParamInternal(OracleParameter param)
        {
            param.OracleDbType = OracleDbType.Double;
            param.Value = ParamValue;
        }

        protected override void FromOracleParamInternal(OracleParameter param)
        {
            if (IsNull(param.Value) == true) ParamValue = null;
            else ParamValue = Double.Parse(param.Value.ToString(), System.Globalization.NumberStyles.Integer | System.Globalization.NumberStyles.AllowDecimalPoint,
                                           System.Globalization.CultureInfo.InvariantCulture);                
        }

        protected override string TypeToString()
        {
            return "Double";
        }

        protected override string ValueToString()
        {
            return ParamValue.HasValue ? ParamValue.Value.ToString() : NullStringConst;
        }
    }

    public sealed class OraParamDateTime : OraParamBase<DateTime?>
    {
        public OraParamDateTime(string name, System.Data.ParameterDirection direction, DateTime? value) : base(name, DbType.DateTime, direction, value, 0) { }

        protected override void ToOracleParamInternal(OracleParameter param)
        {
            param.OracleDbType = OracleDbType.Date;
            param.Value = ParamValue;
        }

        protected override void FromOracleParamInternal(OracleParameter param)
        {
            if (IsNull(param.Value) == true) ParamValue = null;
            else 
            {
                OracleDate date = (OracleDate)param.Value;
                ParamValue = date.Value;
            }
        }

        protected override string TypeToString()
        {
            return "DateTime";
        }

        protected override string ValueToString()
        {
            return ParamValue.HasValue ? ParamValue.Value.ToString("dd.MM.yyyy HH:mm:ss") : NullStringConst;
        }
    }

    public sealed class OraParamString : OraParamBase<string>
    {
        public OraParamString(string name, System.Data.ParameterDirection direction, string value) : base(name, DbType.String, direction, value, 0) { }

        public OraParamString(string name, System.Data.ParameterDirection direction, string value, int size) : base(name, DbType.String, direction, value, size) { }

        protected override void ToOracleParamInternal(OracleParameter param)
        {
            param.OracleDbType = OracleDbType.Varchar2;
            param.Value = ParamValue;
        }

        protected override void FromOracleParamInternal(OracleParameter param)
        {
            if (IsNull(param.Value) == true) ParamValue = null;
            else ParamValue = param.Value.ToString();
        }

        protected override string TypeToString()
        {
            return "String";
        }

        protected override string ValueToString()
        {
            return string.IsNullOrEmpty(ParamValue) ? NullStringConst : ParamValue;
        }
    }

    public sealed class OraParamCLOB : OraParamBase<string>
    {
        public OraParamCLOB(string name, System.Data.ParameterDirection direction, string value) : base(name, DbType.CLOB, direction, value, 0) { }

        protected override void ToOracleParamInternal(OracleParameter param)
        {
            param.OracleDbType = OracleDbType.Clob;
            param.Value = ParamValue;
        }

        protected override void FromOracleParamInternal(OracleParameter param)
        {
            if (IsNull(param.Value) == true) ParamValue = null;
            else
            {
                OracleClob clob = (OracleClob)param.Value;
                ParamValue = clob.Value;                        
            }
        }

        protected override string TypeToString()
        {
            return "CLOB";
        }

        protected override string ValueToString()
        {
            return string.IsNullOrEmpty(ParamValue) ? NullStringConst : ParamValue;
        }
    }

    public sealed class OraParamBLOB : OraParamBase<byte[]>
    {
        public OraParamBLOB(string name, System.Data.ParameterDirection direction, byte[] value)
            : base(name, DbType.BLOB, direction, value, 0) 
        {
            ParamSize = value==null? 0 : value.Length;
        }

        /// <summary>
        /// Attention! In this procedure stream don't dispose - it must be done in up-level procedure
        /// </summary>
        public OraParamBLOB(string name, System.Data.ParameterDirection direction, Stream stream)
            : base(name, DbType.BLOB, direction, null, 0) 
        {
            if (stream == null) throw new ArgumentNullException("stream", "Param can not be null");           

            int size = (int)stream.Length;
            byte[] data = new byte[stream.Length];
            stream.Read(data, 0, size);

            ParamValue = data;
            ParamSize = size;
        }

        protected override void ToOracleParamInternal(OracleParameter param)
        {
            param.OracleDbType = OracleDbType.Blob;
            param.Value = ParamValue;            
        }

        protected override void FromOracleParamInternal(OracleParameter param)
        {
            if (IsNull(param.Value) == true) ParamValue = null;
            else
            {
                OracleBlob blob = (OracleBlob)param.Value;
                ParamValue = blob.Value;
            }
        }

        protected override string TypeToString()
        {
            return "BLOB";
        }

        protected override string ValueToString()
        {
            return ParamSize == 0 ? NullStringConst : string.Format("byte[{0}]", ParamSize);
        }
    }

    public sealed class OraParamRefCursor : OraParamBase<System.Data.DataTable>
    {
        public OraParamRefCursor(string name, System.Data.ParameterDirection direction)
            : base(name, DbType.RefCursor, direction, null, 0) 
        {
            ParamValue = new System.Data.DataTable();
        }

        protected override void ToOracleParamInternal(OracleParameter param)
        {
            param.OracleDbType = OracleDbType.RefCursor;            
        }

        protected override void FromOracleParamInternal(OracleParameter param)
        {
            if (IsNull(param.Value) == true)
                ParamValue.Clear();
            else
            {
                ParamValue.Clear();
                OracleRefCursor cursor = (OracleRefCursor)param.Value;
                OracleDataReader reader = cursor.GetDataReader();
                ParamValue.Load(reader);
            }
        }

        protected override string TypeToString()
        {
            return "RefCursor";
        }

        protected override string ValueToString()
        {
            return "";
        }
    }

}
