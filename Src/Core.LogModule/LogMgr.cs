using System;
using Core.SDK.Log;
using NLog.Config;
using NLog.Targets;

namespace Core.LogModule
{
    public class LogMgr : ILogMgr
    {
        public LogMgr()
        {
            NLog.LogManager.EnableLogging();           
        }

        public LogMgr(string path)
        {
            LoggingConfiguration config = new XmlLoggingConfiguration(path);
            NLog.LogManager.Configuration = config;
            //ConfigurationItemFactory.Default.Targets.RegisterDefinition("GeneralTarget", typeof(GeneralTarget));
            //InitListenerMethod();
            GeneralTarget.NewLogEvent += new Action<LogLevel, string>(OnNewLogEvent);
            NLog.LogManager.EnableLogging();
            _defaultLogger =  new Logger(NLog.LogManager.GetLogger("DefaultLogger"));
        }        

        public event Action<Core.SDK.Log.LogLevel, string> NewLogEvent; 

        public ILogger GetLogger(string name)
        {
            if (string.IsNullOrEmpty(name)) return _defaultLogger;
            else return new Logger(NLog.LogManager.GetLogger(name.PadRight(30, '.'))); 
        }

        public ILogger DefaultLogger()
        {
            return  _defaultLogger; 
        }

        public bool IsLoggingEnabled 
        {
            get { return NLog.LogManager.IsLoggingEnabled(); }
        }

        Logger _defaultLogger;

        private static void InitListenerMethod()
        {
            MethodCallTarget target = new MethodCallTarget();
            target.ClassName = typeof(LogListener).AssemblyQualifiedName;
            target.MethodName = "OnNewLogWrite";
            target.Parameters.Add(new MethodCallParameter("${level}"));
            target.Parameters.Add(new MethodCallParameter("${message}"));
            NLog.Config.SimpleConfigurator.ConfigureForTargetLogging(target, NLog.LogLevel.Debug);           
        }

        void OnNewLogEvent(LogLevel level, string message)
        {
            if (NewLogEvent != null)
                NewLogEvent(level, message);
        }
    }    
}
