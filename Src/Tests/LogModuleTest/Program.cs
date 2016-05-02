using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;

namespace LogModuleTest
{
    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Core.SDK.Log.ILogMgr logMgr = new Core.LogModule.LogMgr();
            Core.SDK.Log.ILogger logger = logMgr.GetLogger("Main");
            logger.Debug("Start application");
            logger.Debug("Test exception", new NotSupportedException("Test exception"));
            Application.Run(new Form1(logMgr));
            logger.Debug("Finish application");
        }
    }
}
