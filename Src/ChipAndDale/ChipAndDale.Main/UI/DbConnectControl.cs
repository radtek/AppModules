using System;
using System.Windows.Forms;
using Core.SDK.Composite.UI;

namespace ChipAndDale.Main.UI
{
    public partial class DbConnectControl : DevExpress.XtraEditors.XtraUserControl, IDialogResult
    {
        public DbConnectControl(string login, string password)
        {
            InitializeComponent();
            LoginTextEdit.Text = login;
            PasswordTextEdit.Text = password;
        }

        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);
            Form parentForm = this.Parent as Form;
            parentForm.CancelButton = CancelButton;
            parentForm.AcceptButton = OkButton;
        }

        internal string Login
        {
            get { return LoginTextEdit.Text; }
        }

        internal string Password
        {
            get { return PasswordTextEdit.Text; }
        }

        object IDialogResult.OkButton
        {
            get { return OkButton; }
        }

        object IDialogResult.CancelButton
        {
            get { return CancelButton; }
        }             
              

        
        
        #region private       

        private void PasswordTextEdit_TextChanged(object sender, EventArgs e)
        {
            ValidateData();
        }  

        private void ValidateData()
        {
            OkButton.Enabled = (!string.IsNullOrEmpty(LoginTextEdit.Text) && !string.IsNullOrEmpty(PasswordTextEdit.Text));
        }

        #endregion        
    }
}
