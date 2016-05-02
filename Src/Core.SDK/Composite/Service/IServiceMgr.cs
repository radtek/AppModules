using System;
using System.Collections.Generic;
using System.Text;

namespace Core.SDK.Composite.Service
{
    public interface IServiceMgr 
    {
        T GetInstance<T>();

        T GetInstance<T>(string key);

        T GetInstanceNoEx<T>();

        T GetInstanceNoEx<T>(string key);

        void AddInstance<T>(T instance);

        void AddInstance<T>(T instance, string key);

        object GetInstance(Type serviceType);
        
        object GetInstance(Type serviceType, string key);

        object GetInstanceNoEx(Type serviceType);

        object GetInstanceNoEx(Type serviceType, string key);
        
        IEnumerable<object> GetAllInstances(Type serviceType);
                      
        IEnumerable<T> GetAllInstances<T>();

        void Trace();
    }
}
