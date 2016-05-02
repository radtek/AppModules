using System;
using System.Data;

namespace Core.SDK.Db
{
    public interface IDbCommand
    {
        void AddDBParam(IDbParam param);
        string CommandText { get; set; }
        CommandType CommandType { get; set; }
        System.Collections.Generic.Dictionary<string, IDbParam> Params { get; set; }
    }
}
