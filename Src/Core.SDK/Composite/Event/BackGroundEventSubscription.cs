using System;
using System.Collections.Generic;
using System.Text;
using System.ComponentModel;
using Core.SDK.Composite.Common;

namespace Core.SDK.Composite.Event
{
    public class BackgroundEventSubscription<T> : EventSubscription<T>
    {        
        public BackgroundEventSubscription(IDelegateReference actionReference, IDelegateReference filterReference)
            : base(actionReference, filterReference)
        {
        }

        public override void InvokeAction(Action<T> action, T argument)
        {
            BackgroundWorker worker = new BackgroundWorker();
            worker.DoWork += (sender, e) => action((T)e.Argument);
            
            worker.RunWorkerAsync(argument);
        }
    }
}
