using ChipAndDale.SDK.Common;
using Core.SDK.Common;
using Core.SDK.Composite.Service;
using Core.SDK.Composite.UI;
using Core.SDK.Log;

namespace ChipAndDale.Request.Command
{    
    internal class RequestCreateCommand : ICommand
    {
        internal RequestCreateCommand(IMainController mainController) 
        {
            _mainController = mainController;
            IServiceMgr serviceMgr = ServiceMgr.Current;
            _logMgr = serviceMgr.GetInstance<ILogMgr>();
            _logger = _logMgr.GetLogger("RequestCreateCommand");
            _commonDbService = serviceMgr.GetInstance<ICommonService>();
            _logger.Debug("Create.");
            _logger.Debug("Interfaces: ICommonDbService = {0}; Request.IMainController = {1}", _commonDbService.ToStateString(), _mainController.ToStateString());
        }

        public void Execute()
        {
            _logger.Debug("Execute");
            _mainController.OpenRequestEditForm(null, false);
        }

        public string Name
        {
            get { return "RequestCreateCommand"; }
        }

        public string Caption
        {
            get { return "Створення нового звернення"; }
        }

        public string Hint
        {
            get { return "Створення нового звернення"; }
        }

        public System.Drawing.Image Image
        {
            get { return Properties.Resources.AddRequest_18; }
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
        IMainController _mainController;
        ICommonService _commonDbService;
        #endregion private
    }
}
