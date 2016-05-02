using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DevExpress.XtraBars.Docking2010;
using DevExpress.XtraBars.Docking;
using Core.SDK.Composite.Common;
using System.Windows.Forms;
using DevExpress.XtraBars.Docking2010.Views;
using DevExpress.XtraBars.Docking2010.Views.Tabbed;
using Core.SDK.Composite.UI;
using Core.SDK.Log;
using System.IO;

namespace Core.XtraCompositeModule
{
    public class DocumentViewRegion : ViewRegionBase
    {
        public DocumentViewRegion(string regionName, DocumentManager documentManager, DockManager dockManager, ILogMgr logMgr) :
            base(regionName, documentManager, dockManager, logMgr)
        {
            SubscribeToEvent();
        }

        public override IdentKey AddView(IView view)
        {
            return AddView(view, ViewDockingState.Fill, ViewDockingStateHelper.All);
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
            try
            {
                control.Name = view.Name; //<<>>
                doc = CreateDocument(control);
                doc.Image = view.Image;
                doc.ControlName = view.Name;
                doc.Caption = view.Caption;
                doc.Image = view.Image;
                doc.Tag = key;
                doc.Properties.AllowFloat = (allowStates & ViewDockingStateFlagExt.Float) == ViewDockingStateFlagExt.Float ? DevExpress.Utils.DefaultBoolean.True : DevExpress.Utils.DefaultBoolean.False;
                doc.Properties.AllowDock = (allowStates & ViewDockingStateFlagExt.Dock) == ViewDockingStateFlagExt.Dock ? DevExpress.Utils.DefaultBoolean.True : DevExpress.Utils.DefaultBoolean.False;
                doc.Properties.AllowClose = (allowStates & ViewDockingStateFlagExt.Close) == ViewDockingStateFlagExt.Close ? DevExpress.Utils.DefaultBoolean.True : DevExpress.Utils.DefaultBoolean.False;
                doc.Properties.AllowActivate = (allowStates & ViewDockingStateFlagExt.Activate) == ViewDockingStateFlagExt.Activate ? DevExpress.Utils.DefaultBoolean.True : DevExpress.Utils.DefaultBoolean.False;
                _views.Add(key, view);                
                return key;
            }
            catch (Exception ex)
            {
                ClearDocumentResource(key, view);
                throw ex;
            }
        }

        public override void RemoveView(IdentKey key)
        {
            base.RemoveView(key);            
            BaseDocument document = GetDocumentByIdentKey(key);
            if (document == null) throw new InvalidOperationException("View's document with such key is not find.");            
            if (document.IsFloating) _documentManager.View.FloatDocuments.Remove(document); 
            else _documentManager.View.Documents.Remove(document);            
            document.Dispose();
            _views.Remove(key);            
        }

        public override void ActivateView(IdentKey key)
        {
            base.ActivateView(key);
            BaseDocument document = null;
            document = GetDocumentByIdentKey(key);
            _documentManager.View.ActivateDocument(document.Control);            
        }        

        protected BaseDocument CreateDocument(Control control)
        {
            TabbedView tabbedView = _documentManager.View as TabbedView; 
            BaseDocument doc = tabbedView.Controller.AddDocument(control); 
            //tabbedView.AddDocument(control); 
            //BaseDocument doc = tabbedView.Documents[tabbedView.Documents.Count-1]; 
            control.Dock = DockStyle.Fill;
            if (tabbedView.DocumentGroups.Count > 0)
            {
                tabbedView.Controller.Dock(doc as Document, tabbedView.DocumentGroups[0]); 
            }
            tabbedView.ActivateDocument(doc.Control);
            return doc;
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
                _documentManager.View.RestoreLayoutFromStream(stream);
            }
        }

        public override byte[] SaveLayout()
        {
            _logger.Debug("SaveLayout");
            using (MemoryStream stream = new MemoryStream())
            {
                _documentManager.View.SaveLayoutToStream(stream);                
                return stream.ToArray();
            }
        }

        #region private

