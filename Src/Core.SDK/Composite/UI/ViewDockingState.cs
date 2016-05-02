using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Core.SDK.Composite.UI
{
    [Flags]
    public enum ViewDockingStateFlagExt
    { 
        None,
        Left,
        Top,
        Right,
        Bottom,
        Fill,
        Dock,
        Float,
        Close,
        Activate,
        Hide
    }

    public enum ViewDockingState
    {
        None,
        Left,
        Top,
        Right,
        Bottom,
        Fill,
        Float,
        Hide
    }

    public static class ViewDockingStateHelper
    {
        public static ViewDockingStateFlagExt All = ViewDockingStateFlagExt.Left | ViewDockingStateFlagExt.Top | ViewDockingStateFlagExt.Right | ViewDockingStateFlagExt.Bottom | ViewDockingStateFlagExt.Fill | ViewDockingStateFlagExt.Float
                | ViewDockingStateFlagExt.Fill | ViewDockingStateFlagExt.Dock | ViewDockingStateFlagExt.Close | ViewDockingStateFlagExt.Hide | ViewDockingStateFlagExt.Activate;
    }
}
