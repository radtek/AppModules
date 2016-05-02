using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Core.SDK.Log;

namespace Core.UtilsModule
{
    public static class InvokerHlp
    {
        public static void WithExceptionSuppress(Action action)
        {
            WithExceptionSuppress(action, null);
        }    

        public static void WithExceptionSuppress(Action action, ILogger logger)
        {
            try { action.Invoke(); } 
            catch (Exception ex) 
            { 
                if (logger != null) logger.Debug(ex);
            }
        }
        
    }
}
