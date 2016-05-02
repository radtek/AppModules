using System;
namespace Core.SDK.Composite.UI
{
    public interface IMessageBoxMgr
    {
        bool? ShowDialog(Core.SDK.Log.LogLevel level, string message, string caption, System.Drawing.Image image);
        bool? ShowDialog(string message);
        
        void ShowError(Exception exception);
        void ShowError(string message);
        void ShowError(string message, Exception exception);
        void ShowInfo(string message);
        void ShowWarning(string message);
        void ShowMessage(Core.SDK.Log.LogLevel level, string message, string caption, System.Drawing.Image image);

        void ShowErrorWithDetail(string message, string fullText);
        void ShowErrorWithDetail(string message, Exception exception);
        void ShowErrorWithDetail(Exception exception);
        void ShowWarningWithDetail(string message, string fullText);
        void ShowInfoWithDetail(string message, string fullText);
        void ShowMessageWithDetail(Core.SDK.Log.LogLevel level, string message, string fullText, string caption, System.Drawing.Image image);        
        
        void ShowWaitScreen(string caption, string description);
        void HideWaitScreen();
        void ShowAndHideWaitScreen(Action action, string caption, string description);
    }
}
