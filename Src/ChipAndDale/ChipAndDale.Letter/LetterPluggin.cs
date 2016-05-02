using System;
using System.ComponentModel.Composition;
using ChipAndDale.Letter.Command;
using ChipAndDale.Letter.Properties;
using ChipAndDale.SDK;
using ChipAndDale.SDK.EventMessage;
using Core.SDK.Composite.Common;
using Core.SDK.Composite.Event;
using Core.SDK.Composite.Pluggin;
using Core.SDK.Composite.Service;
using Core.SDK.Composite.UI;
using Core.UtilsModule;

namespace ChipAndDale.Letter
{
    [Export(typeof(IPluggin))]
    public class LetterPluggin : PlugginBase
    {
        public LetterPluggin()
            : base()
        { }
        
        protected override void InitInternal()
        {
            IServiceMgr serviceMgr = ServiceMgr.Current;
            _regionMgr = serviceMgr.GetInstance<IRegionMgr>();

            _plugginMenuCommand = new LetterPlugginMenuCommand();
            _bugListCommand = new LetterListCommand();
            _bugCreateCommand = new LetterCreateCommand();

            _plugginMenuCommandKey = _regionMgr.GetCommandRegion(RegionName.PlugginMenuItem).AddCommand(_plugginMenuCommand);
            
            _plugginMenuLetterListCommandKey = _regionMgr.GetCommandRegion(RegionName.PlugginMenuItem).AddCommand(_bugListCommand, _plugginMenuCommand.Name);
            _mainToolBarLetterListCommandKey = _regionMgr.GetCommandRegion(RegionName.MainToolBar).AddCommand(_bugListCommand);

            _plugginMenuLetterCreateCommandKey = _regionMgr.GetCommandRegion(RegionName.PlugginMenuItem).AddCommand(_bugCreateCommand, _plugginMenuCommand.Name);
            _mainToolBarLetterCreateCommandKey = _regionMgr.GetCommandRegion(RegionName.MainToolBar).AddCommand(_bugCreateCommand);
            
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
            return Settings.Default.LetterPlugginUIName;
        }

        protected override string GetInternalName()
        {
            return "LetterPluggin";
        }

        protected override IdentKey GetInternalIdent()
        {
            return _identKey;
        }

        public override void Dispose()
        {
            base.Dispose();

            InvokerHlp.WithExceptionSuppress(() => { _regionMgr.GetCommandRegion(RegionName.PlugginMenuItem).RemoveCommand(_plugginMenuLetterListCommandKey); }, _logger);
            InvokerHlp.WithExceptionSuppress(() => { _regionMgr.GetCommandRegion(RegionName.MainToolBar).RemoveCommand(_mainToolBarLetterListCommandKey); }, _logger);

            InvokerHlp.WithExceptionSuppress(() => { _regionMgr.GetCommandRegion(RegionName.PlugginMenuItem).RemoveCommand(_plugginMenuLetterCreateCommandKey); }, _logger);
            InvokerHlp.WithExceptionSuppress(() => { _regionMgr.GetCommandRegion(RegionName.MainToolBar).RemoveCommand(_mainToolBarLetterCreateCommandKey); }, _logger);            
            
            InvokerHlp.WithExceptionSuppress(() => { _regionMgr.GetCommandRegion(RegionName.PlugginMenuItem).RemoveCommand(_plugginMenuCommandKey); }, _logger);

            Unsubscribe();
        }


        #region private

        readonly IdentKey _identKey = new IdentKey(new Guid("E2B517A5-1DB8-4AAD-B3B9-6867D7BC6DA5"));
        
        IRegionMgr _regionMgr;
        IEventMgr _eventMgr;

        IdentKey _plugginMenuCommandKey;

        IdentKey _plugginMenuLetterListCommandKey;
        IdentKey _mainToolBarLetterListCommandKey;

        IdentKey _plugginMenuLetterCreateCommandKey;
        IdentKey _mainToolBarLetterCreateCommandKey;        

        LetterPlugginMenuCommand _plugginMenuCommand;
        LetterListCommand _bugListCommand;
        LetterCreateCommand _bugCreateCommand;  
        

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
