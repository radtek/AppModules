using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Core.SDK.Composite.UI;
using Core.SDK.Composite.Service;
using Core.SDK.Log;

namespace ChipAndDale.Request.Command
{    
    internal class RequestPlugginMenuCommand : ICommand
    {
        internal RequestPlugginMenuCommand() 
        {            
            IServiceMgr serviceMgr = ServiceMgr.Current;
            _logMgr = serviceMgr.GetInstance<ILogMgr>();
            _logger = _logMgr.GetLogger("RequestPlugginMenuCommand");            
            _logger.Debug("Create.");            
        }

        public void Execute()
        { }

        public string Name
        {
            get { return "RequestPlugginMenuCommand"; }
        }

        public string Caption
        {
            get { return "Звернення"; }
        }

        public string Hint
        {
            get { return "Меню зверненнь"; }
        }

        public System.Drawing.Image Image
        {
            get { return Properties.Resources.Request; }
        }

        public bool IsVisible
        {
            get { return true; }
        }

        public bool IsEnabled
        {
            get { return true; }
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
            get { return global::Core.SDK.Composite.UI.CommandType.Group; }
        }

        #region private 
        ILogMgr _logMgr;
        ILogger _logger;        
        #endregion private
    }
}
