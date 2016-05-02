using System;
using System.Collections.Generic;
using System.Text;

namespace Core.SDK.Composite.Event
{
    public interface IEventMgr
    {        
        TEventType GetEvent<TEventType>() where TEventType : EventBase;
    }
}
