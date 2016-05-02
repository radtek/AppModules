using System;

namespace Core.SDK.Db
{
    public class DbTransaction : IDisposable
    {        
        public DbTransaction(IDbMgr dbManager)
        {
            _DBManager = dbManager;
            Success = false;
            _DBManager.StartTransaction();
        }

        public void Dispose()
        {
            if (Success)
                _DBManager.CommitTransaction();
            else
                _DBManager.RollbackTransaction();
        }

        public bool Success { get; set; }

                
        private IDbMgr _DBManager;
    }
}
