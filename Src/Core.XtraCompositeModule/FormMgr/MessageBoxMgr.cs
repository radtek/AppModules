using System;
using System.Text;
using Core.SDK.Common;
using Core.SDK.Composite.UI;
using Core.SDK.Log;
using Core.UtilsModule;
using DevExpress.XtraEditors;
using DevExpress.XtraSplashScreen;

namespace Core.XtraCompositeModule.DialogMgr
{
    public class MessageBoxMgr : IMessageBoxMgr
    {
        public MessageBoxMgr(XtraForm parentForm, ILogMgr logMgr, SplashScreenManager splashScreenMgr, Type waitFormType)
        {
            if (parentForm == null) throw new ArgumentNullException("ParentForm param can not be null.");
            if (logMgr == null) throw new ArgumentNullException("LogMgr param can not be null.");
            if (logMgr == null) throw new ArgumentNullException("SplashScreenMgr param can not be null.");
            //if (splashScreenMgr.ActiveSplashFormTypeInfo == null) throw new ArgumentNullException("SplashScreenManager can not have null ActiveSplashFormTypeInfo property.");
          
            _parentForm = parentForm;           
            _logMgr = logMgr;
            _splashScreenMgr = splashScreenMgr;
            _waitFormType = waitFormType;
            _logger = _logMgr.GetLogger("MessageBoxMgr");
            _logger.Info("Create.");
        }    

        public void ShowInfo(string message)
        {
            ShowMessageFormInternal(LogLevel.Info, message, "Інформація", null);            
        }

        public void ShowWarning(string message)
        {
            ShowMessageFormInternal(LogLevel.Warn, message, "Попередження", null);
        }

        public void ShowError(string message)
        {
            ShowMessageFormInternal(LogLevel.Error, message, "Помилка", null);
        }

        public void ShowError(Exception exception)
        {
            ShowMessageFormInternal(LogLevel.Error, exception.Message, "Помилка", null);
        }

        public void ShowError(string message, Exception exception)
        {
            StringBuilder sb = new StringBuilder(message.ToStringExt());
            sb.AppendLine(Hlp.GetExceptionText(exception).Substring(0, 200) + "...");
            ShowMessageFormInternal(LogLevel.Error, sb.ToString(), "Помилка", null);
        }

        public void ShowMessage(LogLevel level, string message, string caption, System.Drawing.Image image)
        {
            ShowMessageFormInternal(level, message, caption, image);
        }

        
        
        
        
        public void ShowErrorWithDetail(string message, string fullText)
        {
            ShowMessageWithDetail(LogLevel.Error, message, fullText, "Помилка", null);
        }

        public void ShowErrorWithDetail(string message, Exception exception)
        {
            ShowMessageWithDetail(LogLevel.Error, message, Hlp.GetExceptionText(exception), "Помилка", null);
        }

        public void ShowErrorWithDetail(Exception exception)
        {
            ShowMessageWithDetail(LogLevel.Error, "Невідома помилка", Hlp.GetExceptionText(exception), "Помилка", null);
        }

        public void ShowWarningWithDetail(string message, string fullText)
        {
            ShowMessageWithDetail(LogLevel.Warn, message, fullText, "Попередження", null);
        }

        public void ShowInfoWithDetail(string message, string fullText)
        {
            ShowMessageWithDetail(LogLevel.Info, message, fullText, "Інформація", null);
        }


        public void ShowMessageWithDetail(LogLevel level, string message, string fullText, string caption, System.Drawing.Image image)
        {
            using (MessageFormWithDetail form = new MessageFormWithDetail(level, message, fullText, caption, image))
            {
                _logger.WriteLog(level, "{0}\nFull:\n{1}", message, fullText);
                form.ShowDialog();
            }
        }

        
        
        
        
        public bool? ShowDialog(string message)
        {
            return ShowDialog(LogLevel.Info, message, "", null);
        }

        public bool? ShowDialog(LogLevel level, string message, string caption, System.Drawing.Image image)
        {
            using (MessageDialogForm form = new MessageDialogForm(level, message, caption, image))
            {
                _logger.WriteLog(level, message);
                form.ShowDialog();
                return form.DialogResultExt;
            }
        }

        public void ShowWaitScreen(string caption, string description)
        {
            SplashScreenManager.ShowForm(_parentForm, _waitFormType, true, true);
            SplashScreenManager.Default.SetWaitFormCaption(string.IsNullOrEmpty(caption) ? _defaultSplashScreenCaption : caption);
            SplashScreenManager.Default.SetWaitFormDescription(string.IsNullOrEmpty(description) ? _defaultSplashScreenDesc : description);            
        }

        public void HideWaitScreen()
        {
            SplashScreenManager.CloseForm();

        }

        public void ShowAndHideWaitScreen(Action action, string caption, string description)
        {
            try
            {
                ShowWaitScreen(caption, description);
                if (action != null) action();
            }
            finally
            {
                HideWaitScreen();
            }
        }


        #region private 

        XtraForm _parentForm;
        SplashScreenManager _splashScreenMgr;
        Type _waitFormType;
        string _defaultSplashScreenCaption = "Зачекайте...";
        string _defaultSplashScreenDesc = "";

        ILogMgr _logMgr;
        ILogger _logger;

        private void ShowMessageFormInternal(LogLevel level, string message, string caption, System.Drawing.Image image)
        {
            _logger.WriteLog(level, message);
            using (MessageForm form = new MessageForm(level, message, caption, image))
            {
                form.ShowDialog();
            }
        }

        #endregion private
    }
}
