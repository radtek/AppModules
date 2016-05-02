using System;
using System.ComponentModel.Composition;
using System.ComponentModel.Composition.Hosting;
using System.Threading;
using System.Windows.Forms;
using ChipAndDale.Main.Db;
using ChipAndDale.Main.EventMessage;
using ChipAndDale.Main.Nsi;
using ChipAndDale.Main.UI;
using ChipAndDale.SDK;
using ChipAndDale.SDK.Common;
using ChipAndDale.SDK.Nsi;
using Core.CompositeModule;
using Core.LogModule;
using Core.OracleModule;
using Core.OracleModule.Utils;
using Core.SDK.Composite.Event;
using Core.SDK.Composite.Pluggin;
using Core.SDK.Composite.Service;
using Core.SDK.Composite.UI;
using Core.SDK.Db;
using Core.SDK.Log;
using Core.SDK.Setting;
using Core.SettingModule;
using Core.XtraCompositeModule;
using Core.XtraCompositeModule.DialogMgr;
using DevExpress.XtraSplashScreen;
using ChipAndDale.SDK.EventMessage;

namespace ChipAndDale.Main
{
    internal class Bootstrapper : BootStrapperBase
    {
        internal Bootstrapper()
        { 
            UnhandleExceptionHandler unhandleExceptionHandler = new UnhandleExceptionHandler();
            Application.ThreadException += new ThreadExceptionEventHandler(unhandleExceptionHandler.ExceptionHandler);
        }

        protected override ILogMgr CreateLogMgr()
        {
            return new LogMgr(@"NLog.config");
        }

        protected override AggregateCatalog CreateAgregateCatalog()
        {
            AggregateCatalog catalog = new AggregateCatalog();
            catalog.Catalogs.Add(new DirectoryCatalog(@"Pluggins"));            
            return catalog;
        }

        protected override IEventMgr CreateEventMgr()
        {
            return new EventMgr(LogMgr);
        }

        protected override PlugginMgr CreatePlugginMgr()
        {
            return new PlugginMgr(LogMgr);
        }

        protected override void RegisterServices()
        {
            InitEnvironment();

            SplashScreenManager.ShowForm(typeof(InitialSplashScreen));

            InitMainForm();

            InitCoreService();

            InitSpecificService();

            _mainForm.SetService();

            SubscribeEvent();               
        }                       
                

        protected override void StartWork()
        {           
            Application.Run(_mainForm);
        }


        #region private

        MainForm _mainForm;
        IMainController _mainController;
        IPlugginMgr _plugginMgr;
        ISettingMgr _settingMgr;
        IRegionMgr _regionMgr;
        IEventMgr _eventMgr;
        IDbMgr _dbMgr;


        #region Init

        private void InitEnvironment()
        {
            Environment.SetEnvironmentVariable("TNS_ADMIN", Environment.CurrentDirectory + "\\" + Properties.Settings.Default.OraPath, EnvironmentVariableTarget.Process);
            Environment.SetEnvironmentVariable("NLS_LANG", "AMERICAN_AMERICA.CL8MSWIN1251", EnvironmentVariableTarget.Process);
            Environment.SetEnvironmentVariable("Path",Environment.CurrentDirectory + "\\" + Properties.Settings.Default.OraPath + ";" + Environment.GetEnvironmentVariable("Path"), EnvironmentVariableTarget.Process);
        }

        void InitMainForm()
        {
            _mainForm = new MainForm();            
        }        

        private void InitCoreService()
        {
            IDbConnection dbConnection = new OraConnection();
            CompositionContainer.ComposeExportedValue<IDbConnection>(dbConnection);

            _dbMgr = new OraDBMgr(dbConnection, LogMgr);
            CompositionContainer.ComposeExportedValue<IDbMgr>(_dbMgr);

            _settingMgr = new SettingMgr(LogMgr);
            _settingMgr.ReadWriteProvider = new OraDBSettingReadWriter(_dbMgr, LogMgr);
            CompositionContainer.ComposeExportedValue<ISettingMgr>(_settingMgr);

            _regionMgr = new RegionMgr(LogMgr);
            CompositionContainer.ComposeExportedValue<IRegionMgr>(_regionMgr);

            int firstMenuIndex = 2;
            BarCommandRegion mainMenuRegion = new BarCommandRegion(RegionName.MainMenu, _mainForm.MainMenu, firstMenuIndex);
            _regionMgr.AddCommandRegion(RegionName.MainMenu, new MainMenuCommandRegionDecorator(RegionName.MainMenu, mainMenuRegion));
            _regionMgr.AddCommandRegion(RegionName.PlugginMenuItem, new SubMenuCommandRegionDecorator(RegionName.PlugginMenuItem, mainMenuRegion, _mainForm.PlugginBarButtonItem));

            int firstToolBarIndex = 2;
            BarCommandRegion toolBarRegion = new BarCommandRegion(RegionName.MainToolBar, _mainForm.MainToolBar, firstToolBarIndex);
            _regionMgr.AddCommandRegion(RegionName.MainToolBar, toolBarRegion);

            IViewRegion documentRegion = new DocumentViewRegion(RegionName.DocumentRegion, _mainForm.MainDocumentManager, _mainForm.MainDockManager, LogMgr);
            _regionMgr.AddViewRegion(RegionName.DocumentRegion, documentRegion);

            IViewRegion dockPanelRegion = new DockingViewRegion(RegionName.DockPanelRegion, _mainForm.MainDocumentManager, _mainForm.MainDockManager, LogMgr);
            _regionMgr.AddViewRegion(RegionName.DockPanelRegion, dockPanelRegion);

            IViewFormMgr viewFormMgr = new ViewFormMgr(_mainForm, _mainForm.MainTaskBar, LogMgr);
            CompositionContainer.ComposeExportedValue<IViewFormMgr>(viewFormMgr);

            IMessageBoxMgr messageBoxMgr = new MessageBoxMgr(_mainForm, LogMgr, null, typeof(MainWaitForm));
            CompositionContainer.ComposeExportedValue<IMessageBoxMgr>(messageBoxMgr);

            _plugginMgr = ServiceMgr.Current.GetInstance<IPlugginMgr>();
            _eventMgr = ServiceMgr.Current.GetInstance<IEventMgr>();
        }

