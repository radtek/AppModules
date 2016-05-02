using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Core.SDK.Composite.Event.EventMessage
{    
    public class DbCommandExecutedMessage : EventMessageBase
    {
        public DbCommandExecutedMessage(string commandText, bool isSuccess)
        {
            _isSuccess = isSuccess;
            _commandText = commandText;
        }

        bool _isSuccess;
        public bool IsSuccess
        {
            get { return _isSuccess; }
        }

        string _commandText;
        public string CommandText
        {
            get { return _commandText; }
        }
    }
}
