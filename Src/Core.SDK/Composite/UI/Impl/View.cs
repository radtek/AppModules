using System;
using System.Collections.Generic;
using System.Text;
using Core.SDK.Common;
using System.Drawing;
using Core.SDK.Composite.Common;
using System.Windows.Forms;

namespace Core.SDK.Composite.UI
{
    public class View : ViewBase<object>
    {
        public View(object control)
            : this(string.Empty, control, string.Empty, null, null, null, string.Empty, null, ViewDockingState.Right, ViewDockingStateHelper.All)
        { }

        public View(object control, string caption)
            : this(string.Empty, control, caption, null, null, null, string.Empty, null, ViewDockingState.Right, ViewDockingStateHelper.All)
        { }

        public View(object control, OnClosingDelegate onClosingHandler)
            : this(string.Empty, control, string.Empty, onClosingHandler, null, null, string.Empty, null, ViewDockingState.Right, ViewDockingStateHelper.All)
        { }
        
        public View(object control, string caption, OnClosingDelegate onClosingHandler)
            : this(string.Empty, control, caption, onClosingHandler, null, null, string.Empty, null, ViewDockingState.Right, ViewDockingStateHelper.All)
        { }

        public View(object control, string caption, OnClosingDelegate onClosingHandler, ActivateDelegate onActivateHandler)
            : this(string.Empty, control, caption, onClosingHandler, onActivateHandler, null, string.Empty, null, ViewDockingState.Right, ViewDockingStateHelper.All)
        { }

        public View(string name, object control, string caption, OnClosingDelegate onClosingHandler, ActivateDelegate onActivateHandler, BooleanDelegate canCloseHandler, string hint, Image image, ViewDockingState dockingState, ViewDockingStateFlagExt allowDockingStates)
            : base(name, control, caption, hint, image)
        {            
            _onCloseHandler = onClosingHandler;
            _canCloseHandler = canCloseHandler;
            _onActivateHandler = onActivateHandler;                                 
        }        

        public override bool OnClosing(Nullable<bool> result)
        {
            if (_onCloseHandler != null) return _onCloseHandler.Invoke(result);
            else return true;
        }

        public override void OnAfterClose()
        { }

        public override bool CanClose
        {
            get
            {
                if (_canCloseHandler == null) return true;
                else return _canCloseHandler.Invoke();
            }
        }

        public override void OnActivate(bool isActive)
        {
            if (_onActivateHandler != null) _onActivateHandler.Invoke(isActive);
        }

        public override void ChangeDockingState(ViewDockingState dockOption)
        {}        

        #region private
        
        BooleanDelegate _canCloseHandler;
        OnClosingDelegate _onCloseHandler;
        ActivateDelegate _onActivateHandler;                

        #endregion private
    }
}
