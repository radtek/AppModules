using System;
using System.Collections.Generic;
using System.Text;
using Core.SDK.Composite.Common;

namespace Core.SDK.Composite.Event
{
    public interface IEventSubscription
    {
        IdentKey SubscriptionId { get; set; }
       
        Action<object> GetSubscriptionAcion();

        string ActionName { get; }
    }
}
