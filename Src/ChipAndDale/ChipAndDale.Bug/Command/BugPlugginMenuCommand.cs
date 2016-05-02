using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Core.SDK.Composite.UI;
using Core.SDK.Composite.Service;
using Core.SDK.Log;

namespace ChipAndDale.Bug.Command
{    
    internal class BugPlugginMenuCommand : ICommand
    {
        internal BugPlugginMenuCommand() 
        {            
            IServiceMgr serviceMgr = ServiceMgr.Current;
            _logMgr = serviceMgr.GetInstance<ILogMgr>();
            _logger = _logMgr.GetLogger("BugPlugginMenuCommand");            
            _logger.Debug("Create.");            
        }

        public void Execute()
        { }

        public string Name
        {
            get { return "BugPlugginMenuCommand"; }
        }

        public string Caption
        {
            get { return "Баги"; }
        }

        public string Hint
        {
            get { return "Меню багів"; }
        }

        public System.Drawing.Image Image
        {
            get { return Properties.Resources.Bug_18; }
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
