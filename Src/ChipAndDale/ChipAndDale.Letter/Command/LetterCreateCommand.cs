using ChipAndDale.SDK.Common;
using Core.SDK.Common;
using Core.SDK.Composite.Service;
using Core.SDK.Composite.UI;
using Core.SDK.Log;

namespace ChipAndDale.Letter.Command
{    
    internal class LetterCreateCommand : ICommand
    {
        internal LetterCreateCommand() 
        {            
            IServiceMgr serviceMgr = ServiceMgr.Current;
            _logMgr = serviceMgr.GetInstance<ILogMgr>();
            _logger = _logMgr.GetLogger("LetterCreateCommand");
            _commonDbService = serviceMgr.GetInstance<ICommonService>();
            _logger.Debug("Create.");
            _logger.Debug("Interfaces: ICommonDbService = {0};", _commonDbService.ToStateString());
        }

        public void Execute()
        {
            _logger.Debug("Execute");            
        }

        public string Name
        {
            get { return "LetterCreateCommand"; }
        }

        public string Caption
        {
            get { return "Створення нового листа"; }
        }

        public string Hint
        {
            get { return "Створення нового листа"; }
        }

        public System.Drawing.Image Image
        {
            get { return Properties.Resources.AddLetter; }
        }

        public bool IsVisible
        {
            get { return true; }
        }

        public bool IsEnabled
        {
            get 
            {
                return true;/*_commonDbService.IsConnect*/;
            }
        }

        public bool IsChecked
        {
            get { return false; }
        }

        public bool HasState
        {
            get { return false; }
        }

        public CommandType CommandType
        {
            get { return global::Core.SDK.Composite.UI.CommandType.Button; }
        }

        #region private 
        ILogMgr _logMgr;
        ILogger _logger;        
        ICommonService _commonDbService;
        #endregion private
    }
}
