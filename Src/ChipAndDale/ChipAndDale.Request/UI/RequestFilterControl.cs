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
using ChipAndDale.SDK.Request;
using Core.UtilsModule;

namespace ChipAndDale.Request.UI
{
    internal partial class RequestFilterControl : DevExpress.XtraEditors.XtraUserControl, IDialogResult
    {
        public RequestFilterControl(RequestFilterViewModel viewModel)
        {
            InitializeComponent();
            _viewModel = viewModel;            
        }

        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);
            RequestListFilterViewModeBindingSource.DataSource = _viewModel;
            TagCheckedComboBoxEdit.Properties.DataSource = TagsBindingSource;
            TagCheckedComboBoxEdit.Properties.DisplayMember = "Name";
            TagCheckedComboBoxEdit.Properties.ValueMember = "Id";

            StateCheckedComboBoxEdit.Properties.DataSource = RequestStatesBindingSource;
            StateCheckedComboBoxEdit.Properties.DisplayMember = "Value";
            StateCheckedComboBoxEdit.Properties.ValueMember = "Key";

            //if (string.IsNullOrEmpty(_viewModel.Filter.StatusList)) StateCheckedComboBoxEdit.CheckAll();
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

        RequestFilterViewModel _viewModel;

        private void FilterLookUpEdit_EditValueChanged(object sender, EventArgs e)
        {
            RequestListFilterEntity filter = FilterLookUpEdit.EditValue as RequestListFilterEntity;
            _viewModel.OnCloneFilter(filter);
        }

        #endregion              
    }
}
