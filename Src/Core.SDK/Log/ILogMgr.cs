using System;

namespace Core.SDK.Log
{
    public enum LogLevel
    {
        Undefined = 0,
        Debug,
        Info,
        Warn,
        Error,
        Fatal
    }

    public class LogItem
    {
        public LogItem(LogLevel level, string text, string name, Exception ex)
        {
            Level = level;
            Text = text;
            Name = name;
            LogException = ex;
        }

        public LogLevel Level { get; private set; }
        public string Text { get; private set; }
        public string Name { get; private set; }
        public Exception LogException { get; private set; }
    }

    public class LogEventArgs : EventArgs
    {
        public LogEventArgs(LogItem log)
        {
            Log = log;
        }

        public LogItem Log { get; private set; }
    }

    public interface ILogMgr
    {
        ILogger GetLogger(string name);

        bool IsLoggingEnabled { get; }

        ILogger DefaultLogger();

        event Action<Core.SDK.Log.LogLevel, string> NewLogEvent;
    }

}
