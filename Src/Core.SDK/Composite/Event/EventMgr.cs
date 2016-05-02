using System;
using System.Collections.Generic;
using System.Text;
using Core.SDK.Log;

namespace Core.SDK.Composite.Event
{
    public class EventMgr : IEventMgr
    {        
        public EventMgr(ILogMgr logMgr)
        {
            _logMgr = logMgr;
            _logger = _logMgr.GetLogger("EventMgr");           
            _logger.Info("Create.");
        }


        public TEventType GetEvent<TEventType>() where TEventType : EventBase
        {
            TEventType eventInstance = null;
            foreach (EventBase eventBase in _events)
            {
                if (eventBase.GetType() == typeof(TEventType))
                {
                    eventInstance = eventBase as TEventType;
                    break;
                }
            }                    
            if (eventInstance == null)
            {
                eventInstance = Activator.CreateInstance<TEventType>();
                eventInstance.LogMgr = _logMgr;
                _events.Add(eventInstance);
            }
            return eventInstance;
        }

        #region private
        private readonly List<EventBase> _events = new List<EventBase>();
        ILogMgr _logMgr;
        ILogger _logger;
        #endregion private
    }
}
