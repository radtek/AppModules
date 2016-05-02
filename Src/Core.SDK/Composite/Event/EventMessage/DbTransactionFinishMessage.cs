using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Core.SDK.Composite.Event.EventMessage
{
    public enum TransactionResult
    { 
        None,
        Commit,
        SavePoint,
        Rollback,
        RollbackToSavePoint
    }

    public class DbTransactionFinishMessage : EventMessageBase
    {
        public DbTransactionFinishMessage(TransactionResult result)
        {
            _transactionResult = result;
        }

        TransactionResult _transactionResult;
        public TransactionResult Result
        {
            get { return _transactionResult; }
        }
    }
}
