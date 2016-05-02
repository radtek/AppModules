using System;
using System.Collections.Generic;
using System.Text;
using System.Drawing;
using Core.SDK.Common;

namespace Core.SDK.Composite.UI
{    
    public class Command : ICommand
    {        
        public Command(string caption, ExecuteDelegate executeHandler)
            : this(caption, executeHandler, null, null, null, null, string.Empty, null, UI.CommandType.Button)
        { }

        public Command(string caption, ExecuteDelegate executeHandler, BooleanDelegate isEnabledHandler)
            : this(caption, executeHandler, isEnabledHandler, null, null, null, string.Empty, null, UI.CommandType.Button)
        { }

        public Command(string caption, ExecuteDelegate executeHandler, BooleanDelegate isEnabledHandler, string name, BooleanDelegate isVisibleHandler,
            BooleanDelegate isCheckedHandler, string hint, Image image, CommandType commandType)
        {
            if (_commandType != UI.CommandType.Group && executeHandler == null) throw new ArgumentNullException("No executeHandler");

            _caption = caption;
            _executeHandler = executeHandler;
            _isEnabledHandler = isEnabledHandler;
            _name = name;
            _isVisibleHandler = isVisibleHandler;
            _isCheckedHandler = isCheckedHandler;
            _hint = hint;
            _image = image;
            _commandType = commandType;
        }

        public void Execute()
        {
            _executeHandler.Invoke();
        }

        public string Caption
        {
            get { return _caption; }
        }

        public string Name
        {
            get { return _name; }
        }

        public string Hint
        {
            get { return _hint; }
        }

        public Image Image
        {
            get { return _image; }
        }

        public bool IsVisible
        {
            get
            {
                if (_isVisibleHandler == null) return true;
                else return _isVisibleHandler.Invoke();
            }
        }

        public bool IsEnabled
        {
            get
            {
                if (_isEnabledHandler == null) return true;
                else return _isEnabledHandler.Invoke();
            }
        }

        public bool IsChecked 
        {
            get
            {
                if (_isCheckedHandler == null) return true;
                else return _isCheckedHandler.Invoke();
            }
        }

        public bool HasState
        {
            get
            {
                return (_isCheckedHandler != null);                
            }
        }

        public CommandType CommandType
        {
            get { return _commandType; }
        }

        public override string ToString()
        {
            return string.Format("Command: Name = {0}, Caption = {1}.", Name, Caption);
        }

        #region private
        string _caption;
        string _name;
        string _hint;
        Image _image;
        BooleanDelegate _isEnabledHandler;
        BooleanDelegate _isVisibleHandler;
        BooleanDelegate _isCheckedHandler;
        ExecuteDelegate _executeHandler;
        CommandType _commandType;
        #endregion private
    }
}
