using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Core.CompositeModule;
using Core.SDK.Log;
using System.ComponentModel.Composition.Hosting;
using Core.SDK.Composite.Event;
using Core.SDK.Composite.Pluggin;
using Core.LogModule;

namespace Main
{
    internal class Bootstrapper : BootStrapperBase
    {
        protected override ILogMgr CreateLogMgr()
        {
            return new LogMgr(@"J:\Other_project\MyUtils\Deps\NLog\NLog.config");
        }

        protected override AggregateCatalog CreateAgregateCatalog()
        {
            AggregateCatalog catalog = new AggregateCatalog();
            catalog.Catalogs.Add(new DirectoryCatalog(@"J:\Projects\AppModules\trunk\Src\Tests\DemoProject\DemoPluggin\bin\Debug\"));
            //catalog.Catalogs.Add(new AssemblyCatalog(typeof(Pluggin).Assembly));
            return catalog; // .Catalogs.Add(
        }

        protected override IEventMgr CreateEventMgr()
        {
            return new EventMgr(LogMgr);
        }

        protected override PlugginMgr CreatePlugginMgr()
        {
            return new PlugginMgr(LogMgr);
        }

        protected override void RegisterServices()
        { }

        protected override void StartWork()
        { }
    }
}
