using System;
using System.Collections.Generic;
using System.Text;

namespace Core.SDK.Composite.Event
{   
    public enum ThreadOption
    {        
        // The call is done on the same thread on which the event was published.        
        PublisherThread,        
        // The call is done on the UI thread.
        UIThread,
        // The call is done asynchronously on a background thread.        
        BackgroundThread
    }
}
