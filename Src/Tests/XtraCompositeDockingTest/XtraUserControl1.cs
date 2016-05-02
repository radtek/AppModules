using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Text;
using System.Windows.Forms;
using DevExpress.XtraEditors;
using Core.SDK.Composite.UI;

namespace XtraCompositeDockingTest
{
    public partial class XtraUserControl1 : DevExpress.XtraEditors.XtraUserControl, IDialogResult
    {
        public XtraUserControl1()
        {
            InitializeComponent();
        }        

        public object OkButton
        {
            get { return OkSButton; }
        }

        public object CancelButton
        {
            get { return CancelSButton; }
        }
    }
}
