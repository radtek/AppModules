using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using Core.SDK.Composite.Common;

namespace Core.SDK.Composite.Event
{
    public class CompositeEvent<T> : EventBase
    {       
        public IdentKey Subscribe(Action<T> action)
        {
            return Subscribe(action, ThreadOption.PublisherThread);             
        }
       
        public IdentKey Subscribe(Action<T> action, ThreadOption threadOption)
        {
            return Subscribe(action, threadOption, false);
        }

        public IdentKey Subscribe(Action<T> action, bool keepSubscriberReferenceAlive)
        {
            return Subscribe(action, ThreadOption.PublisherThread, keepSubscriberReferenceAlive);
        }

       
        public IdentKey Subscribe(Action<T> action, ThreadOption threadOption, bool keepSubscriberReferenceAlive)
        {
            return Subscribe(action, threadOption, keepSubscriberReferenceAlive, null);
        }

       
        public virtual IdentKey Subscribe(Action<T> action, ThreadOption threadOption, bool keepSubscriberReferenceAlive, Predicate<T> filter)
        {
            IDelegateReference actionReference = new DelegateReference(action, keepSubscriberReferenceAlive);
            IDelegateReference filterReference;
            if (filter != null)
            {
                filterReference = new DelegateReference(filter, keepSubscriberReferenceAlive);
            }
            else
            {
                filterReference = new DelegateReference(new Predicate<T>(delegate { return true; }), true);
            }
            EventSubscription<T> subscription;
            switch (threadOption)
            {
                case ThreadOption.PublisherThread:
                    subscription = new EventSubscription<T>(actionReference, filterReference);
                    break;
                case ThreadOption.BackgroundThread:
                    subscription = new BackgroundEventSubscription<T>(actionReference, filterReference);
                    break;
                case ThreadOption.UIThread:
                    subscription = new DispatcherEventSubscription<T>(actionReference, filterReference, SynchronizationContext.Current);
                    break;
                default:
                    subscription = new EventSubscription<T>(actionReference, filterReference);
                    break;
            }
            Logger.Debug(string.Format("Subscribed by {0}.{1} [{2}]", action.Method.Module.Name, action.Method.Name, threadOption.ToString()));
            return base.InternalSubscribe(subscription);
        }


        public virtual void Publish(T arg)
        {
            Logger.Debug(string.Format("Publish {0} = {1}", typeof(T).Name, arg == null ? "null" : arg.ToString()));
            base.InternalPublish(arg);
        }

        public virtual void PublishNoEx(T arg)
        {
            Logger.Debug(string.Format("Publish {0} = {1}", typeof(T).Name, arg == null ? "null" : arg.ToString()));
            base.InternalPublishNoEx(arg);
        }

        public virtual void Unsubscribe(Action<T> subscriber)
        {            
            lock (Subscriptions)
            {
                Logger.Debug(string.Format("Unsubscribed {0} {1}", subscriber == null ? "null" : subscriber.Method.Module.Name, subscriber == null ? "null" : subscriber.Method.Name));
                foreach (IEventSubscription s in Subscriptions)
                {
                    EventSubscription<T> subscription = s as EventSubscription<T>;
                    if ((subscription != null) && (subscription.Action == subscriber))
                    {
                        Subscriptions.Remove(s);
                        break;
                    }
                }               
            }            
        }

        public virtual bool Contains(Action<T> subscriber)
        {            
            lock (Subscriptions)
            {
                foreach (IEventSubscription s in Subscriptions)
                {
                    EventSubscription<T> subscription = s as EventSubscription<T>;
                    if ((subscription != null) && (subscription.Action == subscriber))
                    {
                        return true;
                    }
                }  
            }
            return false;
        }

    }
}
