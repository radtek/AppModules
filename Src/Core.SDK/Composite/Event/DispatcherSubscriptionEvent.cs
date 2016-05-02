using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using Core.SDK.Composite.Common;

namespace Core.SDK.Composite.Event
{
    public class DispatcherEventSubscription<T> : EventSubscription<T>
    {
        SynchronizationContext _context = null;
       
        public DispatcherEventSubscription(IDelegateReference actionReference, IDelegateReference filterReference, SynchronizationContext context)
            : base(actionReference, filterReference)
        {
            _context = context;
        }

        public override void InvokeAction(Action<T> action, T argument)
        {
            if (_context != null)
            {
                SendOrPostCallback callback = new SendOrPostCallback(arg => action(argument));
                _context.Post(callback, null);
            }
            else
            {
                action.Invoke(argument);
            }
        }
    }
}
