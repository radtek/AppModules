using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Core.SDK.Composite.Service;
using System.ComponentModel.Composition.Hosting;
using System.ComponentModel.Composition;
using Core.SDK.Log;

namespace Core.CompositeModule
{    
    public class ServiceMgrImpl : ServiceMgrImplBase
    {
        public ServiceMgrImpl(ILogMgr logMgr)
        {
            _logMgr = logMgr;
            _logger = _logMgr.GetLogger("ServiceMgrImpl");            
            _logger.Info("Create.");
        }        

        public ServiceMgrImpl(ILogMgr logMgr, CompositionContainer compositionContainer)
            : this(logMgr)
        {
            this._compositionContainer = compositionContainer;
        }

        public override void AddInstance<T>(T instance)
        {
            _compositionContainer.ComposeExportedValue<T>(instance);
        }

        public override void AddInstance<T>(T instance, string key)
        {
            _compositionContainer.ComposeExportedValue<T>(key, instance);
        }
       
        protected override IEnumerable<object> DoGetAllInstances(Type serviceType)
        {
            List<object> instances = new List<object>();

            IEnumerable<Lazy<object, object>> exports = this._compositionContainer.GetExports(serviceType, null, null);
            if (exports != null)
            {
                instances.AddRange(exports.Select(export => export.Value));
            }

            return instances;
        }
      
        protected override object DoGetInstance(Type serviceType, string key)
        {
            IEnumerable<Lazy<object, object>> exports = this._compositionContainer.GetExports(serviceType, null, key);
            if ((exports != null) && (exports.Count() > 0))
            {             
                return exports.Single().Value;
            }

            throw new ActivationException(
                this.FormatActivationExceptionMessage(new CompositionException("Export not found"), serviceType, key));
        }

        public override void Trace()
        {
            //object o = _compositionContainer.Catalog.Parts;
        }

        #region private        
        private readonly CompositionContainer _compositionContainer;
        #endregion private
    }
}
