using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NLog;

namespace Core.LogModule
{
    internal static class LogHelper
    {
        internal static Core.SDK.Log.LogLevel FromNLogLevel(LogLevel nLogLevel)
        {
            Core.SDK.Log.LogLevel level = Core.SDK.Log.LogLevel.Undefined;
            if (LogLevel.Info == nLogLevel || LogLevel.Trace == nLogLevel) level = Core.SDK.Log.LogLevel.Info;
            else if (LogLevel.Error == nLogLevel) level = Core.SDK.Log.LogLevel.Error;
            else if (LogLevel.Fatal == nLogLevel) level = Core.SDK.Log.LogLevel.Fatal;
            else if (LogLevel.Debug == nLogLevel) level = Core.SDK.Log.LogLevel.Debug;
            else if (LogLevel.Warn == nLogLevel) level = Core.SDK.Log.LogLevel.Warn;
            return level;
        }
    }
}
