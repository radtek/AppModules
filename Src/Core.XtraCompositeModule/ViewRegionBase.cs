using System;
using System.Collections.Generic;
using Core.SDK.Composite.UI;
using DevExpress.XtraBars.Docking2010;
using DevExpress.XtraBars.Docking;
using Core.SDK.Composite.Common;
using Core.SDK.Log;
using System.Windows.Forms;

namespace Core.XtraCompositeModule
{
    public abstract class ViewRegionBase : IViewRegion
    {
        public ViewRegionBase(string regionName, DocumentManager documentManager, DockManager dockManager, ILogMgr logMgr)
        {
            _regionName = regionName;
            _logMgr = logMgr;
            _logger = _logMgr.GetLogger(_regionName);            
            _documentManager = documentManager;
            _dockManager = dockManager;
            _logger.Info("Create.");

            if (string.IsNullOrEmpty(regionName)) throw new ArgumentNullException("RegionName can not be null or empty.");
            if (documentManager == null) throw new ArgumentNullException("DocumentManager  can not be null.");
            if (dockManager == null) throw new ArgumentNullException("dockManager  can not be null.");
            if (logMgr == null) throw new ArgumentNullException("logMgr can not be null.");
            
            _views = new Dictionary<IdentKey, IView>();
            _activeViews = new Dictionary<IdentKey, IView>();
        }
        
        public string Name
        {
            get { return _regionName; }
        }

        public IEnumerable<IView> Views
        {
            get { return _views.Values; }
        }

        public IEnumerable<IView> ActiveViews
        {
            get { return _activeViews.Values; }
        }

        public abstract IdentKey AddView(IView view, ViewDockingState state, ViewDockingStateFlagExt allowStates);
        public abstract IdentKey AddView(IView view, ViewDockingState state);
        public virtual IdentKey AddView(IView view)
        {
            Control control = view.UserControl as Control;
            if (control == null) throw new InvalidCastException("IView.Control must be inherited from Control class");
            _logger.Debug("Add " + view.ToString());
            return new IdentKey();
        }

        public virtual void RemoveView(IdentKey key)
        {
            if (key == null) throw new ArgumentNullException("Key can not be null.");

            IView view = null;
            if (!_views.TryGetValue(key, out view)) throw new InvalidOperationException("View with such key is not find.");

            if (!view.CanClose) throw new InvalidOperationException("View with such key has false CanClose property.");

            _logger.Debug("Remove " + view.ToString());
        }

        public virtual void ActivateView(IdentKey key)
        {
            if (key == null) throw new ArgumentNullException("Key can not be null.");            

            IView view = null;
            if (!_views.TryGetValue(key, out view)) throw new InvalidOperationException("View with such key is not find.");

            _logger.Debug("Activate " + view.ToString());       
        }

        public abstract void RestoreLayout(byte[] bytes);
        public abstract byte[] SaveLayout();

        #region impl

        string _regionName;
        protected ILogMgr _logMgr;
        protected ILogger _logger;
        protected DocumentManager _documentManager;
        protected DockManager _dockManager;
        protected Dictionary<IdentKey, IView> _views;
        protected Dictionary<IdentKey, IView> _activeViews;                
        
        #endregion impl
    }
}
