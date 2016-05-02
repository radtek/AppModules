using System;
using System.Drawing;
using System.Windows.Forms;
using ChipAndDale.Main.Nsi;
using Core.SDK.Composite.Service;
using Core.SDK.Log;
using DevExpress.LookAndFeel;
using DevExpress.Skins;
using DevExpress.XtraBars;
using DevExpress.XtraEditors;
using DevExpress.XtraRichEdit.API.Native;
using DevExpress.XtraSplashScreen;
using Core.SDK.Composite.Event;
using ChipAndDale.Main.EventMessage;
using ChipAndDale.SDK.EventMessage;
using Core.SDK.Composite.UI;


namespace ChipAndDale.Main.UI
{
    public partial class MainForm : XtraForm, IMainForm
    {
        public MainForm()
        {
            _logMgr = ServiceMgr.Current.GetInstance<ILogMgr>();            
            _logger = _logMgr.GetLogger("MainForm");
            _logger.Debug("Create.");
            _eventMgr = ServiceMgr.Current.GetInstance<IEventMgr>(); 
            
            InitializeComponent();            
            InitLogPanel();
            InitSkinMenu();                                    
            Hide();
        }

        #region IMainForm

        public void SetService()
        {
            _mainController = ServiceMgr.Current.GetInstance<IMainController>();
            _nsiController = ServiceMgr.Current.GetInstance<INsiController>();
        }       

        #endregion IMainForm


        #region Override

        protected override void OnLoad(EventArgs e)
        {            
            base.OnLoad(e);
            FireMainFormLoadedEvent();
            SplashScreenManager.CloseForm();
            WindowState = FormWindowState.Maximized;

            UpdateConnectButtonState();                                          
        }

        protected override void OnClosed(EventArgs e)
        {
            base.OnClosed(e);
            FireMainFormClosedEvent();
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
            _logMgr.NewLogEvent -= new Action<global::Core.SDK.Log.LogLevel, string>(OnNewLogEvent);
        }

        #endregion Override


        #region private

        IMainController _mainController;
        INsiController _nsiController;
        ILogMgr _logMgr;
        ILogger _logger;
        IEventMgr _eventMgr;
        bool _isAutoScrollLogPanel;


        #region Log panel

        void InitLogPanel()
        {
            LogRichEditControl.Document.AppendText("Початок роботи програми ..." + Environment.NewLine);
            _isAutoScrollLogPanel = true;
            AutoScrollLogPanelBarButton.Down = _isAutoScrollLogPanel;
            _logMgr.NewLogEvent += new Action<global::Core.SDK.Log.LogLevel, string>(OnNewLogEvent);
        }  

        void OnNewLogEvent(global::Core.SDK.Log.LogLevel level, string message)
        {
            LogRichEditControl.BeginUpdate();
            DocumentRange range = LogRichEditControl.Document.AppendText(message + Environment.NewLine); 
            if (level == LogLevel.Error || level == LogLevel.Fatal)
            {
                CharacterProperties characterProperty = LogRichEditControl.Document.BeginUpdateCharacters(range);                
                characterProperty.BackColor = Color.HotPink;                
                LogRichEditControl.Document.EndUpdateCharacters(characterProperty);
            }
            else if (level == LogLevel.Warn)
            {
                CharacterProperties characterProperty = LogRichEditControl.Document.BeginUpdateCharacters(range);
                characterProperty.ForeColor = Color.HotPink;
                LogRichEditControl.Document.EndUpdateCharacters(characterProperty);
            }          
            if (_isAutoScrollLogPanel)
            {
                LogRichEditControl.Document.CaretPosition = range.End;
                LogRichEditControl.ScrollToCaret();
            }
            LogRichEditControl.EndUpdate();
        }

        private void LogDockPanel_ClosedPanel(object sender, DevExpress.XtraBars.Docking.DockPanelEventArgs e)
        {
            LogPanelBarButton.Down = false;
        }

        #endregion Log panel


        #region Skin

        void InitSkinMenu()
        {
            SkinContainerCollection skins = SkinManager.Default.Skins;
            for (int i = 0; i < skins.Count; i++)
            {
                BarItem item = new BarCheckItem(MainBarManager, DefaultLookAndFeel.LookAndFeel.SkinName == skins[i].SkinName);
                item.Name = skins[i].SkinName; item.Caption = skins[i].SkinName;
                item.ItemClick += new ItemClickEventHandler(SkinItem_ItemClick);
                SkinBarSubItem.AddItem(item);
            }
        }

        #endregion Skin


        #region Fired events

        void FireMainFormLoadedEvent()
        {
            _eventMgr.GetEvent<MainFormLoadedEvent>().Publish(new MainFormLoadedMessage());
        }

        void FireMainFormClosedEvent()
        {
            _eventMgr.GetEvent<MainFormClosedEvent>().Publish(new MainFormClosedMessage());
        }        

        #endregion Fired events


        #region Menu item clicks

        void SkinItem_ItemClick(object sender, ItemClickEventArgs e)
        {
            BarCheckItem item = e.Item as BarCheckItem;
            if (item == null) return;

            DefaultLookAndFeel.LookAndFeel.SkinName = item.Caption;
        }     

        private void ConnectBarButtonItem_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {            
            _mainController.OnConnect();
            UpdateConnectButtonState();
        }        

        private void DisconnectBarButtonItem_ItemClick(object sender, ItemClickEventArgs e)
        {
            _mainController.OnDisconnect();
            UpdateConnectButtonState();
        }

        private void UpdateConnectButtonState()
        {
            ConnectBarButtonItem.Enabled = !_mainController.IsConnect;
            DisconnectBarButtonItem.Enabled = _mainController.IsConnect;
        }

        private void UserNsiBarButton_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {            
            _nsiController.OpenUsersEditDialog();
        }            

        private void OrgNsiBarButton_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {           
            _nsiController.OpenOrgsEditDialog();
        }

        private void TagNsiBarButton_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {           
            _nsiController.OpenTagsEditDialog();
        }

        private void AppNsiBarButton_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {            
            _nsiController.OpenAppsEditDialog();
        }        

        private void LogPanelBarButton_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            if (LogPanelBarButton.Down) LogDockPanel.Show(); else LogDockPanel.Hide();
        }

        private void AutoScrollLogPanelBarButton_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            _isAutoScrollLogPanel = AutoScrollLogPanelBarButton.Down;
        }

        private void ClearLogPanelBarButton_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            LogRichEditControl.Document.Text = string.Empty;
        }

        private void SaveSettingBarButtonItem_ItemClick(object sender, ItemClickEventArgs e)
        {
            _mainController.OnSaveSetting();
        }

        private void LoadSettingBarButtonItem_ItemClick(object sender, ItemClickEventArgs e)
        {
            _mainController.OnLoadSetting();
        }

        #endregion Menu item clicks        

        #endregion private
    }
}