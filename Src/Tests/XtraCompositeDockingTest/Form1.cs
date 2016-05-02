using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using DevExpress.Skins;
using DevExpress.LookAndFeel;
using DevExpress.UserSkins;
using DevExpress.XtraEditors;
using Core.SDK.Composite.UI;
using Core.SDK.Log;
using Core.SDK.Composite.Common;
using Core.XtraCompositeModule;
using Core.UICompositeModule;
using Core.XtraCompositeModule.DialogMgr;


namespace XtraCompositeDockingTest
{
    public partial class Form1 : XtraForm
    {
        public Form1()
        {
            InitializeComponent();

            _LogMgr = new Core.LogModule.LogMgr(@"J:\Other_project\MyUtils\Deps\NLog\NLog.config");
            _Logger = _LogMgr.GetLogger("XtraCompositeTest");
            _Logger.Info("Start XtraCompositeTest.");

            _regionMgr = new RegionMgr(_LogMgr);
            DocumentViewRegion docRegion = new DocumentViewRegion("DocumentRegion", documentManager1, dockManager1, _LogMgr);            
            _regionMgr.AddViewRegion("DocumentRegion", docRegion);
            DockingViewRegion dockingRegion = new DockingViewRegion("DockingRegion", documentManager1, dockManager1, _LogMgr);
            _regionMgr.AddViewRegion("DockingRegion", dockingRegion);

            _dialogMgr = new ViewFormMgr(this, bar3, _LogMgr);
        }

        IRegionMgr _regionMgr;
        ILogMgr _LogMgr;
        ILogger _Logger;
        IdentKey _key;
        IdentKey _key2, _key3;
        IView _docView;
        bool _isEnable = true;
        IViewFormMgr _dialogMgr;

        Dictionary<IdentKey, IView> _dict = new Dictionary<IdentKey,IView>();

        IView CreateView()
        { 
            XtraUserControl1 control = new XtraUserControl1();
            string str = "ViewCaption" + DateTime.Now.ToLongTimeString();
            Core.SDK.Composite.UI.View view = new Core.SDK.Composite.UI.View(control, str, (closeType) => { return true; }, (isActive) => { });            
            return view;
        }

        #region Docking
        private void barButtonItem1_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            _docView = CreateView();
            _key = _regionMgr.GetViewRegion("DocumentRegion").AddView(CreateView());
        }

        private void barButtonItem2_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            if (_key == null) return;

            _regionMgr.GetViewRegion("DocumentRegion").RemoveView(_key);
            _key = null;
        }

        private void barButtonItem3_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            if (_key == null) return;

            _regionMgr.GetViewRegion("DocumentRegion").ActivateView(_key);
        }




        private void barButtonItem4_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            _docView = CreateView();
            _key2 = _regionMgr.GetViewRegion("DockingRegion").AddView(CreateView());            
        }

        private void barButtonItem5_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            if (_key2 == null) return;

            _regionMgr.GetViewRegion("DockingRegion").RemoveView(_key2);
            _key2 = null;
        }

        private void barButtonItem6_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            if (_key2 == null) return;

            _regionMgr.GetViewRegion("DockingRegion").ActivateView(_key2);
        }

        private void barButtonItem7_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            documentManager1.View.SaveLayoutToXml(@"C:\temp\111222.xml");
        }

        private void barButtonItem8_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            documentManager1.View.RestoreLayoutFromXml(@"C:\temp\111222.xml");
        }
        #endregion Docking

        private void ShowModalItem_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            IView view = CreateView();
            _dict.Add(new IdentKey(), view);
            DialogResult res = _dialogMgr.ShowModal(view);
        }

        private void ShowNoModalItem_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            IView view = CreateView();
            _key = _dialogMgr.ShowNoModal(view, DialogFormOption.ShowMinimizeButton);
            _dict.Add(_key, view);
        }

        private void ChangeStateNoModelItem_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            bool state = _dialogMgr.IsActiveNoModal(_key);
            _dialogMgr.ChangeStateNoModal(_key, !state);
        }

        private void CloseNoModelItem_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            _dialogMgr.CloseNoModal(_key);
        }
    }
}