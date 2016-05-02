using System;
using System.Collections.Generic;
using Core.SDK.Log;
using Core.SDK.Composite.Common;

namespace Core.SDK.Composite.Event
{
    public abstract class EventBase
    {
        private readonly List<IEventSubscription> _subscriptions = new List<IEventSubscription>();

        ILogMgr _logMgr;
        ILogger _logger;
        public ILogMgr LogMgr
        {
            protected get { return _logMgr; }
            set 
            { 
                _logMgr = value;
                _logger = _logMgr.GetLogger("Event " + this.GetType().FullName);
            }
        }

        protected ILogger Logger
        {
            get { return _logger; }
            private set { _logger = value; }
        }
       
        protected ICollection<IEventSubscription> Subscriptions
        {
            get { return _subscriptions; }
        }
        
        protected virtual IdentKey InternalSubscribe(IEventSubscription eventSubscription)
        {
            eventSubscription.SubscriptionId = new IdentKey();
            lock (Subscriptions)
            {
                Action<object> act = eventSubscription.GetSubscriptionAcion();                
                Subscriptions.Add(eventSubscription);                
            }
            return eventSubscription.SubscriptionId;
        }

        protected virtual void InternalPublish(object arguments)
        {
            List<Action<object>> executionStrategies = GetListOfSubscriptionAcion();
            foreach (var executionStrategy in executionStrategies)
            {
                executionStrategy(arguments);
            }
        }

        protected virtual void InternalPublishNoEx(object arguments)
        {
            List<Action<object>> executionStrategies = GetListOfSubscriptionAcion();
            foreach (var executionStrategy in executionStrategies)
            {
                try { executionStrategy(arguments); }
                catch (Exception ex) { Logger.Warn(ex); }
            }
        }
        
        public virtual void Unsubscribe(IdentKey id)
        {
            lock (Subscriptions)
            {                
                foreach(IEventSubscription subscription in Subscriptions)
                    if (subscription.SubscriptionId == id)
                {
                    Subscriptions.Remove(subscription);
                    Logger.Debug(string.Format("Unsubscribed by SubscriptionId {0}", subscription == null ? "null" : subscription.ActionName));
                    return;
                }
            }
        }
        
        public virtual bool Contains(IdentKey id)
        {
            lock (Subscriptions)
            {
                foreach (IEventSubscription subscription in Subscriptions)
                    if (subscription.SubscriptionId == id) return true;                 
            }
            return false;
        }

        private List<Action<object>> GetListOfSubscriptionAcion()
        {
            List<Action<object>> returnList = new List<Action<object>>();

            lock (Subscriptions)
            {
                for (var i = Subscriptions.Count - 1; i >= 0; i--)
                {
                    Action<object> listItem = _subscriptions[i].GetSubscriptionAcion();

                    if (listItem == null)
                    {                       
                        _subscriptions.RemoveAt(i);
                    }
                    else
                    {
                        returnList.Add(listItem);
                    }
                }
            }

            return returnList;
        }
    }
}