using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DevExpress.XtraBars.Docking2010;
using DevExpress.XtraBars.Docking;
using Core.SDK.Composite.Common;
using System.Windows.Forms;
using DevExpress.XtraBars.Docking2010.Views;
using Core.SDK.Composite.UI;
using Core.SDK.Log;
using System.IO;

namespace Core.XtraCompositeModule
{
    public class DockingViewRegion: ViewRegionBase
    {
        public DockingViewRegion(string regionName, DocumentManager documentManager, DockManager dockManager, ILogMgr logMgr) :
            base(regionName, documentManager, dockManager, logMgr)
        {
            SubscribeToEvent();
        }

        public override IdentKey AddView(IView view)
        {
            return AddView(view, ViewDockingState.Float, ViewDockingStateHelper.All);
        }

        public override IdentKey AddView(IView view, ViewDockingState state)
        {
            return AddView(view, state, ViewDockingStateHelper.All);
        }
        
        public override IdentKey AddView(IView view, ViewDockingState state, ViewDockingStateFlagExt allowStates)
        {            
            IdentKey key = base.AddView(view);
            Control control = view.UserControl as Control;
            BaseDocument doc = null;
            DockPanel panel = null;            
            try
            {
                panel = _dockManager.AddPanel(ToDockingStyle(state));
                panel.Controls.Add(control);
                control.Dock = DockStyle.Fill;
                panel.Name = view.Name;
                panel.Text = view.Caption;
                panel.Header = view.Caption;
                panel.Image = view.Image;
                panel.Tag = key;
                panel.AccessibleName = view.Name;
                panel.Options.AllowDockLeft = (allowStates & ViewDockingStateFlagExt.Left) == ViewDockingStateFlagExt.Left;
                panel.Options.AllowDockTop = (allowStates & ViewDockingStateFlagExt.Top) == ViewDockingStateFlagExt.Top;
                panel.Options.AllowDockRight = (allowStates & ViewDockingStateFlagExt.Right) == ViewDockingStateFlagExt.Right;
                panel.Options.AllowDockBottom = (allowStates & ViewDockingStateFlagExt.Bottom) == ViewDockingStateFlagExt.Bottom;
                panel.Options.AllowDockFill = (allowStates & ViewDockingStateFlagExt.Fill) == ViewDockingStateFlagExt.Fill;
                panel.Options.AllowFloating = (allowStates & ViewDockingStateFlagExt.Float) == ViewDockingStateFlagExt.Float;
                panel.Options.AllowDockAsTabbedDocument = false;
                doc = _documentManager.View.Controller.RegisterDockPanel(panel.FloatForm);
                _documentManager.View.Controller.Dock(doc);                
                _views.Add(key, view);                
                return key;
            }
            catch (Exception ex)
            {
                ClearDocumentResource(key, doc, panel, view);
                throw ex;
            }
        }

        public override void RemoveView(IdentKey key)
        {
            base.RemoveView(key);
            DockPanel panel = GetPanelByIdentKey(key);
            if (panel == null) throw new InvalidOperationException("View's panel with such key is not find.");            
            _dockManager.RemovePanel(panel);
            panel.Dispose();
            _views.Remove(key);           
        }        

        public override void ActivateView(IdentKey key)
        {
            base.ActivateView(key);
            DockPanel panel = GetPanelByIdentKey(key);
            if (panel == null) throw new InvalidOperationException("View's panel with such key is not find.");            
            _dockManager.ActivePanel = panel;            
        }

        public override void RestoreLayout(byte[] bytes)
        {
            _logger.Debug("ResoreLayout");
            if (bytes == null || bytes.Length == 0) 
            {
               _logger.Debug("Null input bytes, nothing to restore."); 
                return;
            }

            using (MemoryStream stream = new MemoryStream(bytes))
            {
                _dockManager.RestoreLayoutFromStream(stream);
            }
        }

        public override byte[] SaveLayout()
        {
            _logger.Debug("SaveLayout");
            using (MemoryStream stream = new MemoryStream())
            {
                _dockManager.SaveLayoutToStream(stream);
                return stream.ToArray();
            }
        }

        
        
