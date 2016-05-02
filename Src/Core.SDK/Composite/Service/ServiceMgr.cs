using System;
using System.Collections.Generic;
using System.Text;

namespace Core.SDK.Composite.Service
{
    public static class ServiceMgr
    {
        private static ServiceMgrProvider currentProvider;

        public static IServiceMgr Current
        {
            get { return currentProvider(); }
        }
       
        public static void SetServiceMgrProvider(ServiceMgrProvider newProvider)
        {
            currentProvider = newProvider;
        }
    }

    public delegate IServiceMgr ServiceMgrProvider();
}
