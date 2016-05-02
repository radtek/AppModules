using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using DevExpress.XtraEditors;

namespace Core.XtraCompositeModule.DialogMgr
{
    public partial class MessageFormWithDetail : DevExpress.XtraEditors.XtraForm
    {
        public MessageFormWithDetail()
        {
            InitializeComponent();
            HideDetailPanel();
        }

        int _detailHeight = 170;

        public MessageFormWithDetail(Core.SDK.Log.LogLevel level, string message, string fullText, string caption, System.Drawing.Image image)
            : this()
        {
            Icon = Properties.Resources.applications;
            Text = caption;
            if (image == null)
            {
                if (level == Core.SDK.Log.LogLevel.Error || level == Core.SDK.Log.LogLevel.Fatal) ImageEdit.Image = Properties.Resources.delete_48x48;
                else if (level == Core.SDK.Log.LogLevel.Warn) ImageEdit.Image = Properties.Resources.alert_48x48;
                else ImageEdit.Image = Properties.Resources.info_48x48;
            }
            else ImageEdit.Image = image;            
            MessageLabel.Text = message;
            DetailMemoEdit.Text = fullText; 
        }

        bool? _dialogResultExt;
        public bool? DialogResultExt
        {
            get { return _dialogResultExt; }
            protected set { _dialogResultExt = value; }
        }

        private void OkButton_Click(object sender, EventArgs e)
        {
            DialogResultExt = true;
            Close();
        }

        private void DetailButton_Click(object sender, EventArgs e)
        {
            if (DetailPanelControl.Visible)
            {
                HideDetailPanel();
            }
            else
            {
                ShowDetailPanel();
            }
        }

        private void ShowDetailPanel()
        {
            DetailButton.Text = "Сховати";
            Height = Height + _detailHeight;
            DetailPanelControl.Height = _detailHeight;
            DetailPanelControl.Visible = true;
        }

        private void HideDetailPanel()
        {
            DetailButton.Text = "Показати";
            Height = Height - _detailHeight;
            DetailPanelControl.Visible = false;
        }       
    }
}