        private BaseDocument GetDocumentByIdentKey(IdentKey key)
        {
            foreach (BaseDocument doc in _documentManager.View.Documents)
            {
                IdentKey docKey = doc.Tag as IdentKey;
                if (key != null && docKey == key) return doc;
            }
            foreach (BaseDocument doc in _documentManager.View.FloatDocuments)
            {
                IdentKey docKey = doc.Tag as IdentKey;
                if (key != null && docKey == key) return doc;
            }
            return null;
        }

        private void ClearDocumentResource(IdentKey key, IView view)
        {            
            try { _documentManager.View.RemoveDocument(view.UserControl as Control); } catch { }
            try { _views.Remove(key); } catch { }
        }

        private void SubscribeToEvent()
        {
            _documentManager.View.DocumentActivated += new DocumentEventHandler(OnDocumentActivated);
            _documentManager.View.DocumentDeactivated += new DocumentEventHandler(OnDocumentDeactivated);
            _documentManager.View.DocumentClosing += new DocumentCancelEventHandler(OnDocumentClosing);
            _documentManager.View.DocumentClosed += new DocumentEventHandler(OnDocumentClosed);
            _documentManager.View.EndDocking += new DocumentEventHandler(OnEndDocking);
            _documentManager.View.EndFloating += new DocumentEventHandler(OnEndFloating);            
        }

        void View_ControlReleasing(object sender, ControlReleasingEventArgs e)
        {
            e.Cancel = false;
            e.DisposeControl = false;
        }        

        void OnEndFloating(object sender, DocumentEventArgs e)
        {
            BaseDocument doc = e.Document;
            IdentKey key = doc.Tag as IdentKey;
            IView view = null;
            if (key == null || !_views.TryGetValue(key, out view)) return;
            _logger.Debug("OnEndFloating event. " + view.ToString());
        }

        void OnEndDocking(object sender, DocumentEventArgs e)
        {
            BaseDocument doc = e.Document;
            IdentKey key = doc.Tag as IdentKey;
            IView view = null;
            if (key == null || !_views.TryGetValue(key, out view)) return;
            _logger.Debug("OnEndDocking event. " + view.ToString());
        }
       
        void OnDocumentClosing(object sender, DocumentCancelEventArgs e)
        {
            BaseDocument doc = e.Document;
            IdentKey key = doc.Tag as IdentKey;
            IView view = null;
            if (key == null || !_views.TryGetValue(key, out view)) return;

            if (!view.CanClose || !view.OnClosing(null)) e.Cancel = true;
            else if (_documentManager.View.FloatDocuments.IndexOf(doc) == -1)
            {
                TabbedView tabbedView = _documentManager.View as TabbedView;
                //tabbedView.ReleaseDeferredLoadControl(e.Document);
                e.Cancel = true;
                doc.Form.Hide();
                doc.Form.MdiParent = null;
                tabbedView.Controller.RemoveDocument(doc);
                view.OnAfterClose();
                _views.Remove(key);
            }

            _logger.Debug("OnDocumentClosing event. " + view.ToString());
        }

        void OnDocumentClosed(object sender, DocumentEventArgs e)
        {
            BaseDocument doc = e.Document;
            IdentKey key = doc.Tag as IdentKey;
            IView view = null;
            if (key == null || !_views.TryGetValue(key, out view)) return;
            view.OnAfterClose();
            _views.Remove(key);
            _logger.Debug("OnDocumentClosed event. " + view.ToString());           
        }

        void OnDocumentActivated(object sender, DocumentEventArgs e)
        {
            BaseDocument activeDoc = e.Document;            
            IView view = null;

            IdentKey key = activeDoc.Tag as IdentKey;
            if (key != null && _views.TryGetValue(key, out view))
            {
                view.OnActivate(true);
                _logger.Debug("OnDocumentActivated event. " + view.ToString());
            }
        }

        void OnDocumentDeactivated(object sender, DocumentEventArgs e)
        {
            BaseDocument activeDoc = e.Document;
            IView view = null;

            IdentKey key = activeDoc.Tag as IdentKey;
            if (key != null && _views.TryGetValue(key, out view))
            {
                view.OnActivate(false);
                _logger.Debug("OnDocumentDeactivated event. " + view.ToString());
            }
        }

        #endregion private
    }
}
