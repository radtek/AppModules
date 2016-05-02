using System.Data;
using System.Data.Common;

namespace Core.SDK.Db
{
    public enum DbType
    {
        Int32,
        Double,
        String,
        DateTime,
        CLOB,
        BLOB,
        RefCursor
    }

    public interface IDbParam
    {
        string ParamName { get; set; }

        DbType ParamType { get; }

        ParameterDirection ParamDirection { get; set; }

        int ParamSize { get; set; }

        object GetValue();

        //TVal GetValue<TVal>();

        void FromDBParam(DbParameter param);

        DbParameter ToDBParam();
    }
}