        #region private 

        private DockPanel GetPanelByIdentKey(IdentKey key)
        {
            foreach (DockPanel panel in _dockManager.Panels)
            {
                IdentKey panelKey = panel.Tag as IdentKey;
                if (key != null && panelKey == key) return panel;
            }
            return null;
        }

        private void ClearDocumentResource(IdentKey key, BaseDocument doc, DockPanel panel, IView view)
        {
            _views.Remove(key); 
        }

        private void SubscribeToEvent()
        {
            _dockManager.ActivePanelChanged += new ActivePanelChangedEventHandler(OnActivePanelChanged);                        
            _dockManager.ClosingPanel +=new DockPanelCancelEventHandler(OnClosingPanel);
            _dockManager.ClosedPanel +=new DockPanelEventHandler(OnClosedPanel);
            _dockManager.EndDocking += new EndDockingEventHandler(OnEndDocking);            
            _dockManager.TabbedChanged += new DockPanelEventHandler(OnTabbedChanged);
        }

        void OnClosedPanel(object sender, DockPanelEventArgs e)
        {
            DockPanel panel = e.Panel;
            IdentKey key = panel.Tag as IdentKey;
            IView view = null;
            if (key == null || !_views.TryGetValue(key, out view)) return;
            view.OnAfterClose();           
            _logger.Debug("OnClosedPanel event. " + view.ToString());
        }

        void OnTabbedChanged(object sender, DockPanelEventArgs e)
        {
            DockPanel panel = e.Panel;
            IdentKey key = panel.Tag as IdentKey;
            IView view = null;
            if (key == null || !_views.TryGetValue(key, out view)) return;
            _logger.Debug("OnTabbedChanged event. " + view.ToString());              
        }

        void OnEndDocking(object sender, EndDockingEventArgs e)
        {
            DockPanel panel = e.Panel; 
            IdentKey key = panel.Tag as IdentKey;
            IView view = null;
            if (key == null || !_views.TryGetValue(key, out view)) return;
            _logger.Debug("OnEndDocking event. " + view.ToString());  
        }

        void OnClosingPanel(object sender, DockPanelCancelEventArgs e)
        {
            DockPanel panel = e.Panel;
            IdentKey key = panel.Tag as IdentKey;
            IView view = null;
            if (key == null || !_views.TryGetValue(key, out view)) return;

            if (!view.CanClose || !view.OnClosing(null)) e.Cancel = true;
            _logger.Debug("OnClosingPanel event. " + view.ToString());  
        }

        void OnActivePanelChanged(object sender, ActivePanelChangedEventArgs e)
        {            
            IdentKey key = null;
            IView view = null;
            DockPanel panel = e.Panel;
            if (panel != null)
            {
                key = panel.Tag as IdentKey;                
                if (key == null || !_views.TryGetValue(key, out view)) return;
                _logger.Debug("OnActivePanelChanged event. Activate. " + view.ToString());
            }

            panel = e.OldPanel;
            if (panel != null)
            {
                key = panel.Tag as IdentKey;
                if (key == null || !_views.TryGetValue(key, out view)) return;
                _logger.Debug("OnActivePanelChanged event. Deacivate" + view.ToString());
            }
        }

        private DockingStyle ToDockingStyle(ViewDockingState viewDockingOption)
        {
            DockingStyle result = DockingStyle.Left;
            switch (viewDockingOption)  
            {
                case ViewDockingState.Left:
                    result = DockingStyle.Left;
                    break;
                case ViewDockingState.Top:
                    result = DockingStyle.Top;
                    break;
                case ViewDockingState.Right:
                    result = DockingStyle.Right;
                    break;
                case ViewDockingState.Bottom:
                    result = DockingStyle.Bottom;
                    break;
                case ViewDockingState.Float:
                    result = DockingStyle.Float;
                    break;
                case ViewDockingState.Fill:
                    result = DockingStyle.Fill;
                    break; 
                default:
                    throw new ArgumentException("Invalid value of viewDockingOption."); 
            }
            return result;
        }

        #endregion private
    }
}
