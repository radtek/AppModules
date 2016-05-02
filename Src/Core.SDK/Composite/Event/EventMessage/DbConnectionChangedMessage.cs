using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;

namespace Core.SDK.Composite.Event.EventMessage
{
    public class DbConnectionChangedMessage : EventMessageBase
    {
        public DbConnectionChangedMessage(ConnectionState prevState, ConnectionState newState)
        {
            _prevState = prevState;
            _newState = newState;
        }

        ConnectionState _prevState;
        public ConnectionState PrevConnectionState
        {
            get { return _prevState; }
        }

        ConnectionState _newState;
        public ConnectionState NewConnectionState
        {
            get { return _newState; }
        }
    }
}
