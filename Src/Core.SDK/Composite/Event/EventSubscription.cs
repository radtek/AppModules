using System;
using System.Collections.Generic;
using System.Text;
using System.Globalization;
using Core.SDK.Composite.Common;

namespace Core.SDK.Composite.Event
{
    public class EventSubscription<T> : IEventSubscription
    {
        private readonly IDelegateReference _actionReference;
        private readonly IDelegateReference _filterReference;
        private readonly string _actionName;
        
        public EventSubscription(IDelegateReference actionReference, IDelegateReference filterReference)
        {
            if (actionReference == null)
                throw new ArgumentNullException("actionReference");
            if (!(actionReference.Target is Action<T>))
                throw new ArgumentException("Argument must be Action<T> type", "actionReference");

            if (filterReference == null)
                throw new ArgumentNullException("filterReference");
            if (!(filterReference.Target is Predicate<T>))
                throw new ArgumentException("Argument must be Predicate<T> type", "filterReference");

            _actionReference = actionReference;
            _filterReference = filterReference;
            _actionName = ((Action<T>)_actionReference.Target).Method.Name;
        }
       
        public Action<T> Action
        {
            get { return (Action<T>)_actionReference.Target; }
        }
        
        public Predicate<T> Filter
        {
            get { return (Predicate<T>)_filterReference.Target; }
        }

        public string ActionName
        {
            get { return _actionName; }
        }
       
        public IdentKey SubscriptionId { get; set; }


        public virtual Action<object> GetSubscriptionAcion()
        {
            Action<T> action = this.Action; 
            Predicate<T> filter = this.Filter;
            if (action != null && filter != null)
            {
                return arg =>
                {
                    T argument = default(T);
                    if (arg != null)
                    {
                        argument = (T)arg;
                    }
                    if (filter(argument))
                    {
                        InvokeAction(action, argument);
                    }
                };
            }
            return null;
        }
       
        public virtual void InvokeAction(Action<T> action, T argument)
        {
            action(argument);
        }
    }
}
