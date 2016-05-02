using System;
using System.ComponentModel.Composition;
using ChipAndDale.Task.Command;
using ChipAndDale.Task.Properties;
using ChipAndDale.SDK;
using ChipAndDale.SDK.EventMessage;
using Core.SDK.Composite.Common;
using Core.SDK.Composite.Event;
using Core.SDK.Composite.Pluggin;
using Core.SDK.Composite.Service;
using Core.SDK.Composite.UI;
using Core.UtilsModule;

namespace ChipAndDale.Task
{
    [Export(typeof(IPluggin))]
    public class TaskPluggin : PlugginBase
    {
        public TaskPluggin()
            : base()
        { }
        
        protected override void InitInternal()
        {
            IServiceMgr serviceMgr = ServiceMgr.Current;
            _regionMgr = serviceMgr.GetInstance<IRegionMgr>();

            _plugginMenuCommand = new TaskPlugginMenuCommand();
            _bugListCommand = new TaskListCommand();
            _bugCreateCommand = new TaskCreateCommand();

            _plugginMenuCommandKey = _regionMgr.GetCommandRegion(RegionName.PlugginMenuItem).AddCommand(_plugginMenuCommand);
            
            _plugginMenuTaskListCommandKey = _regionMgr.GetCommandRegion(RegionName.PlugginMenuItem).AddCommand(_bugListCommand, _plugginMenuCommand.Name);
            _mainToolBarTaskListCommandKey = _regionMgr.GetCommandRegion(RegionName.MainToolBar).AddCommand(_bugListCommand);

            _plugginMenuTaskCreateCommandKey = _regionMgr.GetCommandRegion(RegionName.PlugginMenuItem).AddCommand(_bugCreateCommand, _plugginMenuCommand.Name);
            _mainToolBarTaskCreateCommandKey = _regionMgr.GetCommandRegion(RegionName.MainToolBar).AddCommand(_bugCreateCommand);
            
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
            return Settings.Default.TaskPlugginUIName;
        }

        protected override string GetInternalName()
        {
            return "TaskPluggin";
        }

        protected override IdentKey GetInternalIdent()
        {
            return _identKey;
        }

        public override void Dispose()
        {
            base.Dispose();

            InvokerHlp.WithExceptionSuppress(() => { _regionMgr.GetCommandRegion(RegionName.PlugginMenuItem).RemoveCommand(_plugginMenuTaskListCommandKey); }, _logger);
            InvokerHlp.WithExceptionSuppress(() => { _regionMgr.GetCommandRegion(RegionName.MainToolBar).RemoveCommand(_mainToolBarTaskListCommandKey); }, _logger);

            InvokerHlp.WithExceptionSuppress(() => { _regionMgr.GetCommandRegion(RegionName.PlugginMenuItem).RemoveCommand(_plugginMenuTaskCreateCommandKey); }, _logger);
            InvokerHlp.WithExceptionSuppress(() => { _regionMgr.GetCommandRegion(RegionName.MainToolBar).RemoveCommand(_mainToolBarTaskCreateCommandKey); }, _logger);            
            
            InvokerHlp.WithExceptionSuppress(() => { _regionMgr.GetCommandRegion(RegionName.PlugginMenuItem).RemoveCommand(_plugginMenuCommandKey); }, _logger);

            Unsubscribe();
        }


        #region private

        readonly IdentKey _identKey = new IdentKey(new Guid("E2B517A5-1DB8-4AAD-B3B9-6867D7BC6DA5"));
        
        IRegionMgr _regionMgr;
        IEventMgr _eventMgr;

        IdentKey _plugginMenuCommandKey;

        IdentKey _plugginMenuTaskListCommandKey;
        IdentKey _mainToolBarTaskListCommandKey;

        IdentKey _plugginMenuTaskCreateCommandKey;
        IdentKey _mainToolBarTaskCreateCommandKey;        

        TaskPlugginMenuCommand _plugginMenuCommand;
        TaskListCommand _bugListCommand;
        TaskCreateCommand _bugCreateCommand;  
        

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
