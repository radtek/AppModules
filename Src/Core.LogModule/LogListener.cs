using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NLog;

namespace Core.LogModule
{
    public static class LogListener
    {
        public static event Action<Core.SDK.Log.LogLevel, string> LogEventDelegate;

        public static void OnNewLogWrite(string level, string message)
        {
            SendToPublicListener(Core.SDK.Log.LogLevel.Error, message);
        }

        private static void SendToPublicListener(Core.SDK.Log.LogLevel level, string message)
        {
            try
            {
                if (LogEventDelegate != null) LogEventDelegate(level, message);
            }
            catch { }
        } 

    }
}
