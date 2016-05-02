using System;
using System.ComponentModel.Composition;
using ChipAndDale.Bug.Command;
using ChipAndDale.Bug.Properties;
using ChipAndDale.SDK;
using ChipAndDale.SDK.EventMessage;
using Core.SDK.Composite.Common;
using Core.SDK.Composite.Event;
using Core.SDK.Composite.Pluggin;
using Core.SDK.Composite.Service;
using Core.SDK.Composite.UI;
using Core.UtilsModule;

namespace ChipAndDale.Bug
{
    [Export(typeof(IPluggin))]
    public class BugPluggin : PlugginBase
    {
        public BugPluggin()
            : base()
        { }
        
        protected override void InitInternal()
        {
            IServiceMgr serviceMgr = ServiceMgr.Current;
            _regionMgr = serviceMgr.GetInstance<IRegionMgr>();

            _plugginMenuCommand = new BugPlugginMenuCommand();
            _bugListCommand = new BugListCommand();
            _bugCreateCommand = new BugCreateCommand();

            _plugginMenuCommandKey = _regionMgr.GetCommandRegion(RegionName.PlugginMenuItem).AddCommand(_plugginMenuCommand);
            
            _plugginMenuBugListCommandKey = _regionMgr.GetCommandRegion(RegionName.PlugginMenuItem).AddCommand(_bugListCommand, _plugginMenuCommand.Name);
            _mainToolBarBugListCommandKey = _regionMgr.GetCommandRegion(RegionName.MainToolBar).AddCommand(_bugListCommand);

            _plugginMenuBugCreateCommandKey = _regionMgr.GetCommandRegion(RegionName.PlugginMenuItem).AddCommand(_bugCreateCommand, _plugginMenuCommand.Name);
            _mainToolBarBugCreateCommandKey = _regionMgr.GetCommandRegion(RegionName.MainToolBar).AddCommand(_bugCreateCommand);
            
            _eventMgr = serviceMgr.GetInstance<IEventMgr>();

            Subscribe();
        }        

        protected override void PreprocessInternal()
        {
           // _mainController.Prepocess();
        }

        protected override void RunInternal()
        { }

        protected override string GetUIName()
        {
            return Settings.Default.BugPlugginUIName;
        }

        protected override string GetInternalName()
        {
            return "BugPluggin";
        }

        protected override IdentKey GetInternalIdent()
        {
            return _identKey;
        }

        public override void Dispose()
        {
            base.Dispose();

            InvokerHlp.WithExceptionSuppress(() => { _regionMgr.GetCommandRegion(RegionName.PlugginMenuItem).RemoveCommand(_plugginMenuBugListCommandKey); }, _logger);
            InvokerHlp.WithExceptionSuppress(() => { _regionMgr.GetCommandRegion(RegionName.MainToolBar).RemoveCommand(_mainToolBarBugListCommandKey); }, _logger);

            InvokerHlp.WithExceptionSuppress(() => { _regionMgr.GetCommandRegion(RegionName.PlugginMenuItem).RemoveCommand(_plugginMenuBugCreateCommandKey); }, _logger);
            InvokerHlp.WithExceptionSuppress(() => { _regionMgr.GetCommandRegion(RegionName.MainToolBar).RemoveCommand(_mainToolBarBugCreateCommandKey); }, _logger);            
            
            InvokerHlp.WithExceptionSuppress(() => { _regionMgr.GetCommandRegion(RegionName.PlugginMenuItem).RemoveCommand(_plugginMenuCommandKey); }, _logger);

            Unsubscribe();
        }


        #region private

        readonly IdentKey _identKey = new IdentKey(new Guid("3DF2B89F-C0A3-40F0-9A29-881F302BA21C"));
        
        IRegionMgr _regionMgr;
        IEventMgr _eventMgr;

        IdentKey _plugginMenuCommandKey;

        IdentKey _plugginMenuBugListCommandKey;
        IdentKey _mainToolBarBugListCommandKey;

        IdentKey _plugginMenuBugCreateCommandKey;
        IdentKey _mainToolBarBugCreateCommandKey;        

        BugPlugginMenuCommand _plugginMenuCommand;
        BugListCommand _bugListCommand;
        BugCreateCommand _bugCreateCommand;  
        

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
            //_mainController.LoadSetting();
        }

        void OnSaveSetting(SettingMessage message)
        {
            //_mainController.SaveSetting();
        }

        #endregion private
    }
}
