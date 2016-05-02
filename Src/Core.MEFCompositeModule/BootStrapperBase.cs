using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Core.SDK.Log;
using System.ComponentModel.Composition;
using System.ComponentModel.Composition.Hosting;
using Core.SDK.Composite.Event;
using Core.SDK.Composite.Service;
using Core.SDK.Composite.Pluggin;

namespace Core.CompositeModule
{
    public abstract class BootStrapperBase
    {
        public BootStrapperBase()
        {  }

        public void Run()
        {
            _logMgr = CreateLogMgr();
            if (_logMgr == null) throw new NullReferenceException("LogMgr can not be null.");
            _logger = _logMgr.GetLogger("BootStrapper");
            _logger.Info("Run.");
            
            AggregateCatalog catalog = CreateAgregateCatalog();
            if (catalog == null) FatalError(new NullReferenceException("AggregateCatalog can not be null."));

            _container = new CompositionContainer(catalog);
            if (_container == null) FatalError(new NullReferenceException("CompositionContainer can not be null."));            

            IEventMgr eventMgr = CreateEventMgr();
            if (eventMgr == null) FatalError(new NullReferenceException("EventMgr can not be null."));

            PlugginMgr plugginMgr = CreatePlugginMgr();           
            if (plugginMgr == null) FatalError(new NullReferenceException("PlugginMgr can not be null."));

            _container.ComposeExportedValue<ILogMgr>(LogMgr);
            _container.ComposeExportedValue<IEventMgr>(eventMgr);
            _container.ComposeExportedValue<IServiceMgr>(new ServiceMgrImpl(_logMgr, _container));
            IServiceMgr serviceMgr = _container.GetExportedValue<IServiceMgr>();
            ServiceMgr.SetServiceMgrProvider(() => serviceMgr);

            _container.ComposeExportedValue<IPlugginMgr>(plugginMgr);
            _container.ComposeParts(plugginMgr);             
            
            RegisterServices();
            StartWork();
        }

        protected void FatalError(Exception ex)
        {
            _logger.Fatal(ex);
            throw ex;
        }        
        
        protected ILogger Logger 
        { 
            get { return _logger; } 
        }

        protected ILogMgr LogMgr
        {
            get { return _logMgr; }
        }

        protected CompositionContainer CompositionContainer
        {
            get { return _container; }
        }

        protected abstract ILogMgr CreateLogMgr();
        protected abstract AggregateCatalog CreateAgregateCatalog();
        protected abstract IEventMgr CreateEventMgr();
        protected abstract PlugginMgr CreatePlugginMgr();
        protected abstract void RegisterServices();  
        protected abstract void StartWork();
        
        #region private 
        CompositionContainer _container;
        ILogger _logger;
        ILogMgr _logMgr;
        #endregion private
    }
}
