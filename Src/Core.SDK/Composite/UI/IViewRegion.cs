using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Core.SDK.Composite.Common;

namespace Core.SDK.Composite.UI
{
    public interface IViewRegion : IRegion
    {
        IEnumerable<IView> Views { get; }
        IEnumerable<IView> ActiveViews { get; }
        IdentKey AddView(IView view);
        IdentKey AddView(IView view, ViewDockingState state);
        IdentKey AddView(IView view, ViewDockingState state, ViewDockingStateFlagExt allowStates);
        void RemoveView(IdentKey key);
        void ActivateView(IdentKey key);
        void RestoreLayout(byte[] bytes);
        byte[] SaveLayout();
    }
}
