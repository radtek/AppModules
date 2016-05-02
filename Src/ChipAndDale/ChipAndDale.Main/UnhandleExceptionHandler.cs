using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Windows.Forms;

namespace ChipAndDale.Main
{
    internal class UnhandleExceptionHandler
    {
        public void ExceptionHandler(object sender, ThreadExceptionEventArgs e)
        {
            try
            {                
                DialogResult result = ShowThreadExceptionDialog(e.Exception);
                if (result == DialogResult.Abort) Application.Exit();
            }
            catch
            {            
                try
                {
                    MessageBox.Show("Фатальна помилка",
                        "Fatal Error",
                        MessageBoxButtons.OK,
                        MessageBoxIcon.Stop);
                }
                finally
                {
                    Application.Exit();
                }
            }
        }
      
        private DialogResult ShowThreadExceptionDialog(Exception ex)
        {
            string errorMessage =
                "Необроблене виключення:\n\n" +
                ex.Message + "\n\n" +
                ex.GetType() +
                "\n\nСтек:\n" +
                ex.StackTrace;

            return MessageBox.Show(errorMessage,
                "Помилка",
                MessageBoxButtons.AbortRetryIgnore,
                MessageBoxIcon.Stop);
        }
    }
}
