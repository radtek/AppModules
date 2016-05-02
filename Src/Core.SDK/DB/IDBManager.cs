using System.Data;

namespace Core.SDK.Db
{
    public interface IDbMgr
    {        
        void Execute(IDbCommand command);
        DataTable ExecuteSelect(IDbCommand command);

        void StartTransaction();
        void CommitTransaction();
        void RollbackTransaction();

        CheckConnectionMode CheckConnectionMode { get; set; }

        //void SetModulInfo(string name, string action);
    }

    public enum CheckConnectionMode
    {
        NoCheck,
        CheckAndReconnect
    }
}
