using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Core.SDK.Composite.Event.EventMessage
{
    public class DbConnectionChangedEvent: CompositeEvent<DbConnectionChangedMessage>
    { }

    public class DbTransactionFinishEvent : CompositeEvent<DbTransactionFinishMessage>
    { }

    public class DbCommandExecutedEvent : CompositeEvent<DbCommandExecutedMessage>
    { }
}
