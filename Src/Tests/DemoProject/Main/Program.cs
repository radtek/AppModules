using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;
using Core.SDK.Composite.Service;
using Core.SDK.Composite.Pluggin;
using Core.SDK.Log;

namespace Main
{
    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            Bootstrapper boot = new Bootstrapper();
            boot.Run();

            IServiceMgr serviceMgr = ServiceMgr.Current;            
            IPlugginMgr plugMgr = serviceMgr.GetInstance<IPlugginMgr>(); 
            ILogMgr logMgr = serviceMgr.GetInstance<ILogMgr>();
            ILogger logger = logMgr.GetLogger("Main");
            serviceMgr.AddInstance<ILogger>(logger);

            foreach (IPluggin plug in plugMgr.Pluggins)
            {
                logger.Info(plug.Name);
            }

            serviceMgr.Trace();
            ILogger l = serviceMgr.GetInstance<ILogger>();

            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new MainForm());
        }        
    }
}
