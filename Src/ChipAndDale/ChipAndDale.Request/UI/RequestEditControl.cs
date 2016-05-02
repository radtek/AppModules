using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Text;
using System.Windows.Forms;
using DevExpress.XtraEditors;
using ChipAndDale.Request.ViewModel;
using Core.SDK.Composite.UI;
using Core.SDK.Log;
using ChipAndDale.SDK.Common;
using System.IO;
using DevExpress.XtraRichEdit.API.Native;
using System.Text.RegularExpressions;
using DevExpress.XtraRichEdit;

namespace ChipAndDale.Request.UI
{
    internal partial class RequestEditControl : DevExpress.XtraEditors.XtraUserControl, IDialogResult
    {
        public RequestEditControl(RequestEditViewModel viewModel)
        {
            InitializeComponent();

            _viewModel = viewModel;
        }

        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);

            if (!string.IsNullOrEmpty(_viewModel.Request.Comments))
            {
                InitialRichEditControl.DataBindings.Add(new System.Windows.Forms.Binding("Text", this.RequestBindingSource, "Comments", true, System.Windows.Forms.DataSourceUpdateMode.OnPropertyChanged));
                InitialRichEditControl.ReadOnly = true;
                InitialRichEditControl.Views.SimpleView.BackColor = System.Drawing.Color.FromArgb(234, 235, 242);
            }

            if (_viewModel.Request.IsNewEntity)
                MainTabbedControlGroup.SelectedTabPage = GeneralParamLayoutControlGroup;
            else MainTabbedControlGroup.SelectedTabPage = CommentLayoutControlGroup;

            if (!QuickViewAttachButton.Checked)
            {
                AttachHtmlPanel.Document.Text = Properties.Resources.AttachQuickViewTurnOff;
                AttachHtmlPanel.Enabled = false;
            }

            if (MainTabbedControlGroup.SelectedTabPage == CommentLayoutControlGroup)
            {
                RequestBriefRichEditControl_TextChanged(null, null);
            }

