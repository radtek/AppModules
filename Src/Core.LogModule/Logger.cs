 using Core.SDK.Log;
using System;

namespace Core.LogModule
{
    public class Logger : ILogger
    {
        public Logger(NLog.Logger logger)
        {
            _logger = logger;             
        }        

        public string Name
        {
            get { return _logger.Name; }
        }        

        public void Debug(string message)
        {
            Log(new LogItem(LogLevel.Debug, message, Name, null));
        }

        public void Debug(string format, params object[] args)
        {
            string message = string.Format(format, args);
            Log(new LogItem(LogLevel.Debug, message, Name, null));
        }

        public void Debug(Exception exception)
        {
            Log(new LogItem(LogLevel.Debug, "", Name, exception));
        }

        public void Debug(Exception exception, string message)
        {
            Log(new LogItem(LogLevel.Debug, message, Name, exception));
        }

        public void Debug(Exception exception, string format, params object[] args)
        {
            string message = string.Format(format, args);
            Log(new LogItem(LogLevel.Debug, message, Name, exception));
        }

        
        public void Info(string message)
        {
            Log(new LogItem(LogLevel.Info, message, Name, null));
        }

        public void Info(string format, params object[] args)
        {
            string message = string.Format(format, args);
            Log(new LogItem(LogLevel.Info, message, Name, null));
        }

        public void Info(Exception exception)
        {
            Log(new LogItem(LogLevel.Info, "", Name, exception));
        }

        public void Info(Exception exception, string message)
        {            
            Log(new LogItem(LogLevel.Info, message, Name, exception));
        }

        public void Info(Exception exception, string format, params object[] args)
        {
            string message = string.Format(format, args);
            Log(new LogItem(LogLevel.Info, message, Name, exception));
        }

        
        public void Warn(string message)
        {
            Log(new LogItem(LogLevel.Warn, message, Name, null));
        }

        public void Warn(string format, params object[] args)
        {
            string message = string.Format(format, args);
            Log(new LogItem(LogLevel.Warn, message, Name, null));
        }

        public void Warn(Exception exception)
        {
            Log(new LogItem(LogLevel.Warn, "", Name, exception));
        }

        public void Warn(Exception exception, string message)
        {
            Log(new LogItem(LogLevel.Warn, message, Name, exception));
        }

        public void Warn(Exception exception, string format, params object[] args)
        {
            string message = string.Format(format, args);
            Log(new LogItem(LogLevel.Warn, message, Name, exception));
        }

        
        public void Error(string message)
        {
            Log(new LogItem(LogLevel.Error, message, Name, null));
        }

        public void Error(string format, params object[] args)
        {
            string message = string.Format(format, args);
            Log(new LogItem(LogLevel.Error, message, Name, null));
        }

        public void Error(Exception exception)
        {
            Log(new LogItem(LogLevel.Error, "", Name, exception));
        }

        public void Error(Exception exception, string message)
        {
            Log(new LogItem(LogLevel.Error, message, Name, exception));
        }

        public void Error(Exception exception, string format, params object[] args)
        {
            string message = string.Format(format, args);
            Log(new LogItem(LogLevel.Error, message, Name, null));
        }

        
        public void Fatal(string message)
        {
            Log(new LogItem(LogLevel.Fatal, message, Name, null));
        }

        public void Fatal(string format, params object[] args)
        {
            string message = string.Format(format, args);
            Log(new LogItem(LogLevel.Fatal, message, Name, null));
        }

        public void Fatal(Exception exception)
        {
            Log(new LogItem(LogLevel.Fatal, "", Name, exception));
        }

        public void Fatal(Exception exception, string message)
        {
            Log(new LogItem(LogLevel.Fatal, message, Name, exception));
        }

        public void Fatal(Exception exception, string format, params object[] args)
        {
            string message = string.Format(format, args);
            Log(new LogItem(LogLevel.Fatal, message, Name, exception));
        }



        public void WriteLog(LogLevel level, string message)
        {
            Log(new LogItem(level, message, Name, null));
        }

        public void WriteLog(LogLevel level, string format, params object[] args)
        {
            string message = string.Format(format, args);
            Log(new LogItem(level, message, Name, null));
        }

        public void WriteLog(LogLevel level, Exception exception)
        {
            Log(new LogItem(level, "", Name, exception));
        }

        public void WriteLog(LogLevel level, Exception exception, string message)
        {
            Log(new LogItem(level, message, Name, exception));
        }

        public void WriteLog(LogLevel level, Exception exception, string format, params object[] args)
        {
            string message = string.Format(format, args);
            Log(new LogItem(level, message, Name, exception));
        }


        
        public bool IsDebugEnabled 
        { 
            get { return _logger.IsDebugEnabled; }
        }
        public bool IsInfoEnabled 
        {
            get { return _logger.IsInfoEnabled; }
        }
        public bool IsWarnEnabled 
        {
            get { return _logger.IsWarnEnabled; }
        }
        public bool IsErrorEnabled 
        {
            get { return _logger.IsErrorEnabled; }
        }
        public bool IsFatalEnabled 
        {
            get { return _logger.IsFatalEnabled; }
        }

        #region private
        NLog.Logger _logger;

        void Log(LogItem item)
        {
            if (item == null) throw new ArgumentNullException("item");            

            if (item.LogException != null)
            {
                switch (item.Level)
                {
                    case LogLevel.Fatal:
                        _logger.FatalException(item.Text, item.LogException);
                        break;

                    case LogLevel.Error:
                        _logger.ErrorException(item.Text, item.LogException);
                        break;

                    case LogLevel.Warn:
                        _logger.WarnException(item.Text, item.LogException);
                        break;

                    case LogLevel.Info:
                        _logger.InfoException(item.Text, item.LogException);
                        break;

                    case LogLevel.Debug:
                        _logger.DebugException(item.Text, item.LogException);
                        break;

                    default:
                        _logger.InfoException(item.Text, item.LogException);
                        break;
                }
            }
            else
            {
                switch (item.Level)
                {
                    case LogLevel.Fatal:
                        _logger.Fatal(item.Text);
                        break;

                    case LogLevel.Error:
                        _logger.Error(item.Text);
                        break;

                    case LogLevel.Warn:
                        _logger.Warn(item.Text);
                        break;

                    case LogLevel.Info:
                        _logger.Info(item.Text);
                        break;

                    case LogLevel.Debug:
                        _logger.Debug(item.Text);
                        break;

                    default:
                        _logger.Info(item.Text);
                        break;
                }
            }
        }
        #endregion private
    }
}
