using System;
using ChipAndDale.Request.Command;
using ChipAndDale.Request.Db;
using ChipAndDale.Request.Properties;
using ChipAndDale.SDK;
using ChipAndDale.SDK.Request;
using Core.SDK.Composite.Common;
using Core.SDK.Composite.Pluggin;
using Core.SDK.Composite.Service;
using Core.SDK.Composite.UI;
using Core.UtilsModule;
using System.ComponentModel.Composition;
using Core.SDK.Composite.Event;
using ChipAndDale.SDK.EventMessage;

namespace ChipAndDale.Request
{
    [Export(typeof(IPluggin))]
    public class RequestPluggin : PlugginBase
    {
        public RequestPluggin() : base()
        { }
        
        protected override void InitInternal()
        {
            IServiceMgr serviceMgr = ServiceMgr.Current;
            _regionMgr = serviceMgr.GetInstance<IRegionMgr>();

            IDbAccessor dbSccessor = new DbAccessor();
            _mainController = new MainController(dbSccessor);
            serviceMgr.AddInstance<IRequestService>(_mainController);

            _plugginMenuCommand = new RequestPlugginMenuCommand();

            _requestListCommand = new RequestListCommand(_mainController);            
            _requestCreateCommand = new RequestCreateCommand(_mainController);
            _requestPeriodReportCommand = new RequestPeriodReportCommand(_mainController);

            _plugginMenuCommandKey = _regionMgr.GetCommandRegion(RegionName.PlugginMenuItem).AddCommand(_plugginMenuCommand);

            _plugginMenuRequestListCommandKey = _regionMgr.GetCommandRegion(RegionName.PlugginMenuItem).AddCommand(_requestListCommand, _plugginMenuCommand.Name);
            _mainToolBarRequestListCommandKey = _regionMgr.GetCommandRegion(RegionName.MainToolBar).AddCommand(_requestListCommand);

            _plugginMenuRequestCreateCommandKey = _regionMgr.GetCommandRegion(RegionName.PlugginMenuItem).AddCommand(_requestCreateCommand, _plugginMenuCommand.Name);
            _mainToolBarRequestCreateCommandKey = _regionMgr.GetCommandRegion(RegionName.MainToolBar).AddCommand(_requestCreateCommand);

            _plugginMenuPeriodReportCommandKey = _regionMgr.GetCommandRegion(RegionName.PlugginMenuItem).AddCommand(_requestPeriodReportCommand, _plugginMenuCommand.Name);
            _mainToolBarPeriodReportCommandKey = _regionMgr.GetCommandRegion(RegionName.MainToolBar).AddCommand(_requestPeriodReportCommand);

            _eventMgr = serviceMgr.GetInstance<IEventMgr>();

            Subscribe();
        }        

        protected override void PreprocessInternal()
        {
            _mainController.Prepocess();
        }

        protected override void RunInternal()
        { }

        protected override string GetUIName()
        {
            return Settings.Default.RequestName;
        }

        protected override string GetInternalName()
        {
            return "RequestPluggin";
        }

        protected override IdentKey GetInternalIdent()
        {
            return _identKey;
        }

        public override void Dispose()
        {
            base.Dispose();

            InvokerHlp.WithExceptionSuppress(() => { _regionMgr.GetCommandRegion(RegionName.PlugginMenuItem).RemoveCommand(_plugginMenuRequestListCommandKey); }, _logger);
            InvokerHlp.WithExceptionSuppress(() => { _regionMgr.GetCommandRegion(RegionName.MainToolBar).RemoveCommand(_mainToolBarRequestListCommandKey); }, _logger);
            
            InvokerHlp.WithExceptionSuppress(() => { _regionMgr.GetCommandRegion(RegionName.PlugginMenuItem).RemoveCommand(_plugginMenuRequestCreateCommandKey); }, _logger);
            InvokerHlp.WithExceptionSuppress(() => { _regionMgr.GetCommandRegion(RegionName.MainToolBar).RemoveCommand(_mainToolBarRequestCreateCommandKey); }, _logger);

            InvokerHlp.WithExceptionSuppress(() => { _regionMgr.GetCommandRegion(RegionName.PlugginMenuItem).RemoveCommand(_plugginMenuPeriodReportCommandKey); }, _logger);
            InvokerHlp.WithExceptionSuppress(() => { _regionMgr.GetCommandRegion(RegionName.MainToolBar).RemoveCommand(_mainToolBarPeriodReportCommandKey); }, _logger);
            
            InvokerHlp.WithExceptionSuppress(() => { _regionMgr.GetCommandRegion(RegionName.PlugginMenuItem).RemoveCommand(_plugginMenuCommandKey); }, _logger);

            Unsubscribe();
        }


        #region private

        readonly IdentKey _identKey = new IdentKey(new Guid("B6F14B6A-A772-44B2-88E2-33B1D293D9C9"));
        IMainController _mainController;
        IRegionMgr _regionMgr;
        IEventMgr _eventMgr;

        IdentKey _plugginMenuRequestListCommandKey;
        IdentKey _mainToolBarRequestListCommandKey;
        
        IdentKey _plugginMenuRequestCreateCommandKey;
        IdentKey _mainToolBarRequestCreateCommandKey;

        IdentKey _plugginMenuPeriodReportCommandKey;
        IdentKey _mainToolBarPeriodReportCommandKey;

        
        IdentKey _plugginMenuCommandKey;
        
        RequestListCommand _requestListCommand;
        RequestPlugginMenuCommand _plugginMenuCommand;
        RequestCreateCommand _requestCreateCommand;        
        RequestPeriodReportCommand _requestPeriodReportCommand;

        private void Subscribe()
        {
            _eventMgr.GetEvent<LoadSettingEvents>().Subscribe(OnLoadSetting);
            _eventMgr.GetEvent<SaveSettingEvents>().Subscribe(OnSaveSetting);
        }

        private void Unsubscribe()
        {
            _eventMgr.GetEvent<LoadSettingEvents>().Unsubscribe(OnLoadSetting);
            _eventMgr.GetEvent<SaveSettingEvents>().Unsubscribe(OnSaveSetting);
        }

        void OnLoadSetting(SettingMessage message)
        {
            _mainController.LoadSetting();
        }

        void OnSaveSetting(SettingMessage message)
        {
            _mainController.SaveSetting();
        }

        #endregion private
    }
}
