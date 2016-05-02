using System;
using System.Collections.Generic;
using NLog; 
using NLog.Targets;

namespace Core.LogModule
{    
    [Target("GeneralTarget")] 
    public sealed class GeneralTarget: TargetWithLayout 
    {
        public GeneralTarget()
        { }        
 
        public static event Action<Core.SDK.Log.LogLevel, string> NewLogEvent;

        protected override void Write(LogEventInfo logEvent) 
        { 
            string logMessage = this.Layout.Render(logEvent);
            SendToListener(LogHelper.FromNLogLevel(logEvent.Level), logMessage);            
        }

        private void SendToListener(Core.SDK.Log.LogLevel level, string message) 
        {
            try
            {
                if (NewLogEvent != null) NewLogEvent(level, message);
            }
            catch { }
        } 
    } 
}
