using System;
using System.Collections.Generic;
using System.Text;

namespace Core.SDK.Composite.UI
{
    public abstract class CommandInvokerBase : ICommandInvoker, IDisposable
    {        
        public CommandInvokerBase(ICommand command)
        {
            _command = command;
        }

        public ICommand Command
        {
            get { return _command; }
        } 

        public abstract void UpdateUI();
        public abstract void Dispose();

        protected void DoExecute(object sender, EventArgs e)
        {
            OnExecute();
        }

        protected virtual void OnExecute()
        {
            _command.Execute();
            UpdateUI();
        }              

        #region private
        private ICommand _command;
        #endregion private
    }
}
