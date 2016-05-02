using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Core.SDK.Composite.Event;

namespace ChipAndDale.SDK.Request.EventMessage
{
    public enum RequestChangeType
    { 
        None,
        Create,
        Update,
        Delete
    }

    public class RequestChangedMessage : EventMessageBase
    {
        public RequestChangedMessage(RequestChangeType changeType, RequestEntity request)
        {
            _changeType = changeType;
            _request = request;
        }

        RequestChangeType _changeType;
        public RequestChangeType ChangeType
        {
            get { return _changeType; }
        }

        RequestEntity _request;
        public RequestEntity Request
        {
            get { return _request; }
        }
    }
}