            RequestViewModelBinding.DataSource = _viewModel;                        
            this.DataBindings.Add("StatusMessageProperty", RequestViewModelBinding, "StatusMessage");                        
            _viewModel.Request.Attaches.SetBindingSource(AttachEntityBindingSource);            
        }

        public string StatusMessageProperty
        {

            get { return MessageStaticItem.Caption; }

            set 
            {
                if (value == null || string.IsNullOrEmpty(value))
                {
                    MessageStaticItem.Caption = string.Empty;
                    return;
                }

                string[] strList = value.Split((new string[] {"###"}), StringSplitOptions.None);
                if (strList == null || strList.Length != 2)
                {
                    MessageStaticItem.Caption = string.Empty;
                    return;
                }

                if (string.Equals(strList[0], LogLevel.Error.ToString())) MessageStaticItem.Appearance.ForeColor = Color.Red;
                else if (string.Equals(strList[0], LogLevel.Warn.ToString())) MessageStaticItem.Appearance.ForeColor = Color.Violet;
                else MessageStaticItem.Appearance.ForeColor = Color.White;
                
                MessageStaticItem.Caption = strList[1];
                
                StatusMessageUpdateTimer.Enabled = true;
            }

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

        RequestEditViewModel _viewModel;

        private void OkButton_Click(object sender, EventArgs e)
        {
            ShowValidationResult();

            Form parent = Parent as Form; 
            parent.DialogResult = DialogResult.OK;
            parent.Close();
        }

        private void CancelButton_Click(object sender, EventArgs e)
        {
            Form parent = Parent as Form;
            parent.DialogResult = DialogResult.Cancel;
            parent.Close();
        }

        private void ApplyButton_Click(object sender, EventArgs e)
        {
            ShowValidationResult();
            _viewModel.OnApplyRequest();
        }

        private void RefreshButton_Click(object sender, EventArgs e)
        {
            RequestErrorProvider.DataSource = null;
            _viewModel.OnRefreshRequest();
        }

        private void StatusMessageUpdateTimer_Tick(object sender, EventArgs e)
        {
            MessageStaticItem.Caption = string.Empty;
            StatusMessageUpdateTimer.Enabled = false;
        }

        void ShowValidationResult()
        {
            RequestErrorProvider.DataSource = RequestBindingSource;
        }

        private void AddAttacheButton_Click(object sender, EventArgs e)
        {
            _viewModel.OnAddAttaches(false);
        }

        private void AddWithArchiveAttachButton_Click(object sender, EventArgs e)
        {
            _viewModel.OnAddAttaches(true);
        }

        private void SaveAllAttachButton_Click(object sender, EventArgs e)
        {
            _viewModel.OnSaveAttaches(true);
        }

        private void SaveAttachButton_Click(object sender, EventArgs e)
        {
            _viewModel.OnSaveAttaches(false);
        }

        private void RemoveButton_Click(object sender, EventArgs e)
        {
            _viewModel.OnRemoveAttaches();
        }

        private void ExecAttachButton_Click(object sender, EventArgs e)
        {
            _viewModel.OnExecuteAttach();
        }

        private void AddCommentButton_Click(object sender, EventArgs e)
        {
            if (!string.IsNullOrEmpty(NewCommentMemoEdit.Text))
                if (_viewModel.OnAddComment(NewCommentMemoEdit.Text))
                    NewCommentMemoEdit.Text = string.Empty;            
        }

        private void AttachEntityBindingSource_CurrentChanged(object sender, EventArgs e)
        {
            if (AttachEntityBindingSource.Current != null && QuickViewAttachButton.Checked)
            {
                AttachEntity entity = AttachEntityBindingSource.Current as AttachEntity;
                string ext = Path.GetExtension(entity.Name);
                if (ext == ".html" || ext == ".htm")
                    AttachHtmlPanel.Document.HtmlText = Encoding.Default.GetString(entity.Blob);
                else AttachHtmlPanel.Document.Text = "";
            }
        }

        private void QuickViewAttachButton_CheckedChanged(object sender, EventArgs e)
        {
            if (QuickViewAttachButton.Checked)
            {
                //AttachViewLayoutControlItem.Visibility = DevExpress.XtraLayout.Utils.LayoutVisibility.Always;
                AttachHtmlPanel.Enabled = true;
                AttachHtmlPanel.Text = "";
                AttachEntityBindingSource_CurrentChanged(null, null);
            }
            else
            {
                AttachHtmlPanel.Document.Text = "";
                AttachHtmlPanel.Text = Properties.Resources.AttachQuickViewTurnOff;
                AttachHtmlPanel.Enabled = false;
                //AttachViewLayoutControlItem.Visibility = DevExpress.XtraLayout.Utils.LayoutVisibility.Never;
            }
        }

        private void CommentRichEdit_TextChanged(object sender, EventArgs e)
        {
            RichEditControl richEdit = sender as RichEditControl;
            if (richEdit == null) return;

            DocumentRange[] ranges = null;
            ranges = richEdit.Document.FindAll(new Regex(@"\[[^\[\]]+\]", RegexOptions.Compiled));
            if (ranges == null || ranges.Length == 0) return;

            richEdit.BeginUpdate();
            foreach (DocumentRange range in ranges)
            {
                CharacterProperties characterProperty = richEdit.Document.BeginUpdateCharacters(range);
                characterProperty.ForeColor = Color.DarkRed;
                characterProperty.Bold = true;                
                richEdit.Document.EndUpdateCharacters(characterProperty);
            }

            richEdit.Document.CaretPosition = richEdit.Document.Range.End;
            richEdit.ScrollToCaret();
            richEdit.EndUpdate();
        }

        private void RequestBriefRichEditControl_TextChanged(object sender, EventArgs e)
        {
            DocumentRange[] ranges = null;
            ranges = RequestBriefRichEditControl.Document.FindAll(new Regex("\"[^\"]+\""));
            if (ranges == null || ranges.Length == 0) return;

            RequestBriefRichEditControl.BeginUpdate();
            foreach (DocumentRange range in ranges)
            {
                CharacterProperties characterProperty = RequestBriefRichEditControl.Document.BeginUpdateCharacters(range);
                characterProperty.ForeColor = Color.DarkViolet;
                characterProperty.Bold = true;
                RequestBriefRichEditControl.Document.EndUpdateCharacters(characterProperty);
            }         
            RequestBriefRichEditControl.EndUpdate();            
        }

        private void MainTabbedControlGroup_SelectedPageChanged(object sender, DevExpress.XtraLayout.LayoutTabPageChangedEventArgs e)
        {
            if (e.Page == CommentLayoutControlGroup)
            {
                RequestBriefRichEditControl_TextChanged(null, null);
            }

            if (_viewModel.Request.IsNewEntity && !InitialRichEditControl.ReadOnly && !string.IsNullOrEmpty(InitialRichEditControl.Text))
            {
                InitialRichEditControl.ReadOnly = true;
                InitialRichEditControl.Views.SimpleView.BackColor = System.Drawing.Color.FromArgb(234, 235, 242);
                string text = InitialRichEditControl.Text;
                InitialRichEditControl.Text = "";
                InitialRichEditControl.DataBindings.Add(new System.Windows.Forms.Binding("Text", this.RequestBindingSource, "Comments", true, System.Windows.Forms.DataSourceUpdateMode.OnPropertyChanged));
                _viewModel.OnAddComment(text);
            }
        }       

        #endregion private                                                                        
    }
}
