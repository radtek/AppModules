using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using DevExpress.XtraEditors;
using Core.SDK.Composite.UI;

namespace Core.XtraCompositeModule.DialogMgr
{
    public partial class FormBase : DevExpress.XtraEditors.XtraForm
    {
        public FormBase()
        {
            InitializeComponent();            
        }

        public FormBase(DialogFormOption options)
            : this()
        {
            _dialogFormOptions = options;
            Init(_dialogFormOptions);
        }       

        public DialogSetting DialogSetting
        {
            get 
            { 
                DialogSetting setting = new DialogSetting();
                setting.Position = Location;
                setting.Size = Size;
                setting.IsMinimize = this.WindowState == FormWindowState.Minimized;
                return setting;
            }
        }

        public DialogFormOption DialogFormOption
        {
            get { return _dialogFormOptions; }
        }

        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);           
        }

        protected virtual void Init(DialogFormOption options)
        {
            if ((options & DialogFormOption.FixedSize) == DialogFormOption.FixedSize) this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            if ((options & DialogFormOption.FullScreen) == DialogFormOption.FullScreen) this.WindowState = FormWindowState.Maximized;
            if ((options & DialogFormOption.Minimize) == DialogFormOption.Minimize) this.WindowState = FormWindowState.Minimized;
            if ((options & DialogFormOption.ShowInOSTaskBar) == DialogFormOption.ShowInOSTaskBar) this.ShowInTaskbar = true; else this.ShowInTaskbar = false;
            if ((options & DialogFormOption.ShowCloseButton) == DialogFormOption.ShowCloseButton) this.CloseBox = true; else this.CloseBox = false;
            if ((options & DialogFormOption.ShowMaximizeButton) == DialogFormOption.ShowMaximizeButton) this.MaximizeBox = true; else this.MaximizeBox = false;
            if ((options & DialogFormOption.ShowMinimizeButton) == DialogFormOption.ShowMinimizeButton) this.MinimizeBox = true; else this.MinimizeBox = false;
            if ((options & DialogFormOption.ShowIcon) == DialogFormOption.ShowIcon) this.ShowIcon = true; else this.ShowIcon = false; 
        }

        protected DialogFormOption _dialogFormOptions;        
    }
}