        private void InitSpecificService()
        {
            ICommonDbAccessor commonDbAccessor = new CommonDbAccessor();
            CompositionContainer.ComposeExportedValue<ICommonDbAccessor>(commonDbAccessor);

            INsiDbAccessor nsiDbAccessor = new NsiDbAccessor();
            CompositionContainer.ComposeExportedValue<INsiDbAccessor>(nsiDbAccessor);

            INsiController nsiController = new NsiController();
            CompositionContainer.ComposeExportedValue<INsiController>(nsiController);
            CompositionContainer.ComposeExportedValue<INsiService>(nsiController);

            _mainController = new MainController();
            CompositionContainer.ComposeExportedValue<IMainController>(_mainController);
            CompositionContainer.ComposeExportedValue<ICommonService>(_mainController);
        }

        #endregion Init


        #region Events

        void SubscribeEvent()
        {
            _eventMgr.GetEvent<FirstDbConnectEvent>().Subscribe(OnFirstDbConnect);
            _eventMgr.GetEvent<MainFormLoadedEvent>().Subscribe(OnMainFormLoaded);
            _eventMgr.GetEvent<MainFormClosedEvent>().Subscribe(OnMainFormClosed);
            _eventMgr.GetEvent<MainFormLoadSettingEvent>().Subscribe(OnLoadSetting);
            _eventMgr.GetEvent<MainFormSaveSettingEvent>().Subscribe(OnSaveSetting);
        }

        void UnsubscribeEvent()
        {
            _eventMgr.GetEvent<FirstDbConnectEvent>().Unsubscribe(OnFirstDbConnect);
            _eventMgr.GetEvent<MainFormLoadedEvent>().Unsubscribe(OnMainFormLoaded);
            _eventMgr.GetEvent<MainFormClosedEvent>().Unsubscribe(OnMainFormClosed);
            _eventMgr.GetEvent<MainFormLoadSettingEvent>().Unsubscribe(OnLoadSetting);
            _eventMgr.GetEvent<MainFormSaveSettingEvent>().Unsubscribe(OnSaveSetting);
        }

        void OnFirstDbConnect(FirstDbConnectMessage message)
        {
            PreprocessAndRunPluggins();
        }

        void OnMainFormLoaded(MainFormLoadedMessage message)
        {
            InitPluggins();            
        }

        void OnMainFormClosed(MainFormClosedMessage message)
        {
            UnsubscribeEvent();
            DisposePluggins();
        }

        void OnLoadSetting(SettingMessage message)
        {                        
            FireLoadSettingEvent(message);
            RestoreRegionLayout();
        }        

        void OnSaveSetting(SettingMessage message)
        {
            if (!_mainController.IsConnect) return;

            SaveRegionLayout();
            FireSaveSettingEvent(message);
        }

        void FireLoadSettingEvent(SettingMessage message)
        {
            _eventMgr.GetEvent<LoadSettingEvents>().PublishNoEx(message);
        }

        void FireSaveSettingEvent(SettingMessage message)
        {
            _eventMgr.GetEvent<SaveSettingEvents>().PublishNoEx(message);
        }

        #endregion Events


        #region Pluggins

        void InitPluggins()
        {
            _plugginMgr.Init();
        }

        void PreprocessAndRunPluggins()
        {            
            _plugginMgr.Preprocess();

            OnLoadSetting(null);
            
            _plugginMgr.Run();
        }
        

        private void DisposePluggins()
        {
            OnSaveSetting(null);
            
            _plugginMgr.Dispose();
        }        

        #endregion Pluggins


        #region Settings

        public class RegionSetting
        {
            public byte[] RegionLayout { get; set; }
        }

        private void RestoreRegionLayout()
        {
            foreach (IViewRegion region in _regionMgr.ViewRegions)
            {
                RegionSetting setting = new RegionSetting();
                _settingMgr.LoadSetting(_mainController.Login, Properties.Settings.Default.ModuleName, region.Name, setting);
                if (setting.RegionLayout != null) region.RestoreLayout(setting.RegionLayout);
            }
        }

        private void SaveRegionLayout()
        {                       
            foreach (IViewRegion region in _regionMgr.ViewRegions)
            {
                RegionSetting setting = new RegionSetting();
                setting.RegionLayout = region.SaveLayout();
                _settingMgr.SaveSetting(_mainController.Login, Properties.Settings.Default.ModuleName, region.Name, setting, true);
            }
        }

        #endregion Settings

        #endregion private
    }
}
