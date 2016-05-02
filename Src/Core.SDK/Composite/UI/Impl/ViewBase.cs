using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Core.SDK.Composite.Common;
using System.Drawing;
using System.ComponentModel;

namespace Core.SDK.Composite.UI
{
    public abstract class ViewBase<T> : INotifyPropertyChanged, IView where T : class
    {
        public ViewBase()
            : this(string.Empty, null, string.Empty, string.Empty, null)
        { }

        public ViewBase(object control)
            : this(string.Empty, control, string.Empty, string.Empty, null)
        { }

        public ViewBase(object control, string caption, string hint, Image image)
            : this(string.Empty, control, caption, hint, image)
        { }

        public ViewBase(string name, object control, string caption, string hint, Image image)
        {            
            _control = (T)control;
            _caption = caption;           
            _hint = hint;
            _image = image;
            if (string.IsNullOrEmpty(name)) _name = GenerateName(); 
            else _name = name;    
        }        

        public string Name
        {
            get { return _name; }
            set { _name = value; }
        }

        public object UserControl
        {
            get { return _control; }
        }

        public string Caption
        {
            get { return _caption; }
            set { _caption = value; }
        }

        public string Hint
        {
            get { return _hint; }
            set { _hint = value; }
        }

        public Image Image
        {
            get { return _image; }
            set { _image = value; }
        }

        public IdentKey Ident
        {
            get { return _ident; }
            set { _ident = value; }
        }

        public DialogSetting DialogSetting
        {
            get { return _dialogSetting; }
            set { _dialogSetting = value; }
        }


        public virtual bool OnClosing(Nullable<bool> result)
        {
            return true;
        }

        public virtual void OnAfterClose()
        { }

        public virtual bool CanClose
        {
            get { return true; }
        }

        public virtual void OnActivate(bool isActive)
        { }

        public virtual void ChangeDockingState(ViewDockingState dockOption)
        { }

        public override string ToString()
        {
            return string.Format("View: Name = {0}, Caption = {1}.", Name, Caption);
        }

        #region INotifyPropertyChanged

        public event PropertyChangedEventHandler PropertyChanged;

        protected bool SetPropertyValue<TProperty>(string propertyName, ref TProperty oldValue, ref TProperty newValue)
        {
            return SetPropertyValue<TProperty>(propertyName, ref oldValue, ref newValue, true);
        }

        protected bool SetPropertyValue<TProperty>(string propertyName, ref TProperty oldValue, ref TProperty newValue, bool checkEquals)
        {
            if (oldValue == null && newValue == null)
            {
                return false;
            }

            if ((oldValue == null && newValue != null) || !oldValue.Equals((TProperty)newValue) || !checkEquals)
            {
                oldValue = newValue;
                FirePropertyChanged(propertyName);
                return true;
            }

            return false;
        }

        protected void FirePropertyChanged(string propertyName)
        {
            if (this.PropertyChanged != null)
            {
                this.PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
            }
        }

        #endregion INotifyPropertyChanged


        #region private

        protected T _control;
        protected IdentKey _ident;
        protected string _caption;
        protected string _hint;
        protected string _name;
        protected Image _image;
        protected DialogSetting _dialogSetting;


        private string GenerateName()
        {
            return string.Format("View{0}", DateTime.Now.Ticks.ToString());
        }

        #endregion private
    }
}
