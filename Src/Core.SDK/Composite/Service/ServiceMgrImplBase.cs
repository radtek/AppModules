using System;
using System.Collections.Generic;
using System.Text;
using System.Globalization;
using Core.SDK.Log;

namespace Core.SDK.Composite.Service
{
    public abstract class ServiceMgrImplBase : IServiceMgr
    {                
        public virtual object GetService(Type serviceType)
        {
            return GetInstance(serviceType, null);
        }

        public virtual object GetInstance(Type serviceType)
        {
            return GetInstance(serviceType, null);
        }

        public virtual object GetInstance(Type serviceType, string key)
        {
            try
            {
                return DoGetInstance(serviceType, key);
            }
            catch (Exception ex)
            {
                throw new ActivationException(
                    FormatActivationExceptionMessage(ex, serviceType, key),
                    ex);
            }
        }

        public virtual object GetInstanceNoEx(Type serviceType)
        {
            return GetInstanceNoEx(serviceType, null);
        }

        public virtual object GetInstanceNoEx(Type serviceType, string key)
        {
            try
            {
                return DoGetInstance(serviceType, key);
            }
            catch (Exception ex)
            {
                if (_logger != null) _logger.Warn(FormatActivationExceptionMessage(ex, serviceType, key), ex);
                return null;
            }
        }

        public abstract void AddInstance<T>(T instance);        

        public abstract void AddInstance<T>(T instance, string key);

        public virtual IEnumerable<object> GetAllInstances(Type serviceType)
        {
            try
            {
                return DoGetAllInstances(serviceType);
            }
            catch (Exception ex)
            {
                throw new ActivationException(
                    FormatActivateAllExceptionMessage(ex, serviceType),
                    ex);
            }
        }
       
        public virtual TService GetInstance<TService>()
        {
            return (TService)GetInstance(typeof(TService), null);
        }
     
        public virtual TService GetInstance<TService>(string key)
        {
            return (TService)GetInstance(typeof(TService), key);
        }

        public virtual TService GetInstanceNoEx<TService>()
        {
            return (TService)GetInstanceNoEx(typeof(TService), null);
        }

        public virtual TService GetInstanceNoEx<TService>(string key)
        {
            return (TService)GetInstanceNoEx(typeof(TService), key);
        }
        
        public virtual IEnumerable<TService> GetAllInstances<TService>()
        {
            foreach (object item in GetAllInstances(typeof(TService)))
            {
                yield return (TService)item;
            }
        }
       
        protected abstract object DoGetInstance(Type serviceType, string key);
        
        protected abstract IEnumerable<object> DoGetAllInstances(Type serviceType);

        protected virtual string FormatActivationExceptionMessage(Exception actualException, Type serviceType, string key)
        {
            return string.Format(CultureInfo.CurrentUICulture,  serviceType.Name, key);
        }

        protected virtual string FormatActivateAllExceptionMessage(Exception actualException, Type serviceType)
        {
            return string.Format(CultureInfo.CurrentUICulture, serviceType.Name);
        }

        public abstract void Trace();

        
        
        protected ILogMgr _logMgr;
        protected  ILogger _logger;
    }

    public partial class ActivationException : Exception
    {       
        public ActivationException() { }

        public ActivationException(string message) : base(message) { }
        
        public ActivationException(string message, Exception innerException) : base(message, innerException) { }
    }
}
